namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON.Transformers
{
    using Arm;
    using Newtonsoft.Json.Linq;

    internal class ValueToParameterArmTemplateTransformer : ArmTemplateTransformer
    {
        public ValueToParameterArmTemplateTransformer(ArmTemplateTransformer nextTransformer) : base(nextTransformer)
        {
        }

        protected override ArmTemplate TransformInner(ArmTemplate armTemplate, IArmPropertyValueResolver armPropertyValueResolver)
        {
            var properties = armTemplate.Json.SelectToken("properties");
            var parts = properties.SelectToken("lenses.0.parts");

            UpdateParts(parts, armPropertyValueResolver);
            UpdateTemplateMetadata(armTemplate.Json, armPropertyValueResolver);

            return armTemplate;
        }

        private static void UpdateParts(JToken parts, IArmPropertyValueResolver armPropertyValueResolver)
        {
            int partNumber = 0;

            while (true)
            {
                var part = parts.SelectToken(partNumber.ToString());
                if (part != null)
                {
                    var inputs = part.SelectToken("metadata.inputs").AsJEnumerable();

                    UpdateComponentIds(inputs, armPropertyValueResolver);
                    UpdatePartSubTitle(inputs, armPropertyValueResolver);
                    RemoveDashboardId(inputs);
                }
                else
                {
                    return;
                }

                partNumber++;
            }
        }

        private static void UpdateTemplateMetadata(JObject properties, IArmPropertyValueResolver armPropertyValueResolver)
        {
            RemoveTemplateId(properties);
            properties.ReplacePropertyValueWith(ArmTemplateDynamicProperty.DashboardName, armPropertyValueResolver);
            AddArmApiVersion(properties);
            properties.GetObject("tags").ReplacePropertyValueWith(ArmTemplateDynamicProperty.DashboardDisplayName, armPropertyValueResolver);
        }

        private static void AddArmApiVersion(JObject inputJson)
        {
            inputJson.GetProperty("type").AddAfterSelf(new JProperty("apiVersion", ArmPropertyValueConstants.ApiVersion));
        }

        private static void RemoveTemplateId(JObject inputJson)
        {
            inputJson.GetProperty("id")?.Remove();
        }

        private static void RemoveDashboardId(IJEnumerable<JToken> inputs)
        {
            inputs.GetObjectByName("DashboardId")?.Remove();
        }

        private static void UpdatePartSubTitle(IJEnumerable<JToken> inputs, IArmPropertyValueResolver armPropertyValueResolver)
        {
            inputs.GetObjectByName("PartSubTitle")?.ReplacePropertyValueWith("value", ArmTemplateDynamicProperty.AppInsightsName, armPropertyValueResolver);
        }

        private static void UpdateComponentIds(IJEnumerable<JToken> inputs, IArmPropertyValueResolver armPropertyValueResolver)
        {
            var componentIdInput = inputs.GetObjectByName("ComponentId")?.GetObject("value");
            if (componentIdInput != null)
            {
                componentIdInput.ReplacePropertyValueWith(ArmTemplateDynamicProperty.SubscriptionId, armPropertyValueResolver);
                componentIdInput.ReplacePropertyValueWith(ArmTemplateDynamicProperty.ResourceGroupName, armPropertyValueResolver);
                componentIdInput.ReplacePropertyValueWith(ArmTemplateDynamicProperty.AppInsightsName, armPropertyValueResolver);
            }
        }
    }
}