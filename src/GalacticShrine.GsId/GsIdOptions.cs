using System;

namespace GalacticShrine.GsId;

/**
 * <summary>
 *   [FR] Centralise les options globales du système GsId.
 *   [EN] Centralizes the global options of the GsId system.
 * </summary>
 **/
public static class GsIdOptions
{
    private static GsIdCase _DefaultCase = GsIdCase.Upper;
    private static GsIdFormat _DefaultTextFormat = GsIdFormat.D;
    private static GsIdFormat _DefaultJsonFormat = GsIdFormat.D;
    private static GsIdFormat _DefaultDatabaseFormat = GsIdFormat.N;
    private static bool _IsLocked;

    /**
     * <summary>
     *   [FR] Définit la casse par défaut utilisée par les opérations qui ne la précisent pas explicitement.
     *   [EN] Defines the default casing used by operations that do not explicitly specify it.
     * </summary>
     **/
    public static GsIdCase DefaultCase
    {
        get => _DefaultCase;
        set
        {
            EnsureUnlocked();
            _DefaultCase = value;
        }
    }

    /**
     * <summary>
     *   [FR] Définit le format texte par défaut utilisé par ToString() et TryFormat() sans format explicite.
     *   [EN] Defines the default text format used by ToString() and TryFormat() without an explicit format.
     * </summary>
     **/
    public static GsIdFormat DefaultTextFormat
    {
        get => _DefaultTextFormat;
        set
        {
            EnsureUnlocked();
            _DefaultTextFormat = ValidateFormat(value, nameof(DefaultTextFormat));
        }
    }

    /**
     * <summary>
     *   [FR] Définit le format JSON par défaut utilisé par GsIdJsonConverter sans format explicite.
     *   [EN] Defines the default JSON format used by GsIdJsonConverter without an explicit format.
     * </summary>
     **/
    public static GsIdFormat DefaultJsonFormat
    {
        get => _DefaultJsonFormat;
        set
        {
            EnsureUnlocked();
            _DefaultJsonFormat = ValidateFormat(value, nameof(DefaultJsonFormat));
        }
    }

    /**
     * <summary>
     *   [FR] Définit le format base de données par défaut utilisé par les intégrations de persistance.
     *   [EN] Defines the default database format used by persistence integrations.
     * </summary>
     **/
    public static GsIdFormat DefaultDatabaseFormat
    {
        get => _DefaultDatabaseFormat;
        set
        {
            EnsureUnlocked();
            _DefaultDatabaseFormat = ValidateFormat(value, nameof(DefaultDatabaseFormat));
        }
    }

    /**
     * <summary>
     *   [FR] Indique si les options globales sont verrouillées.
     *   [EN] Indicates whether the global options are locked.
     * </summary>
     **/
    public static bool IsLocked
        => _IsLocked;

    /**
     * <summary>
     *   [FR] Configure en une seule opération les options globales du système GsId.
     *   [EN] Configures the global options of the GsId system in a single operation.
     * </summary>
     **/
    public static void Configure(
        GsIdCase? DefaultCase = null,
        GsIdFormat? DefaultTextFormat = null,
        GsIdFormat? DefaultJsonFormat = null,
        GsIdFormat? DefaultDatabaseFormat = null)
    {
        EnsureUnlocked();

        if (DefaultCase.HasValue)
        {
            _DefaultCase = DefaultCase.Value;
        }

        if (DefaultTextFormat.HasValue)
        {
            _DefaultTextFormat = ValidateFormat(DefaultTextFormat.Value, nameof(DefaultTextFormat));
        }

        if (DefaultJsonFormat.HasValue)
        {
            _DefaultJsonFormat = ValidateFormat(DefaultJsonFormat.Value, nameof(DefaultJsonFormat));
        }

        if (DefaultDatabaseFormat.HasValue)
        {
            _DefaultDatabaseFormat = ValidateFormat(DefaultDatabaseFormat.Value, nameof(DefaultDatabaseFormat));
        }
    }

    /**
     * <summary>
     *   [FR] Verrouille les options globales pour empêcher toute modification ultérieure.
     *   [EN] Locks the global options to prevent any further changes.
     * </summary>
     **/
    public static void Lock()
        => _IsLocked = true;

    /**
     * <summary>
     *   [FR] Restaure les options globales par défaut du système GsId.
     *   [EN] Restores the default global options for the GsId system.
     * </summary>
     **/
    public static void Reset()
    {
        EnsureUnlocked();
        _DefaultCase = GsIdCase.Upper;
        _DefaultTextFormat = GsIdFormat.D;
        _DefaultJsonFormat = GsIdFormat.D;
        _DefaultDatabaseFormat = GsIdFormat.N;
    }

    private static void EnsureUnlocked()
    {
        if (_IsLocked)
        {
            throw new InvalidOperationException("Les options GsId sont verrouillées et ne peuvent plus être modifiées.");
        }
    }

    private static GsIdFormat ValidateFormat(GsIdFormat Format, string ParameterName)
        => Format switch
        {
            GsIdFormat.N => GsIdFormat.N,
            GsIdFormat.D => GsIdFormat.D,
            _ => throw new ArgumentOutOfRangeException(ParameterName, Format, "Le format GsId demandé n'est pas supporté."),
        };
}
