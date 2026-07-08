namespace ChronicleKeeper.Core.Exceptions
{
    /// <summary>
    /// Pozivatelj nema pravo nad traženim resursom (npr. nije vlasnik svijeta).
    /// GlobalExceptionMiddleware je pretvara u HTTP 403 s porukom.
    /// </summary>
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string message) : base(message) { }
    }
}
