using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Админка_RX.Классы
{
    internal class OData
    {
        private static Properties.Settings Параметры = Properties.Settings.Default;
        private static string Url = Параметры.Ссылка;
        // URL сервиса интеграции
        private static string OdataUrl = Url.TrimEnd('/') + "/integration/odata/";
        // Логин для Basic-аутентификации.
        private static string Login = Параметры.Логин;
        // Пароль для Basic-аутентификации.
        private static string Password = Параметры.Пароль;
        private static int MaxCount = Параметры.MaxCount;

        private static readonly HttpClient _client = CreateHttpClient();


        #region Клиент
        public static HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler();

            var client = new HttpClient(handler, disposeHandler: false); // Важно: не удалять хендлер
           // client.Timeout = TimeSpan.FromSeconds(30); // Твой обычный таймаут ожидания ответа
            client.BaseAddress = new Uri(OdataUrl);


            // Авторизация
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Login}:{Password}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        #endregion




        public static async Task<List<Dictionary<string, object>>> GetDataAsync(string entityName, int top = 100)
        {

            try
            {
                // Запрос: entityName?$top=...
                string requestPath = $"{entityName}?$top={top}";


                var response = await _client.GetAsync(requestPath);

                if (!response.IsSuccessStatusCode)
                {
                    string errorBody = await response.Content.ReadAsStringAsync();
                    throw new Exception(
                        $"Сервер вернул ошибку {(int)response.StatusCode}.\n" +
                        $"Точный URL запроса: {_client.BaseAddress}{requestPath}\n" +
                        $"Ответ сервера: {errorBody.Substring(0, Math.Min(300, errorBody.Length))}..."
                    );
                }

                string jsonContent = await response.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(jsonContent))
                {
                    var root = doc.RootElement;
                    if (!root.TryGetProperty("value", out var valueArray) || valueArray.ValueKind != JsonValueKind.Array)
                        throw new Exception("Ответ не содержит ожидаемого поля 'value'. Возможно, это HTML-ошибка.");

                    var result = new List<Dictionary<string, object>>();
                    foreach (var item in valueArray.EnumerateArray())
                    {
                        result.Add(ParseItem(item));
                    }
                    return result;
                }
            }
            catch (JsonException jex)
            {
                throw new Exception($"Ошибка парсинга JSON. Сервер вернул не JSON. Детали: {jex.Message}");
            }
            catch (TargetInvocationException ex)
            {
                // ВОТ ЗДЕСЬ ИСТИНА:

                MessageBox.Show("Настоящая ошибка: " + ex.InnerException?.Message);
                MessageBox.Show("Тип ошибки: " + ex.InnerException?.GetType().Name);

                //if (ex.InnerException is HttpRequestException httpEx)
                //    MessageBox.Show("HTTP ошибка: " + httpEx.StatusCode);

                
                throw; // дальше не идём, пока не поймём причину
            }

        }


        public static async Task<List<Dictionary<string, object>>> GetAllDataAsync(
        string entityName,
        int pageSize = 50, int maxPages = 200)
        {
            var allResults = new List<Dictionary<string, object>>();
            // Первая ссылка
            string nextLink = $"{entityName}?$top={pageSize}";
            int skip = 0;
            int page = 0;



            while (page < maxPages)
            {
                

                try
                {
                    // Формируем URL вручную: добавляем $top и $skip
                    string requestUrl = $"{entityName}?$top={pageSize}&$skip={skip}";
                    var response = await _client.GetAsync(requestUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorBody = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Ошибка HTTP {(int)response.StatusCode} на странице {page}. Ответ: {errorBody}");
                    }

                    string jsonContent = await response.Content.ReadAsStringAsync();

                    using (var doc = JsonDocument.Parse(jsonContent))
                    {
                        var root = doc.RootElement;

                        if (!root.TryGetProperty("value", out var valueArray) || valueArray.ValueKind != JsonValueKind.Array)
                            throw new Exception("Неверный формат ответа: нет поля 'value'.");

                        int currentCount = 0;
                        foreach (var item in valueArray.EnumerateArray())
                        {
                            allResults.Add(ParseItem(item));
                            currentCount++;
                        }

                        //MessageBox.Show($"✅ Страница {page}: загружено {currentCount} записей. Всего: {allResults.Count}");


                        // ГЛАВНАЯ ПРОВЕРКА: если мы получили меньше, чем просили — значит, данные кончились
                        if (currentCount < pageSize)
                        {
                            //MessageBox.Show("🛑 Получено меньше записей, чем размер страницы. Данные закончились.");

                            break; // Выходим из цикла
                        }
                    } // using doc освобождает память

                    skip += pageSize; // Готовим смещение для следующей итерации




                    
                }
                catch (Exception ex) when (ex is TargetInvocationException || ex.InnerException is TargetInvocationException)
                {
                    // Если вдруг обертка всё-таки вылезет, достаём суть
                    var realEx = ex is TargetInvocationException tEx ? tEx.InnerException : ex;
                    throw new Exception("Критическая ошибка при загрузке страницы. Реальная причина: " + realEx.Message, realEx);
                }
            }

            //MessageBox.Show($"✅ Готово! Всего загружено: {allResults.Count} записей.");

            return allResults;
        }



        private static Dictionary<string, object> ParseItem(JsonElement element)
        {
            var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            foreach (var prop in element.EnumerateObject())
            {
                if (prop.Name.StartsWith("@odata.", StringComparison.Ordinal))
                    continue;

                object value;
                if (prop.Value.ValueKind == JsonValueKind.Null) value = null;
                else if (prop.Value.ValueKind == JsonValueKind.String) value = prop.Value.GetString();
                else if (prop.Value.ValueKind == JsonValueKind.Number)
                {
                    value = prop.Value.TryGetDouble(out double d) ? (object)d : prop.Value.GetInt32();
                }
                else if (prop.Value.ValueKind == JsonValueKind.True) value = true;
                else if (prop.Value.ValueKind == JsonValueKind.False) value = false;
                else if (prop.Value.ValueKind == JsonValueKind.Object) value = ParseItem(prop.Value);
                else if (prop.Value.ValueKind == JsonValueKind.Array)
                {
                    var list = new List<object>();
                    foreach (var inner in prop.Value.EnumerateArray())
                    {
                        // ВОТ ЭТОТ МОМЕНТ ИСПРАВЛЕН: обычный if вместо тернарного
                        if (inner.ValueKind == JsonValueKind.Object)
                        {
                            list.Add(ParseItem(inner));
                        }
                        else
                        {
                            list.Add(inner.ToString());
                        }
                    }

                    value = list;
                }
                else value = prop.Value.ToString();

                dict[prop.Name] = value;
            }
            return dict;
        }

        private List<string> _conditions = new List<string>();
        private bool _hasStarted = false;

        // --- Операторы сравнения ---

        public OData Eq(string field, object value)
        {
            AddCondition($"{field} eq {FormatValue(value)}");
            return this;
        }

        public OData Ne(string field, object value) // not equal
        {
            AddCondition($"{field} ne {FormatValue(value)}");
            return this;
        }

        public OData Gt(string field, object value) // greater than
        {
            AddCondition($"{field} gt {FormatValue(value)}");
            return this;
        }

        public OData Lt(string field, object value) // less than
        {
            AddCondition($"{field} lt {FormatValue(value)}");
            return this;
        }

        public OData Ge(string field, object value) // greater or equal
        {
            AddCondition($"{field} ge {FormatValue(value)}");
            return this;
        }

        public OData Le(string field, object value) // less or equal
        {
            AddCondition($"{field} le {FormatValue(value)}");
            return this;
        }

        // --- Текстовые операторы (для строк) ---

        public OData StartsWith(string field, string value)
        {
            // В OData это функция: startswith(Name, 'Admin')
            AddCondition($"startswith({field}, '{EscapeSingleQuotes(value)}')");
            return this;
        }

        public OData EndsWith(string field, string value)
        {
            AddCondition($"endswith({field}, '{EscapeSingleQuotes(value)}')");
            return this;
        }

        public OData Contains(string field, string value)
        {
            // Внимание: contains() есть не во всех реализациях OData! 
            // Directum RX его поддерживает. Если сервер ругнется — используй StartsWith/EndsWith.
            AddCondition($"contains({field}, '{EscapeSingleQuotes(value)}')");
            return this;
        }

        // --- Логические связки ---

        public OData And()
        {
            if (_hasStarted && _conditions.Count > 0)
                _conditions[_conditions.Count - 1] += " and";
            // Мы не добавляем новую строку в список, а дописываем " and" к последнему условию.
            // Но для простоты реализации ниже мы будем собирать всё в одну строку при Build().
            // Поэтому этот метод здесь просто для читаемости цепочки, логика сборки в Build().
            return this;
        }

        // Для C# 7.3 проще не усложнять цепочкой "and", а просто добавлять условия.
        // Все условия в OData по умолчанию соединяются через AND, если ты их перечисляешь через & в URL.
        // Наш метод Build() сам склеит их через " and ".

        // --- Сборка результата ---

        public string Build()
        {
            if (_conditions.Count == 0)
                return null;

            // OData требует, чтобы условия были соединены словом " and " (с пробелами)
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _conditions.Count; i++)
            {
                sb.Append(_conditions[i]);
                if (i < _conditions.Count - 1)
                    sb.Append(" and ");
            }

            return sb.ToString();
        }

        // Вспомогательный метод для добавления
        private void AddCondition(string condition)
        {
            _conditions.Add(condition);
            _hasStarted = true;
        }

        // Превращает C# значение в строку для OData
        private string FormatValue(object value)
        {
            if (value == null)
                return "null";

            if (value is bool b)
                return b.ToString().ToLower(); // true / false без кавычек

            if (value is DateTime dt)
                // Формат ISO 8601 обязателен для дат в OData
                return $"'{dt.ToString("o")}'";

            if (value is string s)
                return $"'{EscapeSingleQuotes(s)}'"; // Строки всегда в одинарных кавычках

            // Числа (int, double, decimal) идут без кавычек
            return value.ToString();
        }

        // Защита от поломки фильтра, если в тексте есть одинарная кавычка
        private string EscapeSingleQuotes(string s)
        {
            return s.Replace("'", "''");
        }

    }
}
