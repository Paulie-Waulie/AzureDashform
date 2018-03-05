namespace ManicStreetCoder.AzureDashform.ViewModel
{
    using System.Collections.ObjectModel;
    using AzureDashform.Windows.UI.Model;
    using AzureDashform.Windows.UI.Service;
    using AzureDashform.Windows.UI.ViewModel.Validation;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Views;

    public class MainViewModel : ViewModelBase
    {
        private ITransformationService transformationService;
        private ITransformationFileService transformationFileService;
        private TransformationDetails details = new TransformationDetails();

        public MainViewModel(ITransformationFileService fileService, ITransformationService transformationService)
        {
            this.details = new TransformationDetails(@"C:\Dashboard.json");
            this.ValidationErrors = new ObservableCollection<ValidationError>();
            this.transformationFileService = fileService;
            this.transformationService = transformationService;
        }

        public RelayCommand TransformCommand => new RelayCommand(this.Transform);

        public string SourceFilePath
        {
            get { return this.details.SourceFilePath; }
            set
            {
                this.details.SourceFilePath = value;
                this.RaisePropertyChanged(nameof(SourceFilePath));
            }
        }

        public string OutputFilePath
        {
            get { return this.details.OutputFilePath; }
            set
            {
                this.details.OutputFilePath = value;
                this.RaisePropertyChanged(nameof(OutputFilePath));
            }
        }

        public ObservableCollection<ValidationError> ValidationErrors { get; private set; }

        private void Transform()
        {
            if (this.IsValid())
            {

                var inputTemplate = this.transformationFileService.GetInputDashboardArmTemplate(this.SourceFilePath);
                var outputTemplate = this.transformationService.Transform(inputTemplate);

                this.transformationFileService.SaveOutputDashboardArmTemplate(outputTemplate, this.OutputFilePath);
            }
        }

        private bool IsValid()
        {
            var result = new MainViewModelValidator().Validate(this);
            this.ValidationErrors = new ObservableCollection<ValidationError>(result.ValidationErrors);
            this.RaisePropertyChanged(nameof(ValidationErrors));

            return result.IsValid;
        }
    }
}