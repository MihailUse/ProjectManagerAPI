using API.Models.User;
using AutoMapper;
using Common;
using DAL.Entities;

namespace API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // models to entity
            CreateMap<UpdateUserModel, User>();

            CreateMap<CreateUserModel, User>()
                .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashHelper.GetHash(s.Password)))
                .ForMember(d => d.Avatar, m => m.MapFrom(s => ImageHelper.GenerateImage(8, 8, 3, 2)));

            // entity to model
            CreateMap<User, UserModel>();
        }

        private byte[] _uploadImage(IFormFile file)
        {
            using MemoryStream ms = new MemoryStream();
            file.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
