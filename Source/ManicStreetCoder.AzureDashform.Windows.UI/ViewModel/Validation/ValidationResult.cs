namespace ManicStreetCoder.AzureDashform.Windows.UI.ViewModel.Validation
{
    using System.Collections.Generic;
    using System.Linq;

    public class ValidationResult
    {
        public ValidationResult(IEnumerable<ValidationError> validationErrors)
        {
            this.ValidationErrors = validationErrors;
        }

        public IEnumerable<ValidationError> ValidationErrors { get; }

        public bool IsValid => !this.ValidationErrors.Any();
    }
}