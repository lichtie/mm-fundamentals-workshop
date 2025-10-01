# Storage Component - Pulumi Component Provider

A reusable Pulumi component that provisions Azure Storage infrastructure with best practices built-in.

## What's Deployed

This component creates the following Azure resources:

1. **Azure Storage Account** (StorageV2)
   - Standard LRS (Locally Redundant Storage) SKU
   - Named as `{baseName}storage`
   - Configured for general-purpose v2 storage

2. **Blob Container**
   - Named "data"
   - Private access level (no public access)
   - Created within the provisioned storage account

All resources are deployed as children of the component resource for proper lifecycle management and organization.

## Prerequisites

- [.NET Core 9.0](https://dotnet.microsoft.com/download) or later
- [Pulumi CLI](https://www.pulumi.com/docs/get-started/install/)
- Azure subscription and credentials configured
- An existing Azure Resource Group

## Required Inputs

The component requires the following input parameters:

| Parameter | Type | Description |
|-----------|------|-------------|
| `ResourceGroupName` | `Input<string>` | Name of the existing Azure Resource Group where resources will be created |
| `BaseName` | `Input<string>` | Base name used to generate resource names (storage account will be `{baseName}storage`) |

### Usage Example

```csharp
using Pulumi;
using System.Threading.Tasks;

class Program
{
    static Task<int> Main()
    {
        return Deployment.RunAsync(() =>
        {
            var storage = new StorageComponent("my-storage", new StorageComponentArgs
            {
                ResourceGroupName = "my-resource-group",
                BaseName = "myapp"
            });

            return new Dictionary<string, object?>
            {
                ["storageAccountName"] = storage.StorageAccountName,
                ["containerName"] = storage.ContainerName
            };
        });
    }
}
```

## Outputs

The component provides the following outputs:

| Output | Type | Description |
|--------|------|-------------|
| `StorageAccountName` | `Output<string>` | The name of the created Azure Storage Account |
| `ContainerName` | `Output<string>` | The name of the created blob container (always "data") |

## Building and Using the Component

### Build the Component Provider

```bash
# Restore dependencies and build
dotnet build

# The component provider will be built to bin/Debug/netcoreapp9.0/storage-component
```

### Register as a Pulumi Plugin

```bash
# Install the component provider locally
pulumi plugin install resource storage-component v0.1.0 --file ./bin/Debug/netcoreapp9.0
```

## Component Configuration

The component is configured via `PulumiPlugin.yaml`:

```yaml
name: storage-component
version: 0.1.0
runtime: dotnet
```

## Project Structure

```
.
├── Program.cs                          # Component provider host entry point
├── StorageComponent.cs                 # Component resource implementation
├── mm-fundamentals-component.csproj    # .NET project file
├── PulumiPlugin.yaml                   # Pulumi plugin configuration
└── README.md                           # This file
```

## Resource Naming

- **Storage Account**: `{BaseName}storage` (e.g., if BaseName is "myapp", storage account will be "myappstorage")
- **Blob Container**: Always named "data"

Note: Azure Storage Account names must be globally unique, 3-24 characters, lowercase letters and numbers only.

## Security Considerations

- The blob container is created with `PublicAccess.None`, ensuring data is not publicly accessible
- Storage account uses Standard_LRS for cost-effective local redundancy
- All resources are created as child resources of the component for proper access control and lifecycle management
