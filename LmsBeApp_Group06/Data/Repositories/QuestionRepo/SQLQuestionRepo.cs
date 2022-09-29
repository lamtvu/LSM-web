using System;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;
using Microsoft.EntityFrameworkCore;

namespace LmsBeApp_Group06.Data.Repositories.QuestionRepo
{
    public class SQLQuestionRepo : IQuestionRepo
    {
        private readonly LmsAppContext _context;
        public SQLQuestionRepo(LmsAppContext context)
        {
            this._context = context;
        }
        public async Task Create(Question question)
        {
            if (question is null)
            {
                throw new ArgumentNullException(nameof(question));
            }
            await _context.AddAsync(question);
        }

        public async Task Delete(Question question)
        {
            if (question is null)
            {
                throw new ArgumentNullException(nameof(question));
            }
            _context.Remove(question);
        }

        public async Task DeleteRange(Question[] questions)
        {
            if (questions is null)
            {
                throw new ArgumentNullException(nameof(questions));
            }
            _context.RemoveRange(questions);
        }

        public async Task<Question> GetById(int id)
        {
            return await _context.Questions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PageDataDto<Question>> GetByQuizId(int quizId, int start, int limit)
        {
            var questions = await _context.Questions.Where(x => x.QuizId == quizId).ToListAsync();
            return new PageDataDto<Question> { Data = questions.OrderBy(x => x.Id).Skip(start).Take(limit), Count = questions.Count};
        }

        public async Task<System.Collections.Generic.IEnumerable<Question>> GetByQuizId(int quizId)
        {
            var questions = await _context.Questions.Where(x => x.QuizId == quizId).OrderBy(x => x.Id).ToListAsync();
            return questions;
        }

        public async Task<Question> GetDetail(int id)
        {
            return await _context.Questions.Include(x=>x.Answers).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }
    }
}
