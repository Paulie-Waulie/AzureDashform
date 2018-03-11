namespace ManicStreetCoder.AzureDashform.Windows.UI.Service
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using Exceptions;
    using Model;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class TransformationService : ITransformationService
    {
        public OutputDashboardArmTemplate Transform(InputDashboardArmTemplate inputTemplate)
        {
            string outputJson;
            try
            {
                var inputJson = JObject.Parse(inputTemplate.TemplateJson);

                ReplaceValuesWithParameters(inputJson);
                AddDocumetHeader(inputJson);

                StringBuilder sb = new StringBuilder();
                using (StringWriter sw = new StringWriter(sb))
                {
                    using (JsonTextWriter writer = new JsonTextWriter(sw))
                    {
                        writer.Indentation = 4;
                        writer.Formatting = Formatting.Indented;
                        writer.IndentChar = ' ';
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(writer, inputJson);
                        outputJson = sb.ToString();
                    }
                }
            }
            catch (JsonReaderException e)
            {
                throw new InvalidInputTemplateException(e);
            }

            return new OutputDashboardArmTemplate(outputJson);
        }

        private static void AddDocumetHeader(JObject inputJson)
        {
            var schema = new JProperty("$schema", "https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#");
            var contentVersion = new JProperty("contentVersion", "1.0.0.0");
            inputJson.First.AddBeforeSelf(schema);
            schema.AddAfterSelf(contentVersion);
            contentVersion.AddAfterSelf(BuildParameters());
        }

        private static void ReplaceValuesWithParameters(JObject inputJson)
        {
            string jsonPartsPath = "properties.lenses.0.parts";
            var parts = inputJson.SelectToken(jsonPartsPath);
            if (parts == null)
            {
                throw new InvalidInputTemplateException($"Could not find parts proprty with path {jsonPartsPath} in Json file.");
            }

            int partNumber = 0;
            bool partExists = true;

            while (partExists)
            {
                var part = parts.SelectToken(partNumber.ToString());
                if (part != null)
                {
                    var inputs = part.SelectToken("metadata.inputs").AsJEnumerable();
                    var componentIdInput = inputs.Single(x => ((JValue)x.SelectToken("name")).Value<string>().Equals("ComponentId")).SelectToken("value");
                    ((JValue)componentIdInput.SelectToken("SubscriptionId")).Value = "[parameters(\'subscriptionId\')]";
                    ((JValue)componentIdInput.SelectToken("ResourceGroup")).Value = "[parameters(\'resourceGroupName\')]";
                    ((JValue)componentIdInput.SelectToken("Name")).Value = "[parameters(\'appinsightsName\')]";

                    var partSubtitle = inputs.SingleOrDefault(x => ((JValue) x.SelectToken("name")).Value<string>().Equals("PartSubTitle"));
                    if (partSubtitle != null)
                    {
                        ((JValue) partSubtitle.SelectToken("value")).Value = "[parameters('appinsightsName')]";
                    }

                    var dashboardId = inputs.SingleOrDefault(x => ((JValue)x.SelectToken("name")).Value<string>().Equals("DashboardId"));
                    dashboardId?.Remove();
                }
                else
                {
                    partExists = false;
                }

                partNumber++;
            }

            inputJson.SelectToken("id").Parent.Remove();
            var name = (JValue)inputJson.SelectToken("name");
            name.Value = "[parameters(\'dashboardName\')]";
            name.Parent.AddBeforeSelf(new JProperty("metadata", new JObject()));
            inputJson.SelectToken("type").Parent.AddAfterSelf(new JProperty("apiVersion", "2015-08-01-preview"));
            ((JValue)inputJson.SelectToken("tags.hidden-title")).Value = "[parameters(\'dashboardDisplayName\')]";
        }

        private static JProperty BuildParameters()
        {
            return new JProperty("parameters", new JObject(
                GetParameterProperty("appinsightsName"),
                GetParameterProperty("dashboardId"),
                GetParameterProperty("dashboardName"),
                GetParameterProperty("dashboardDisplayName"),
                GetParameterProperty("resourceGroupName"),
                GetParameterProperty("subscriptionId")
            ));
        }

        private static JProperty GetParameterProperty(string parameterName)
        {
            return new JProperty(parameterName, new JObject(new JProperty("type", "string")));
        }
    }
}
