namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.Arm
{
    internal class ArmTemplateDynamicProperty
    {
        public static ArmTemplateDynamicProperty SubscriptionId = new ArmTemplateDynamicProperty(ArmTemplatePropertyNameConstants.SubscriptionId, "subscriptionId");
        public static ArmTemplateDynamicProperty ResourceGroupName = new ArmTemplateDynamicProperty(ArmTemplatePropertyNameConstants.ResourceGroup, "resourceGroup-name");
        public static ArmTemplateDynamicProperty AppInsightsName = new ArmTemplateDynamicProperty(ArmTemplatePropertyNameConstants.AppInsightsName, "appInsights-name");
        public static ArmTemplateDynamicProperty DashboardName = new ArmTemplateDynamicProperty(ArmTemplatePropertyNameConstants.DashboardName, "dashboard-name");
        public static ArmTemplateDynamicProperty DashboardDisplayName = new ArmTemplateDynamicProperty(ArmTemplatePropertyNameConstants.DashboardDisplayName, "dashboard-displayName");

        private ArmTemplateDynamicProperty(string armTemplateProperty, string dynamicValue)
        {
            this.ArmTemplatePropertyName = armTemplateProperty;
            this.DynamicValue = dynamicValue;
        }
        
        public string ArmTemplatePropertyName { get; }

        public string DynamicValue { get; }
    }
}