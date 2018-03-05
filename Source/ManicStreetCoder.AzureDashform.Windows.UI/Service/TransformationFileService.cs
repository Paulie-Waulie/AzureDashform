namespace ManicStreetCoder.AzureDashform.Windows.UI.Service
{
    using Model;

    public class TransformationFileService : ITransformationFileService
    {
        public InputDashboardArmTemplate GetInputDashboardArmTemplate(string sourceInputFilePath)
        {
            return new InputDashboardArmTemplate("Some JSON");
        }

        public void SaveOutputDashboardArmTemplate(OutputDashboardArmTemplate outputDashboardArmTemplate, string outputFilePath)
        {
            
        }
    }
}