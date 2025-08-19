namespace Hackton.Domain.VideoResult.Exceptions
{
    public class VideoResultNotFound : Exception
    {
        public VideoResultNotFound() : base(message: "Resultado não encontrado.")
        {

        }
    }
}
