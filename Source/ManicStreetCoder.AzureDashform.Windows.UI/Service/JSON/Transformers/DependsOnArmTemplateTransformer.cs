namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON.Transformers
{
    using Arm;
    using Newtonsoft.Json.Linq;

    internal class DependsOnArmTemplateTransformer : ArmTemplateTransformer
    {
        public DependsOnArmTemplateTransformer(ArmTemplateTransformer nextTransformer) : base(nextTransformer)
        {
        }

        protected override ArmTemplate TransformInner(ArmTemplate armTemplate, IArmPropertyValueResolver armPropertyValueResolver)
        {
            var dependencies = new JArray();
            dependencies.Add(WrapVariableSelector(armPropertyValueResolver.GetValue(ArmTemplateDynamicProperty.AppInsightsName)));
            foreach (var additionalResource in armTemplate.AdditionalResourceNames)
            {
                dependencies.Add(WrapVariableSelector(armPropertyValueResolver.GetValue(additionalResource)));
            }

            armTemplate.Json.Add(new JProperty("dependsOn", dependencies));
            return armTemplate;
        }

        private static string WrapVariableSelector(string variableSelector)
        {
            return $"[{variableSelector}]";
        }
    }
}