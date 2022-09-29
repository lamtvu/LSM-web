using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;
using Microsoft.EntityFrameworkCore;

namespace LmsBeApp_Group06.Data.Repositories.SubmissionQuizRepo
{
    public class SQLSubmissionSQLRepo : ISubmissionQuizRepo
    {
        private readonly LmsAppContext _context;
        public SQLSubmissionSQLRepo(LmsAppContext context)
        {
            this._context = context;
        }
        public async Task Create(SubmissionQuiz submissionQuiz)
        {
            if (submissionQuiz is null)
            {
                throw new ArgumentNullException(nameof(submissionQuiz));
            }
            await _context.SubmisstionQuizzes.AddAsync(submissionQuiz);
        }

        public async Task Delete(SubmissionQuiz submissionQuiz)
        {
            if (submissionQuiz is null)
            {
                throw new ArgumentNullException(nameof(submissionQuiz));
            }
            _context.SubmisstionQuizzes.Remove(submissionQuiz);
        }

        public async Task<SubmissionQuiz> GetById(int id)
        {
            return await _context.SubmisstionQuizzes.Include(x => x.Answers).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PageDataDto<SubmissionQuiz>> GetByQuizId(int quizId, int start, int limit, string searchValue)
        {
            List<SubmissionQuiz> submissions;
            if (!String.IsNullOrWhiteSpace(searchValue))
            {
                submissions = await _context.SubmisstionQuizzes.Include(x => x.Student)
                .Where(x => x.QuizId == quizId && x.Student.Username.Contains(searchValue))
                .ToListAsync();
            }
            else
            {
                submissions = await _context.SubmisstionQuizzes.Where(x => x.QuizId == quizId)
                .Include(x => x.Student).OrderBy(x => x.Id).ToListAsync();
            }
            return new PageDataDto<SubmissionQuiz> { Data = submissions.Skip(start).Take(limit), Count = submissions.Count };
        }

        public async Task<SubmissionQuiz> GetByUsernameAndQuizId(int userId, int quizId)
        {
            var SubmisstionQuiz = await _context.SubmisstionQuizzes.Include(x => x.Answers)
            .FirstOrDefaultAsync(x => x.StudentId == userId && x.QuizId == quizId);
            return SubmisstionQuiz;
        }

        public async Task<SubmissionQuiz> GetDetail(int id)
        {
            return await _context.SubmisstionQuizzes.Include(x => x.Quiz)
            .Include(x => x.Student).Include(x => x.Answers)
            .ThenInclude(x => x.Question).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<SubmissionQuiz>> GetOwnedSubmissions(int classId, int userId)
        {
            var submissions = _context.SubmisstionQuizzes.Include(x => x.Quiz)
            .Where(x => x.StudentId == userId && x.Quiz.ClassId == classId);
            return submissions;
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }
    }
}
