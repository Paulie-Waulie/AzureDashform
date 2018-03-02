namespace ManicStreetCoder.AzureDashform.ViewModel
{
    using System.Windows;
    using AzureDashform.Windows.UI.Model;
    using AzureDashform.Windows.UI.Service;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class MainViewModel : ViewModelBase
    {
        private ITransformationService transformationService;
        private ITransformationFileService transformationFileService;

        public MainViewModel(ITransformationFileService fileService, ITransformationService transformationService)
        {
            transformationFileService = fileService;
            this.transformationService = transformationService;
            this.Details = new TransformationDetails(@"C:\Temp\Summmit.json");
        }

        public RelayCommand TransformCommand => new RelayCommand(this.Transform);

        public TransformationDetails Details { get; set; }

        private void Transform()
        {
            var inputTemplate = this.transformationFileService.GetInputDashboardArmTemplate(this.Details);
            var outputTemplate = this.transformationService.Transform(inputTemplate);

            this.transformationFileService.SaveOutputDashboardArmTemplate(outputTemplate);
        }
    }
}