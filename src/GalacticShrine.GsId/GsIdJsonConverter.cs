using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GalacticShrine.GsId;

/**
 * <summary>
 *   [FR] Fournit la sérialisation JSON d'un GsId.
 *   [EN] Provides JSON serialization for a GsId.
 * </summary>
 **/
public sealed class GsIdJsonConverter : JsonConverter<GsId>
{
    private readonly GsIdFormat? _Format;
    private readonly GsIdCase? _Case;

    /**
     * <summary>
     *   [FR] Initialise une nouvelle instance en utilisant le format JSON global et la casse globale.
     *   [EN] Initializes a new instance using the global JSON format and global casing.
     * </summary>
     **/
    public GsIdJsonConverter()
        : this(null, null)
    {
    }

    /**
     * <summary>
     *   [FR] Initialise une nouvelle instance avec un format explicite et la casse globale.
     *   [EN] Initializes a new instance with an explicit format and the global casing.
     * </summary>
     * <param name="Format">
     *   [FR] Format JSON à utiliser.
     *   [EN] JSON format to use.
     * </param>
     **/
    public GsIdJsonConverter(GsIdFormat Format)
        : this(Format, null)
    {
    }

    /**
     * <summary>
     *   [FR] Initialise une nouvelle instance avec un format et une casse explicites.
     *   [EN] Initializes a new instance with an explicit format and casing.
     * </summary>
     * <param name="Format">
     *   [FR] Format JSON à utiliser, ou <see langword="null"/> pour utiliser les options globales.
     *   [EN] JSON format to use, or <see langword="null"/> to use global options.
     * </param>
     * <param name="Case">
     *   [FR] Casse JSON à utiliser, ou <see langword="null"/> pour utiliser les options globales.
     *   [EN] JSON casing to use, or <see langword="null"/> to use global options.
     * </param>
     **/
    public GsIdJsonConverter(GsIdFormat? Format, GsIdCase? Case)
    {
        _Format = Format;
        _Case = Case;
    }

    /**
     * <summary>
     *   [FR] Lit un GsId depuis une valeur chaîne JSON.
     *   [EN] Reads a GsId from a JSON string value.
     * </summary>
     * <param name="Reader">
     *   [FR] Lecteur JSON UTF-8.
     *   [EN] UTF-8 JSON reader.
     * </param>
     * <param name="TypeToConvert">
     *   [FR] Type cible à convertir.
     *   [EN] Target type to convert.
     * </param>
     * <param name="Options">
     *   [FR] Options du sérialiseur JSON.
     *   [EN] JSON serializer options.
     * </param>
     * <returns>
     *   [FR] GsId désérialisé.
     *   [EN] Deserialized GsId.
     * </returns>
     **/
    public override GsId Read(ref Utf8JsonReader Reader, Type TypeToConvert, JsonSerializerOptions Options)
    {
        string? Value = Reader.GetString();

        if (string.IsNullOrWhiteSpace(Value))
        {
            throw new JsonException("La valeur JSON GsId doit être une chaîne non vide.");
        }

        try
        {
            return GsId.Parse(Value);
        }
        catch (Exception Exception)
        {
            throw new JsonException("Impossible de désérialiser la valeur GsId.", Exception);
        }
    }

    /**
     * <summary>
     *   [FR] Écrit un GsId sous forme de chaîne JSON.
     *   [EN] Writes a GsId as a JSON string.
     * </summary>
     * <param name="Writer">
     *   [FR] Écrivain JSON UTF-8.
     *   [EN] UTF-8 JSON writer.
     * </param>
     * <param name="Value">
     *   [FR] Valeur GsId à écrire.
     *   [EN] GsId value to write.
     * </param>
     * <param name="Options">
     *   [FR] Options du sérialiseur JSON.
     *   [EN] JSON serializer options.
     * </param>
     **/
    public override void Write(Utf8JsonWriter Writer, GsId Value, JsonSerializerOptions Options)
        => Writer.WriteStringValue(Value.ToString(_Format ?? GsIdOptions.DefaultJsonFormat, _Case ?? GsIdOptions.DefaultCase));
}
