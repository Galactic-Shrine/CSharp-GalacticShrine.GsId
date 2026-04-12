namespace GalacticShrine.GsId;

/**
 * <summary>
 *   [FR] Représente les formats de chaîne supportés par GsId.
 *   [EN] Represents the string formats supported by GsId.
 * </summary>
 **/
public enum GsIdFormat
{
    /**
     * <summary>
     *   [FR] Format normalisé sans tirets, sur 64 caractères.
     *   [EN] Normalized format without hyphens, using 64 characters.
     * </summary>
     **/
    N = 0,

    /**
     * <summary>
     *   [FR] Format lisible avec 5 tirets selon le motif 16-8-8-8-8-16.
     *   [EN] Readable format with 5 hyphens using the 16-8-8-8-8-16 pattern.
     * </summary>
     **/
    D = 1,
}
