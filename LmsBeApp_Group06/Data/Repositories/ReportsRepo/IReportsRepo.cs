using LmsBeApp_Group06.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Data.Repositories.ReportsRepo
{
    public interface IReportsRepo
    {
 
        Task<IEnumerable<Report>> GetAll(int classid);
        Task<IEnumerable<Report>> GetAll(string searchQuery, int classid);
        Task<Report> GetReport(int reportid);
        Task Delete(Report report);
        Task DeleteAll(IEnumerable<Report> reports);    
        Task<Report> Detail(int id);      
        Task SaveChange();
        Task<IEnumerable<Report>> GetByPage(int classid, int start, int limit);
        Task<IEnumerable<Report>> GetByPage(string searchQuery, int classid, int start, int limit);

    }
}
