using LmsBeApp_Group06.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Data.Repositories.ReportsRepo
{
    public class SQLReportsRepo: IReportsRepo
    {
        private readonly LmsAppContext _context;
        public SQLReportsRepo(LmsAppContext context)
        {
            this._context = context;

        }
       
        public async Task<IEnumerable<Report>> GetAll(int classid)
        {
            var reports = await _context.Reports.Include(r=>r.Class).Include(r=>r.Sender)
                                                .OrderByDescending(r => r.Id)
                                                .Where(r=>r.ClassId==classid)
                                                .ToListAsync();
            return reports.ToList();
        }

        public async Task<IEnumerable<Report>> GetAll(string searchQuery, int classid)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return await GetAll(classid);
            }

            var collection = await GetAll(classid);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var search = searchQuery.Trim();
                collection = collection.Where(a => a.Sender.Username.Contains(search)
                      || a.Sender.FullName.Contains(search)
                      || a.Sender.Email.Contains(search));
            }

            return collection.ToList();
        }

        public async Task<Report> GetReport(int reportid)
        {
            var report = await _context.Reports.Include(r => r.Class).Include(r => r.Sender)
                                                .FirstOrDefaultAsync(r => r.Id == reportid);                                            
            if (report == null) return null;
            return report;
        }

        public async Task Create(Report report )
        {
            if (report==null)
            {
                throw new ArgumentNullException();
            }
            await _context.Reports.AddAsync(report);
          
        }

        public async Task Delete(Report report)
        {
            if (report == null)
            {
                throw new ArgumentNullException();
            }
            
           _context.Reports.Remove(report);
        }

        public async Task DeleteAll(IEnumerable<Report> reports)
        {
            _context.Reports.RemoveRange(reports);
        }
    
        public async Task<Report> Detail(int id)
        {
            return await _context.Reports.Include(r => r.Class)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    
        public async Task<IEnumerable<Report>> GetByPage(int classid, int start, int limit)
        {
            var reports = _context.Reports.Include(u => u.Class)
                                                  .OrderByDescending(r => r.Id).Where(r => r.ClassId == classid)
                                                  .Skip(start).Take(limit);
            return reports.ToList();
        }
        public async Task<IEnumerable<Report>> GetByPage(string searchQuery, int classid, int start, int limit)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return await GetByPage(classid,start, limit);
            }

            var collection = await GetByPage(classid, start, limit);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var search = searchQuery.Trim();
                collection = collection.Where(a => a.Sender.Username.Contains(search)
                      || a.Sender.FullName.Contains(search)
                      || a.Sender.Email.Contains(search));
            }

            return collection.ToList();
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

    }
}
