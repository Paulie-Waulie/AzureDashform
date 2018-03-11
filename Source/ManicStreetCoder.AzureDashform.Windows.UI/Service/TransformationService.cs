namespace ManicStreetCoder.AzureDashform.Windows.UI.Service
{
    using System.IO;
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
                outputJson = WriteOutputJsonWithFormatting(inputJson);
            }
            catch (JsonReaderException e)
            {
                throw new InvalidInputTemplateException(e);
            }

            return new OutputDashboardArmTemplate(outputJson);
        }

        private static void AddDocumetHeader(JObject inputJson)
        {
            inputJson.AddFirst(BuildParameters());
            inputJson.AddFirst(new JProperty("contentVersion", "1.0.0.0"));
            inputJson.AddFirst(new JProperty("$schema", "https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#"));
        }

        private static void ReplaceValuesWithParameters(JObject inputJson)
        {
            string jsonPartsPath = "properties.lenses.0.parts";
            var parts = inputJson.SelectToken(jsonPartsPath);
            if (parts == null)
            {
                throw new InvalidInputTemplateException($"Could not find parts proprty with path {jsonPartsPath} in Json file.");
            }

            UpdateParts(parts);
            UpdateTemplateMetadata(inputJson);
        }

        private static void UpdateTemplateMetadata(JObject inputJson)
        {
            RemoveTemplateId(inputJson);

            inputJson.ReplacePropertyValueWithParameter(ArmPropertyParameter.DashboardName);

            AddEmptyMetadataProperty(inputJson);
            AddArmApiVersion(inputJson);

            inputJson.GetObject("tags").ReplacePropertyValueWithParameter(ArmPropertyParameter.DashboardDisplayName);
        }

        private static void AddEmptyMetadataProperty(JObject inputJson)
        {
            inputJson.GetProperty("name").AddBeforeSelf(new JProperty("metadata", new JObject()));
        }

        private static void AddArmApiVersion(JObject inputJson)
        {
            inputJson.SelectToken("type").Parent.AddAfterSelf(new JProperty("apiVersion", "2015-08-01-preview"));
        }

        private static void RemoveTemplateId(JObject inputJson)
        {
            inputJson.SelectToken("id").Parent.Remove();
        }

        private static void UpdateParts(JToken parts)
        {
            int partNumber = 0;
            bool partExists = true;

            while (partExists)
            {
                var part = parts.SelectToken(partNumber.ToString());
                if (part != null)
                {
                    var inputs = part.SelectToken("metadata.inputs").AsJEnumerable();

                    UpdateComponentIds(inputs);
                    UpdatePartSubTitle(inputs);
                    RemoveDashboardId(inputs);
                }
                else
                {
                    partExists = false;
                }

                partNumber++;
            }
        }

        private static void RemoveDashboardId(IJEnumerable<JToken> inputs)
        {
            inputs.GetObjectByName("DashboardId")?.Remove();
        }

        private static void UpdatePartSubTitle(IJEnumerable<JToken> inputs)
        {
            inputs.GetObjectByName("PartSubTitle")?.ReplacePropertyValueWithParameter("value", ArmPropertyParameter.AppinsightsName);
        }

        private static void UpdateComponentIds(IJEnumerable<JToken> inputs)
        {
            var componentIdInput = inputs.GetObjectByName("ComponentId").GetObject("value");
            componentIdInput.ReplacePropertyValueWithParameter(ArmPropertyParameter.SubscriptionId);
            componentIdInput.ReplacePropertyValueWithParameter(ArmPropertyParameter.ResourceGroupName);
            componentIdInput.ReplacePropertyValueWithParameter(ArmPropertyParameter.AppinsightsName);
        }

        private static JProperty BuildParameters()
        {
            return new JProperty("parameters", new JObject(
                CreateParameterProperty(ArmParameterNames.AppinsightsName),
                CreateParameterProperty(ArmParameterNames.DashboardName),
                CreateParameterProperty(ArmParameterNames.DashboardDisplayName),
                CreateParameterProperty(ArmParameterNames.ResourceGroupName),
                CreateParameterProperty(ArmParameterNames.SubscriptionId)
            ));
        }

        private static JProperty CreateParameterProperty(string parameterName)
        {
            return new JProperty(parameterName, new JObject(new JProperty("type", "string")));
        }

        private static string WriteOutputJsonWithFormatting(JObject inputJson)
        {
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
                    return sb.ToString();
                }
            }
        }
    }
}
