using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Models;
using Microsoft.EntityFrameworkCore;

namespace LmsBeApp_Group06.Data.Repositories.SectionRepo
{
    public class SQLSectionRepo : ISectionRepo
    {
        private readonly LmsAppContext _context;
        public SQLSectionRepo(LmsAppContext context)
        {
            this._context = context;
        }
        public Task Change(Section section)
        {
            // nothing
            throw new System.NotImplementedException();
        }

        public async Task Create(Section section, Course course)
        {

            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }
            if (course == null)
            {
                throw new ArgumentNullException(nameof(section));
            }
            await _context.AddAsync(section);
            section.Course = course;
        }

        public async Task Delete(Section section)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }
            _context.sections.Remove(section);
        }

        public async Task<IEnumerable<Section>> GetByCourseId(int courseId)
        {
            var sections = _context.Sections.Include(x => x.Course).Include(x => x.Contents).Where(x => x.CourseId == courseId);
            return sections;
        }

        public async Task<Section> GetById(int id)
        {
            return await _context.Sections.FindAsync(id);
        }

        public async Task<Section> GetDetail(int id)
        {
            return await _context.Sections.Include(x => x.Contents).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }
    }
}
