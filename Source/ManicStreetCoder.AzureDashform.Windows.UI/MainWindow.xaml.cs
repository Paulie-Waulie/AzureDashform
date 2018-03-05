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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InputFileDialog_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "JSON Files (*.json)|*.json" };
            if (dialog.ShowDialog() ?? false)
            {
                ((MainViewModel) this.DataContext).SourceFilePath = dialog.FileName;
            }
        }
    }
}
