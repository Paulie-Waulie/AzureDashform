namespace ManicStreetCoder.AzureDashform.Windows.UI.Model
{
    public class TransformationDetails
    {
        public TransformationDetails()
        {
        }

        public TransformationDetails(string outputFilePath)
        {
            this.OutputFilePath = outputFilePath;
        }

        public string SourceFilePath { get; set; }

        public string OutputFilePath { get; set; }

        public bool CompleteOutputTemplate { get; set; }
    }
}
