# GalacticShrine.GsId (C#)

**言語:** [Français](./README.md) · [English](./README.en.md) · [Español](./README.es.md) · [Italiano](./README.it.md) · **日本語**

`GsId` 256 ビット識別子を生成・解析・検証・シリアライズするための `.NET net8.0` ライブラリです。

## 状態

- バージョン: `1.0.2`
- レベル: 本番運用

## 主なポイント

- `N` / `D` フォーマット
- `Upper` / `Lower` の文字種対応
- `GsIdOptions.DefaultCase`
- `GsIdOptions.DefaultTextFormat`
- `GsIdOptions.DefaultJsonFormat`
- `GsIdOptions.DefaultDatabaseFormat`
- `GsIdOptions.Lock()`
- `GsIdOptionsConfiguration` による `IConfiguration` からの読み込み

## `appsettings.json` 設定

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

## 形式の補足

- `ToString("N")` / `ToString("D")` は大文字出力を強制します
- `ToString("n")` / `ToString("d")` は小文字出力を強制します
