using FaturasHandler.Data.Context;
using FaturasHandler.Data.Models;

namespace FaturasHandler.Data.Repository
{
    public class UserRepository : Repository<UserData>, IUserRepository

    {
        public UserRepository(FaturaDbContext context) : base(context)
        {
        }


    }
}
