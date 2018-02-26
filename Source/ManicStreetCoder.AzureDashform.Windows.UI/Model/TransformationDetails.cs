namespace ManicStreetCoder.AzureDashform.Windows.UI.Model
{
    public class TransformationDetails
    {
        public TransformationDetails()
        {
        }

        public TransformationDetails(string sourceFilePath)
        {
            this.SourceFilePath = sourceFilePath;
        }

        public string SourceFilePath { get; set; }

        public string OutputFilePath { get; set; }
    }
}
