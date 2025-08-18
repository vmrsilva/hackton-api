using Hackton.Domain.VideoResult.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackton.Domain.Interfaces.VideoResult.Repository
{
    public interface IVideoResultRepository
    {
        Task<VideoResultEntity> GetByVideoId(Guid VideoId);
    }
}
