namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON
{
    using System;
    using Exceptions;
    using Model;
    using Newtonsoft.Json.Linq;
    using Transformers;

    internal static class DashboardJsonTemplateTransformationService
    {
        internal static ArmTemplate TransformTemplate(InputDashboardArmTemplate inputTemplate)
        {
            try
            {
                var inputJson = JObject.Parse(inputTemplate.TemplateJson);
                var armTemplate = new ArmTemplate(inputJson);
                ArmTemplateTransformer rootTransformer = CreateTransformer();
                rootTransformer.Transform(armTemplate);

                return armTemplate;
            }
            catch (Exception e)
            {
                throw new InvalidInputTemplateException(e);
            }
        }

        private static ArmTemplateTransformer CreateTransformer()
        {
            return new RootObjectArmTemplateTransformer(
                new ValueToParameterArmTemplateTransformer(
                    new ResourceIdArmTemplateTransformer(
                    new DocumentHeaderArmTemplateTransformer(null))));
        }
    }
}
