namespace ManicStreetCoder.AzureDashform.Windows.UI
{
    using System.Windows;
    using AzureDashform.ViewModel;
    using Microsoft.Win32;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string FileFilter = "JSON Files (*.json)|*.json";

        public MainWindow()
        {
            InitializeComponent();
        }

        private MainViewModel ViewModel => (MainViewModel) this.DataContext;

        private void InputFileDialog_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = FileFilter };
            if (dialog.ShowDialog() ?? false)
            {
                ViewModel.SourceFilePath = dialog.FileName;
            }
        }

        private void OutputFileDialog_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog { Filter = FileFilter };
            if (dialog.ShowDialog() ?? false)
            {
                ViewModel.OutputFilePath = dialog.FileName;
            }
        }
    }
}
