namespace ManicStreetCoder.AzureDashform.Windows.UI
{
    using System;
    using System.Windows;
    using System.Windows.Forms;
    using AzureDashform.ViewModel;
    using GalaSoft.MvvmLight.Messaging;
    using MessageBox = System.Windows.MessageBox;
    using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
    using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string FileFilter = "JSON Files (*.json)|*.json";

        public MainWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<Exception>(this, HandleError);
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

        private void HandleError(Exception exception)
        {
            MessageBox.Show(exception.Message, "Transform Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OutputFileDialog_OnClick(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK))
                {
                    ViewModel.OutputFolderPath = dialog.SelectedPath;
                }
            }
        }
    }
}
