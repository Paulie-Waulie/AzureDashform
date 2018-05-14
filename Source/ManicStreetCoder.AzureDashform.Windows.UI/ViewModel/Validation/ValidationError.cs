namespace ManicStreetCoder.AzureDashform.Windows.UI.ViewModel.Validation
{
    using System.Collections.Generic;

    public class ValidationError
    {
        public ValidationError(string validationMessage)
        {
            this.ValidationMessage = validationMessage;
        }

        public string ValidationMessage { get; }

        public override bool Equals(object obj)
        {
            return obj is ValidationError error && this.ValidationMessage == error.ValidationMessage;
        }

        public override int GetHashCode()
        {
            return -755278967 + EqualityComparer<string>.Default.GetHashCode(this.ValidationMessage);
        }
    }
}
