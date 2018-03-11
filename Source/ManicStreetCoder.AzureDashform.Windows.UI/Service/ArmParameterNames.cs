namespace ManicStreetCoder.AzureDashform.Windows.UI.Service
{
    public static class ArmParameterNames
    {
        public const string SubscriptionId = "subscriptionId";
        public const string ResourceGroupName = "resourceGroupName";
        public const string AppinsightsName = "appinsightsName";
        public const string DashboardName = "dashboardName";
        public const string DashboardDisplayName = "dashboardDisplayName";
    }

    public class ArmPropertyParameter
    {
        public static ArmPropertyParameter SubscriptionId = new ArmPropertyParameter("subscriptionId");
        public static ArmPropertyParameter ResourceGroupName = new ArmPropertyParameter("resourceGroupName", "ResourceGroup");
        public static ArmPropertyParameter AppinsightsName = new ArmPropertyParameter("appinsightsName", "Name");
        public static ArmPropertyParameter DashboardName = new ArmPropertyParameter("dashboardName", "name");
        public static ArmPropertyParameter DashboardDisplayName = new ArmPropertyParameter("dashboardDisplayName", "hidden-title");

        private ArmPropertyParameter(string parameterName)
        {
            ParameterName = parameterName;
            var propertyName = $"{parameterName.Substring(0, 1).ToUpper()}{parameterName.Substring(1)}";
            ArmTemplatePropertyName = propertyName;
        }
        
        private ArmPropertyParameter(string parameterName, string armTemplatePropertyName)
        {
            ParameterName = parameterName;
            ArmTemplatePropertyName = armTemplatePropertyName;
        }

        public string ParameterName { get; }

        public string ArmTemplatePropertyName { get; }
    }
}