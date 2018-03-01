namespace ManicStreetCoder.AzureDashform.Windows.UI.Model
{
    public class DashboardArmTemplate
    {
        protected DashboardArmTemplate(string templateJson)
        {
            TemplateJson = templateJson;
        }

        public string TemplateJson { get; }
    }
}