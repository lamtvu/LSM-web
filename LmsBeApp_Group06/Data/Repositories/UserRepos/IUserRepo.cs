using System.Collections.Generic;
using System.Threading.Tasks;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Data.Repositories.UserRepos
{
    public interface IUserRepo
    {
        Task CreateUser(User user);
        Task<PageDataDto<User>> GetAll();
        Task<PageDataDto<User>> GetAll(string searchQuery);
        Task<PageDataDto<User>> GetByPage(int start, int limit, string searchValue);
        Task<User> GetById(int id);
        Task<User> GetByUsername(string username);
        Task<User> GetByEmail(string email);
        Task Delete(User user);
        Task Change(User user);
        Task<User> GetDetail(int id);
        Task<User> GetDetail(string username);
        Task SaveChange();

    }
}