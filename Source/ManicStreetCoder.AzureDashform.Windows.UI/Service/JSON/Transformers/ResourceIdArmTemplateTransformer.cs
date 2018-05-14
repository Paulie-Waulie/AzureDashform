namespace ManicStreetCoder.AzureDashform.Windows.UI.Service.JSON.Transformers
{
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;
    using Arm;
    using Newtonsoft.Json.Linq;

    internal class ResourceIdArmTemplateTransformer : ArmTemplateTransformer
    {
        public ResourceIdArmTemplateTransformer(ArmTemplateTransformer nextTransformer) : base(nextTransformer)
        {
        }

        protected override ArmTemplate TransformInner(ArmTemplate armTemplate, IArmPropertyValueResolver armPropertyValueResolver)
        {
            var resourceIdProperties = armTemplate.Json.GetAllChildProperties("resourceId");
            var additionalResourceNames = new HashSet<string>();
            foreach (var resourceIdProperty in resourceIdProperties)
            {
                var resourceId = new ResourceId(resourceIdProperty.Value.Value<string>(), armPropertyValueResolver);
                resourceIdProperty.Value = resourceId.ToParameterisedString();
                additionalResourceNames.Add(resourceId.ResourceParameterName);
            }
            armTemplate.AdditionalResourceNames.AddRange(additionalResourceNames);
            return armTemplate;
        }

        private class ResourceId
        {
            private readonly IArmPropertyValueResolver armPropertyValueResolver;
            private static readonly Regex ResourceIdPattern = new Regex(@"\/subscriptions\/(?<subscription>.+?)\/resourceGroups\/(?<resourceGroup>.+?)\/providers\/(?<provider>.+?)\/(?<providerType>.+?)\/(?<resourceName>.+)");

            public ResourceId(string resourceIdString, IArmPropertyValueResolver armPropertyValueResolver)
            {
                this.armPropertyValueResolver = armPropertyValueResolver;
                Match match = ResourceIdPattern.Match(resourceIdString);
                this.ResourceIdString = resourceIdString;
                this.SubscriptionId = match.Groups["subscription"].Value;
                this.ResourceGroup = match.Groups["resourceGroup"].Value;
                this.Provider = match.Groups["provider"].Value;
                this.ProviderType = match.Groups["providerType"].Value;
                this.ResourceName = match.Groups["resourceName"].Value;
            }

            private string ResourceIdString { get; }

            private string SubscriptionId { get; }

            private string ResourceGroup { get; }

            private string Provider { get; }

            private string ProviderType { get; }

            private string ResourceName { get; }

            public string ResourceParameterName => $"{this.ResourceName}-resourceName";

            public string ToParameterisedString()
            {
                if (this.armPropertyValueResolver is ArmParameterPropertyValueResolver)
                {
                    return this.GetDirectResourceId();
                }

                return this.GetLookupResourceId();
            }

            private string GetLookupResourceId()
            {
                var builder = new StringBuilder();
                builder.Append("[resourceId(")
                    .Append(this.armPropertyValueResolver.GetValue(ArmTemplateDynamicProperty.SubscriptionId))
                    .Append(", ")
                    .Append(this.armPropertyValueResolver.GetValue(ArmTemplateDynamicProperty.ResourceGroupName))
                    .Append(", '")
                    .Append(this.Provider)
                    .Append("/")
                    .Append(this.ProviderType)
                    .Append("', ")
                    .Append(this.armPropertyValueResolver.GetValue(this.ResourceParameterName))
                    .Append(")]");

                return builder.ToString();
            }

            private string GetDirectResourceId()
            {
                var builder = new StringBuilder();
                builder.Append("[concat('")
                    .Append("/subscriptions/',")
                    .Append(this.armPropertyValueResolver.GetValue(ArmTemplateDynamicProperty.SubscriptionId))
                    .Append(",'/resourceGroups/',")
                    .Append(this.armPropertyValueResolver.GetValue(ArmTemplateDynamicProperty.ResourceGroupName))
                    .Append(",'/providers/")
                    .Append(this.Provider)
                    .Append("/")
                    .Append(this.ProviderType)
                    .Append("/',")
                    .Append(this.armPropertyValueResolver.GetValue(this.ResourceParameterName))
                    .Append(")]");

                return builder.ToString();
            }
        }
    }
}