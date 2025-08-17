# ASP.NET Core Action Filters

[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/license/mit)
[![Language](https://img.shields.io/badge/Language-C%23-blue.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Frameworks](https://img.shields.io/badge/Frameworks-.NET_8.0+,_ASP.NET_Core_8.0+_-green.svg)](https://dotnet.microsoft.com/download/dotnet-core)
[![CI](https://github.com/olumuyiwa-agboola/Olumuyiwa.DotNetDevKit/actions/workflows/Olumuyiwa.DotNetDevKit.AspNetCore.ActionFilters.yml/badge.svg)](https://github.com/olumuyiwa-agboola/Olumuyiwa.DotNetDevKit/actions/workflows/Olumuyiwa.DotNetDevKit.AspNetCore.ActionFilters.yml)

A collection of reusable [action filters](https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters) that I have implemented and used in 
several ASP.NET Core applications, providing common functionality for some of or all the action methods in each application.

## 1. ```ValidateRequestParameters```
Validates the parameters of an action method using the ```IValidator<T>``` implementation registered in the dependency injection container.

#### Usage
1. Install package from NuGet via the .NET CLI, Package Manager Console or any other preferred method:
	- .NET CLI
   ```bash
   dotnet add package Olumuyiwa.DotNetDevKit.AspNetCore.ActionFilters
   ```
	- Package Manager Console
   ```bash
   Install-Package Olumuyiwa.DotNetDevKit.AspNetCore.ActionFilters
   ```

2. Register the ```IValidator<T>``` implementation in the dependency inject container in the `ConfigureServices` method of your `Startup.cs` file or 
directly in your `Program.cs` file:

```csharp
services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
```

3. Apply the `ValidateRequestParameters` attribute to your action method:
```csharp
[HttpPost]
[ValidateRequestParameters]
public async Task<IActionResult> CreateUser(CreateUserRequest request)
{
	// Your action method implementation
}
```
or to your entire controller:
```csharp
[ApiController]
[ValidateRequestParameters]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
	// Your controller implementation
}
```
or to all your controllers:
```csharp
services.AddControllers(options =>
{
	options.Filters.Add<ValidateRequestParametersAttribute>();
});
```

## 2. ```Coming soon...```
