# GalacticShrine.GsId (C#)

**Lingue:** [Français](./README.md) · [English](./README.en.md) · [Español](./README.es.md) · **Italiano** · [日本語](./README.jp.md)

Libreria `.NET net8.0` per generare, analizzare, validare e serializzare identificatori `GsId` a 256 bit.

## Stato

- Versione: `1.0.2`
- Livello: produzione

## Punti chiave

- formati `N` e `D`
- supporto `Upper` e `Lower`
- `GsIdOptions.DefaultCase`
- `GsIdOptions.DefaultTextFormat`
- `GsIdOptions.DefaultJsonFormat`
- `GsIdOptions.DefaultDatabaseFormat`
- `GsIdOptions.Lock()`
- caricamento da `IConfiguration` tramite `GsIdOptionsConfiguration`

## Configurazione `appsettings.json`

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

## Formati rapidi

- `ToString("N")` / `ToString("D")` forzano l’output maiuscolo
- `ToString("n")` / `ToString("d")` forzano l’output minuscolo
