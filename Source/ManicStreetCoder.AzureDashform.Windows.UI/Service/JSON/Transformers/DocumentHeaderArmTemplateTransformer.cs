namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON.Transformers
{
    using Newtonsoft.Json.Linq;

    internal class DocumentHeaderArmTemplateTransformer : ArmTemplateTransformer
    {
        public DocumentHeaderArmTemplateTransformer(ArmTemplateTransformer nextTransformer) : base(nextTransformer)
        {
        }   

        protected override ArmTemplate TransformInner(ArmTemplate armTemplate)
        {
            armTemplate.Json.AddFirst(new JProperty(ArmPropertyNameConstants.Variables, new JObject()));
            armTemplate.Json.AddFirst(BuildTemplateParameters(armTemplate));
            armTemplate.Json.AddFirst(new JProperty(ArmPropertyNameConstants.ContentVersion, ArmPropertyValueConstants.ContentVersion));
            armTemplate.Json.AddFirst(new JProperty("$schema", ArmPropertyValueConstants.TemplateSchema));

            return armTemplate;
        }

        private static JProperty BuildTemplateParameters(ArmTemplate armTemplate)
        {
            var parameters = new JObject(
                CreateParameterProperty(ArmParameterProperty.AppinsightsName),
                CreateParameterProperty(ArmParameterProperty.DashboardName),
                CreateParameterProperty(ArmParameterProperty.DashboardDisplayName),
                CreateParameterProperty(ArmParameterProperty.ResourceGroupName),
                CreateParameterProperty(ArmParameterProperty.SubscriptionId)
            );

            foreach (var additionalParameter in armTemplate.AdditionalParameterNames)
            {
                parameters.Add(CreateParameterProperty(additionalParameter));
            }

            return new JProperty(ArmPropertyNameConstants.Parameters, parameters);
        }

        private static JProperty CreateParameterProperty(ArmParameterProperty parameter)
        {
            return new JProperty(parameter.ParameterName, CreateParameter());
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