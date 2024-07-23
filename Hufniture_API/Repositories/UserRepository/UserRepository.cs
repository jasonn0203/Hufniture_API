using Hufniture_API.Data;

namespace Hufniture_API.Repositories.UserRepository
{
    public class UserRepository : GenericRepository<AppUser>, IUserRepository
    {
        public UserRepository(HufnitureDbContext context) : base(context)
        {
        }
    }
}
