using System.Security.Cryptography;

namespace GalacticShrine.GsId;

/**
 * <summary>
 *   [FR] Fournit les mécanismes de génération sécurisée des GsId.
 *   [EN] Provides secure GsId generation mechanisms.
 * </summary>
 **/
public static class GsIdGenerator
{
    /**
     * <summary>
     *   [FR] Génère un nouveau GsId aléatoire sur 256 bits.
     *   [EN] Generates a new random 256-bit GsId.
     * </summary>
     * <returns>
     *   [FR] Un nouvel identifiant GsId.
     *   [EN] A new GsId identifier.
     * </returns>
     **/
    public static GsId NewGsId()
    {
        Span<byte> Bytes = stackalloc byte[GsIdConstants.ByteLength];
        RandomNumberGenerator.Fill(Bytes);

        return new GsId(Bytes);
    }
}
