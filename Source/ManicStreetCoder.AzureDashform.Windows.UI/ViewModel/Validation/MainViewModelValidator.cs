namespace ManicStreetCoder.AzureDashform.Windows.UI.ViewModel.Validation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using AzureDashform.ViewModel;

    public class MainViewModelValidator
    {
        public ValidationResult Validate(MainViewModel viewModel)
        {
            var errors = new List<ValidationError>();

            if (IsInValidJsonFilePath(viewModel.SourceFilePath))
            {
                errors.Add(new ValidationError("Please provide a valid input source file path."));
            }

            if (IsFilePathInvalid(viewModel.OutputFolderPath))
            {
                errors.Add(new ValidationError("Please provide a valid output file path."));
            }

            return new ValidationResult(errors);
        }

        private static bool IsFilePathInvalid(string filePath)
        {
            try
            {
                return string.IsNullOrWhiteSpace(filePath) ||
                    !Path.IsPathRooted(filePath);
            }
            catch (ArgumentException)
            {
                return true;
            }
        }

        private static bool IsInValidJsonFilePath(string filePath)
        {
            return IsFilePathInvalid(filePath) || !filePath.EndsWith(".json");
        }
    }
}