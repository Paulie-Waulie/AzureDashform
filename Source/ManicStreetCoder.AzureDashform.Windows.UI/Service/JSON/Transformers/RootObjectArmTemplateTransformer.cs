namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON.Transformers
{
    using Newtonsoft.Json.Linq;

    internal class RootObjectArmTemplateTransformer : ArmTemplateTransformer
    {
        public RootObjectArmTemplateTransformer(ArmTemplateTransformer nextTransformer) : base(nextTransformer)
        {
        }

        protected override ArmTemplate TransformInner(ArmTemplate armTemplate)
        {
            var resources = new JArray();
            var properties = armTemplate.Json.Properties();
            var resourcesObject = new JObject();
            foreach (var property in properties)
            {
                resourcesObject.Add(property);
            }

            resources.Add(resourcesObject);
            armTemplate.ReplaceJson(new JObject(new JProperty("resources", resources)));
            return armTemplate;
        }
    }
}