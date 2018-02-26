using GalaSoft.MvvmLight;

namespace ManicStreetCoder.AzureDashform.ViewModel
{
    using AzureDashform.Windows.UI.Model;

    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            this.Details = new TransformationDetails(@"C:\Temp\Summmit.json");
        }

        public TransformationDetails Details { get; set; }
    }
}