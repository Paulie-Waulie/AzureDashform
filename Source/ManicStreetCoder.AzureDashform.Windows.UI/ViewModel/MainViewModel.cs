using GalaSoft.MvvmLight;

namespace ManicStreetCoder.AzureDashform.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            this.SourceFilePath = @"C:\Temp\Summmit.json";
        }

        public string SourceFilePath { get; set; }

        public string OutputFilePath { get; set; }
    }
}