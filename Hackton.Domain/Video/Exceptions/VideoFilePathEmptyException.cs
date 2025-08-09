namespace Hackton.Domain.Video.Exceptions
{
    public class VideoFilePathEmptyException : Exception
    {
        public VideoFilePathEmptyException() : base(message: "Diretório do video não encontrado.")
        {

        }
    }
}
