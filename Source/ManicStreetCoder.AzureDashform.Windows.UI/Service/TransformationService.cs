namespace ManicStreetCoder.AzureDashform.Windows.UI.Service
{
    using JSON;
    using ManicStreetCoder.AzureDashform.Windows.UI.Model;

    public class TransformationService : ITransformationService
    {
        public OutputDashboardArmTemplate Transform(InputDashboardArmTemplate inputTemplate, TransformationDetails transformationDetails)
        {
            var armTemplate = DashboardJsonTemplateTransformationService.TransformTemplate(inputTemplate);
            var parametersJson = DashboardJsonParametersService.CreateParameters(armTemplate.AdditionalParameterNames);
            var outputJson = JsonWriter.CreateOutputJsonWithFormatting(armTemplate.Json);
            return new OutputDashboardArmTemplate(outputJson, parametersJson);
        }
    }
}
