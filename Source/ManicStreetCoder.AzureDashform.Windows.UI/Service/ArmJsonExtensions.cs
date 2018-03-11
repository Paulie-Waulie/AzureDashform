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

        public static JToken GetArmValueToken(this JObject armObject, string tokenName)
        {
            return armObject.SelectToken(tokenName);
        }

        public static JToken GetArmValueToken(this JObject armObject)
        {
            return armObject.GetArmValueToken("value");
        }

        public static JValue GetArmValueTokenValue(this JObject armObject, string tokenName)
        {
            return (JValue)armObject.GetArmValueToken(tokenName);
        }

        public static JValue GetArmValueTokenValue(this JObject armObject)
        {
            return (JValue)armObject.GetArmValueToken();
        }

        public static JObject GetArmValueTokenObject(this JObject armObject)
        {
            return (JObject)armObject.GetArmValueToken();
        }

        public static void UpdatePropertyValue(this JObject jObject, string propertyName, string value)
        {
            jObject.Property(propertyName).Value = value;
        }

        public static void UpdateValueTokenValue(this JValue jValue, string value)
        {
            jValue.Value = value;
        }
    }
}
