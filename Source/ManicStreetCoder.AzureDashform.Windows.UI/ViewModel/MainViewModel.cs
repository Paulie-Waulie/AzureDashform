namespace ManicStreetCoder.AzureDashform.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using AzureDashform.Windows.UI.Model;
    using AzureDashform.Windows.UI.Service;
    using AzureDashform.Windows.UI.ViewModel.Validation;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class MainViewModel : ViewModelBase
    {
        private ITransformationService transformationService;
        private ITransformationFileService transformationFileService;
        private TransformationDetails details = new TransformationDetails();

        public MainViewModel(ITransformationFileService fileService, ITransformationService transformationService)
        {
            this.details = new TransformationDetails(@"C:\Folder\Filee.json");
            this.ValidationErrors = new ObservableCollection<ValidationError>();
            this.transformationFileService = fileService;
            this.transformationService = transformationService;
        }

        public RelayCommand TransformCommand => new RelayCommand(this.Transform);

        public TransformationDetails Details => details;

        public ObservableCollection<ValidationError> ValidationErrors { get; }

        private void Transform()
        {
            if (this.IsValid())
            {

                var inputTemplate = this.transformationFileService.GetInputDashboardArmTemplate(this.Details);
                var outputTemplate = this.transformationService.Transform(inputTemplate);

                this.transformationFileService.SaveOutputDashboardArmTemplate(outputTemplate);
            }
        }

        private bool IsValid()
        {
            this.ValidationErrors.Clear();
            this.RaisePropertyChanged(nameof(ValidationErrors));


            if (string.IsNullOrWhiteSpace(this.details.SourceFilePath))
            {
                this.ValidationErrors.Add(new ValidationError("Please provide a valid input source file path."));
            }

            if (string.IsNullOrWhiteSpace(this.details.OutputFilePath))
            {
                this.ValidationErrors.Add(new ValidationError("Please provide a valid output file path."));
            }

            return !this.ValidationErrors.Any();
        }
    }
}