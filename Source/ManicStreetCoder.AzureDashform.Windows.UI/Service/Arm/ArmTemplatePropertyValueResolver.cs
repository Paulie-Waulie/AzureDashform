namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.Arm
{
    internal class ArmTemplatePropertyValueResolver : IArmPropertyValueResolver
    {
        public string GetValue(ArmTemplateDynamicProperty property)
        {
            if (property.Equals(ArmTemplateDynamicProperty.SubscriptionId))
            {
                return "subscription().subscriptionId";
            }

            if (property.Equals(ArmTemplateDynamicProperty.ResourceGroupName))
            {
                return "resourceGroup().name";
            }

            return GetVariableSelector(property.DynamicValue);
        }

        public string GetValue(string propertyName)
        {
            return GetVariableSelector(propertyName);
        }

        private static string GetVariableSelector(string variable)
        {
            return $"variables(\'{variable}\')";
        }
    }
}