[![Nuget](https://img.shields.io/badge/package-PackZero.Configuration-brightgreen.svg?maxAge=259200)](https://www.nuget.org/packages/PackZero.Configuration)
[![CodeQL](https://github.com/msx752/PackZero.Configuration/actions/workflows/codeql.yml/badge.svg?branch=main)](https://github.com/msx752/PackZero.Configuration/actions/workflows/codeql.yml)
[![MIT](https://img.shields.io/badge/License-MIT-blue.svg?maxAge=259200)](https://github.com/msx752/PackZero.Configuration/blob/main/LICENSE.md)

# PackZero.Configuration
extension for the IConfiguration, appsettings.json mapping easily used class object rely on EnvironmentName (especially for the ConsoleApp)

# How to Use
``` c#
using PackZero.Configuration;
```
Example Configuration cs file
``` c#
public class GeneralConfig
{
    public GeneralConfig()
    {
        DeveloperIds = new List<ulong>();
    }

    public string DiscordAppToken { get; set; }
    public List<ulong> DeveloperIds { get; set; }
    public ulong DeveloperServerId { get; set; }
}
```
displaying appsettings.json scope 
``` json
{
	"GeneralConfig": {
	    "DiscordAppId": 543218350067605504,
	    "DiscordAppToken": "tokenValue",
	    "DeveloperIds": [ 321249607107395585 ],
	    "DeveloperServerId": 543218350067605504
  	}
}

```
Service Collection defition

``` c#
services.AddAppZeroConfiguration(typeof(GeneralConfig));
```

# Features
- ConsoleApp Environment Configuration rely on appsettings.{environmentName}.json
usage
``` c#
		var host = Host.CreateDefaultBuilder(args);

		host.UseAppZeroConfiguration();
```