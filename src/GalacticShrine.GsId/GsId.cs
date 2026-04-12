using System;
using System.Buffers.Binary;

namespace GalacticShrine.GsId;

/**
 * <summary>
 *   [FR] Représente un identifiant GsId 256 bits.
 *   [EN] Represents a 256-bit GsId identifier.
 * </summary>
 **/
public readonly struct GsId : IEquatable<GsId>, IFormattable
{
    private const string UpperHexAlphabet = "0123456789ABCDEF";
    private const string LowerHexAlphabet = "0123456789abcdef";

    private readonly ulong _Part1;
    private readonly ulong _Part2;
    private readonly ulong _Part3;
    private readonly ulong _Part4;

    private GsId(ulong Part1, ulong Part2, ulong Part3, ulong Part4)
    {
        _Part1 = Part1;
        _Part2 = Part2;
        _Part3 = Part3;
        _Part4 = Part4;
    }

    internal GsId(ReadOnlySpan<byte> Bytes)
        : this(
            BinaryPrimitives.ReadUInt64BigEndian(Bytes[..8]),
            BinaryPrimitives.ReadUInt64BigEndian(Bytes.Slice(8, 8)),
            BinaryPrimitives.ReadUInt64BigEndian(Bytes.Slice(16, 8)),
            BinaryPrimitives.ReadUInt64BigEndian(Bytes.Slice(24, 8)))
    {
        if (Bytes.Length != GsIdConstants.ByteLength)
        {
            throw new ArgumentException($"Un GsId doit contenir exactement {GsIdConstants.ByteLength} octets.", nameof(Bytes));
        }
    }

    /**
     * <summary>
     *   [FR] Représente la valeur GsId vide, composée uniquement de zéros.
     *   [EN] Represents the empty GsId value, composed only of zeroes.
     * </summary>
     **/
    public static GsId Empty => default;

    /**
     * <summary>
     *   [FR] Indique si l'identifiant courant est la valeur vide.
     *   [EN] Indicates whether the current identifier is the empty value.
     * </summary>
     **/
    public bool IsEmpty
        => _Part1 == 0UL
        && _Part2 == 0UL
        && _Part3 == 0UL
        && _Part4 == 0UL;

    /**
     * <summary>
     *   [FR] Génère un nouveau GsId 256 bits aléatoire.
     *   [EN] Generates a new random 256-bit GsId.
     * </summary>
     * <returns>
     *   [FR] Un nouvel identifiant GsId.
     *   [EN] A new GsId identifier.
     * </returns>
     **/
    public static GsId NewGsId()
        => GsIdGenerator.NewGsId();

    /**
     * <summary>
     *   [FR] Analyse une chaîne au format GsId et retourne l'identifiant correspondant.
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
        => GsIdParser.Parse(Value);

    /**
     * <summary>
     *   [FR] Tente d'analyser une chaîne au format GsId sans lever d'exception en cas d'échec.
     *   [EN] Attempts to parse a GsId string without throwing an exception on failure.
     * </summary>
     * <param name="Value">
     *   [FR] Chaîne GsId à analyser.
     *   [EN] GsId string to parse.
     * </param>
     * <param name="Result">
     *   [FR] Identifiant analysé lorsque l'opération réussit ; sinon <see cref="Empty"/>.
     *   [EN] Parsed identifier when the operation succeeds; otherwise <see cref="Empty"/>.
     * </param>
     * <returns>
     *   [FR] <see langword="true"/> si l'analyse réussit ; sinon <see langword="false"/>.
     *   [EN] <see langword="true"/> when parsing succeeds; otherwise <see langword="false"/>.
     * </returns>
     **/
    public static bool TryParse(string? Value, out GsId Result)
        => GsIdParser.TryParse(Value, out Result);

    /**
     * <summary>
     *   [FR] Retourne les 32 octets bruts de l'identifiant.
     *   [EN] Returns the 32 raw bytes of the identifier.
     * </summary>
     * <returns>
     *   [FR] Tableau contenant les octets bruts du GsId.
     *   [EN] Array containing the raw GsId bytes.
     * </returns>
     **/
    public byte[] ToByteArray()
    {
        byte[] Bytes = new byte[GsIdConstants.ByteLength];
        WriteBytes(Bytes);
        return Bytes;
    }

    /**
     * <summary>
     *   [FR] Retourne la représentation chaîne selon le format demandé avec la casse globale par défaut.
     *   [EN] Returns the string representation according to the requested format using the global default casing.
     * </summary>
     * <param name="Format">
     *   [FR] Format de sortie à utiliser.
     *   [EN] Output format to use.
     * </param>
     * <returns>
     *   [FR] Représentation texte du GsId.
     *   [EN] Text representation of the GsId.
     * </returns>
     **/
    public string ToString(GsIdFormat Format)
        => ToString(Format, GsIdOptions.DefaultCase);

    /**
     * <summary>
     *   [FR] Retourne la représentation chaîne selon le format et la casse demandés.
     *   [EN] Returns the string representation according to the requested format and casing.
     * </summary>
     * <param name="Format">
     *   [FR] Format de sortie à utiliser.
     *   [EN] Output format to use.
     * </param>
     * <param name="Case">
     *   [FR] Casse de sortie à utiliser.
     *   [EN] Output casing to use.
     * </param>
     * <returns>
     *   [FR] Représentation texte du GsId.
     *   [EN] Text representation of the GsId.
     * </returns>
     **/
    public string ToString(GsIdFormat Format, GsIdCase Case)
    {
        return Format switch
        {
            GsIdFormat.N => FormatN(Case),
            GsIdFormat.D => FormatD(Case),
            _ => throw new FormatException($"Le format GsId '{Format}' n'est pas supporté."),
        };
    }

    /**
     * <summary>
     *   [FR] Retourne la représentation normalisée au format N avec la casse globale par défaut.
     *   [EN] Returns the normalized N-format representation using the global default casing.
     * </summary>
     * <returns>
     *   [FR] Chaîne normalisée au format N.
     *   [EN] Normalized N-format string.
     * </returns>
     **/
    public string ToNormalizedString()
        => ToString(GsIdFormat.N, GsIdOptions.DefaultCase);

    /**
     * <summary>
     *   [FR] Retourne la représentation normalisée au format N avec la casse demandée.
     *   [EN] Returns the normalized N-format representation using the requested casing.
     * </summary>
     * <param name="Case">
     *   [FR] Casse de sortie à utiliser.
     *   [EN] Output casing to use.
     * </param>
     * <returns>
     *   [FR] Chaîne normalisée au format N.
     *   [EN] Normalized N-format string.
     * </returns>
     **/
    public string ToNormalizedString(GsIdCase Case)
        => ToString(GsIdFormat.N, Case);

    /**
     * <summary>
     *   [FR] Retourne la représentation texte par défaut selon <see cref="GsIdOptions.DefaultTextFormat"/> et <see cref="GsIdOptions.DefaultCase"/>.
     *   [EN] Returns the default text representation according to <see cref="GsIdOptions.DefaultTextFormat"/> and <see cref="GsIdOptions.DefaultCase"/>.
     * </summary>
     * <returns>
     *   [FR] Représentation texte par défaut du GsId.
     *   [EN] Default text representation of the GsId.
     * </returns>
     **/
    public override string ToString()
        => ToString(GsIdOptions.DefaultTextFormat, GsIdOptions.DefaultCase);

    /**
     * <summary>
     *   [FR] Retourne une représentation formatée compatible avec <see cref="IFormattable"/>.
     *   [EN] Returns a formatted representation compatible with <see cref="IFormattable"/>.
     * </summary>
     * <param name="Format">
     *   [FR] Format demandé. `N` et `D` forcent les majuscules ; `n` et `d` forcent les minuscules.
     *   [EN] Requested format. `N` and `D` force uppercase; `n` and `d` force lowercase.
     * </param>
     * <param name="FormatProvider">
     *   [FR] Fournisseur de format, non utilisé.
     *   [EN] Format provider, unused.
     * </param>
     * <returns>
     *   [FR] Représentation texte formatée.
     *   [EN] Formatted text representation.
     * </returns>
     **/
    public string ToString(string? Format, IFormatProvider? FormatProvider)
    {
        _ = FormatProvider;

        if (string.IsNullOrWhiteSpace(Format))
        {
            return ToString();
        }

        return Format[0] switch
        {
            'N' => ToString(GsIdFormat.N, GsIdCase.Upper),
            'D' => ToString(GsIdFormat.D, GsIdCase.Upper),
            'n' => ToString(GsIdFormat.N, GsIdCase.Lower),
            'd' => ToString(GsIdFormat.D, GsIdCase.Lower),
            _ => throw new FormatException($"Le format GsId '{Format}' n'est pas supporté."),
        };
    }

    /**
     * <summary>
     *   [FR] Tente d'écrire la représentation formatée dans un tampon de caractères.
     *   [EN] Attempts to write the formatted representation into a character buffer.
     * </summary>
     * <param name="Destination">
     *   [FR] Tampon de destination.
     *   [EN] Destination buffer.
     * </param>
     * <param name="CharsWritten">
     *   [FR] Nombre de caractères écrits.
     *   [EN] Number of characters written.
     * </param>
     * <param name="Format">
     *   [FR] Format demandé. Vide utilise les options globales ; `N`/`D` majuscules ; `n`/`d` minuscules.
     *   [EN] Requested format. Empty uses global options; `N`/`D` uppercase; `n`/`d` lowercase.
     * </param>
     * <returns>
     *   [FR] <see langword="true"/> si l'écriture réussit ; sinon <see langword="false"/>.
     *   [EN] <see langword="true"/> when writing succeeds; otherwise <see langword="false"/>.
     * </returns>
     **/
    public bool TryFormat(Span<char> Destination, out int CharsWritten, ReadOnlySpan<char> Format = default)
    {
        if (Format.IsEmpty)
        {
            return TryFormat(Destination, out CharsWritten, GsIdOptions.DefaultTextFormat, GsIdOptions.DefaultCase);
        }

        return Format[0] switch
        {
            'N' => TryFormat(Destination, out CharsWritten, GsIdFormat.N, GsIdCase.Upper),
            'D' => TryFormat(Destination, out CharsWritten, GsIdFormat.D, GsIdCase.Upper),
            'n' => TryFormat(Destination, out CharsWritten, GsIdFormat.N, GsIdCase.Lower),
            'd' => TryFormat(Destination, out CharsWritten, GsIdFormat.D, GsIdCase.Lower),
            _ => throw new FormatException($"Le format GsId '{Format[0]}' n'est pas supporté."),
        };
    }

    /**
     * <summary>
     *   [FR] Tente d'écrire la représentation formatée avec la casse globale par défaut.
     *   [EN] Attempts to write the formatted representation using the global default casing.
     * </summary>
     * <param name="Destination">
     *   [FR] Tampon de destination.
     *   [EN] Destination buffer.
     * </param>
     * <param name="CharsWritten">
     *   [FR] Nombre de caractères écrits.
     *   [EN] Number of characters written.
     * </param>
     * <param name="Format">
     *   [FR] Format de sortie à utiliser.
     *   [EN] Output format to use.
     * </param>
     * <returns>
     *   [FR] <see langword="true"/> si l'écriture réussit ; sinon <see langword="false"/>.
     *   [EN] <see langword="true"/> when writing succeeds; otherwise <see langword="false"/>.
     * </returns>
     **/
    public bool TryFormat(Span<char> Destination, out int CharsWritten, GsIdFormat Format)
        => TryFormat(Destination, out CharsWritten, Format, GsIdOptions.DefaultCase);

    /**
     * <summary>
     *   [FR] Tente d'écrire la représentation formatée avec le format et la casse demandés.
     *   [EN] Attempts to write the formatted representation using the requested format and casing.
     * </summary>
     * <param name="Destination">
     *   [FR] Tampon de destination.
     *   [EN] Destination buffer.
     * </param>
     * <param name="CharsWritten">
     *   [FR] Nombre de caractères écrits.
     *   [EN] Number of characters written.
     * </param>
     * <param name="Format">
     *   [FR] Format de sortie à utiliser.
     *   [EN] Output format to use.
     * </param>
     * <param name="Case">
     *   [FR] Casse de sortie à utiliser.
     *   [EN] Output casing to use.
     * </param>
     * <returns>
     *   [FR] <see langword="true"/> si l'écriture réussit ; sinon <see langword="false"/>.
     *   [EN] <see langword="true"/> when writing succeeds; otherwise <see langword="false"/>.
     * </returns>
     **/
    public bool TryFormat(Span<char> Destination, out int CharsWritten, GsIdFormat Format, GsIdCase Case)
    {
        Span<char> NormalizedValue = stackalloc char[GsIdConstants.HexLength];
        WriteNormalizedCharacters(NormalizedValue, Case);

        if (Format == GsIdFormat.N)
        {
            if (Destination.Length < GsIdConstants.HexLength)
            {
                CharsWritten = 0;
                return false;
            }

            NormalizedValue.CopyTo(Destination);
            CharsWritten = GsIdConstants.HexLength;
            return true;
        }

        if (Format == GsIdFormat.D)
        {
            if (Destination.Length < GsIdConstants.FormattedLength)
            {
                CharsWritten = 0;
                return false;
            }

            NormalizedValue[..16].CopyTo(Destination[..16]);
            Destination[16] = '-';

            NormalizedValue.Slice(16, 8).CopyTo(Destination.Slice(17, 8));
            Destination[25] = '-';

            NormalizedValue.Slice(24, 8).CopyTo(Destination.Slice(26, 8));
            Destination[34] = '-';

            NormalizedValue.Slice(32, 8).CopyTo(Destination.Slice(35, 8));
            Destination[43] = '-';

            NormalizedValue.Slice(40, 8).CopyTo(Destination.Slice(44, 8));
            Destination[52] = '-';

            NormalizedValue.Slice(48, 16).CopyTo(Destination.Slice(53, 16));

            CharsWritten = GsIdConstants.FormattedLength;
            return true;
        }

        throw new FormatException($"Le format GsId '{Format}' n'est pas supporté.");
    }

    /**
     * <summary>
     *   [FR] Compare l'identifiant courant avec un autre GsId.
     *   [EN] Compares the current identifier with another GsId.
     * </summary>
     * <param name="Other">
     *   [FR] Autre identifiant à comparer.
     *   [EN] Other identifier to compare.
     * </param>
     * <returns>
     *   [FR] <see langword="true"/> si les deux identifiants sont égaux ; sinon <see langword="false"/>.
     *   [EN] <see langword="true"/> when both identifiers are equal; otherwise <see langword="false"/>.
     * </returns>
     **/
    public bool Equals(GsId Other)
        => _Part1 == Other._Part1
        && _Part2 == Other._Part2
        && _Part3 == Other._Part3
        && _Part4 == Other._Part4;

    /**
     * <summary>
     *   [FR] Compare l'identifiant courant avec un objet.
     *   [EN] Compares the current identifier with an object.
     * </summary>
     * <param name="Object">
     *   [FR] Objet à comparer.
     *   [EN] Object to compare.
     * </param>
     * <returns>
     *   [FR] <see langword="true"/> si l'objet représente le même GsId ; sinon <see langword="false"/>.
     *   [EN] <see langword="true"/> when the object represents the same GsId; otherwise <see langword="false"/>.
     * </returns>
     **/
    public override bool Equals(object? Object)
        => Object is GsId Other && Equals(Other);

    /**
     * <summary>
     *   [FR] Retourne le code de hachage de l'identifiant courant.
     *   [EN] Returns the hash code of the current identifier.
     * </summary>
     * <returns>
     *   [FR] Code de hachage de l'identifiant.
     *   [EN] Hash code of the identifier.
     * </returns>
     **/
    public override int GetHashCode()
        => HashCode.Combine(_Part1, _Part2, _Part3, _Part4);

    /**
     * <summary>
     *   [FR] Indique si deux GsId sont égaux.
     *   [EN] Indicates whether two GsId values are equal.
     * </summary>
     * <param name="Left">
     *   [FR] Premier GsId à comparer.
     *   [EN] First GsId to compare.
     * </param>
     * <param name="Right">
     *   [FR] Second GsId à comparer.
     *   [EN] Second GsId to compare.
     * </param>
     * <returns>
     *   [FR] <see langword="true"/> si les valeurs sont égales ; sinon <see langword="false"/>.
     *   [EN] <see langword="true"/> when the values are equal; otherwise <see langword="false"/>.
     * </returns>
     **/
    public static bool operator ==(GsId Left, GsId Right)
        => Left.Equals(Right);

    /**
     * <summary>
     *   [FR] Indique si deux GsId sont différents.
     *   [EN] Indicates whether two GsId values are different.
     * </summary>
     * <param name="Left">
     *   [FR] Premier GsId à comparer.
     *   [EN] First GsId to compare.
     * </param>
     * <param name="Right">
     *   [FR] Second GsId à comparer.
     *   [EN] Second GsId to compare.
     * </param>
     * <returns>
     *   [FR] <see langword="true"/> si les valeurs sont différentes ; sinon <see langword="false"/>.
     *   [EN] <see langword="true"/> when the values are different; otherwise <see langword="false"/>.
     * </returns>
     **/
    public static bool operator !=(GsId Left, GsId Right)
        => !Left.Equals(Right);

    private string FormatN(GsIdCase Case)
    {
        Span<char> Buffer = stackalloc char[GsIdConstants.HexLength];
        _ = TryFormat(Buffer, out _, GsIdFormat.N, Case);
        return new string(Buffer);
    }

    private string FormatD(GsIdCase Case)
    {
        Span<char> Buffer = stackalloc char[GsIdConstants.FormattedLength];
        _ = TryFormat(Buffer, out _, GsIdFormat.D, Case);
        return new string(Buffer);
    }

    private void WriteNormalizedCharacters(Span<char> Destination, GsIdCase Case)
    {
        string HexAlphabet = Case == GsIdCase.Lower
            ? LowerHexAlphabet
            : UpperHexAlphabet;

        Span<byte> Bytes = stackalloc byte[GsIdConstants.ByteLength];
        WriteBytes(Bytes);

        for (int Index = 0; Index < Bytes.Length; Index++)
        {
            byte Value = Bytes[Index];
            Destination[Index * 2] = HexAlphabet[Value >> 4];
            Destination[(Index * 2) + 1] = HexAlphabet[Value & 0x0F];
        }
    }

    private void WriteBytes(Span<byte> Destination)
    {
        if (Destination.Length < GsIdConstants.ByteLength)
        {
            throw new ArgumentException($"Le tampon de destination doit contenir au moins {GsIdConstants.ByteLength} octets.", nameof(Destination));
        }

        BinaryPrimitives.WriteUInt64BigEndian(Destination[..8], _Part1);
        BinaryPrimitives.WriteUInt64BigEndian(Destination.Slice(8, 8), _Part2);
        BinaryPrimitives.WriteUInt64BigEndian(Destination.Slice(16, 8), _Part3);
        BinaryPrimitives.WriteUInt64BigEndian(Destination.Slice(24, 8), _Part4);
    }
}
