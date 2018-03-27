namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON.Transformers
{
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;

    internal class ArmTemplate
    {
        public ArmTemplate(JObject json)
        {
            this.Json = json;
            this.AdditionalParameterNames = new List<string>();
        }

        public List<string> AdditionalParameterNames { get; }

        public JObject Json { get; private set; }

        public void ReplaceJson(JObject json)
        {
            this.Json = json;
        }
    }

    internal abstract class ArmTemplateTransformer
    {
        private readonly ArmTemplateTransformer nextTransformer;

        protected ArmTemplateTransformer(ArmTemplateTransformer nextTransformer)
        {
            this.nextTransformer = nextTransformer;
        }

        public ArmTemplate Transform(ArmTemplate armTemplate)
        {
            armTemplate = TransformInner(armTemplate);
            return this.nextTransformer != null ? this.nextTransformer.Transform(armTemplate) : armTemplate;
        }

        protected abstract ArmTemplate TransformInner(ArmTemplate armTemplate);
    }
}