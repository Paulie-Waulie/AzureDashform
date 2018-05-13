namespace ManicStreetCoder.AzureDashform.Windows.UI.Service
{
    using Model;

    public interface ITransformationService
    {
        OutputDashboardArmTemplate Transform(InputDashboardArmTemplate inputTemplateParameters, TransformationDetails transformationDetails);
    }
}