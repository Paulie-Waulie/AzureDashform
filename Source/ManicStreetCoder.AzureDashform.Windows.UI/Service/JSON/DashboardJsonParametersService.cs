namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;

    public static class DashboardJsonParametersService
    {
        public static string CreateParameters(List<string> additionalParameters)
        {
            var rootObject = new JObject();
            rootObject.Add(new JProperty(ArmPropertyNameConstants.Schema, ArmPropertyValueConstants.ParametersSchema));
            rootObject.Add(new JProperty(ArmPropertyNameConstants.ContentVersion, ArmPropertyValueConstants.ContentVersion));
            rootObject.Add(new JProperty(ArmPropertyNameConstants.Parameters, CreateParametersObject(additionalParameters)));

            return JsonWriter.CreateOutputJsonWithFormatting(rootObject);
        }

        private static JObject CreateParametersObject(List<string> additionalParameters)
        {
            var parametersObject = new JObject(
                CreateParameterProperty(ArmParameterProperty.ResourceGroupName),
                CreateParameterProperty(ArmParameterProperty.SubscriptionId),
                CreateParameterProperty(ArmParameterProperty.AppinsightsName),
                CreateParameterProperty(ArmParameterProperty.DashboardName),
                CreateParameterProperty(ArmParameterProperty.DashboardDisplayName));

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

        private static JProperty CreateParameterProperty(ArmParameterProperty parameter)
        {
            var parameterName = WrapParamter(parameter.ParameterValue);
            return new JProperty(parameter.ParameterName, new JObject(new JProperty("value", parameterName)));
        }

        private static JProperty CreateAdditionalParameterProperty(string parameter)
        {
            var parameterName = WrapParamter(UppercaseFirstLetter(parameter));
            return new JProperty(parameter, new JObject(new JProperty("value", parameterName)));
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