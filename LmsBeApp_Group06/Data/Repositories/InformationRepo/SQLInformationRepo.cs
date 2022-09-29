using System;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Models;
using Microsoft.EntityFrameworkCore;

namespace LmsBeApp_Group06.Data.Repositories.InformationRepo
{
    public class SQLInformationRepo : IInformationRepo
    {
        private readonly LmsAppContext _context;
        public SQLInformationRepo(LmsAppContext context)
        {
            this._context = context;
        }
        public Task Change(Information information)
        {
            //nothing
            throw new System.NotImplementedException();
        }

        public async Task CreateInformation(Information information)
        {
            if (information == null)
            {
                throw new ArgumentNullException();
            }
            await _context.Informations.AddAsync(information);
        }

        public async Task Delete(Information information)
        {
            if (information == null)
            {
                throw new ArgumentNullException();
            }
            _context.Informations.Remove(information);
        }

        public async Task<Information> GetById(int id)
        {
            return await _context.Informations.FindAsync(id);
        }

        public async Task<Information> GetByUsername(string username)
        {
            return await _context.Informations.Include(i => i.User)
            .FirstOrDefaultAsync(i => i.User.Username == username);
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }
    }
}
