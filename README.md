<snippet>
  <content><![CDATA[
# ${1:Azure Dashform}

A simple application to do the leg work of creating an Azure dashboard ARM template and parameterising obvious values such as the subscription Id, the Application Insight Resource Group, the dashboard name etc.

This allows the same custom Azure Dashboards to be deployed through a pipeline, with the parts that chnage per environment to be parameterised.

The tool will also add the required metadata and the parameters object at the top of the template, remove the Id property from the bottom of the template. A seperate parameters JSON file will also be created which can be used to provide values which are parameterised between environments. The token format used by default is the one used by Octopus Deploy. A choice of token formats will be added in the future.

https://docs.microsoft.com/en-us/azure/azure-portal/azure-portal-dashboards-create-programmatically

## Usage

1. Create a dashboard in the Azure portal, typically this will be a test environment.
2. Make it as colourful and wonderful as you can, including lots of useful charts/stats.
3. Make a note of the identifier of the dashboard in the portal Url.
4. Use the Azure Resource Explorer to find your dashboard.
5. Copy the JSON content from the Resource Explorer and save it into a new JSON file locally on your machine.
6. Run the application and choose the file saved in Step 5.
7. Choose an output directory if the default one is not desirable.
8. Click the Dashform It button.
9. You should then have two files created, one is the template itself and one is the parameters file.
10. If your dashboard references resources directly such as Key Vaults or Cosmos DB then additional parameters will be added to the parameters file. The name of this parameter will be the same as the resource name, this will probably not be desirable, e.g. MyKeyVaultDevTestResourceName. If this is modified you will also need to update all references to the parameter name in the template file.

## Tests

Because it can be quite difficult to test each facet of the JSON parsing and token replacement. The valid JSON tests use a Golden Master approach, for a given input we have two expected given outputs, the actuals are compared against the expected. This does support an outside in testing approach but it may make it difficult to diagnose the exact line of code which is causing the problem if a test fails.

## License

This project is licensed under the terms of the GNU GENERAL PUBLIC LICENSE.
]]></content>
  <tabTrigger>readme</tabTrigger>
</snippet>