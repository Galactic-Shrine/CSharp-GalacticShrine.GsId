# GalacticShrine.GsId (C#)

**Idiomas:** [Français](./README.md) · [English](./README.en.md) · **Español** · [Italiano](./README.it.md) · [日本語](./README.jp.md)

Biblioteca `.NET net8.0` para generar, analizar, validar y serializar identificadores `GsId` de 256 bits.

## Estado

- Versión: `1.0.0`
- Nivel: producción

## Puntos clave

- formatos `N` y `D`
- soporte de mayúsculas y minúsculas `Upper` y `Lower`
- `GsIdOptions.DefaultCase`
- `GsIdOptions.DefaultTextFormat`
- `GsIdOptions.DefaultJsonFormat`
- `GsIdOptions.DefaultDatabaseFormat`
- `GsIdOptions.Lock()`
- carga desde `IConfiguration` mediante `GsIdOptionsConfiguration`

## Configuración `appsettings.json`

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

## Formatos rápidos

- `ToString("N")` / `ToString("D")` fuerzan la salida en mayúsculas
- `ToString("n")` / `ToString("d")` fuerzan la salida en minúsculas
