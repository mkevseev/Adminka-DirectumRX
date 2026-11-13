using Microsoft.OData.Edm;
using Simple.OData.Client;
using Simple.OData.Client.V4.Adapter.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Админка_RX.Классы
{
    internal class DirectumRX
    {
        private static Properties.Settings Параметры = Properties.Settings.Default;
        private static string Url = Параметры.Ссылка;
        // URL сервиса интеграции
        private static string OdataUrl = Url + "/integration/odata/";
        // Логин для Basic-аутентификации.
        private static string Login = Параметры.Логин;
        // Пароль для Basic-аутентификации.
        private static string Password = Параметры.Пароль;
        private static int MaxCount = Параметры.MaxCount;


        #region ODataClient

        /// <summary>
        /// Подключение
        /// </summary>
        /// <returns></returns>
        public static ODataClient ODataClient()
        {
            // Настройки Simple OData Client: добавление ко всем запросам URL сервиса и
            // заголовка с данными аутентификации.
            var odataClientSettings = new ODataClientSettings(new Uri(OdataUrl));
            odataClientSettings.IgnoreUnmappedProperties = true;
            odataClientSettings.BeforeRequest += message =>
            {
                var authenticationHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Login}:{Password}"));
                message.Headers.Add("Authorization", "Basic " + authenticationHeaderValue);
            };

            // МОЖНО ВЫЛОВИТЬ ОШИБКУ
            //odataClientSettings.TraceFilter = ODataTrace.ResponseContent; //ODataTrace.All;//ODataTrace.RequestContent | ODataTrace.ResponseContent; // только запросы и ответы

            //odataClientSettings.OnTrace = (format, args) =>
            //{
            //    var message = string.Format(format, args);
            //    // Можно добавить префикс или таймстемп
            //    MessageBox.Show($"[OData] {message}");
            //    //Console.WriteLine($"[OData] {message}");
            //};

            return new ODataClient(odataClientSettings);
        }

        public static ODataClient ODataClient(string url, string login, string password)
        {
            Url = url;
            OdataUrl = Url + "/integration/odata/";
            Login = login;
            Password = password;
            ODataClient();

            return ODataClient();
        }

        /// <summary>
        /// Подключение
        /// </summary>
        /// <returns></returns>
        public static async void ODataClient2()
        {

            var requestUrl = Параметры.Ссылка;
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            // Добавление заголовков для авторизации.
            request.Headers.Add("username", ".");
            request.Headers.Add("password", "!");
            // Создание запроса.
            var httpClient = new HttpClient();
            try
            {
                var responce = await httpClient.SendAsync(request);
                if (responce.IsSuccessStatusCode)
                {
                    MessageBox.Show("OK");
                }
                else
                {
                    MessageBox.Show($"Error: responce status code '{responce.StatusCode}'");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: responce status code '{ex.Message}'");
            }



        }

        #endregion



        #region Получение

        public static async Task<IEnumerable<IDictionary<string, object>>> ПолучитьAsync(ODataClient oDataClient, string Имя, string Фильтр)
        {
            return await ПолучитьAsync(oDataClient, Имя, Фильтр, new string[] { });
        }

        public static async Task<IEnumerable<IDictionary<string, object>>> ПолучитьAsync(ODataClient oDataClient, string Имя)
        {
            return await ПолучитьAsync(oDataClient, Имя, null, new string[] { });
        }

        public static async Task<IEnumerable<IDictionary<string, object>>> ПолучитьAsync(ODataClient oDataClient, string Имя, string[] ДопРеквизиты)
        {
            return await ПолучитьAsync(oDataClient, Имя, null, ДопРеквизиты);
        }

        public static async Task<IEnumerable<IDictionary<string, object>>> ПолучитьAsync(ODataClient oDataClient, string Имя, string[] ДопРеквизиты, string ИД)
        {
            return await ПолучитьAsync(oDataClient, Имя, $"Id eq {long.Parse(ИД)}", ДопРеквизиты);
        }

        public static async Task<IEnumerable<IDictionary<string, object>>> ПолучитьAsync(ODataClient oDataClient, string Имя, string Фильтр, string[] ДопРеквизиты)
        {
            try
            {
                List<IDictionary<string, object>> results = new List<IDictionary<string, object>>();
                int skipCount = 0;
                int pageSize = 100;
                bool hasMore = true;

                while (hasMore)
                {
                    // $skip и $top — стандартный механизм пагинации в OData
                    var query = oDataClient.For(Имя)
                        .Filter(Фильтр)          // любой фильтр
                                                 //.Expand(ДопРеквизиты)
                        .Skip(skipCount)
                        .Top(pageSize);

                    if (ДопРеквизиты.Any())
                        query = query.QueryOptions($"$expand=*,{string.Join("($expand=*),", ДопРеквизиты)}($expand=*)");


                    var page = await query.FindEntriesAsync();

                    if (page == null || page.Count() == 0)
                    {
                        hasMore = false;
                        break;
                    }

                    results.AddRange(page.ToList());

                    skipCount += pageSize;

                    if (skipCount >= MaxCount)
                    {
                        hasMore = false;
                        break;
                    }

                    // Если страница меньше размера страницы — значит, это последняя страница
                    hasMore = page.Count() == pageSize;
                }

                return results.AsEnumerable();

            }
            catch (WebRequestException ex)
            {
                //    MessageBox.Show($"Статус: {ex.Code}");
                //    MessageBox.Show($"URL: {ex.RequestUri}");
                MessageBox.Show($"Код: {ex.Code}\nОтвет сервера: {ex.Response}");

                if (ex.Code == System.Net.HttpStatusCode.NotFound)
                {
                    // своя обработка 404
                }

                return null;
            }

        }

        public static async Task<IEnumerable<IDictionary<string, object>>> ПолучитьВсеИД(ODataClient oDataClient, string Имя)
        {

            string[] ids = { "Id" };
            //Получить все сущности через basic API.
            var associatedApplications = await oDataClient.For(Имя)
                .Select(ids)
                .FindEntriesAsync();

            return associatedApplications;

        }

        public static async Task<IEnumerable<IDictionary<string, object>>> ПолучитьПоследнееИД(ODataClient oDataClient, string Имя)
        {
            try
            {
                string[] ids = { "Id" };
                //Получить все сущности через basic API.
                var associatedApplications = await oDataClient.For(Имя)
                    .Select(ids)
                    .OrderByDescending(ids)
                    .Top(1)
                    .FindEntriesAsync();

                return associatedApplications;
            }
            catch (Exception ex)
            {
                var f = ex.Message;
            }

            return null;
        }

        #endregion

        #region Изменение

        public static async Task<IDictionary<string, object>> Изменить(ODataClient oDataClient, string Имя, string ИД, IDictionary<string, object> Изменения)
        {
            try
            {
                if (string.IsNullOrEmpty(ИД))
                    return null;


                //Получить все сущности.
                var associatedApplications = await oDataClient.For(Имя)
                    .Key(long.Parse(ИД))
                    .Set(Изменения)
                    .UpdateEntryAsync();

                return associatedApplications;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public static async Task<IDictionary<string, object>> Изменить(ODataClient oDataClient, string Имя, string ИД, string[] ДопРеквизиты, IDictionary<string, object> Изменения)
        {


            try
            {
                if (string.IsNullOrEmpty(ИД))
                    return null;

                var query = oDataClient.For(Имя).Key(long.Parse(ИД));

                query = query.QueryOptions($"$expand=*,{string.Join("($expand=*),", ДопРеквизиты)}($expand=*)");
                //Получить все сущности.
                var associatedApplications = await query
                    .Set(Изменения)
                    .UpdateEntryAsync();

                return associatedApplications;
            }
            catch (Exception ex)
            {
                var f = ex.Message;
                return null;
            }
        }

        public static async void ИзменитьПакетно(ODataClient oDataClient, IEdmModel edmModel, string Имя, string ИД, string[] ДопРеквизиты, IDictionary<string, object> Изменения)
        {
            //if (!string.IsNullOrEmpty(ИД))
            //    return;
            // Создание пакетного запроса.
            try
            {
                var фильтр = $"Id eq {long.Parse(ИД)}";

                var odataBatchRequest = new ODataBatch(oDataClient);

                var элементEdmМодели = MetadataRX.ПолучитьЭлементEdmМодели(edmModel, Имя);

                var структурныйТип = MetadataRX.ПолучитьСтруктурированныйТип(элементEdmМодели);
                // Получим все имена и типы реквизитов типа сущности для дальнейшего сравнения 
                var типОбъекта = элементEdmМодели.EntityType();
                var nameType = типОбъекта.Name;

                IDictionary<string, object> всеРеквизиты = new Dictionary<string, object>();
                var реквизиты = MetadataRX.ПолучитьВсеРеквизитыТипа(структурныйТип, всеРеквизиты);


                long idNewInd = 0;

                foreach (var item in Изменения)
                {

                    var name = item.Key;


                    var property = типОбъекта.FindProperty(name);

                    var типРеквизита = MetadataRX.ПолучитьТипРеквизита(property);

                    if (типРеквизита == MetadataRX.ТипРеквизита.Обычный)
                    {
                        IDictionary<string, object> val = new Dictionary<string, object>
                        {
                            { item.Key, item.Value }
                        };

                        odataBatchRequest += async ODataClient => await oDataClient.For(Имя).Key(long.Parse(ИД))
                          .Set(val)
                          .UpdateEntryAsync();

                    }

                    if (типРеквизита == MetadataRX.ТипРеквизита.Сущность)
                    {

                        IDictionary<string, object> val = new Dictionary<string, object>
                        {
                            { item.Key, item.Value }
                        };

                        odataBatchRequest += async ODataClient => await oDataClient.For(Имя).Key(long.Parse(ИД))
                        .Expand(item.Key)
                        .Set(val)
                        .UpdateEntryAsync();//InsertEntryAsync();// UpdateEntryAsync();


                    }

                    if (типРеквизита == MetadataRX.ТипРеквизита.Коллекция)
                    {
                        var NavigProperties = типОбъекта.NavigationProperties().Where(s => s.Name == name);
                        var типСущ = элементEdmМодели.FindNavigationTarget(NavigProperties.First()); // Тип данных

                        var элементEdmМоделиКоллекции = MetadataRX.ПолучитьЭлементEdmМодели(edmModel, типСущ.Name);

                        var структурныйТипКоллекции = MetadataRX.ПолучитьСтруктурированныйТип(элементEdmМоделиКоллекции);
                        // Получим все имена и типы реквизитов типа сущности для дальнейшего сравнения 
                        var типОбъектаКоллекции = элементEdmМоделиКоллекции.EntityType();
                        var nameTypeКоллекции = типОбъектаКоллекции.Name;

                        IDictionary<string, object> всеРеквизитыКоллекции = new Dictionary<string, object>();
                        var реквизитыКоллекции = MetadataRX.ПолучитьВсеРеквизитыТипа(структурныйТипКоллекции, всеРеквизитыКоллекции);


                        var arrs = (IDictionary<string, object>[])item.Value;
                        foreach (var arr in arrs)
                        {
                            long id = -1;

                            foreach (var it in arr)
                            {
                                if (it.Key == "Id")
                                {
                                    if (it.Value.ToString() == "-1")
                                    {
                                        idNewInd--;
                                        id = idNewInd;

                                    }
                                    else
                                        id = long.Parse(it.Value.ToString());
                                }
                            }


                            if (id < 0)
                            {
                                // Создание
                                odataBatchRequest += async ODataClient => await oDataClient.For(Имя).Key(long.Parse(ИД))
                                                                                   .NavigateTo(item.Key) //.Key(id)
                                                                                   .Set(arr)
                                                                                   .InsertEntryAsync();
                            }
                            else
                            {
                                // Изменение
                                odataBatchRequest += async ODataClient => await oDataClient.For(Имя).Key(long.Parse(ИД))
                                                                                   .NavigateTo(item.Key).Key(id)
                                                                                   .Set(arr)
                                                                                   .UpdateEntryAsync();
                            }


                        }

                    }


                }


                await odataBatchRequest.ExecuteAsync();

            }
            catch (WebRequestException ex)
            {
            //    MessageBox.Show($"Статус: {ex.Code}");
            //    MessageBox.Show($"URL: {ex.RequestUri}");
                MessageBox.Show($"Код: {ex.Code}\nОтвет сервера: {ex.Response}");

                if (ex.Code == System.Net.HttpStatusCode.NotFound)
                {
                    // своя обработка 404
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        public struct ДляИзменения
        {
            public string name;
            public int age;

            public void Print()
            {
                Console.WriteLine($"Имя: {name}  Возраст: {age}");

            }
        }


        public static async void ИзменитьПакетно(ODataClient oDataClient, string Имя, string ИД, string[] ДопРеквизиты, IDictionary<string, object> Изменения)
        {
            if (!string.IsNullOrEmpty(ИД))
            {
                // Создание пакетного запроса.
                var odataBatchRequest = new ODataBatch(oDataClient);
                try
                {
                    var фильтр = $"Id eq {long.Parse(ИД)}";
                    var query = oDataClient.For(Имя).Filter(фильтр);

                    //query = query.QueryOptions(string.Join(",", реквизиты));
                    query = query.QueryOptions($"$expand=*,{string.Join("($expand=*),", ДопРеквизиты)}($expand=*)");

                    query.Set(Изменения);
                    odataBatchRequest += async oDataClient2 => await query.UpdateEntryAsync();

                    await odataBatchRequest.ExecuteAsync();
                }
                catch (WebRequestException ex)
                {
                    MessageBox.Show($"Код: {ex.Code}\nОтвет сервера: {ex.Response}");

                    if (ex.Code == System.Net.HttpStatusCode.NotFound)
                    {
                        // своя обработка 404
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }

        #endregion

        #region Создание

        public static void Создание(ODataClient oDataClient, string Имя, IDictionary<string, object> Реквизиты)
        {

            СозданиеЗаписи(oDataClient, Имя, Реквизиты);

            //var s = await Получить(oDataClient, "IDepartments"); // "IFinArchiveMovingTMCTasks"
            //string sss = "";
            //MessageBox.Show(sss);
            //foreach (var ss in s)
            //{
            //    sss += "\n";
            //    foreach (var property in ss)
            //        //В консоль выведутся имена и значения свойств для каждой сущности.
            //        sss += $"{property.Key}: {property.Value}" + "\n";
            //    MessageBox.Show(sss);
            //}



        }


        static async void СозданиеЗаписи(ODataClient oDataClient, string Имя, IDictionary<string, object> Реквизиты)
        {

            // Создание пакетного запроса.
            var odataBatchRequest = new ODataBatch(oDataClient);
            // Создание переменной postRequestsResult для сохранения ответов сервиса интеграции.
            object postRequestsResult;


            // Создание документа типа SimpleDocument. В подзапросе ему присваивается
            // идентификатор -1. Идентификатор используется для обращения к сущности
            // только внутри текущего запроса.
            odataBatchRequest += async odataClient2 => postRequestsResult = await oDataClient.For(Имя)
              .Set(new { Id = -1 })
              .InsertEntryAsync();

            odataBatchRequest += async odataClient2 => await oDataClient.For(Имя).Key(-1)
            .Set(Реквизиты)
            .UpdateEntryAsync();

            //// Поиск подходящего приложения-обработчика для документа. Для упрощения
            //// считаем, что в ответе сервиса интеграции будет список с одним
            //// соответствием.
            //var associatedApplication = await odataClient.For("IAssociatedApplications")
            //  .Filter("Extension eq 'doc'")
            //  .FindEntriesAsync();
            //var associatedApplicationId = associatedApplication.First()["Id"];
            //// Создание версии документа для сущности с идентификатором -1, созданной в
            //// предыдущем подзапросе. В подзапросе создается запись свойства-коллекции
            //// с идентификатором -2, который используется для обращения к сущности
            //// только внутри текущего запроса.
            //odataBatchRequest += async odataClient => await odataClient.For("ISimpleDocuments").Key(-1)
            //  .NavigateTo("Versions")
            //  .Set(new { Id = -2, Number = 1, AssociatedApplication = new { Id = associatedApplicationId } })
            //  .InsertEntryAsync();
            //// Добавление строки, закодированной в Base64, в свойство Body версии
            //// документа. Для примера взята строка "111111". В подзапросе строка
            //// добавляется в запись свойства-коллекции с идентификатором -2, созданную
            //// в предыдущем подзапросе.
            //odataBatchRequest += async odataClient => await odataClient.For("ISimpleDocuments").Key(-1)
            //  .NavigateTo("Versions").Key(-2)
            //  .NavigateTo("Body")
            //  .Set(new { Value = "MTExMTEx" })
            //  .InsertEntryAsync();
            // Выполнение пакетного запроса.
            try
            {
                await odataBatchRequest.ExecuteAsync();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        #endregion


        #region Функции

        public async static Task<IDictionary<string, object>> IsCurrentUserAdmin(ODataClient oDataClient)
        {
            IDictionary<string, object> getRequestToFunction = new Dictionary<string, object>();

            if (oDataClient == null)
            {
                getRequestToFunction.Add("result", false);
                return getRequestToFunction;
            }


            try
            {
                // Создание запроса.
                getRequestToFunction = await oDataClient.For("Company")
                  .Function("IsCurrentUserAdmin")
                  .ExecuteAsSingleAsync();

            }
            catch (Exception ex)
            {
                var d = ex.Message;
                getRequestToFunction.Add("result", false);
            }


            return getRequestToFunction;
        }

        #endregion





        #region АРХИВ возможностей

        //public static async Task<IEnumerable<IDictionary<string, object>>> ПолучитьAsync(ODataClient oDataClient, string Имя, string ИД, string[] ДопРеквизиты)
        //{
        //    try
        //    {
        //        var query = oDataClient.For(Имя).Key(long.Parse(ИД));

        //        //query = query.QueryOptions(string.Join(",", реквизиты));
        //        query = query.QueryOptions($"$expand=*,{string.Join("($expand=*),", ДопРеквизиты)}($expand=*)");
        //        return await query.FindEntriesAsync();
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        #endregion

    }
}
