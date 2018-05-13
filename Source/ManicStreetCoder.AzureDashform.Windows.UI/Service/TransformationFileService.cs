namespace ManicStreetCoder.AzureDashform.Windows.UI.Service
{
    using System.IO;
    using Model;

    public class TransformationFileService : ITransformationFileService
    {
        public InputDashboardArmTemplate GetInputDashboardArmTemplate(TransformationDetails transformationDetails)
        {
            return new InputDashboardArmTemplate(File.ReadAllText(transformationDetails.SourceFilePath));
        }

        public void SaveOutputDashboardArmTemplate(OutputDashboardArmTemplate outputDashboardArmTemplate, TransformationDetails transformationDetails)
        {
            Directory.CreateDirectory(transformationDetails.OutputFilePath);

            File.WriteAllText($@"{transformationDetails.OutputFilePath}\dashboard.json", outputDashboardArmTemplate.TemplateJson);
            File.WriteAllText($@"{transformationDetails.OutputFilePath}\parameters.json", outputDashboardArmTemplate.ParametersJson);
        }
    }
}