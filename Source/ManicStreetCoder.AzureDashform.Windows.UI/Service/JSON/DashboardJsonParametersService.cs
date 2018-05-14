namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON
{
    using System.Collections.Generic;
    using System.Linq;
    using Arm;
    using Model;
    using Newtonsoft.Json.Linq;

    public static class DashboardJsonParametersService
    {
        public static string CreateParameters(List<string> additionalParameters, TransformationDetails transformationDetails)
        {
            var rootObject = new JObject();
            if (transformationDetails.DashboardIsCompleteTemplate)
            {
                rootObject.Add(new JProperty(ArmTemplatePropertyNameConstants.Schema, ArmPropertyValueConstants.ParametersSchema));
                rootObject.Add(new JProperty(ArmTemplatePropertyNameConstants.ContentVersion, ArmPropertyValueConstants.ContentVersion));
                rootObject.Add(new JProperty(ArmTemplatePropertyNameConstants.Parameters, CreateParametersObject(additionalParameters)));
            }

            return JsonWriter.CreateOutputJsonWithFormatting(rootObject);
        }

        private static JObject CreateParametersObject(List<string> additionalParameters)
        {
            var parametersObject = new JObject(
                CreateParameterProperty(ArmTemplateDynamicProperty.ResourceGroupName),
                CreateParameterProperty(ArmTemplateDynamicProperty.SubscriptionId),
                CreateParameterProperty(ArmTemplateDynamicProperty.AppInsightsName),
                CreateParameterProperty(ArmTemplateDynamicProperty.DashboardName),
                CreateParameterProperty(ArmTemplateDynamicProperty.DashboardDisplayName));

            AddAdditionalParameters(parametersObject, additionalParameters);

            return parametersObject;
        }

        private static void AddAdditionalParameters(JObject parametersObject, List<string> additionalParameters)
        {
            foreach (var additionalParameter in additionalParameters)
            {
                parametersObject.Add(CreateAdditionalParameterProperty(additionalParameter));
            }
        }

        private static JProperty CreateParameterProperty(ArmTemplateDynamicProperty parameter)
        {
            var parameterName = WrapParamter(FormatParameterTokenValue(parameter.DynamicValue));
            return new JProperty(parameter.DynamicValue, new JObject(new JProperty("value", parameterName)));
        }

        private static JProperty CreateAdditionalParameterProperty(string parameter)
        {
            var parameterName = WrapParamter(FormatParameterTokenValue(parameter));
            return new JProperty(parameter, new JObject(new JProperty("value", parameterName)));
        }

        private static string FormatParameterTokenValue(string parameter)
        {
            var segments = parameter.Split('-');
            return string.Join(".", segments.Select(UppercaseFirstLetter));
        }

        private static string UppercaseFirstLetter(string parameter)
        {
            return parameter.FirstOrDefault().ToString().ToUpper() + parameter.Substring(1);
        }

        private static string WrapParamter(string parameter)
        {
            return $"#{{{parameter}}}";
        }
    }
}