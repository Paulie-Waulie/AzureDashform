namespace ManicStreetCoder.AzureDashform.Windows.UI.ViewModel.Validation
{
    using System.Collections.Generic;
    using AzureDashform.ViewModel;

    public class MainViewModelValidator
    {
        public ValidationResult Validate(MainViewModel viewModel)
        {
            var errors = new List<ValidationError>();

            if (IsFilePathInvalid(viewModel.Details.SourceFilePath))
            {
                errors.Add(new ValidationError("Please provide a valid input source file path."));
            }

            if (IsFilePathInvalid(viewModel.Details.OutputFilePath))
            {
                errors.Add(new ValidationError("Please provide a valid output file path."));
            }

            return new ValidationResult(errors);
        }

        private static bool IsFilePathInvalid(string filePath)
        {
            return string.IsNullOrWhiteSpace(filePath) || !filePath.EndsWith(".json");
        }
    }
}