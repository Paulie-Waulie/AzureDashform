namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON
{
    using Newtonsoft.Json.Linq;

    public static class DashboardJsonParametersService
    {
        public static string CreateParameters()
        {
            var rootObject = new JObject();
            rootObject.Add(new JProperty(ArmPropertyNameConstants.Schema, ArmPropertyValueConstants.ParametersSchema));
            rootObject.Add(new JProperty(ArmPropertyNameConstants.ContentVersion, ArmPropertyValueConstants.ContentVersion));
            rootObject.Add(new JProperty(ArmPropertyNameConstants.Parameters, new JObject(
                CreateParameterProperty(ArmParameterProperty.ResourceGroupName),
                CreateParameterProperty(ArmParameterProperty.SubscriptionId),
                CreateParameterProperty(ArmParameterProperty.AppinsightsName),
                CreateParameterProperty(ArmParameterProperty.DashboardName),
                CreateParameterProperty(ArmParameterProperty.DashboardDisplayName)
            )));

            return JsonWriter.CreateOutputJsonWithFormatting(rootObject);
        }

        private static JProperty CreateParameterProperty(ArmParameterProperty parameter)
        {
            return new JProperty(parameter.ParameterName, new JObject(new JProperty("value", parameter.ParameterValue)));
        }
    }
}