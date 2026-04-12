namespace GalacticShrine.GsId;

/**
 * <summary>
 *   [FR] Fournit les opérations de validation des représentations textuelles GsId.
 *   [EN] Provides validation operations for textual GsId representations.
 * </summary>
 **/
public static class GsIdValidator
{
    /**
     * <summary>
     *   [FR] Indique si la valeur représente un GsId valide, quel que soit le format supporté.
     *   [EN] Indicates whether the value represents a valid GsId in any supported format.
     * </summary>
     * <param name="Value">
     *   [FR] Valeur à valider.
     *   [EN] Value to validate.
     * </param>
     * <returns>
     *   [FR] <see langword="true"/> si la valeur est valide ; sinon <see langword="false"/>.
     *   [EN] <see langword="true"/> when the value is valid; otherwise <see langword="false"/>.
     * </returns>
     **/
    public static bool IsValid(string? Value)
        => GsIdParser.TryParse(Value, out _);

    /**
     * <summary>
     *   [FR] Indique si la valeur représente un GsId valide dans un format précis.
     *   [EN] Indicates whether the value represents a valid GsId in a specific format.
     * </summary>
     * <param name="Value">
     *   [FR] Valeur à valider.
     *   [EN] Value to validate.
     * </param>
     * <param name="Format">
     *   [FR] Format attendu.
     *   [EN] Expected format.
     * </param>
     * <returns>
     *   [FR] <see langword="true"/> si la valeur correspond au format attendu ; sinon <see langword="false"/>.
     *   [EN] <see langword="true"/> when the value matches the expected format; otherwise <see langword="false"/>.
     * </returns>
     **/
    public static bool IsValid(string? Value, GsIdFormat Format)
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            return false;
        }

        return Format switch
        {
            GsIdFormat.N => Value.Trim().Length == GsIdConstants.HexLength && IsValid(Value),
            GsIdFormat.D => Value.Trim().Length == GsIdConstants.FormattedLength && IsValid(Value),
            _ => false,
        };
    }
}
