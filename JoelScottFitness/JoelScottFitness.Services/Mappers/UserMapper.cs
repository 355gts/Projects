using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Identity.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class UserMapper : ITypeMapper<AuthUser, UserViewModel>
    {
        public UserViewModel Map(AuthUser fromObject, UserViewModel toObject = null)
        {
            var user = toObject ?? new UserViewModel();

            user.EmailAddress = fromObject.Email;
            user.Id = fromObject.Id;
            user.UserName = fromObject.UserName;

            return user;
        }
    }
}
