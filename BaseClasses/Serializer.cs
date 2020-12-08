using System;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace BaseClasses
{
    public class JsonSerializer
    {
        /// <summary>
        /// JSON反序列化对象，比ASP.NET自带的快9倍
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeserializeFromString<T>(string value)
        {
            return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(value);
            //return JsonConvert.DeserializeObject<T>( value );
        }

        static readonly string IsJsonDefaultUnicodeConfig = System.Configuration.ConfigurationManager.AppSettings.Get("IsJsonDefaultUnicode");

        /// <summary>
        /// JSON序列化对象，比ASP.NET自带的快9倍
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SerializeToString<T>(T value, bool isUnicode = false)
        {
            //bool isDefaultUnicode = false;
            //if (!string.IsNullOrEmpty(IsJsonDefaultUnicodeConfig))
            //{
            //    bool.TryParse(IsJsonDefaultUnicodeConfig, out isDefaultUnicode);
            //}
            //if (isUnicode || isDefaultUnicode)
            //{
            //    ServiceStack.Text.JsConfig.EscapeUnicode = true;
            //}
            //return ServiceStack.Text.JsonSerializer.SerializeToString<T>(value);


            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.None, settings);
        }
        /// <summary>
        /// 有空格格式化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="isUnicode"></param>
        /// <returns></returns>
        public static string SerializeToNoEmptyString<T>(T value, bool isUnicode = false)
        {
            //bool isDefaultUnicode = false;
            //if (!string.IsNullOrEmpty(IsJsonDefaultUnicodeConfig))
            //{
            //    bool.TryParse(IsJsonDefaultUnicodeConfig, out isDefaultUnicode);
            //}
            //if (isUnicode || isDefaultUnicode)
            //{
            //    ServiceStack.Text.JsConfig.EscapeUnicode = true;
            //}
            //return ServiceStack.Text.JsonSerializer.SerializeToString<T>(value);


            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented, settings);
        }
    }

    public class XmlSer
    {
        /// <summary>
        /// 生成过多的字符
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeserializeFromString<T>(string value)
        {
            return ServiceStack.Text.XmlSerializer.DeserializeFromString<T>(value);
        }

        public static string SerializeToString<T>(T value)
        {
            return ServiceStack.Text.XmlSerializer.SerializeToString<T>(value);
        }

        public static string XmlSerialize<T>(T obj)
        {
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var strWriter = new StringWriter())
            {
                var xmlWriter = XmlWriter.Create(strWriter, new XmlWriterSettings { OmitXmlDeclaration = true });
                xmlSerializer.Serialize(xmlWriter, obj, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
                return strWriter.ToString();
            }
        }

        public static T XmlDeSerialize<T>(string str)
        {
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(str)))
            {
                return (T)xmlSerializer.Deserialize(ms);
            }

        }

        public static string Xml(NameValueCollection nvc)
        {
            string _xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            _xml += "<RoomVersionInfo>";
            for (int i = 0; i < nvc.Count; i++)
            {
                _xml += "<" + nvc.GetKey(i) + ">";
                _xml += nvc[i];
                _xml += "</" + nvc.GetKey(i) + ">";
            }
            _xml += "</RoomVersionInfo>";
            return _xml;
        }


        #region Xml

        /// <summary>
        /// 序列化对象为XML字符串
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string XmlSerializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);

            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream, Encoding.UTF8);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }


        public static T XmlDeserialize<T>(string xml) where T : class
        {
            XmlSerializer xmlSer = new XmlSerializer(typeof(T));
            try
            {
                using (StringReader reader = new StringReader(xml))
                {
                    return xmlSer.Deserialize(reader) as T;
                }
            }
            catch
            { }
            return null;
        }

        #endregion

    }
}
