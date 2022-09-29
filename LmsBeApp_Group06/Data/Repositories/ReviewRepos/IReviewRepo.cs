using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Data.Repositories.ReviewRepos
{
    public interface IReviewRepo
    {
 
        Task<IEnumerable<Review>> GetAll(int courseid);
        Task<Review> GetReviewById(int reviewid);
        Task<Review> GetReview(int userid, int courseid);
        Task Create(Review review);
        Task Update(Review review);    
        Task<Review> Detail(int id);      
        Task SaveChange();
        Task<PageDataDto<Review>> GetByPage(int courseid, int start, int limit);
        Task<PageDataDto<Review>> GetByPage(string searchQuery, int courseid, int start, int limit);
    }
}
