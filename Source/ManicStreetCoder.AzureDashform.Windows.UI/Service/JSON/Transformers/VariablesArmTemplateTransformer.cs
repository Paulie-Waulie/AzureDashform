namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON.Transformers
{
    using Arm;
    using Newtonsoft.Json.Linq;

    internal class VariablesArmTemplateTransformer : ArmTemplateTransformer
    {
        public VariablesArmTemplateTransformer(ArmTemplateTransformer nextTransformer) : base(nextTransformer)
        {
        }

        protected override ArmTemplate TransformInner(ArmTemplate armTemplate, IArmPropertyValueResolver armPropertyValueResolver)
        {
            JObject variables = CreateVariablesObject(armTemplate);
            armTemplate.Json.AddFirst(new JProperty(ArmTemplatePropertyNameConstants.Variables, variables));

            return armTemplate;
        }

        private static JObject CreateVariablesObject(ArmTemplate armTemplate)
        {
            var parametersObject = new JObject(
                CreateParameterProperty(ArmTemplateDynamicProperty.AppInsightsName.DynamicValue),
                CreateParameterProperty(ArmTemplateDynamicProperty.DashboardName.DynamicValue),
                CreateParameterProperty(ArmTemplateDynamicProperty.DashboardDisplayName.DynamicValue));

            AddAdditionalParameters(parametersObject, armTemplate);

            return parametersObject;
        }

        private static void AddAdditionalParameters(JObject parametersObject, ArmTemplate armTemplate)
        {
            foreach (var additionalResource in armTemplate.AdditionalResourceNames)
            {
                parametersObject.Add(CreateParameterProperty(additionalResource));
            }
        }

        private static JProperty CreateParameterProperty(string variable)
        {
            return new JProperty(variable, "TO_BE_SET");
        }
    }
}