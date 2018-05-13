namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON.Transformers
{
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json.Linq;

    internal class ResourceIdArmTemplateTransformer : ArmTemplateTransformer
    {
        public ResourceIdArmTemplateTransformer(ArmTemplateTransformer nextTransformer) : base(nextTransformer)
        {
        }

        protected override ArmTemplate TransformInner(ArmTemplate armTemplate)
        {
            var resourceIdProperties = armTemplate.Json.GetAllChildProperties("resourceId");
            var additionalParameters = new HashSet<string>();
            foreach (var resourceIdProperty in resourceIdProperties)
            {
                var resourceId = new ResourceId(resourceIdProperty.Value.Value<string>());
                resourceIdProperty.Value = resourceId.ToParameterisedString();
                additionalParameters.Add(resourceId.ResourceParameterName);
            }
            armTemplate.AdditionalParameterNames.AddRange(additionalParameters);
            return armTemplate;
        }

        private class ResourceId
        {
            private static Regex ResourceIdPattern = new Regex(@"\/subscriptions\/(?<subscription>.+?)\/resourceGroups\/(?<resourceGroup>.+?)\/providers\/(?<provider>.+?)\/(?<providerType>.+?)\/(?<resourceName>.+)");

            public ResourceId(string resourceIdString)
            {
                Match match = ResourceIdPattern.Match(resourceIdString);
                this.ResourceIdString = resourceIdString;
                this.SubscriptionId = match.Groups["subscription"].Value;
                this.ResourceGroup = match.Groups["resourceGroup"].Value;
                this.Provider = match.Groups["provider"].Value;
                this.ProviderType = match.Groups["providerType"].Value;
                this.ResourceName = match.Groups["resourceName"].Value;
            }

            public string ResourceIdString { get; }

            public string SubscriptionId { get; }

            public string ResourceGroup { get; }

            public string Provider { get; }

            public string ProviderType { get; }

            public string ResourceName { get; }

            public string ResourceParameterName => $"{this.ResourceName}-resource-name";

            public string ToParameterisedString()
            {
                var builder = new StringBuilder();
                builder.Append("[concat('")
                       .Append("/subscriptions/',")
                       .Append(ArmParameterProperty.SubscriptionId.ArmParameterSelector())
                       .Append(",'/resourceGroups/',")
                       .Append(ArmParameterProperty.ResourceGroupName.ArmParameterSelector())
                       .Append(",'/providers/")
                       .Append(this.Provider)
                       .Append("/")
                       .Append(this.ProviderType)
                       .Append("/',")
                       .Append(ArmJsonExtensions.ArmParameterSelector(this.ResourceParameterName))
                       .Append(")]");

                return builder.ToString();
            }
        }
    }
}