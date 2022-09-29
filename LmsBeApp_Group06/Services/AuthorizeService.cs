using LmsBeApp_Group06.Data.Repositories.ClassRepo;
using LmsBeApp_Group06.Data.Repositories.UserRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Services
{
    public class AuthorizeService
    {
        private readonly IUserRepo _userRepos;
        private readonly IClassRepo _classRepo;

        public AuthorizeService (IUserRepo userRepo, IClassRepo classRepo)
        {
            this._userRepos = userRepo;
            this._classRepo = classRepo;
        }
        public async Task< bool> IsClassAdmin(string username, int classid)
        {
            var user = await _userRepos.GetByUsername(username);
            var _class =await _classRepo.GetById(classid);

            if (user == null||_class==null) return false;
            if (_class.ClassAdminId == user.Id) return true;
             
            return false;
        }
        public async Task<bool> IsTeacher(string username, int classid)
        {
            var user = await _userRepos.GetByUsername(username);
            var _class = await _classRepo.GetById(classid);

            if (user == null || _class == null) return false;
            if (_class.TeacherId == user.Id) return true;

            return false;
        }

    }
}
