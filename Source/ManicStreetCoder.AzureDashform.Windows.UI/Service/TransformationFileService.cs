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
            Directory.CreateDirectory(outputFilePath);

            File.WriteAllText($@"{outputFilePath}\dashboard.json", outputDashboardArmTemplate.TemplateJson);
            File.WriteAllText($@"{outputFilePath}\parameters.json", outputDashboardArmTemplate.ParametersJson);
        }
    }
}