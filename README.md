# Azure Dashform

A simple application to do the leg work of creating an Azure dashboard ARM template and parameterising obvious values such as the subscription Id, the Application Insight Resource Group, the dashboard name etc.

This allows the same custom Azure Dashboards to be deployed through a pipeline, with the parts that chnage per environment to be parameterised.

The tool can operate in two ways, it can generate a complete template to be used on it's own or a it will create a partial template which can be added to an existing ARM template.

If creating a complete template, the tool will also add the required metadata and the parameters object at the top of the template, remove the Id property from the bottom of the template. A seperate parameters JSON file will also be created which can be used to provide values which are parameterised between environments. The token format used by default is the one used by Octopus Deploy. A choice of token formats will be added in the future.

It is assumed that all referenced resources used by Application Insights reside in the same Azure Resource Group as the Application Insights resource. When creating a complete template, the template could be used to create the dashbaord in the same resource group as the Application Insights resource, but it could also be used to create a dashboard in a different resource group. Therefore, when creating a complete resource template, the tool by default assumes that the resource group name and subscription Id will be provided as parameters. 

When creating a partial template, the tool assumes that all resources will be provisioned as part of the same template and variables will be used to name the resources (most likely using conventions). The variables names are best guess, if they do not match the existing variables name then a simple Find & Replace should solve the problem. The resource group and subscription Id will be looked up using the template resource functions of an ARM template so are not required as parameters or variables.

https://docs.microsoft.com/en-us/azure/azure-portal/azure-portal-dashboards-create-programmatically

## Usage for Complete Template

1. Create a dashboard in the Azure portal, typically this will be in a test environment.
2. Make it as colourful and wonderful as you can, including lots of useful charts/stats.
3. Make a note of the identifier of the dashboard in the portal Url.
4. Use the Azure Resource Explorer to find your dashboard.
5. Copy the JSON content from the Resource Explorer and save it into a new JSON file locally on your machine.
6. Run the application and choose the file saved in Step 5.
7. Choose an output directory if the default one is not desirable.
8. Ensure the "Create Full Template" option is checked.
9. Click the Dashform It button.
10. You should then have two files created, one is the template itself and one is the parameters file.
11. If your dashboard references resources directly such as Key Vaults or Cosmos DB then additional parameters will be added to the parameters file. The name of this parameter will be the same as the resource name, this will probably not be desirable, e.g. MyKeyVaultDevTestResourceName. If this is modified you will also need to update all references to the parameter name in the template file.
12. Deploy the ARM template in your pipeline.

## Usage for Partial Template

1. Create a dashboard in the Azure portal, typically this will be in a test environment.
2. Make it as colourful and wonderful as you can, including lots of useful charts/stats.
3. Make a note of the identifier of the dashboard in the portal Url.
4. Use the Azure Resource Explorer to find your dashboard.
5. Copy the JSON content from the Resource Explorer and save it into a new JSON file locally on your machine.
6. Run the application and choose the file saved in Step 5.
7. Choose an output directory if the default one is not desirable.
8. Ensure the "Create Full Template" option is **not** checked.
9. Click the Dashform It button.
10. You should then have a dashbaord file created. You will also have a parameters file but it will be empty.
11. A collection of variables will be added at the top of the template. This is used to indicate what variables are being referenced by the dashboard template. If the names do not match what already exists in the existing ARM template then perform a Find & Replace to rename all the references to it.
12. If your dashboard references resources directly such as Key Vaults or Cosmos DB then additional variables will be added to the parameters file. The name of this variable will be the same as the resource name, this will probably not be desirable, e.g. MyKeyVaultDevTestResourceName. If this is modified you will also need to update all references to the parameter name in the template file.
13. Remove the variables property from the generted template.
14. Copy the rest of the template into an existing template.
15. Deploy the ARM template in your pipeline.

## Tests

Because it can be quite difficult to test each facet of the JSON parsing and token replacement. The valid JSON tests use a Golden Master approach, for a given input we have two expected given outputs, the actuals are compared against the expected. This does support an outside in testing approach but it may make it difficult to diagnose the exact line of code which is causing the problem if a test fails.

## License

This project is licensed under the terms of the GNU GENERAL PUBLIC LICENSE.

## Latest Build

You should be able to download the compile binaries from:

https://rink.hockeyapp.net/apps/c7d46e099e7247eb98ce595fc2146750