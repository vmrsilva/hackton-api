namespace Hackton.Domain.Video.Exceptions
{
    public class VideoBrokerMessageFailException : Exception
    {
        public VideoBrokerMessageFailException() : base("Falha o enviar video para prcessamento.")
        {

        }
    }
}
