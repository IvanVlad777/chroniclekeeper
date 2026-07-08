namespace ChronicleKeeper.Core.Exceptions
{
    /// <summary>
    /// Poslovna validacija nije prošla (npr. rasa ne pripada vrsti, svijet ne postoji).
    /// GlobalExceptionMiddleware je pretvara u HTTP 400 s porukom.
    /// </summary>
    public class DomainValidationException : Exception
    {
        public DomainValidationException(string message) : base(message) { }
    }
}
