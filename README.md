# Requirements
## .NET 6 SDK
Download at: https://dotnet.microsoft.com/en-us/download/dotnet/6.0

## Visual Studio
Download at: https://visualstudio.microsoft.com/downloads/

# Install Stryker (mutation testing package for .NET)
Stryker guide: https://stryker-mutator.io/docs/stryker-net/getting-started/
```shell
dotnet tool install -g dotnet-stryker
```

# Generate mutation testing report
```shell
dotnet stryker --test-project MyProject.Test/
```
You can find the generated html report in the StrykerOutput directory.

# Goal
The goal of this exercise is to first generate the mutation tests and look at the results.
After inspecting the results, fix the unit tests so that you achieve 100% mutation coverage.
