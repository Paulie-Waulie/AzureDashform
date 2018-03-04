namespace ManicStreetCoder.AzureDashform.Windows.UI.ViewModel.Validation
{
    using System.Collections.Generic;
    using AzureDashform.ViewModel;

    public class MainViewModelValidator
    {
        public ValidationResult Validate(MainViewModel viewModel)
        {
            var errors = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(viewModel.Details.SourceFilePath))
            {
                errors.Add(new ValidationError("Please provide a valid input source file path."));
            }

            if (string.IsNullOrWhiteSpace(viewModel.Details.OutputFilePath))
            {
                errors.Add(new ValidationError("Please provide a valid output file path."));
            }

            return new ValidationResult(errors);
        }
    }
}