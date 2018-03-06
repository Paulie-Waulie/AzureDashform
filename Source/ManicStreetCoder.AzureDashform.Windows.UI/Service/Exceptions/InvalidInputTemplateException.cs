namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.Exceptions
{
    using System;

    public class InvalidInputTemplateException : Exception
    {
        public InvalidInputTemplateException(Exception innerException)
            :base($"Could not parse input template JSON : {innerException.Message}", innerException)
        {
        }
    }
}
