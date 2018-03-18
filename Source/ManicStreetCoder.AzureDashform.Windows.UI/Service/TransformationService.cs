namespace ManicStreetCoder.AzureDashform.Windows.UI.Service
{
    using JSON;
    using ManicStreetCoder.AzureDashform.Windows.UI.Model;

    public class TransformationService : ITransformationService
    {
        public OutputDashboardArmTemplate Transform(InputDashboardArmTemplate inputTemplate)
        {
            var outputJson = DashboardJsonTemplateTransformationService.TransformTemplate(inputTemplate);
            var parametersJson = DashboardJsonParametersService.CreateParameters();
            return new OutputDashboardArmTemplate(outputJson, parametersJson);
        }
    }
}
