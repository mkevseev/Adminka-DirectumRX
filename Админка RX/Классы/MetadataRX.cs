using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;


namespace Админка_RX.Классы
{


    internal class MetadataRX
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





        public static XmlDocument ПолучитьМетаданные()
        {
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(MetadataUrl);

            //set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;

            var authenticationHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Login}:{Password}"));
            request.Headers.Add("Authorization", "Basic " + authenticationHeaderValue);

            // Get the response.
            WebResponse response = request.GetResponse();

            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);

            // Read the content.
            string responseFromServer = reader.ReadToEnd();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader(responseFromServer));
            xmlDoc.Save(@"C:\Новая папка (2)\ff.xml");
            // Clean up the streams and the response.
            reader.Close();
            response.Close();

            return xmlDoc;
        }

        public static Stream ПолучитьМетаданныеStream()
        {
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(MetadataUrl);

            //set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;

            var authenticationHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Login}:{Password}"));
            request.Headers.Add("Authorization", "Basic " + authenticationHeaderValue);

            // Get the response.
            WebResponse response = request.GetResponse();

            // Get the stream containing content returned by the server.
            return response.GetResponseStream();


        }

        public static IEdmModel ПолучитьEdmМодель()
        {
            IEdmModel edmModel = null;
            using (Stream ss = MetadataRX.ПолучитьМетаданныеStream())
            {
                XmlTextReader reader = new XmlTextReader(ss);

                edmModel = CsdlReader.Parse(reader);
            }

            return edmModel;
        }

        public static List<string> ПолучитьТипыСущностей(IEdmModel edm)
        {

            //var elements = edm.EntityContainer.Elements;
            var elements = edm.EntityContainer.EntitySets();
            var значения = new List<string>();

            elements = elements.OrderBy(e => e.Name);
            foreach (IEdmEntitySet elem in elements)
            {
                var структТип = MetadataRX.ПолучитьСтруктурированныйТип(elem);

                var baseType2 = структТип as IEdmEntityType; // entity name

                var baseType = структТип.BaseType; // entity name

                var propertys = ПолучитьВсеРеквизитыТипа(структТип, new List<IEdmProperty>());

                // Если нет реквиитов или только ИД, то пропускаем вообще
                if (!propertys.Any())
                    continue;
                else if (propertys.Count() == 1 && propertys.First().Name == "Id")
                    continue;
                else if (propertys.Count() == 1)
                    continue;

                if (!DirectumRX.НужныйТипСущности(baseType2))
                    continue;

                var schemaType = baseType2 as IEdmSchemaType;

                значения.Add(elem.Name);

                //if (baseType != null && DirectumRX.НужныйТипСущности(baseType))
                //    значения.Add(elem.Name);
                //else if (baseType == null)
                //    значения.Add(elem.Name);

            }

            return значения;
        }

        public static IEdmEntitySet ПолучитьЭлементEdmМодели(IEdmModel edm, string имя)
        {

            try
            {
                var f = edm.FindDeclaredEntitySet(имя);
                return f;
            }
            catch (System.StackOverflowException)
            { return null; }
            
        }

        public static IEdmEntitySet ПолучитьРеквизитыТипаРеквизита(IEdmModel edm, string имя)
        {
            var elements = edm.EntityContainer.Elements;

            foreach (IEdmEntitySet entity in elements)
            {
                string entityName = entity.Name; // entity name

                IEdmCollectionType edmCollectionType = (IEdmCollectionType)entity.Type;
                IEdmType edmType = edmCollectionType.ElementType.Definition;

                IEdmEntityType edmStructuredType = edmType as IEdmEntityType;
                if (имя == edmStructuredType.Name)
                    return entity;

            }





            //elements.Where(e => e.);

            return null;
        }

        public static List<string> ПолучитьИменаНаследников(IEdmModel edm, string тип)
        {

            var элементEdmМодели = MetadataRX.ПолучитьЭлементEdmМодели(edm, тип);
            var структурныйТип = MetadataRX.ПолучитьСтруктурированныйТип(элементEdmМодели);

            var значения = new List<string>();

            IEdmEntityType baseType = структурныйТип as IEdmEntityType;
            // 10 наследников
            int i = 10;

            for (int j = 1; j != i && baseType != null; j++)
            {
                var type = baseType;

                var name = type.Name;
                var едмМодель = MetadataRX.ПолучитьРеквизитыТипаРеквизита(edm, name);
                if (едмМодель.Name != тип)
                    значения.Add(едмМодель.Name);

                if (baseType == type)
                    baseType = baseType.BaseType as IEdmEntityType;

            }





            return значения;
        }

        public static List<string> ПолучитьИменаНаследников(IEdmModel edm, string основнойТип, string тип, List<string> list, IDictionary<string, string> типИТипыСущности)
        {
            if (string.IsNullOrEmpty(тип))
                return list;

            var элементEdmМодели = MetadataRX.ПолучитьЭлементEdmМодели(edm, тип);
            var структурныйТип = MetadataRX.ПолучитьСтруктурированныйТип(элементEdmМодели);


            IEdmEntityType baseType = структурныйТип as IEdmEntityType;


            var type = baseType.Name;
            string name = string.Empty;
            типИТипыСущности.TryGetValue(type, out name);

            if (основнойТип != name)
                list.Add(name);


            if (baseType.BaseType != null)
            {
                name = string.Empty;
                типИТипыСущности.TryGetValue(((IEdmEntityType)baseType.BaseType).Name, out name);
                return ПолучитьИменаНаследников(edm, основнойТип, name, list, типИТипыСущности);
            }
            else
                return list;
        }


        public static IDictionary<string, string> ПолучитьТипыИТипыСущности(IEdmModel edm, List<string> типыСущностей)
        {
            IDictionary<string, string> типыСущностиИТип = new Dictionary<string, string>();

            foreach (var тип in типыСущностей)
            {
                var элементEdmМодели = MetadataRX.ПолучитьЭлементEdmМодели(edm, тип);
                if (элементEdmМодели != null)
                {
                    var структурныйТип = MetadataRX.ПолучитьСтруктурированныйТип(элементEdmМодели);
                    IEdmEntityType baseType = структурныйТип as IEdmEntityType;
                    if (!типыСущностиИТип.ContainsKey(baseType.Name))
                        типыСущностиИТип.Add(baseType.Name, тип);

                }
            }

            return типыСущностиИТип;
        }


        public static IEdmStructuredType ПолучитьСтруктурированныйТип(IEdmEntitySet entity)
        {
            var entityType = entity.EntityType(); // entity name

            return entityType;
        }



        public static bool БазовыйТип(string тип)
        {
            //String
            //Boolean
            //DateTimeOffset
            //Int32
            //Int64
            //Double
            switch (тип)
            {
                case "String":
                    return true;
                case "Boolean":
                    return true;
                case "DateTimeOffset":
                    return true;
                case "Int32":
                    return true;
                case "Int64":
                    return true;
                case "Double":
                    return true;
                case "Guid":
                    return true;
                case "DateTime": // Д5
                    return true;

                default:
                    return false;
            }
        }


        public enum ТипВвода
        {
            String,
            Boolean,
            DateTimeOffset,
            Int32,
            Int64,
            Double,
            Guid,
            Все
        }

        public static void ОпределитьТипВвода(string типОбычногоРеквизита, ref ТипВвода типВвода)
        {
            типВвода = ТипВвода.Все;

            switch (типОбычногоРеквизита)
            {
                case "String":
                    типВвода = ТипВвода.String;
                    break;

                case "Boolean":
                    типВвода = ТипВвода.Boolean;
                    break;

                case "DateTimeOffset":
                    типВвода = ТипВвода.DateTimeOffset;
                    break;

                case "Int32":
                    типВвода = ТипВвода.Int32;
                    break;

                case "Int64":
                    типВвода = ТипВвода.Int64;
                    break;

                case "Double":
                    типВвода = ТипВвода.Double;
                    break;

                case "Guid":
                    типВвода = ТипВвода.Guid;
                    break;


                default:

                    break;

            }
        }




        public static IDictionary<string, object> ПолучитьВсеРеквизитыТипа(IEdmStructuredType edmType, IDictionary<string, object> лист)
        {
            IEdmEntityType baseType = (IEdmEntityType)edmType;

            var name = baseType.Name;



            foreach (IEdmProperty property in edmType.DeclaredProperties)
            {

                // Имя реквизита
                string propertyName = property.Name;


                IEdmType edmType2 = property.Type.Definition;

                var типРеквизита = ПолучитьТипРеквизита(property);

                if (типРеквизита == ТипРеквизита.Обычный)
                {
                    var primitive = edmType2 as IEdmPrimitiveType;
                    string elementName = primitive.Name;

                    if (!лист.ContainsKey(propertyName))
                        лист.Add(propertyName, elementName);
                }

                if (типРеквизита == ТипРеквизита.Сущность)
                {
                    var primitive = edmType2 as IEdmEntityType;
                    string elementName = primitive.Name;

                    if (!лист.ContainsKey(propertyName))
                        лист.Add(propertyName, elementName);
                }

                if (типРеквизита == ТипРеквизита.Коллекция)
                {
                    var primitive = edmType2 as IEdmCollectionType;
                    var coll = primitive.ElementType.Definition;
                    var type = coll as IEdmStructuredType;
                    var nameType = ((IEdmEntityType)type).Name;

                    if (!лист.ContainsKey(propertyName))
                        лист.Add(propertyName, nameType);
                }

            }


            if (edmType.BaseType != null)
                return ПолучитьВсеРеквизитыТипа(edmType.BaseType, лист);
            else
                return лист;
        }

        public static List<IEdmProperty> ПолучитьВсеРеквизитыТипа(IEdmStructuredType edmType, List<IEdmProperty> лист)
        {
            IEdmEntityType baseType = (IEdmEntityType)edmType;

            var name = baseType.Name;



            foreach (IEdmProperty property in edmType.DeclaredProperties)
                лист.Add(property);


            if (edmType.BaseType != null)
                return ПолучитьВсеРеквизитыТипа(edmType.BaseType, лист);
            else
                return лист;
        }

        public static ТипРеквизита? ПолучитьТипРеквизита(IEdmProperty property)
        {
            // Имя реквизита
            string propertyName = property.Name;

            IEdmType type = property.Type.Definition;

            if (type.TypeKind == EdmTypeKind.Primitive)
                return ТипРеквизита.Обычный;

            if (type.TypeKind == EdmTypeKind.Entity)
                return ТипРеквизита.Сущность;

            if (type.TypeKind == EdmTypeKind.Collection)
                return ТипРеквизита.Коллекция;

            return null;

        }


        public enum ТипРеквизита
        {
            Обычный,
            Сущность,
            Коллекция
        }










        public static void ПолучитьИменаФункции()
        {
            using (Stream stream = MetadataRX.ПолучитьМетаданныеStream())
            {


                XDocument xdoc = XDocument.Load(stream);

                // получаем корневой узел
                var edmx = xdoc.Elements();//.Element("schema");
                var dataServices = edmx.Elements();
                var schema = dataServices.Elements();



                if (schema != null)
                {
                    // ЭТА ТЕМА ПОЛНОСЬЮ РАБОЧАЯ. НУЖНО НЕМНОГО ПОСМОТРЕТЬ ДЕБАГЕР
                    var EntityType = dataServices.Elements().Elements().Where((n) => n.Name.LocalName == "EntityType");
                    var EntitySet = dataServices.Elements().Elements().Where((n) => n.Name.LocalName == "EntityContainer").Elements().Where((n) => n.Name.LocalName == "EntitySet");
                    var ComplexType = dataServices.Elements().Elements().Where((n) => n.Name.LocalName == "ComplexType");
                    var Function = dataServices.Elements().Elements().Where((n) => n.Name.LocalName == "Function");
                    var Action = dataServices.Elements().Elements().Where((n) => n.Name.LocalName == "Action");
                    var NavigationPropertyBinding = dataServices.Elements().Elements().Where((n) => n.Name.LocalName == "NavigationPropertyBinding");

                    var ff = dataServices.Elements().Elements().Where((n) => n.Name.LocalName == "EntityContainer");

                    var fff = Function.Attributes();
                    foreach (var f in Function)
                    {
                        var d = f;
                    }


                    // проходим по всем элементам person

                }
            }
        }



        const string FILENAME = @"C:\Новая папка (2)\ffddd копия.xml";
        public static void Mains()
        {

            //// передаем в конструктор тип класса Person
            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(Edmx));

            //// десериализуем объект
            //using (FileStream fs = new FileStream(FILENAME, FileMode.OpenOrCreate))
            //{
            //    Edmx? person = (Edmx)xmlSerializer.Deserialize(fs);
            //    var dd = person.DataServices.Schema.EntityType;
            //    foreach (var item in dd)
            //    {
            //        var dddd = item;
            //    }
            //}

            //try
            //{
            //    Microsoft.OData.ModelBuilder.
            //    string myxml = File.ReadAllText(FILENAME);

            //    DataServices OdataSchema = new DataServices();
            //    Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(myxml));
            //    XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(DataServices));
            //    OdataSchema = (DataServices)reader.Deserialize(stream);
            //    foreach (var item in OdataSchema.Schema)
            //    {
            //        var ddd = item;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.GetBaseException().Message);
            //}



        }





        #region Секция со списком типов сущностей

        public class EntityContainer
        {
            public string Name { get; set; }
            public string xmlns { get; set; }

            public List<EntitySet> EntitySet { get; set; }

        }
        public class EntitySet
        {
            public string Name { get; set; }
            public string EntityType { get; set; }
            public List<NavigationPropertyBinding> NavigationPropertyBinding { get; set; }
        }

        public class NavigationPropertyBinding
        {
            public string Path { get; set; }
            public string Target { get; set; }

        }

        #endregion


        public static EntityContainer ПолучитьСписокТиповСущностей()
        {
            using (Stream stream = MetadataRX.ПолучитьМетаданныеStream())
            {
                XDocument xdoc = XDocument.Load(stream);

                // получаем корневой узел
                var edmx = xdoc.Elements();
                var dataServices = edmx.Elements();
                var schema = dataServices.Elements();

                EntityContainer entityContainer = new EntityContainer();

                if (schema != null)
                {
                    var EntitySet = dataServices.Elements().Elements().Where((n) => n.Name.LocalName == "EntityContainer").Elements().Where((n) => n.Name.LocalName == "EntitySet");

                    List<EntitySet> entitySet = new List<EntitySet>();


                    foreach (var item in EntitySet)
                    {
                        EntitySet entity = new EntitySet();
                        List<NavigationPropertyBinding> navigationPropertyBinding = new List<NavigationPropertyBinding>();

                        entity.Name = item.Attributes().Where((a) => a.Name == "Name").FirstOrDefault().Value;
                        entity.EntityType = item.Attributes().Where((a) => a.Name == "EntityType").FirstOrDefault().Value;

                        foreach (var item1 in item.Elements())
                        {
                            NavigationPropertyBinding navigationProperty = new NavigationPropertyBinding();
                            navigationProperty.Path = item1.Attributes().Where((a) => a.Name == "Path").FirstOrDefault().Value;
                            navigationProperty.Target = item1.Attributes().Where((a) => a.Name == "Target").FirstOrDefault().Value;
                            navigationPropertyBinding.Add(navigationProperty);
                        }
                        entity.NavigationPropertyBinding = navigationPropertyBinding;
                        entitySet.Add(entity);
                    }
                    entityContainer.EntitySet = entitySet;
                }
                return entityContainer;
            }
        }


        #region Секция с типами сущности модуля

        public class SchemaEntityType
        {
            public string Namespace { get; set; }
            public List<EntityType> EntityType { get; set; }

        }

        public class EntityType
        {

            public string Name { get; set; }
            public string BaseType { get; set; }
            public List<Key> Key { get; set; }
            public List<Property> Properties { get; set; }
            public List<NavigationProperty> NavigationProperty { get; set; }
        }

        public class Property
        {

            public string Name { get; set; }

            public string Type { get; set; }
            public bool Nullable { get; set; }
        }

        public class NavigationProperty
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public bool Nullable { get; set; }
        }

        public class Key
        {
            public List<PropertyRef> PropertyRef { get; set; }
        }
        public class PropertyRef
        {
            public string Name { get; set; }

        }

        #endregion



    }
}
