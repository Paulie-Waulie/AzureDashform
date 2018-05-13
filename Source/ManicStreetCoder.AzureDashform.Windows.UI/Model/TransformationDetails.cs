namespace ManicStreetCoder.AzureDashform.Windows.UI.Model
{
    public class TransformationDetails
    {
        public TransformationDetails()
        {
        }

        public TransformationDetails(string outputFilePath, bool dashboardIsCompleteTemplate)
        {
            this.OutputFilePath = outputFilePath;
            this.DashboardIsCompleteTemplate = dashboardIsCompleteTemplate;
        }

        public string SourceFilePath { get; set; }

        public string OutputFilePath { get; set; }

        public bool DashboardIsCompleteTemplate { get; set; }
    }
}
