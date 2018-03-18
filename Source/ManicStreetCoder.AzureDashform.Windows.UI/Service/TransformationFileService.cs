namespace ManicStreetCoder.AzureDashform.Windows.UI.Service
{
    using System.IO;
    using Model;

    public class TransformationFileService : ITransformationFileService
    {
        public InputDashboardArmTemplate GetInputDashboardArmTemplate(string sourceInputFilePath)
        {
            return new InputDashboardArmTemplate(File.ReadAllText(sourceInputFilePath));
        }

        public void SaveOutputDashboardArmTemplate(OutputDashboardArmTemplate outputDashboardArmTemplate, string outputFilePath)
        {
            File.WriteAllText(outputFilePath, outputDashboardArmTemplate.TemplateJson);
            File.WriteAllText(@"C:\\temp\parameters.json", outputDashboardArmTemplate.ParametersJson);
        }
    }
}