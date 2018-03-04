using System.Collections.Generic;

namespace ManicStreetCoder.AzureDashform.Windows.UI.ViewModel.Validation
{
    public class ValidationError
    {
        public ValidationError(string validationMessage)
        {
            ValidationMessage = validationMessage;
        }

        public string ValidationMessage { get; }

        public override bool Equals(object obj)
        {
            return obj is ValidationError error && ValidationMessage == error.ValidationMessage;
        }

        public override int GetHashCode()
        {
            return -755278967 + EqualityComparer<string>.Default.GetHashCode(ValidationMessage);
        }
    }
}
