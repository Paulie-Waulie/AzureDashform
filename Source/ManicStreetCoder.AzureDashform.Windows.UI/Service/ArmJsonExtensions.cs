namespace ManicStreetCoder.AzureDashform.Windows.UI.Service
{
    using System.Linq;
    using Newtonsoft.Json.Linq;

    public static class ArmJsonExtensions
    {
        public static JObject GetObjectByName(this IJEnumerable<JToken> objects, string name)
        {
            return (JObject)objects.SingleOrDefault(x => ((JValue)x.SelectToken("name")).Value<string>().Equals(name));
        }

        public static JProperty GetProperty(this JObject jObject, string propertyName)
        {
            return jObject.Property(propertyName);
        }

        public static JObject GetObject(this JObject armObject, string tokenName)
        {
            return (JObject)armObject.SelectToken(tokenName);
        }

        public static void UpdatePropertyValue(this JObject jObject, string propertyName, string value)
        {
            var property = jObject.GetProperty(propertyName);
            if (property != null)
            {
                property.Value = value;
            }
        }

        public static void ReplacePropertyValueWithParameter(this JObject jObject, ArmPropertyParameter armPropertyParameter)
        {
            jObject.ReplacePropertyValueWithParameter(armPropertyParameter.ArmTemplatePropertyName, armPropertyParameter);
        }

        public static void ReplacePropertyValueWithParameter(this JObject jObject, string propertyName, ArmPropertyParameter armPropertyParameter)
        {
            var paramterString = $"[parameters(\'{armPropertyParameter.ParameterName}\')]";
            jObject.UpdatePropertyValue(propertyName, paramterString);
        }
    }
}
