namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON
{
    using System;
    using Exceptions;
    using Model;
    using Newtonsoft.Json.Linq;

    internal static class DashboardJsonTemplateTransformationService
    {
        internal static string TransformTemplate(InputDashboardArmTemplate inputTemplate)
        {
            try
            {
                var inputJson = JObject.Parse(inputTemplate.TemplateJson);
                ArmTemplateTransformer rootTransformer = CreateTransformer();
                inputJson = rootTransformer.Transform(inputJson);

                return JsonWriter.CreateOutputJsonWithFormatting(inputJson);
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
                    new DocumentHeaderArmTemplateTransformer(null)));
        }
    }
}
