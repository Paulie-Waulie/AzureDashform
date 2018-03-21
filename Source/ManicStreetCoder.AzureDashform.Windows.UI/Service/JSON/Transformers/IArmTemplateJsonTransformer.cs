namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON.Transformers
{
    using Newtonsoft.Json.Linq;

    internal interface IArmTemplateJsonTransformer
    {
        JObject Transform(JObject inputJson);
    }
}
