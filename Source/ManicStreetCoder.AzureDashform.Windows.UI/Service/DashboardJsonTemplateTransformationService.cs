namespace ManicStreetCoder.AzureDashform.Windows.UI.Service
{
    using System;
    using System.IO;
    using System.Text;
    using Exceptions;
    using Model;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal static class DashboardJsonTemplateTransformationService
    {
        internal static string TransformTemplate(InputDashboardArmTemplate inputTemplate)
        {
            try
            {
                var inputJson = JObject.Parse(inputTemplate.TemplateJson);
                inputJson = WrapRootPropertiesInResourcesObject(inputJson);
                ReplaceValuesWithParameters(inputJson);
                AddDocumetHeader(inputJson);
                return JsonWriter.CreateOutputJsonWithFormatting(inputJson);
            }
            catch (Exception e)
            {
                throw new InvalidInputTemplateException(e);
            }
        }

        private static void AddDocumetHeader(JObject inputJson)
        {
            inputJson.AddFirst(new JProperty(ArmPropertyNameConstants.Variables, new JObject()));
            inputJson.AddFirst(BuildTemplateParameters());
            inputJson.AddFirst(new JProperty(ArmPropertyNameConstants.ContentVersion, ArmPropertyValueConstants.ContentVersion));
            inputJson.AddFirst(new JProperty("$schema", ArmPropertyValueConstants.TemplateSchema));
        }

        private static JObject WrapRootPropertiesInResourcesObject(JObject inputJson)
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

            while (true)
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
                    return;
                }

                partNumber++;
            }
        }

        private static void UpdateTemplateMetadata(JObject properties)
        {
            RemoveTemplateId(properties);
            properties.ReplacePropertyValueWithParameter(ArmParameterProperty.DashboardName);
            AddArmApiVersion(properties);
            properties.GetObject("tags").ReplacePropertyValueWithParameter(ArmParameterProperty.DashboardDisplayName);
        }

        private static void AddArmApiVersion(JObject inputJson)
        {
            inputJson.GetProperty("type").AddAfterSelf(new JProperty("apiVersion", ArmPropertyValueConstants.ApiVersion));
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
            inputs.GetObjectByName("PartSubTitle")?.ReplacePropertyValueWithParameter("value", ArmParameterProperty.AppinsightsName);
        }

        private static void UpdateComponentIds(IJEnumerable<JToken> inputs)
        {
            var componentIdInput = inputs.GetObjectByName("ComponentId").GetObject("value");
            componentIdInput.ReplacePropertyValueWithParameter(ArmParameterProperty.SubscriptionId);
            componentIdInput.ReplacePropertyValueWithParameter(ArmParameterProperty.ResourceGroupName);
            componentIdInput.ReplacePropertyValueWithParameter(ArmParameterProperty.AppinsightsName);
        }

        private static JProperty BuildTemplateParameters()
        {
            return new JProperty(ArmPropertyNameConstants.Parameters, new JObject(
                CreateParameterProperty(ArmParameterProperty.AppinsightsName),
                CreateParameterProperty(ArmParameterProperty.DashboardName),
                CreateParameterProperty(ArmParameterProperty.DashboardDisplayName),
                CreateParameterProperty(ArmParameterProperty.ResourceGroupName),
                CreateParameterProperty(ArmParameterProperty.SubscriptionId)
            ));
        }

        private static JProperty CreateParameterProperty(ArmParameterProperty parameter)
        {
            return new JProperty(parameter.ParameterName, new JObject(new JProperty("type", "string")));
        }
    }
}
