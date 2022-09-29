using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Data.Repositories.ContentRepo
{
    public class SQLContentRepo : IContentRepo
    {
        private readonly LmsAppContext _context;
        public SQLContentRepo(LmsAppContext context)
        {
            this._context = context;

        }
        public Task Change(Content content)
        {
            //nothing
            throw new System.NotImplementedException();
        }

        public async Task Create(Section section, Content content)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }
            content.Section = section;
            await _context.AddAsync(content);
        }

        public async Task Delete(Content content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }
            _context.Remove(content);
        }

        public async Task<Content> GetById(int id)
        {
            return await _context.Contents.FindAsync(id);
        }

        public async Task<IEnumerable<Content>> GetBySectionId(int id)
        {
            var contents = _context.Contents.Where(x=>x.Id == id);
            return contents;
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }
    }
}
