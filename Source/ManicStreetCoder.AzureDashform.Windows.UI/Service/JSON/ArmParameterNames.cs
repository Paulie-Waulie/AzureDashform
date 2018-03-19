namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON
{
    internal class ArmParameterProperty
    {
        public static ArmParameterProperty SubscriptionId = new ArmParameterProperty("SubscriptionId", "subscriptionId", "#{AzureSubscriptionId}");
        public static ArmParameterProperty ResourceGroupName = new ArmParameterProperty("ResourceGroup", "nameOfResourceGroup", "#{ResourceGroup.Name}");
        public static ArmParameterProperty AppinsightsName = new ArmParameterProperty("Name", "appInsightsName", "#{ApplicationInsights.Name}");
        public static ArmParameterProperty DashboardName = new ArmParameterProperty("name", "dashboardName", "#{Dashboard.Name}");
        public static ArmParameterProperty DashboardDisplayName = new ArmParameterProperty("hidden-title", "dashboardDisplayName", "#{Dashboard.DisplayName}");

        private ArmParameterProperty(string armTemplatePropertyName, string parameterName, string parameterValue)
        {
            this.ArmTemplatePropertyName = armTemplatePropertyName;
            this.ParameterName = parameterName;
            this.ParameterValue = parameterValue;
        }
        
        public string ArmTemplatePropertyName { get; }

        public string ParameterName { get; }

        public string ParameterValue { get; }
    }
}