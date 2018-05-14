namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.Arm
{
    internal class ArmParameterPropertyValueResolver : IArmPropertyValueResolver
    {
        public string GetValue(ArmTemplateDynamicProperty property)
        {
            return GetParameterSelector(property.DynamicValue);
        }

        public string GetValue(string propertyName)
        {
            return GetParameterSelector(propertyName);
        }

        private static string GetParameterSelector(string variable)
        {
            return $"parameters(\'{variable}\')";
        }
    }
}