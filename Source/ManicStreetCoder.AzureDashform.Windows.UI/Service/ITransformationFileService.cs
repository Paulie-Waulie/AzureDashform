namespace ManicStreetCoder.AzureDashform.Windows.UI.Service
{
    using Model;

    public interface ITransformationFileService
    {
        InputDashboardArmTemplate GetInputDashboardArmTemplate(string sourceInputFilePath);

        void SaveOutputDashboardArmTemplate(OutputDashboardArmTemplate outputDashboardArmTemplate, string outputFilePath);
    }
}