namespace ChronicleKeeper.Core.Exceptions
{
    /// <summary>
    /// Tražena domenska stavka ne postoji.
    /// GlobalExceptionMiddleware je pretvara u HTTP 404 s porukom.
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, int id)
            : base($"{entityName} with ID {id} was not found.") { }

        public EntityNotFoundException(string message) : base(message) { }
    }
}
