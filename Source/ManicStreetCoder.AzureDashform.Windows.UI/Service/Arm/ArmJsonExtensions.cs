namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.Arm
{
    using System.Collections.Generic;
    using System.Linq;
    using JSON;
    using Newtonsoft.Json.Linq;

    internal static class ArmJsonExtensions
    {
        public static JObject GetObjectByName(this IJEnumerable<JToken> objects, string name)
        {
            return (JObject)objects.SingleOrDefault(x => ((JValue)x.SelectToken("name")).Value<string>().Equals(name));
        }

        public static JProperty GetProperty(this JObject jObject, string propertyName)
        {
            return jObject.Property(propertyName);
        }

        public static IEnumerable<JProperty> GetAllChildProperties(this JObject jObject, string propertyName)
        {
            return jObject.Descendants().OfType<JProperty>().Where(x => x.Name.Equals(propertyName));
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

        public static void ReplacePropertyValueWith(this JObject jObject, ArmTemplateDynamicProperty armPropertyParameter, IArmPropertyValueResolver armPropertyValueResolver)
        {
            jObject.ReplacePropertyValueWith(armPropertyParameter.ArmTemplatePropertyName, armPropertyParameter, armPropertyValueResolver);
        }

        public static void ReplacePropertyValueWith(this JObject jObject, string propertyName, ArmTemplateDynamicProperty armPropertyParameter, IArmPropertyValueResolver armPropertyValueResolver)
        {
            jObject.UpdatePropertyValue(propertyName, $"[{armPropertyValueResolver.GetValue(armPropertyParameter)}]");
        }
    }
}
