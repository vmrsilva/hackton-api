namespace Hackton.Domain.Video.Exceptions
{
    public class VideoNotFoundException : Exception
    {
        public VideoNotFoundException() : base(message: "Video não encontrado.")
        {

        }
    }
}
