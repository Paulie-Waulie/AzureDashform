namespace ManicStreetCoder.AzureDashform.Windows.UI.Service
{
    using Model;

    public class TransformationFileService : ITransformationFileService
    {
        public InputDashboardArmTemplate GetInputDashboardArmTemplate(TransformationDetails transformationDetails)
        {
            return new InputDashboardArmTemplate("Some JSON");
        }

        public void SaveOutputDashboardArmTemplate(OutputDashboardArmTemplate outputDashboardArmTemplate)
        {
            
        }
    }
}