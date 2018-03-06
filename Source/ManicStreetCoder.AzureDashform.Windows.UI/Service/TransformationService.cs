namespace ManicStreetCoder.AzureDashform.Windows.UI.Service
{
    using System;
    using Exceptions;
    using Model;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class TransformationService : ITransformationService
    {
        public OutputDashboardArmTemplate Transform(InputDashboardArmTemplate inputTemplate)
        {
            try
            {
                JObject.Parse(inputTemplate.TemplateJson);
            }
            catch (JsonReaderException e)
            {
                throw new InvalidInputTemplateException(e);
            }

            return new OutputDashboardArmTemplate("Some output JSON");
        }
    }
}
