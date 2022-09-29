using LmsBeApp_Group06.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Dtos;

namespace LmsBeApp_Group06.Data.Repositories.ReviewRepos
{
    public class SQLReviewRepo : IReviewRepo
    {
        private readonly LmsAppContext _context;
        public SQLReviewRepo(LmsAppContext context)
        {
            this._context = context;

        }

        public async Task<IEnumerable<Review>> GetAll(int courseid)
        {
            var reviews = await _context.Reviews.Include(r => r.Course).Include(r => r.Sender)
                                                .Where(r => r.CourseId == courseid)
                                                .ToListAsync();
            return reviews.ToList();
        }

        public async Task<IEnumerable<Review>> GetAll(string searchQuery, int courseid)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return await GetAll(courseid);
            }

            var collection = await GetAll(courseid);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var search = searchQuery.Trim();
                collection = collection.Where(a => a.Sender.Username.Contains(search));
            }

            return collection.ToList();
        }

        public async Task<Review> GetReviewById(int reviewid)
        {
            var review = await _context.Reviews.Include(r => r.Course).Include(r => r.Sender)
                                                .FirstOrDefaultAsync(r => r.Id == reviewid);
            if (review == null) return null;
            return review;
        }

        public async Task<Review> GetReview(int userid, int courseid)
        {
            var review = await _context.Reviews.Include(r => r.Course).Include(r => r.Sender)
                                                .FirstOrDefaultAsync(r => r.CourseId == courseid && r.SenderId == userid);
            if (review == null) return null;
            return review;
        }

        public async Task Create(Review review)
        {
            if (review == null)
            {
                throw new ArgumentNullException();
            }
            await _context.Reviews.AddAsync(review);

        }

        public async Task Update(Review review)
        {
            if (review == null)
            {
                throw new ArgumentNullException();
            }

            _context.Reviews.Update(review);
        }

        public async Task DeleteAll(IEnumerable<Review> reviews)
        {
            _context.Reviews.RemoveRange(reviews);
        }

        public async Task<Review> Detail(int id)
        {
            return await _context.Reviews.Include(r => r.Course)
                                         .Include(r => r.Sender)
                                         .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<PageDataDto<Review>> GetByPage(int courseid, int start, int limit)
        {
            var reviews = _context.Reviews.Include(x=>x.Sender).ToList();
            return new PageDataDto<Review>
            {
                Count = reviews.Count,
                Data = reviews.Skip(start).Take(limit)
            };
        }
        public async Task<PageDataDto<Review>> GetByPage(string searchQuery, int courseid, int start, int limit)
        {
            List<Review> reviews;
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                reviews = _context.Reviews.ToList();
            }
            else
            {
                reviews = _context.Reviews.Where(x => x.Comment.Contains(searchQuery)).ToList();
            }

            return new PageDataDto<Review>
            {
                Count = reviews.Count,
                Data = reviews.Skip(start).Take(limit)
            };
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

    }
}
