namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON
{
    using Newtonsoft.Json.Linq;

    internal class RootObjectArmTemplateTransformer : ArmTemplateTransformer
    {
        public RootObjectArmTemplateTransformer(ArmTemplateTransformer nextTransformer) : base(nextTransformer)
        {
        }

        protected override JObject TransformInner(JObject inputJson)
        {
            var resources = new JArray();
            var properties = inputJson.Properties();
            var resourcesObject = new JObject();
            foreach (var property in properties)
            {
                resourcesObject.Add(property);
            }

            resources.Add(resourcesObject);
            return new JObject(new JProperty("resources", resources));
        }
    }
}