using System;

namespace GalacticShrine.GsId;

/**
 * <summary>
 *   [FR] Représente une erreur de format liée à un GsId.
 *   [EN] Represents a format error related to a GsId.
 * </summary>
 **/
public sealed class GsIdException : FormatException
{
    /**
     * <summary>
     *   [FR] Initialise une nouvelle instance vide.
     *   [EN] Initializes a new empty instance.
     * </summary>
     **/
    public GsIdException()
    {
    }

    /**
     * <summary>
     *   [FR] Initialise une nouvelle instance avec un message.
     *   [EN] Initializes a new instance with a message.
     * </summary>
     * <param name="Message">
     *   [FR] Message de l'exception.
     *   [EN] Exception message.
     * </param>
     **/
    public GsIdException(string? Message)
        : base(Message)
    {
    }

    /**
     * <summary>
     *   [FR] Initialise une nouvelle instance avec un message et une exception interne.
     *   [EN] Initializes a new instance with a message and an inner exception.
     * </summary>
     * <param name="Message">
     *   [FR] Message de l'exception.
     *   [EN] Exception message.
     * </param>
     * <param name="InnerException">
     *   [FR] Exception interne.
     *   [EN] Inner exception.
     * </param>
     **/
    public GsIdException(string? Message, Exception? InnerException)
        : base(Message, InnerException)
    {
    }
}
