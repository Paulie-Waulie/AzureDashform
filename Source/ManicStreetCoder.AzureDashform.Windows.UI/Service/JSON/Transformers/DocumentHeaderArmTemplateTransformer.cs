namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON.Transformers
{
    using Arm;
    using Newtonsoft.Json.Linq;

    internal class DocumentHeaderArmTemplateTransformer : ArmTemplateTransformer
    {
        public DocumentHeaderArmTemplateTransformer(ArmTemplateTransformer nextTransformer) : base(nextTransformer)
        {
        }   

        protected override ArmTemplate TransformInner(ArmTemplate armTemplate, IArmPropertyValueResolver armPropertyValueResolver)
        {
            armTemplate.Json.AddFirst(new JProperty(ArmTemplatePropertyNameConstants.Variables, new JObject()));
            armTemplate.Json.AddFirst(BuildTemplateParameters(armTemplate));
            armTemplate.Json.AddFirst(new JProperty(ArmTemplatePropertyNameConstants.ContentVersion, ArmPropertyValueConstants.ContentVersion));
            armTemplate.Json.AddFirst(new JProperty("$schema", ArmPropertyValueConstants.TemplateSchema));

            return armTemplate;
        }

        private static JProperty BuildTemplateParameters(ArmTemplate armTemplate)
        {
            var parameters = new JObject(
                CreateParameterProperty(ArmTemplateDynamicProperty.AppInsightsName),
                CreateParameterProperty(ArmTemplateDynamicProperty.DashboardName),
                CreateParameterProperty(ArmTemplateDynamicProperty.DashboardDisplayName),
                CreateParameterProperty(ArmTemplateDynamicProperty.ResourceGroupName),
                CreateParameterProperty(ArmTemplateDynamicProperty.SubscriptionId)
            );

            foreach (var additionalParameter in armTemplate.AdditionalResourceNames)
            {
                parameters.Add(CreateParameterProperty(additionalParameter));
            }

            return new JProperty(ArmTemplatePropertyNameConstants.Parameters, parameters);
        }

        private static JProperty CreateParameterProperty(ArmTemplateDynamicProperty parameter)
        {
            return new JProperty(parameter.DynamicValue, CreateParameter());
        }

        private static JProperty CreateParameterProperty(string parameter)
        {
            return new JProperty(parameter, CreateParameter());
        }

        private static JObject CreateParameter()
        {
            return new JObject(new JProperty("type", "string"));
        }
    }
}