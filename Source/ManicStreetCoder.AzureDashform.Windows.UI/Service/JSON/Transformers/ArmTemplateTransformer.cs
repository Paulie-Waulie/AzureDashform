namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON.Transformers
{
    using System.Collections.Generic;
    using Arm;
    using Newtonsoft.Json.Linq;

    internal class ArmTemplate
    {
        public ArmTemplate(JObject json)
        {
            this.Json = json;
            this.AdditionalResourceNames = new List<string>();
        }

        public List<string> AdditionalResourceNames { get; }

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

        public ArmTemplate Transform(ArmTemplate armTemplate, IArmPropertyValueResolver armPropertyValueResolver)
        {
            armTemplate = this.TransformInner(armTemplate, armPropertyValueResolver);
            return this.nextTransformer != null ? this.nextTransformer.Transform(armTemplate, armPropertyValueResolver) : armTemplate;
        }

        protected abstract ArmTemplate TransformInner(ArmTemplate armTemplate, IArmPropertyValueResolver armPropertyValueResolver);
    }
}