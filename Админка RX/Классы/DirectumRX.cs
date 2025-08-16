using Microsoft.OData.Edm;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Data;
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
        private static string MetadataUrl = Url + "/integration/odata/$metadata";
        // Логин для Basic-аутентификации.
        private static string Login = Параметры.Логин;
        // Пароль для Basic-аутентификации.
        private static string Password = Параметры.Пароль;



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

            return new ODataClient(odataClientSettings);
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


        #region Получение данных

        public static async Task<IEnumerable<IDictionary<string, object>>> ПолучитьAsync(ODataClient oDataClient, string Имя)
        {
            try
            {
                var associatedApplications = await oDataClient.For(Имя).FindEntriesAsync();
                return associatedApplications;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<IEnumerable<IDictionary<string, object>>> ПолучитьAsync(ODataClient oDataClient, string Имя, string Фильтр)
        {
            try
            {
                if (string.IsNullOrEmpty(Фильтр))
                    return null;

                //Получить все сущности.
                var associatedApplications = await oDataClient.For(Имя)
                .Filter(Фильтр) // Filter("Status eq 'InProcess'")
                .FindEntriesAsync();

                return associatedApplications;
            }
            catch
            {
                return null;
            }



        }

        public static async Task<IEnumerable<IDictionary<string, object>>> ПолучитьAsync(ODataClient oDataClient, string Имя, string Фильтр, string[] ДопРеквизиты)
        {
            try
            {
                var query = oDataClient.For(Имя).Filter(Фильтр);

                //query = query.QueryOptions(string.Join(",", реквизиты));
                query = query.QueryOptions($"$expand=*,{string.Join("($expand=*),", ДопРеквизиты)}($expand=*)");
                return await query.FindEntriesAsync();
            }
            catch
            {
                return null;
            }
        }

        public static async Task<IEnumerable<IDictionary<string, object>>> ПолучитьПоИДAsync(ODataClient oDataClient, string Имя, string ИД, string[] ДопРеквизиты)
        {
            try
            {
                var query = oDataClient.For(Имя).Key(long.Parse(ИД));

                //query = query.QueryOptions(string.Join(",", реквизиты));
                query = query.QueryOptions($"$expand=*,{string.Join("($expand=*),", ДопРеквизиты)}($expand=*)");
                return await query.FindEntriesAsync();
            }
            catch
            {
                return null;
            }
        }

        public static async Task<IEnumerable<IDictionary<string, object>>> ПолучитьAsync(ODataClient oDataClient, string Имя, string[] ДопРеквизиты)
        {
            try
            {
                //Получить все сущности через basic API.

                return await oDataClient.For(Имя)
                .Expand(ДопРеквизиты)
                .FindEntriesAsync();
            }
            catch
            {
                return null;
            }
        }

        public static async Task<IEnumerable<IDictionary<string, object>>> ПолучитьAsync(ODataClient oDataClient, string Имя, string[] ДопРеквизиты, long top)
        {
            try
            {
                //Получить все сущности через basic API.

                return await oDataClient.For(Имя)
                .Top(top)
                .Expand(ДопРеквизиты)
                .FindEntriesAsync();
            }
            catch
            {
                return null;
            }
        }

        public static async Task<IEnumerable<IDictionary<string, object>>> ПолучитьAsync(ODataClient oDataClient, string Имя, string[] ДопРеквизиты, long top, string Фильтр)
        {
            try
            {
                //Получить все сущности через basic API.

                return await oDataClient.For(Имя)
                .Top(top)
                .Expand(ДопРеквизиты)
                .Filter(Фильтр)
                .FindEntriesAsync();
            }
            catch
            {
                return null;
            }
        }

        #endregion



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

                //foreach (var item in Изменения)
                //{
                //    try
                //    {
                //        if (item.Value != null)
                //        {
                //            if (item.Value.GetType().IsArray || !item.Value.GetType().IsValueType)
                //            {
                //                if (item.Value.GetType().IsArray)
                //                {
                //                    var arr = (IDictionary<string, object>[])item.Value;
                //                    odataBatchRequest += async oDataClient => await oDataClient.For(Имя).Key(int.Parse(ИД))
                //                    .NavigateTo(item.Key)
                //                    .Set(arr)
                //                    .InsertEntryAsync();
                //                }
                //                else
                //                {
                //                    try
                //                    {
                //                        if (!item.Value.GetType().IsValueType)
                //                        {
                //                            var val = (IDictionary<string, object>)item.Value;
                //                            odataBatchRequest += async oDataClient => await oDataClient.For(Имя).Key(int.Parse(ИД))
                //                            .NavigateTo(item.Key)
                //                            .Set(val)
                //                            .InsertEntryAsync();
                //                        }
                //                        else
                //                        {
                //                            odataBatchRequest += async oDataClient => await oDataClient.For(Имя).Key(int.Parse(ИД))
                //                            .Set(item)
                //                            .InsertEntryAsync();

                //                        }
                //                    }
                //                    catch
                //                    {
                //                        try
                //                        {
                //                            odataBatchRequest += async oDataClient => await oDataClient.For(Имя).Key(int.Parse(ИД))
                //                            .Set(item)
                //                            .InsertEntryAsync();
                //                        }
                //                        catch { }
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                odataBatchRequest += async oDataClient => await oDataClient.For(Имя).Key(int.Parse(ИД))
                //                .Set(item)
                //                .InsertEntryAsync();
                //            }
                //        }
                //    }
                //    catch { }

                //}


                //foreach (var item in матрицаСтарыйСогласоватьС)
                //{

                //        odataBatchRequest += async oDataClient => await oDataClient.For(Имя).Key(int.Parse(ИД))
                //                .NavigateTo("ApprovalEmployees")
                //                .Set(item)
                //                .InsertEntryAsync();



                //}
                try
                {
                    var фильтр = $"Id eq {long.Parse(ИД)}";
                    var query = oDataClient.For(Имя).Filter(фильтр);

                    //query = query.QueryOptions(string.Join(",", реквизиты));
                    query = query.QueryOptions($"$expand=*,{string.Join("($expand=*),", ДопРеквизиты)}($expand=*)");


                    odataBatchRequest += async oDataClient2 => await oDataClient.For(Имя).Filter(фильтр)
                                .Set(Изменения)
                                .UpdateEntryAsync();

                    await odataBatchRequest.ExecuteAsync();
                }
                catch (Exception ex)
                {
                    var f = ex.Message;
                }

            }

        }

        public static async Task<IDictionary<string, object>> Проверить(ODataClient oDataClient, IEdmModel edmModel, List<string> типыСущностей, string Имя, string ИД, string[] ДопРеквизиты, IDictionary<string, object> Изменения)
        {
            if (string.IsNullOrEmpty(ИД))
                return null;

            IDictionary<string, object> изменения = new Dictionary<string, object>();

            var элементEdmМодели = MetadataRX.ПолучитьЭлементEdmМодели(edmModel, Имя);

            var структурныйТип = MetadataRX.ПолучитьСтруктурированныйТип(элементEdmМодели);
            // Получим все имена и типы реквизитов типа сущности для дальнейшего сравнения 
            var типОбъекта = элементEdmМодели.EntityType();
            var nameType = типОбъекта.Name;

            IDictionary<string, object> всеРеквизиты = new Dictionary<string, object>();
            var реквизиты = MetadataRX.ПолучитьВсеРеквизитыТипа(структурныйТип, всеРеквизиты);

            // Перепроверим все реквизиты на сходство.
            // Изменяем только если не равны значения
            // Получим сущьность
            var данные = await ПолучитьПоИДAsync(oDataClient, Имя, ИД, ДопРеквизиты);
            var запись = данные.First();
            if (запись.Any())
            {



                foreach (var item in Изменения)
                {
                    try
                    {

                        var name = item.Key;



                        var property = типОбъекта.FindProperty(name);
                        var старыеДанныеРеквизита = запись.Where(r => r.Key == name).First();

                        var реквизит = реквизиты.Where(r => r.Key == name).First();

                        var NavigProperties = типОбъекта.NavigationProperties().Where(s => s.Name == name);

                        var типРеквизита = MetadataRX.ПолучитьТипРеквизита(property);

                        if (типРеквизита == MetadataRX.ТипРеквизита.Обычный)
                        {

                            var типС = типОбъекта.FindProperty(name);

                            var val = item.Value;
                            var oldVal = старыеДанныеРеквизита.Value;

                            // Всегда был ноль, значит равны, по другому не хочет 
                            if (oldVal == null && val == null)
                                continue;
                            // Старое значение null, а новое нет, тогда запишем на изменение
                            if (oldVal == null && val != null)
                            {
                                // Запишем на изменение
                                изменения.Add(item.Key, val);

                            }
                            // Старое значение null, а новое нет, тогда запишем на изменение
                            else if (oldVal != null && val == null)
                            {
                                // Запишем на изменение
                                изменения.Add(item.Key, val);

                            }
                            else
                            {
                                // Есди оба были не пустые, то запишем при расхождению

                                if (oldVal.ToString() != val.ToString())
                                {
                                    // Запишем на изменение
                                    изменения.Add(item.Key, val);
                                }
                            }

                        }

                        if (типРеквизита == MetadataRX.ТипРеквизита.Сущность)
                        {

                            var val = item.Value;
                            var oldVal = старыеДанныеРеквизита.Value;

                            // Всегда был ноль, значит равны, по другому не хочет 
                            if (oldVal == null && val == null)
                                continue;
                            // Старое значение null, а новое нет, тогда запишем на изменение
                            if (oldVal == null && val != null)
                            {
                                // Запишем на изменение
                                изменения.Add(item.Key, val);

                            }
                            // Старое значение null, а новое нет, тогда запишем на изменение
                            else if (oldVal != null && val == null)
                            {
                                // Запишем на изменение
                                изменения.Add(item.Key, val);

                            }
                            else
                            {
                                // Есди оба были не пустые, то запишем при расхождению
                                var id = ((IDictionary<string, object>)val)["Id"];
                                var idOld = ((IDictionary<string, object>)oldVal)["Id"];
                                if (!idOld.Equals(id))
                                {
                                    // Запишем на изменение
                                    изменения.Add(item.Key, val);
                                }
                            }

                        }

                        if (типРеквизита == MetadataRX.ТипРеквизита.Коллекция)
                        {
                            var типСущ = элементEdmМодели.FindNavigationTarget(NavigProperties.First()); // Тип данных

                            var элементEdmМоделиКоллекции = MetadataRX.ПолучитьЭлементEdmМодели(edmModel, типСущ.Name);

                            var структурныйТипКоллекции = MetadataRX.ПолучитьСтруктурированныйТип(элементEdmМоделиКоллекции);
                            // Получим все имена и типы реквизитов типа сущности для дальнейшего сравнения 
                            var типОбъектаКоллекции = элементEdmМоделиКоллекции.EntityType();
                            var nameTypeКоллекции = типОбъектаКоллекции.Name;

                            IDictionary<string, object> всеРеквизитыКоллекции = new Dictionary<string, object>();
                            var реквизитыКоллекции = MetadataRX.ПолучитьВсеРеквизитыТипа(структурныйТипКоллекции, всеРеквизитыКоллекции);


                            var arrs = (IDictionary<string, object>[])item.Value;
                            var arrsOld = (IDictionary<string, object>[])старыеДанныеРеквизита.Value;
                            if (arrs != null)
                            {
                                var newArrs = new List<IDictionary<string, object>>();
                                foreach (var arr in arrs)
                                {
                                    long idstr = 0;

                                    IDictionary<string, object> newArr = new Dictionary<string, object>();
                                    // Или ИД или -1 у новой строки коллекции обработка -1 в изменениях
                                    idstr = long.Parse(arr["Id"].ToString());

                                    bool writeId = false;

                                    // Не новое значение. Новое всегда записываем на изменнение
                                    if (idstr != -1)
                                    {
                                        foreach (var it in arr)
                                        {
                                            bool change = false;
                                            var реквизитКоллекции = реквизитыКоллекции.Where(r => r.Key == it.Key).First();

                                            var NavigPropertiesКоллекции = типОбъектаКоллекции.NavigationProperties().Where(s => s.Name == it.Key);

                                            if (NavigPropertiesКоллекции.Any())
                                            {
                                                var val = it.Value;


                                                var whe = arrsOld.Where(i => i["Id"].Equals(idstr)); // && !((IDictionary<string, object>)i[it.Key])["Id"].Equals(id)


                                                if (whe.Any())
                                                {
                                                    var oldVal = whe.Select(i => i[it.Key]).First();

                                                    // Всегда был ноль, значит равны, по другому не хочет 
                                                    if (oldVal == null && val == null)
                                                        continue;
                                                    // Старое значение null, а новое нет, тогда запишем на изменение
                                                    if (oldVal == null && val != null)
                                                    {
                                                        // Запишем на изменение
                                                        newArr.Add(it.Key, val);
                                                        change = true;

                                                    }
                                                    // Старое значение null, а новое нет, тогда запишем на изменение
                                                    else if (oldVal != null && val == null)
                                                    {
                                                        // Запишем на изменение
                                                        newArr.Add(it.Key, val);
                                                        change = true;

                                                    }
                                                    else
                                                    {
                                                        // Есди оба были не пустые, то запишем при расхождению
                                                        var id = ((IDictionary<string, object>)it.Value)["Id"];
                                                        var idOld = ((IDictionary<string, object>)oldVal)["Id"];
                                                        if (!idOld.Equals(id))
                                                        {
                                                            // Запишем на изменение
                                                            newArr.Add(it.Key, val);
                                                            change = true;
                                                        }
                                                    }


                                                }
                                            }
                                            else
                                            {
                                                // Обычный реквизит

                                                var val = it.Value;

                                                var whe = arrsOld.Where(i => i["Id"].Equals(idstr));

                                                if (whe.Any())
                                                {
                                                    var oldVal = whe.Select(i => i[it.Key]).First();

                                                    // Всегда был ноль, значит равны, по другому не хочет 
                                                    if (oldVal == null && val == null)
                                                        continue;
                                                    // Старое значение null, а новое нет, тогда запишем на изменение
                                                    if (oldVal == null && val != null)
                                                    {
                                                        // Запишем на изменение
                                                        newArr.Add(it.Key, val);
                                                        change = true;

                                                    }
                                                    // Старое значение null, а новое нет, тогда запишем на изменение
                                                    else if (oldVal != null && val == null)
                                                    {
                                                        // Запишем на изменение
                                                        newArr.Add(it.Key, val);
                                                        change = true;

                                                    }
                                                    // Есди оба были не пустые, то запишем при расхождении
                                                    else if (!oldVal.Equals(val))
                                                    {
                                                        // Запишем на изменение
                                                        newArr.Add(it.Key, val);
                                                        change = true;

                                                    }

                                                }

                                            }

                                            // Запишем ИД строки коллекции в которой было изменение если не записывали
                                            if (change && !writeId)
                                            {
                                                newArr.Add("Id", idstr);
                                                writeId = true;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var it in arr)
                                        {
                                            if (it.Key != "MemoIT")
                                                // Тут новая строка коллекции
                                                newArr.Add(it.Key, it.Value);
                                        }

                                    }

                                    // Сложим если были изменения
                                    if (newArr.Any())
                                        newArrs.Add(newArr);
                                }

                                // Запишем изменения если были
                                if (newArrs.Any())
                                    изменения.Add(item.Key, newArrs.ToArray());
                            }


                        }

                    }
                    catch (Exception ex)
                    {
                        var d = ex.Message;
                    }

                }

            }

            return изменения;

        }

        public static bool НужныйТипСущности(IEdmStructuredType тип)
        {
            IEdmStructuredType baseType = тип;
            // 10 наследников

            int i = 10;
            bool ok = true;

            for (int j = 1; j != i && baseType != null; j++)
            {
                var type = baseType;
                var ss = baseType as IEdmEntityType;
                var nameFull = ss.FullName();
                if (nameFull.Contains("Sungero.IntegrationService.Models.Generated.Workflow") || // Задачи и задания

                    //nameFull.Contains("Sungero.IntegrationService.Models.Generated.Docflow.IOfficialDocumentTrackingDto") ||
                    //nameFull.Contains("Sungero.IntegrationService.Models.Generated.Content.IElectronicDocumentVersionsDto") || // Версии документа (есть примеры как добавлять, не тут уж точно)
                    nameFull.Contains("Sungero.IntegrationService.Models.Generated.CoreEntities.IGroupRecipientLinksDto")
                    )
                {
                    ok = false;
                    break;
                }

                if (baseType == type)
                    baseType = baseType.BaseType;
            }

            return ok;
        }


        public static string РусскоеИмяРеквизита(IEdmModel edm, string имяРеквизита, string типРеквизита, string типСущности)
        {
            string имя = string.Empty;
            var Файлы = Параметры.Файлы;

            var типы = MetadataRX.ПолучитьИменаНаследников(edm, типСущности);


            foreach (var тип in типы)
            {
                var элементEdmМодели = MetadataRX.ПолучитьЭлементEdmМодели(edm, тип);

                var структурныйТип = MetadataRX.ПолучитьСтруктурированныйТип(элементEdmМодели);

                var типОбъекта = элементEdmМодели.EntityType();

                var name = типОбъекта.Name;
                if (Файлы.Tables.Contains(name))
                {
                    var tab = Файлы.Tables[name];
                    // Обычный
                    if (tab.Columns.Contains("Property_" + имяРеквизита))
                    {
                        var ind = tab.Columns.IndexOf("Property_" + имяРеквизита);
                        имя = tab.Rows[0][ind].ToString();
                    }

                    // Enum 
                    //if (tab.Columns.Contains("Enum_" + имяРеквизита))
                    //{
                    //    var ind = tab.Columns.IndexOf("Enum_" + имяРеквизита);
                    //    имя =
                    //}
                }
            }

            return имя;
        }

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
            catch
            {
                getRequestToFunction.Add("result", false);
            }


            return getRequestToFunction;
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

            string[] ids = { "Id" };
            //Получить все сущности через basic API.
            var associatedApplications = await oDataClient.For(Имя)
                .Select(ids)
                .OrderByDescending(ids)
                .Top(1)
                .FindEntriesAsync();

            return associatedApplications;

        }

        public static string ПолучитьИмяТипаСущности(string типСущности)
        {
            string имяТипаСущности = string.Empty;

            if (типСущности.StartsWith("I") && типСущности.EndsWith("Dto"))
                имяТипаСущности = типСущности.Substring(1, типСущности.Length - 4);

            return имяТипаСущности;
        }

    }
}
