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
                inputJson = WrapPropertiesNodeInResourcesNode(inputJson);
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
            inputJson.AddFirst(new JProperty("variables", new JObject()));
            inputJson.AddFirst(BuildParameters());
            inputJson.AddFirst(new JProperty("contentVersion", "1.0.0.0"));
            inputJson.AddFirst(new JProperty("$schema", "https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#"));
        }

        private static JObject WrapPropertiesNodeInResourcesNode(JObject inputJson)
        {
            var resources = new JArray();
            var properties = inputJson.Properties();
            var resourcesObject = new JObject();
            foreach (var property in properties)
            {
                resourcesObject.Add(property);
            }

            resources.Add(resourcesObject);
            return new JObject(new JProperty("resources", resources));
        }

        private static void ReplaceValuesWithParameters(JObject inputJson)
        {
            JObject resource = inputJson.GetObject("resources[0]");
            var parts = resource.SelectToken("properties.lenses.0.parts");

            UpdateParts(parts);
            UpdateTemplateMetadata(resource);
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

        private static void UpdateTemplateMetadata(JObject properties)
        {
            RemoveTemplateId(properties);

            properties.ReplacePropertyValueWithParameter(ArmPropertyParameter.DashboardName);

            AddEmptyMetadataProperty(properties);
            AddArmApiVersion(properties);

            properties.GetObject("tags").ReplacePropertyValueWithParameter(ArmPropertyParameter.DashboardDisplayName);
        }

        private static void AddEmptyMetadataProperty(JObject inputJson)
        {
            inputJson.GetProperty("name").AddBeforeSelf(new JProperty("metadata", new JObject()));
        }

        private static void AddArmApiVersion(JObject inputJson)
        {
            inputJson.GetProperty("type").AddAfterSelf(new JProperty("apiVersion", "2015-08-01-preview"));
        }

        private static void RemoveTemplateId(JObject inputJson)
        {
            inputJson.GetProperty("id").Remove();
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
                    writer.Indentation = 1;
                    writer.Formatting = Formatting.Indented;
                    writer.IndentChar = '\t';
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(writer, inputJson);
                    return sb.ToString();
                }
            }
        }
    }
}
