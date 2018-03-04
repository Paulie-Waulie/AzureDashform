namespace ManicStreetCoder.AzureDashform.Windows.UI.Validation
{
    using System;
    using FluentValidation;
    using GalaSoft.MvvmLight.Ioc;

    public class ValidatorFactory : ValidatorFactoryBase
    {
        public override IValidator CreateInstance(Type validatorType)
        {
            return SimpleIoc.Default.GetInstance(validatorType) as IValidator;
        }
    }
}
