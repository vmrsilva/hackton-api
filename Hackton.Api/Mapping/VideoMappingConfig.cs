using Hackton.Api.Controllers.Video.Dto;
using Hackton.Domain.Enums;
using Hackton.Domain.Video.Entity;
using Mapster;

namespace Hackton.Api.Mapping
{
    public class VideoMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<VideoEntity, ResponseVideoDto>()
                .ConstructUsing(src => new ResponseVideoDto
                {
                    Id = src.Id,
                    Title = src.Title,
                    Description = src.Description,
                    Active = src.Active,
                    CreateAt = src.CreateAt,
                    Status = src.Status.GetDescription()
                });
        }
    }
}
