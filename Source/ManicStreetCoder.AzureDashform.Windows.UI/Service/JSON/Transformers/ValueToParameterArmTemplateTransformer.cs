namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON.Transformers
{
    using Newtonsoft.Json.Linq;

    internal class ValueToParameterArmTemplateTransformer : ArmTemplateTransformer
    {
        public ValueToParameterArmTemplateTransformer(ArmTemplateTransformer nextTransformer) : base(nextTransformer)
        {
        }

        protected override ArmTemplate TransformInner(ArmTemplate armTemplate)
        {
            JObject resource = armTemplate.Json.GetObject("resources[0]");
            var parts = resource.SelectToken("properties.lenses.0.parts");

            UpdateParts(parts);
            UpdateTemplateMetadata(resource);

            return armTemplate;
        }

        private static void UpdateParts(JToken parts)
        {
            int partNumber = 0;

            while (true)
            {
                var part = parts.SelectToken(partNumber.ToString());
                if (part != null)
                {
                    var inputs = part.SelectToken("metadata.inputs").AsJEnumerable();

                    UpdateComponentIds(inputs);
                    UpdatePartSubTitle(inputs);
                    RemoveDashboardId(inputs);
                }
                else
                {
                    return;
                }

                partNumber++;
            }
        }

        private static void UpdateTemplateMetadata(JObject properties)
        {
            RemoveTemplateId(properties);
            properties.ReplacePropertyValueWithParameter(ArmParameterProperty.DashboardName);
            AddArmApiVersion(properties);
            properties.GetObject("tags").ReplacePropertyValueWithParameter(ArmParameterProperty.DashboardDisplayName);
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

        private static void UpdatePartSubTitle(IJEnumerable<JToken> inputs)
        {
            inputs.GetObjectByName("PartSubTitle")?.ReplacePropertyValueWithParameter("value", ArmParameterProperty.AppinsightsName);
        }

        private static void UpdateComponentIds(IJEnumerable<JToken> inputs)
        {
            var componentIdInput = inputs.GetObjectByName("ComponentId")?.GetObject("value");
            if (componentIdInput != null)
            {
                componentIdInput.ReplacePropertyValueWithParameter(ArmParameterProperty.SubscriptionId);
                componentIdInput.ReplacePropertyValueWithParameter(ArmParameterProperty.ResourceGroupName);
                componentIdInput.ReplacePropertyValueWithParameter(ArmParameterProperty.AppinsightsName);
            }
        }
    }
}