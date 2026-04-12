using System;
using System.Text.Json;
using Xunit;

namespace GalacticShrine.GsId.Tests;

public sealed class GsIdCoreTests : IDisposable
{
    private const string NormalizedValue = "9F2A6C1E8D4B7A90A13F9C2DE88B421091AF77CB4D6E39A2FC018AD92E7B5C64";
    private const string FormattedValue = "9F2A6C1E8D4B7A90-A13F9C2D-E88B4210-91AF77CB-4D6E39A2-FC018AD92E7B5C64";
    private static readonly string NormalizedValueLower = NormalizedValue.ToLowerInvariant();
    private static readonly string FormattedValueLower = FormattedValue.ToLowerInvariant();

    public GsIdCoreTests()
        => GsIdOptionsTestHelper.Reset();

    public void Dispose()
        => GsIdOptionsTestHelper.Reset();

    [Fact]
    public void ParserNormalize_ShouldUseGlobalDefaultCase()
    {
        GsIdOptions.DefaultCase = GsIdCase.Lower;

        Assert.Equal(NormalizedValueLower, GsIdParser.Normalize(FormattedValue));
    }

    [Fact]
    public void JsonConverter_ShouldUseDefaultJsonOptions()
    {
        GsIdOptions.Configure(
            DefaultCase: GsIdCase.Lower,
            DefaultJsonFormat: GsIdFormat.D);

        JsonSerializerOptions Options = new();
        Options.Converters.Add(new GsIdJsonConverter());

        GsId Id = GsId.Parse(FormattedValue);
        string Json = JsonSerializer.Serialize(Id, Options);

        Assert.Equal($"\"{FormattedValueLower}\"", Json);
    }

    [Fact]
    public void JsonConverter_ShouldUseExplicitOptions_WhenProvided()
    {
        GsIdOptions.Configure(
            DefaultCase: GsIdCase.Lower,
            DefaultJsonFormat: GsIdFormat.D);

        JsonSerializerOptions Options = new();
        Options.Converters.Add(new GsIdJsonConverter(GsIdFormat.N, GsIdCase.Upper));

        GsId Id = GsId.Parse(FormattedValue);
        string Json = JsonSerializer.Serialize(Id, Options);
        GsId Parsed = JsonSerializer.Deserialize<GsId>(Json, Options);

        Assert.Equal($"\"{NormalizedValue}\"", Json);
        Assert.Equal(Id, Parsed);
    }

    [Fact]
    public void Options_ShouldLockConfiguration()
    {
        GsIdOptions.Configure(DefaultCase: GsIdCase.Lower);
        GsIdOptions.Lock();

        Assert.Throws<InvalidOperationException>(() => GsIdOptions.DefaultTextFormat = GsIdFormat.N);
    }
}
