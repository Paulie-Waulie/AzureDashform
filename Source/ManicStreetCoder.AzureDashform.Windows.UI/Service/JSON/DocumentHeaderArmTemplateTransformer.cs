namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON
{
    using Newtonsoft.Json.Linq;

    internal class DocumentHeaderArmTemplateTransformer : ArmTemplateTransformer
    {
        public DocumentHeaderArmTemplateTransformer(ArmTemplateTransformer nextTransformer) : base(nextTransformer)
        {
        }

        protected override JObject TransformInner(JObject inputJson)
        {
            inputJson.AddFirst(new JProperty(ArmPropertyNameConstants.Variables, new JObject()));
            inputJson.AddFirst(BuildTemplateParameters());
            inputJson.AddFirst(new JProperty(ArmPropertyNameConstants.ContentVersion, ArmPropertyValueConstants.ContentVersion));
            inputJson.AddFirst(new JProperty("$schema", ArmPropertyValueConstants.TemplateSchema));

            return inputJson;
        }

        private static JProperty BuildTemplateParameters()
        {
            return new JProperty(ArmPropertyNameConstants.Parameters, new JObject(
                CreateParameterProperty(ArmParameterProperty.AppinsightsName),
                CreateParameterProperty(ArmParameterProperty.DashboardName),
                CreateParameterProperty(ArmParameterProperty.DashboardDisplayName),
                CreateParameterProperty(ArmParameterProperty.ResourceGroupName),
                CreateParameterProperty(ArmParameterProperty.SubscriptionId)
            ));
        }

        private static JProperty CreateParameterProperty(ArmParameterProperty parameter)
        {
            return new JProperty(parameter.ParameterName, new JObject(new JProperty("type", "string")));
        }
    }
}