namespace ManicStreetCoder.AzureDashform.Windows.UI.Model
{
    using System;

    public class DashboardArmTemplate
    {
        protected DashboardArmTemplate(string templateJson)
        {
            if (string.IsNullOrWhiteSpace(templateJson))
            {
                throw new ArgumentNullException(nameof(templateJson));
            }

            TemplateJson = templateJson;
        }

        public string TemplateJson { get; }
    }
}