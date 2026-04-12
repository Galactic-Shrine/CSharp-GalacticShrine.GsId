using System;

namespace GalacticShrine.GsId;

/**
 * <summary>
 *   [FR] Fournit les opérations de normalisation et de conversion des chaînes GsId.
 *   [EN] Provides normalization and conversion operations for GsId strings.
 * </summary>
 **/
public static class GsIdParser
{
    /**
     * <summary>
     *   [FR] Analyse une chaîne GsId et retourne l'identifiant correspondant.
     *   [EN] Parses a GsId string and returns the corresponding identifier.
     * </summary>
     * <param name="Value">
     *   [FR] Chaîne GsId à analyser.
     *   [EN] GsId string to parse.
     * </param>
     * <returns>
     *   [FR] L'identifiant GsId analysé.
     *   [EN] The parsed GsId identifier.
     * </returns>
     **/
    public static GsId Parse(string Value)
    {
        string NormalizedValue = Normalize(Value, GsIdCase.Upper);
        Span<byte> Bytes = stackalloc byte[GsIdConstants.ByteLength];

        for (int Index = 0; Index < GsIdConstants.ByteLength; Index++)
        {
            int Left = ConvertHexChar(NormalizedValue[Index * 2]);
            int Right = ConvertHexChar(NormalizedValue[(Index * 2) + 1]);
            Bytes[Index] = (byte)((Left << 4) | Right);
        }

        return new GsId(Bytes);
    }

    /**
     * <summary>
     *   [FR] Tente d'analyser une chaîne GsId sans lever d'exception en cas d'échec.
     *   [EN] Attempts to parse a GsId string without throwing an exception on failure.
     * </summary>
     * <param name="Value">
     *   [FR] Chaîne GsId à analyser.
     *   [EN] GsId string to parse.
     * </param>
     * <param name="Result">
     *   [FR] Identifiant analysé lorsque l'opération réussit ; sinon <see cref="GsId.Empty"/>.
     *   [EN] Parsed identifier when the operation succeeds; otherwise <see cref="GsId.Empty"/>.
     * </param>
     * <returns>
     *   [FR] <see langword="true"/> si l'analyse réussit ; sinon <see langword="false"/>.
     *   [EN] <see langword="true"/> when parsing succeeds; otherwise <see langword="false"/>.
     * </returns>
     **/
    public static bool TryParse(string? Value, out GsId Result)
    {
        Result = GsId.Empty;

        if (string.IsNullOrWhiteSpace(Value))
        {
            return false;
        }

        try
        {
            Result = Parse(Value);
            return true;
        }
        catch (GsIdException)
        {
            return false;
        }
    }

    /**
     * <summary>
     *   [FR] Normalise une chaîne GsId vers son format N en utilisant la casse globale par défaut.
     *   [EN] Normalizes a GsId string to its N format using the global default casing.
     * </summary>
     * <param name="Value">
     *   [FR] Valeur GsId à normaliser.
     *   [EN] GsId value to normalize.
     * </param>
     * <returns>
     *   [FR] Chaîne normalisée au format N.
     *   [EN] Normalized N-format string.
     * </returns>
     **/
    public static string Normalize(string Value)
        => Normalize(Value, GsIdOptions.DefaultCase);

    /**
     * <summary>
     *   [FR] Normalise une chaîne GsId vers son format N avec la casse demandée.
     *   [EN] Normalizes a GsId string to its N format using the requested casing.
     * </summary>
     * <param name="Value">
     *   [FR] Valeur GsId à normaliser.
     *   [EN] GsId value to normalize.
     * </param>
     * <param name="Case">
     *   [FR] Casse de sortie à appliquer.
     *   [EN] Output casing to apply.
     * </param>
     * <returns>
     *   [FR] Chaîne normalisée au format N.
     *   [EN] Normalized N-format string.
     * </returns>
     **/
    public static string Normalize(string Value, GsIdCase Case)
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            throw new GsIdException("La valeur GsId ne peut pas être nulle, vide ou blanche.");
        }

        string TrimmedValue = Value.Trim();

        if (TrimmedValue.Length == GsIdConstants.HexLength)
        {
            return NormalizeN(TrimmedValue, Case);
        }

        if (TrimmedValue.Length == GsIdConstants.FormattedLength)
        {
            return NormalizeD(TrimmedValue, Case);
        }

        throw new GsIdException(
            $"La valeur GsId doit contenir {GsIdConstants.HexLength} caractères sans tirets ou {GsIdConstants.FormattedLength} caractères avec tirets.");
    }

    private static string NormalizeN(string Value, GsIdCase Case)
    {
        Span<char> Buffer = stackalloc char[GsIdConstants.HexLength];

        for (int Index = 0; Index < Value.Length; Index++)
        {
            char Character = Value[Index];

            if (!IsHexCharacter(Character))
            {
                throw new GsIdException($"Le caractère '{Character}' n'est pas hexadécimal.");
            }

            Buffer[Index] = ApplyCase(Character, Case);
        }

        return new string(Buffer);
    }

    private static string NormalizeD(string Value, GsIdCase Case)
    {
        Span<char> Buffer = stackalloc char[GsIdConstants.HexLength];
        int BufferIndex = 0;

        for (int Index = 0; Index < Value.Length; Index++)
        {
            char Character = Value[Index];

            if (IsHyphenPosition(Index))
            {
                if (Character != '-')
                {
                    throw new GsIdException("La valeur GsId n'utilise pas les positions de tirets officielles.");
                }

                continue;
            }

            if (!IsHexCharacter(Character))
            {
                throw new GsIdException($"Le caractère '{Character}' n'est pas hexadécimal.");
            }

            Buffer[BufferIndex++] = ApplyCase(Character, Case);
        }

        return new string(Buffer);
    }

    private static bool IsHyphenPosition(int Index)
        => Index is 16 or 25 or 34 or 43 or 52;

    private static bool IsHexCharacter(char Character)
        => (Character >= '0' && Character <= '9')
           || (Character >= 'a' && Character <= 'f')
           || (Character >= 'A' && Character <= 'F');

    private static char ApplyCase(char Character, GsIdCase Case)
        => Case == GsIdCase.Lower
            ? char.ToLowerInvariant(Character)
            : char.ToUpperInvariant(Character);

    private static int ConvertHexChar(char Character)
        => Character switch
        {
            >= '0' and <= '9' => Character - '0',
            >= 'a' and <= 'f' => 10 + (Character - 'a'),
            >= 'A' and <= 'F' => 10 + (Character - 'A'),
            _ => throw new GsIdException($"Le caractère '{Character}' n'est pas hexadécimal."),
        };
}
