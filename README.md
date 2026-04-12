# GalacticShrine.GsId (C#)

**Langues :** **Français** · [English](./README.en.md) · [Español](./README.es.md) · [Italiano](./README.it.md) · [日本語](./README.jp.md)

Bibliothèque .NET `net8.0` pour générer, parser, valider et sérialiser des identifiants `GsId` 256 bits.

## Statut

- Version : `1.0.2`
- Niveau : production

## Points clés

- formats `N` et `D`
- support de casse `Upper` et `Lower`
- `GsIdOptions.DefaultCase`
- `GsIdOptions.DefaultTextFormat`
- `GsIdOptions.DefaultJsonFormat`
- `GsIdOptions.DefaultDatabaseFormat`
- `GsIdOptions.Lock()`
- chargement depuis `IConfiguration` via `GsIdOptionsConfiguration`

## Configuration `appsettings.json`

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

## Formats rapides

- `ToString("N")` / `ToString("D")` forcent la sortie majuscule
- `ToString("n")` / `ToString("d")` forcent la sortie minuscule
