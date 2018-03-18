namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON
{
    using Newtonsoft.Json.Linq;

    internal abstract class ArmTemplateTransformer
    {
        private readonly ArmTemplateTransformer nextTransformer;

        protected ArmTemplateTransformer(ArmTemplateTransformer nextTransformer)
        {
            this.nextTransformer = nextTransformer;
        }

        public JObject Transform(JObject inputJson)
        {
            inputJson = TransformInner(inputJson);
            return this.nextTransformer != null ? this.nextTransformer.Transform(inputJson) : inputJson;
        }

        protected abstract JObject TransformInner(JObject inputJson);
    }
}