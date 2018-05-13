namespace ManicStreetCoder.AzureDashform.Windows.UI.Service
{
    using Model;

    public interface ITransformationFileService
    {
        InputDashboardArmTemplate GetInputDashboardArmTemplate(TransformationDetails transformationDetails);

        void SaveOutputDashboardArmTemplate(OutputDashboardArmTemplate outputDashboardArmTemplate, TransformationDetails transformationDetails);
    }
}