using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace GalacticShrine.GsId.Tests;

public sealed class GsIdOptionsConfigurationTests : IDisposable
{
    private const string NormalizedValue = "9F2A6C1E8D4B7A90A13F9C2DE88B421091AF77CB4D6E39A2FC018AD92E7B5C64";
    private const string FormattedValue = "9F2A6C1E8D4B7A90-A13F9C2D-E88B4210-91AF77CB-4D6E39A2-FC018AD92E7B5C64";
    private static readonly string NormalizedValueLower = NormalizedValue.ToLowerInvariant();

    public GsIdOptionsConfigurationTests()
        => GsIdOptionsTestHelper.Reset();

    public void Dispose()
        => GsIdOptionsTestHelper.Reset();

    [Fact]
    public void ConfigureFromConfiguration_ShouldApplyAppSettingsSection()
    {
        Dictionary<string, string?> Values = new()
        {
            ["GsId:DefaultCase"] = "Lower",
            ["GsId:DefaultTextFormat"] = "N",
            ["GsId:DefaultJsonFormat"] = "D",
            ["GsId:DefaultDatabaseFormat"] = "N",
            ["GsId:Lock"] = "true",
        };

        IConfiguration Configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(Values)
            .Build();

        GsIdOptionsConfiguration.ConfigureFromConfiguration(Configuration);

        Assert.Equal(GsIdCase.Lower, GsIdOptions.DefaultCase);
        Assert.Equal(GsIdFormat.N, GsIdOptions.DefaultTextFormat);
        Assert.Equal(GsIdFormat.D, GsIdOptions.DefaultJsonFormat);
        Assert.Equal(GsIdFormat.N, GsIdOptions.DefaultDatabaseFormat);
        Assert.True(GsIdOptions.IsLocked);

        GsId Id = GsId.Parse(FormattedValue);
        Assert.Equal(NormalizedValueLower, Id.ToString());
    }

    [Fact]
    public void ConfigureFromSection_ShouldApplyDirectSection()
    {
        Dictionary<string, string?> Values = new()
        {
            ["DefaultCase"] = "Upper",
            ["DefaultTextFormat"] = "D",
            ["DefaultJsonFormat"] = "N",
            ["DefaultDatabaseFormat"] = "N",
            ["Lock"] = "false",
        };

        IConfiguration Section = new ConfigurationBuilder()
            .AddInMemoryCollection(Values)
            .Build();

        GsIdOptionsConfiguration.ConfigureFromSection(Section);

        Assert.Equal(GsIdCase.Upper, GsIdOptions.DefaultCase);
        Assert.Equal(GsIdFormat.D, GsIdOptions.DefaultTextFormat);
        Assert.Equal(GsIdFormat.N, GsIdOptions.DefaultJsonFormat);
        Assert.Equal(GsIdFormat.N, GsIdOptions.DefaultDatabaseFormat);
        Assert.False(GsIdOptions.IsLocked);
    }
}
