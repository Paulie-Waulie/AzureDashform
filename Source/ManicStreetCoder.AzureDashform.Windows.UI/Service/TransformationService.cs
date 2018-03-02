namespace ManicStreetCoder.AzureDashform.Windows.UI.Service
{
    using System;
    using Model;

    public class TransformationService : ITransformationService
    {
        public OutputDashboardArmTemplate Transform(InputDashboardArmTemplate inputTemplate)
        {
            return new OutputDashboardArmTemplate("Some output JSON");
        }
    }
}
