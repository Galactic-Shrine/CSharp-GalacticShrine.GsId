# GalacticShrine.GsId (C#)

**Languages:** [Français](./README.md) · **English** · [Español](./README.es.md) · [Italiano](./README.it.md) · [日本語](./README.jp.md)

`.NET net8.0` library to generate, parse, validate, and serialize 256-bit `GsId` identifiers.

## Status

- Version: `1.0.0`
- Level: production

## Key points

- `N` and `D` formats
- `Upper` and `Lower` casing support
- `GsIdOptions.DefaultCase`
- `GsIdOptions.DefaultTextFormat`
- `GsIdOptions.DefaultJsonFormat`
- `GsIdOptions.DefaultDatabaseFormat`
- `GsIdOptions.Lock()`
- loading from `IConfiguration` through `GsIdOptionsConfiguration`

## `appsettings.json` configuration

```json
{
  "GsId": {
    "DefaultCase": "Lower",
    "DefaultTextFormat": "N",
    "DefaultJsonFormat": "D",
    "DefaultDatabaseFormat": "N",
    "Lock": true
  }
}
```

```csharp
using GalacticShrine.GsId;
using Microsoft.Extensions.Configuration;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

GsIdOptionsConfiguration.ConfigureFromConfiguration(configuration);
```

## Quick formats

- `ToString("N")` / `ToString("D")` force uppercase output
- `ToString("n")` / `ToString("d")` force lowercase output
