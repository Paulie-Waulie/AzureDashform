namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON
{
    using System;
    using Arm;
    using Exceptions;
    using Model;
    using Newtonsoft.Json.Linq;
    using Transformers;

    internal static class DashboardJsonTemplateTransformationService
    {
        internal static ArmTemplate TransformTemplate(InputDashboardArmTemplate inputTemplate, TransformationDetails transformationDetails)
        {
            try
            {
                var inputJson = JObject.Parse(inputTemplate.TemplateJson);
                var armTemplate = new ArmTemplate(inputJson);
                ArmTemplateTransformer rootTransformer = CreateTransformer(transformationDetails);
                IArmPropertyValueResolver armPropertyValueResolver = CreatePropertyValueResolver(transformationDetails);
                rootTransformer.Transform(armTemplate, armPropertyValueResolver);

                return armTemplate;
            }
            catch (Exception e)
            {
                throw new InvalidInputTemplateException(e);
            }
        }

        private static IArmPropertyValueResolver CreatePropertyValueResolver(TransformationDetails transformationDetails)
        {
            if (transformationDetails.DashboardIsCompleteTemplate)
            {
                return new ArmParameterPropertyValueResolver();
            }

            return new ArmTemplatePropertyValueResolver();
        }

        private static ArmTemplateTransformer CreateTransformer(TransformationDetails transformationDetails)
        {
            return (transformationDetails.DashboardIsCompleteTemplate) ? CreateCompleteTemplateTransformer() : CreatePartialTemplateTransformer();
        }

        private static ArmTemplateTransformer CreateCompleteTemplateTransformer()
        {
            return new ValueToParameterArmTemplateTransformer(
                    new RootObjectArmTemplateTransformer(
                    new ResourceIdArmTemplateTransformer(
                    new DocumentHeaderArmTemplateTransformer(null))));
        }

        private static ArmTemplateTransformer CreatePartialTemplateTransformer()
        {
            return new ValueToParameterArmTemplateTransformer(
                new ResourceIdArmTemplateTransformer(
                    new VariablesArmTemplateTransformer(null)));
        }
    }
}
