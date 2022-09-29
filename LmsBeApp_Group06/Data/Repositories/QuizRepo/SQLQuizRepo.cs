using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Models;
using Microsoft.EntityFrameworkCore;

namespace LmsBeApp_Group06.Data.Repositories.QuizRepo
{
    public class SQLQuizRepo : IQuizRepo
    {
        private readonly LmsAppContext _context;
        public SQLQuizRepo(LmsAppContext context)
        {
            this._context = context;
        }
        public async Task Create(Quiz quiz)
        {
            if (quiz is null)
            {
                throw new ArgumentNullException(nameof(quiz));
            }
            await _context.AddAsync(quiz);
        }

        public async Task Delete(Quiz quiz)
        {
            if (quiz is null)
            {
                throw new ArgumentNullException(nameof(quiz));
            }
            _context.Remove(quiz);
        }

        public async Task<IEnumerable<Quiz>> GetByClassId(int classId)
        {
            return _context.Quizzes.Where(x=>x.ClassId == classId);
        }

        public async Task<Quiz> GetById(int id)
        {
            return await _context.Quizzes.FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task Remove(Quiz quiz)
        {
            if (quiz is null)
            {
                throw new ArgumentNullException(nameof(quiz));
            }
            _context.Quizzes.Remove(quiz);
        }

        public async Task SaveChange()
        {
            _context.SaveChanges();
        }
    }
}