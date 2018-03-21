namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON.Transformers
{
    using System.Text.RegularExpressions;
    using Newtonsoft.Json.Linq;

    internal class ResourceIdArmTemplateTransformer : ArmTemplateTransformer
    {
        public ResourceIdArmTemplateTransformer(ArmTemplateTransformer nextTransformer) : base(nextTransformer)
        {
        }

        protected override JObject TransformInner(JObject inputJson)
        {
            var resourceIds = inputJson.GetAllChildProperties("resourceId");
            foreach (var resourceId in resourceIds)
            {
                var value = resourceId.Value.Value<string>();

                resourceId.Value = ReplaceResourceIdPart(resourceId, "subscriptions", ArmParameterProperty.SubscriptionId);
                resourceId.Value = ReplaceResourceIdPart(resourceId, "resourceGroups", ArmParameterProperty.ResourceGroupName);
            }

            return inputJson;
        }

        private static string ReplaceResourceIdPart(JProperty resourceId, string pathParth, ArmParameterProperty armParameterProperty)
        {
            return Regex.Replace(resourceId.Value.Value<string>(), $@"(?<=\/{pathParth})\/(.+?)\/", $@"/{armParameterProperty.ArmParameterSelector}/");
        }
    }
}