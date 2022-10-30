using API.Models.User;
using AutoMapper;
using Common;

namespace API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserModel, DAL.Entities.User>()
                .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashHelper.GetHash(s.Password)))
                .ForMember(d => d.Avatar, m => m.MapFrom(s => s.Avatar == null ? ImageHelper.GenerateImage(8, 8, 3, 2) : _uploadImage(s.Avatar)));

            CreateMap<DAL.Entities.User, UserModel>();
        }

        private byte[] _uploadImage(IFormFile file)
        {
            using MemoryStream ms = new MemoryStream();
            file.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
