using System;
using Microsoft.Extensions.Configuration;

namespace GalacticShrine.GsId;

/**
 * <summary>
 *   [FR] Fournit le chargement des options GsId depuis une configuration applicative .NET.
 *   [EN] Provides GsId options loading from a .NET application configuration.
 * </summary>
 **/
public static class GsIdOptionsConfiguration
{
    /**
     * <summary>
     *   [FR] Nom par défaut de la section de configuration GsId.
     *   [EN] Default name of the GsId configuration section.
     * </summary>
     **/
    public const string DefaultSectionName = "GsId";

    /**
     * <summary>
     *   [FR] Configure GsIdOptions depuis une section nommée de IConfiguration.
     *   [EN] Configures GsIdOptions from a named IConfiguration section.
     * </summary>
     * <param name="Configuration">
     *   [FR] Configuration applicative .NET.
     *   [EN] .NET application configuration.
     * </param>
     * <param name="SectionName">
     *   [FR] Nom de la section à lire. Par défaut : GsId.
     *   [EN] Name of the section to read. Default: GsId.
     * </param>
     **/
    public static void ConfigureFromConfiguration(IConfiguration Configuration, string SectionName = DefaultSectionName)
    {
        ArgumentNullException.ThrowIfNull(Configuration);

        if (string.IsNullOrWhiteSpace(SectionName))
        {
            throw new ArgumentException("Le nom de section GsId ne peut pas être vide.", nameof(SectionName));
        }

        ConfigureFromSection(Configuration.GetSection(SectionName));
    }

    /**
     * <summary>
     *   [FR] Configure GsIdOptions depuis une section IConfiguration déjà résolue.
     *   [EN] Configures GsIdOptions from an already resolved IConfiguration section.
     * </summary>
     * <param name="Section">
     *   [FR] Section de configuration GsId.
     *   [EN] GsId configuration section.
     * </param>
     **/
    public static void ConfigureFromSection(IConfiguration Section)
    {
        ArgumentNullException.ThrowIfNull(Section);

        GsIdCase? DefaultCase = ParseCase(Section["DefaultCase"], "DefaultCase");
        GsIdFormat? DefaultTextFormat = ParseFormat(Section["DefaultTextFormat"], "DefaultTextFormat");
        GsIdFormat? DefaultJsonFormat = ParseFormat(Section["DefaultJsonFormat"], "DefaultJsonFormat");
        GsIdFormat? DefaultDatabaseFormat = ParseFormat(Section["DefaultDatabaseFormat"], "DefaultDatabaseFormat");

        GsIdOptions.Configure(
            DefaultCase: DefaultCase,
            DefaultTextFormat: DefaultTextFormat,
            DefaultJsonFormat: DefaultJsonFormat,
            DefaultDatabaseFormat: DefaultDatabaseFormat);

        if (ParseBoolean(Section["Lock"], "Lock") == true)
        {
            GsIdOptions.Lock();
        }
    }

    private static GsIdCase? ParseCase(string? Value, string Key)
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            return null;
        }

        return Value.Trim().ToLowerInvariant() switch
        {
            "upper" or "uppercase" or "majuscule" or "majuscules" => GsIdCase.Upper,
            "lower" or "lowercase" or "minuscule" or "minuscules" => GsIdCase.Lower,
            _ => throw new InvalidOperationException($"La valeur GsId.{Key} '{Value}' est invalide. Valeurs acceptées : Upper, Lower."),
        };
    }

    private static GsIdFormat? ParseFormat(string? Value, string Key)
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            return null;
        }

        return Value.Trim().ToUpperInvariant() switch
        {
            "N" => GsIdFormat.N,
            "D" => GsIdFormat.D,
            _ => throw new InvalidOperationException($"La valeur GsId.{Key} '{Value}' est invalide. Valeurs acceptées : N, D."),
        };
    }

    private static bool? ParseBoolean(string? Value, string Key)
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            return null;
        }

        if (bool.TryParse(Value, out bool Result))
        {
            return Result;
        }

        return Value.Trim().ToLowerInvariant() switch
        {
            "1" or "yes" or "y" or "oui" => true,
            "0" or "no" or "n" or "non" => false,
            _ => throw new InvalidOperationException($"La valeur GsId.{Key} '{Value}' est invalide. Valeurs acceptées : true, false."),
        };
    }
}
