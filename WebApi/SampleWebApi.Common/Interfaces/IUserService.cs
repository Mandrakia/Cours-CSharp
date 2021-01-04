using SampleWebApi.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleWebApi.Common.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetShortUsers();
        Task<User> GetFullUser(int id);
        Task UpdateUser(User usr);
        Task DeleteUser(User usr);
        Task CreateUser(User usr);
    }
}
