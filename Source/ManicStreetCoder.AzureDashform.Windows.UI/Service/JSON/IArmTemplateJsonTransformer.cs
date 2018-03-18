namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON
{
    using Newtonsoft.Json.Linq;

    internal interface IArmTemplateJsonTransformer
    {
        JObject Transform(JObject inputJson);
    }
}
