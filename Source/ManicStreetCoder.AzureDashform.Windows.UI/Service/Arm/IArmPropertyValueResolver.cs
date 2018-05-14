namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.Arm
{
    internal interface IArmPropertyValueResolver
    {
        string GetValue(ArmTemplateDynamicProperty property);

        string GetValue(string propertyName);
    }
}