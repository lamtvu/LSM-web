using System.Threading.Tasks;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Data.Repositories.InformationRepo
{
    public interface IInformationRepo
    {
        Task CreateInformation(Information information);
        Task<Information> GetByUsername(string username);
        Task<Information> GetById(int id);
        Task Change(Information information);
        Task Delete(Information information);
        Task SaveChange();
    }
}
