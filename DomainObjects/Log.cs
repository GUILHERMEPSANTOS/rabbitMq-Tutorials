namespace DomainObjects
{
    public class Log
    {
        public string Message { get; set; }

        public Log(string message)
        {
            Message = message;
        }
    }
}