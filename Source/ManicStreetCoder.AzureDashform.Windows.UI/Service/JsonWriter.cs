﻿namespace ManicStreetCoder.AzureDashform.Windows.UI.Service
{
    using System.IO;
    using System.Text;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal class JsonWriter
    {
        internal static string CreateOutputJsonWithFormatting(JObject inputJson)
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