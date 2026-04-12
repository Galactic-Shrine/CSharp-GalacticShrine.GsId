namespace GalacticShrine.GsId;

/**
 * <summary>
 *   [FR] Définit les constantes officielles du format GsId.
 *   [EN] Defines the official constants of the GsId format.
 * </summary>
 **/
public static class GsIdConstants
{
    /**
     * <summary>
     *   [FR] Nombre d'octets bruts d'un GsId.
     *   [EN] Number of raw bytes contained in a GsId.
     * </summary>
     **/
    public const int ByteLength = 32;

    /**
     * <summary>
     *   [FR] Nombre de caractères hexadécimaux d'un GsId sans tirets.
     *   [EN] Number of hexadecimal characters of a GsId without hyphens.
     * </summary>
     **/
    public const int HexLength = 64;

    /**
     * <summary>
     *   [FR] Nombre de caractères d'un GsId au format D.
     *   [EN] Number of characters of a GsId in D format.
     * </summary>
     **/
    public const int FormattedLength = 69;

    /**
     * <summary>
     *   [FR] Nombre de tirets du format D.
     *   [EN] Number of hyphens in the D format.
     * </summary>
     **/
    public const int HyphenCount = 5;

    /**
     * <summary>
     *   [FR] Motif officiel des groupes pour le format D.
     *   [EN] Official group pattern for the D format.
     * </summary>
     **/
    public const string DGroupPattern = "16-8-8-8-8-16";
}
