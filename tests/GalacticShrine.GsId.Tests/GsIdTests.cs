using System;
using Xunit;

namespace GalacticShrine.GsId.Tests;

public sealed class GsIdTests : IDisposable
{
    private const string NormalizedValue = "9F2A6C1E8D4B7A90A13F9C2DE88B421091AF77CB4D6E39A2FC018AD92E7B5C64";
    private const string FormattedValue = "9F2A6C1E8D4B7A90-A13F9C2D-E88B4210-91AF77CB-4D6E39A2-FC018AD92E7B5C64";
    private static readonly string NormalizedValueLower = NormalizedValue.ToLowerInvariant();
    private static readonly string FormattedValueLower = FormattedValue.ToLowerInvariant();

    public GsIdTests()
        => GsIdOptionsTestHelper.Reset();

    public void Dispose()
        => GsIdOptionsTestHelper.Reset();

    [Fact]
    public void NewGsId_ShouldGenerateNonEmptyIdentifier()
    {
        GsId Id = GsId.NewGsId();

        Assert.False(Id.IsEmpty);
        Assert.Equal(GsIdConstants.ByteLength, Id.ToByteArray().Length);
        Assert.Equal(GsIdConstants.HexLength, Id.ToString(GsIdFormat.N).Length);
        Assert.Equal(GsIdConstants.FormattedLength, Id.ToString(GsIdFormat.D).Length);
    }

    [Fact]
    public void Parse_ShouldAcceptFormatN()
    {
        GsId Id = GsId.Parse(NormalizedValue);

        Assert.Equal(NormalizedValue, Id.ToString("N", null));
    }

    [Fact]
    public void Parse_ShouldAcceptFormatD()
    {
        GsId Id = GsId.Parse(FormattedValue);

        Assert.Equal(FormattedValue, Id.ToString("D", null));
    }

    [Fact]
    public void Parse_ShouldAcceptLowercaseAndNormalizeToRequestedCase()
    {
        GsId Id = GsId.Parse(FormattedValueLower);

        Assert.Equal(FormattedValue, Id.ToString(GsIdFormat.D, GsIdCase.Upper));
        Assert.Equal(FormattedValueLower, Id.ToString(GsIdFormat.D, GsIdCase.Lower));
        Assert.Equal(NormalizedValueLower, Id.ToString(GsIdFormat.N, GsIdCase.Lower));
    }

    [Fact]
    public void TryParse_ShouldReturnFalse_WhenValueIsInvalid()
    {
        bool Success = GsId.TryParse("invalid", out GsId Result);

        Assert.False(Success);
        Assert.True(Result.IsEmpty);
    }

    [Fact]
    public void Equals_ShouldCompareByValue()
    {
        GsId First = GsId.Parse(NormalizedValue);
        GsId Second = GsId.Parse(FormattedValue);

        Assert.Equal(First, Second);
        Assert.True(First == Second);
        Assert.False(First != Second);
    }

    [Fact]
    public void Options_ShouldControlDefaultCaseAndTextFormat()
    {
        GsIdOptions.Configure(
            DefaultCase: GsIdCase.Lower,
            DefaultTextFormat: GsIdFormat.N,
            DefaultJsonFormat: GsIdFormat.D,
            DefaultDatabaseFormat: GsIdFormat.N);

        GsId Id = GsId.Parse(FormattedValue);

        Assert.Equal(NormalizedValueLower, Id.ToString());
        Assert.Equal(NormalizedValueLower, GsIdParser.Normalize(FormattedValue));
    }

    [Fact]
    public void Options_ShouldLockConfiguration()
    {
        GsIdOptions.Configure(DefaultCase: GsIdCase.Lower);
        GsIdOptions.Lock();

        Assert.True(GsIdOptions.IsLocked);
        Assert.Throws<InvalidOperationException>(() => GsIdOptions.DefaultCase = GsIdCase.Upper);
        Assert.Throws<InvalidOperationException>(() => GsIdOptions.Reset());
    }
}
