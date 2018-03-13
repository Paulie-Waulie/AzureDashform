namespace ManicStreetCoder.AzureDashform.Windows.UI.Model
{
    public class OutputDashboardArmTemplate : DashboardArmTemplate
    {
        public OutputDashboardArmTemplate(string templateJson, string parametersJson) : base(templateJson)
        {
            this.ParametersJson = parametersJson;
        }

        public string ParametersJson { get; }
    }
}