using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;
using Microsoft.EntityFrameworkCore;

namespace LmsBeApp_Group06.Data.Repositories.AnswerRepo
{
    public class SQLAnswerRepo : IAnswerRepo
    {
        private readonly LmsAppContext _context;
        public SQLAnswerRepo(LmsAppContext context)
        {
            this._context = context;
        }
        public async Task Create(Answer answer)
        {
            if (answer is null)
            {
                throw new ArgumentNullException(nameof(answer));
            }
            await _context.Answers.AddAsync(answer);
        }

        public async Task CreateRange(Answer[] answers)
        {
            if (answers is null)
            {
                throw new ArgumentNullException(nameof(answers));
            }
            await _context.Answers.AddRangeAsync(answers);
        }

        public async Task Delete(Answer answer)
        {
            if (answer is null)
            {
                throw new ArgumentNullException(nameof(answer));
            }
            _context.Entry(answer).Collection(x=>x.SubmissionQuizzes).Load();
            answer.SubmissionQuizzes.Clear();
            _context.Answers.Remove(answer);
        }

        public async Task DeleteRange(Answer[] answer)
        {
            if (answer is null)
            {
                throw new ArgumentNullException(nameof(answer));
            }
            _context.Answers.RemoveRange(answer);
        }

        public Task Edit(Answer answer)
        {
            //nothing
            throw new System.NotImplementedException();
        }

        public async Task<Answer> GetById(int id)
        {
            return await _context.Answers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Answer>> GetByQuestionId(int questionId)
        {
            var answers = _context.Answers.Where(x => x.QuestionId == questionId);
            return answers;
        }

        public async Task<Answer> GetDetail(int id)
        {
            return await _context.Answers.Include(x=>x.Question).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveChage()
        {
            await _context.SaveChangesAsync();
        }
    }
}
