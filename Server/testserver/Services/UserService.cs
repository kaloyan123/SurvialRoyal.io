using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testserver.Data;

namespace testserver.Services
{
    public class UserService
    {
        private ApplicationDbContext dbContext;


        public UserService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<PlayerAccount> GetAllByName(string name)
        {
            return dbContext.Players
                .Where(p => p.Name == name)
                .ToList();

        }
    }
}
