using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace Blog.Core
{
    public class JsonConfig
    {
        public static void Initiliaze(bool isCamelCase = true)
        {
            var config = GlobalConfiguration.Configuration;

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            json.SerializerSettings.Formatting = Formatting.None;

            if (isCamelCase)
                json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}