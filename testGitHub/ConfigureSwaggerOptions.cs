using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = "Test GitHub API",
                Version = description.ApiVersion.ToString(),
                Description = "API with versioning support for testing"
            });
        }

        options.DocInclusionPredicate((docName, apiDesc) =>
        {
            // Primary: Use the GroupName from the versioned API explorer (e.g., "v1.0", "v2.0")
            // The versioned API explorer sets this based on the MapToApiVersion attributes
            var groupName = apiDesc.GroupName;
            if (!string.IsNullOrEmpty(groupName))
            {
                return groupName == docName;
            }

            // Fallback: Check the version model for explicit version mappings
            var versionModel = apiDesc.ActionDescriptor
                .GetApiVersionModel(ApiVersionMapping.Explicit | ApiVersionMapping.Implicit);

            if (versionModel == null)
            {
                return true; // Include if no version info
            }

            // Check declared versions (from MapToApiVersion attributes)
            if (versionModel.DeclaredApiVersions.Any())
            {
                return versionModel.DeclaredApiVersions.Any(v => 
                {
                    var formattedVersion = $"v{v}";
                    return formattedVersion == docName;
                });
            }

            // Check implemented versions (from controller-level ApiVersion attributes)
            var versions = versionModel.ImplementedApiVersions;
            return versions != null && versions.Any(v => 
            {
                var formattedVersion = $"v{v}";
                return formattedVersion == docName;
            });
        });
    }
}
