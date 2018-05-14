namespace ManicStreetCoder.AzureDashform.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using AzureDashform.Windows.UI.Model;
    using AzureDashform.Windows.UI.Service;
    using AzureDashform.Windows.UI.ViewModel.Validation;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class MainViewModel : ViewModelBase
    {
        private readonly ITransformationService transformationService;
        private readonly ITransformationFileService transformationFileService;
        private readonly TransformationDetails details = new TransformationDetails();

        public MainViewModel(ITransformationFileService fileService, ITransformationService transformationService)
        {
            this.details = new TransformationDetails(@"C:\ArmTemplate\Output", true);
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

        public string OutputFolderPath
        {
            get { return this.details.OutputFilePath; }
            set
            {
                this.details.OutputFilePath = value;
                this.RaisePropertyChanged(nameof(OutputFolderPath));
            }
        }

        public bool CreateOutputAsCompleteTemplate
        {
            get { return this.details.DashboardIsCompleteTemplate; }
            set
            {
                this.details.DashboardIsCompleteTemplate = value;
                this.RaisePropertyChanged(nameof(this.CreateOutputAsCompleteTemplate));
            }
        }

        public ObservableCollection<ValidationError> ValidationErrors { get; private set; }

        private void Transform()
        {
            if (this.IsValid())
            {
                try
                {
                    var inputTemplate = this.transformationFileService.GetInputDashboardArmTemplate(this.details);
                    var outputTemplate = this.transformationService.Transform(inputTemplate, this.details);
                    this.transformationFileService.SaveOutputDashboardArmTemplate(outputTemplate, this.details);
                    this.MessengerInstance.Send("The transform succeeded.");
                }
                catch (Exception e)
                {
                    this.MessengerInstance.Send(e);
                }
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