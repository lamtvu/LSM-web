using System.Collections.Generic;
using AutoMapper;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUser, User>();
            CreateMap<RegisterUser, Information>();
            CreateMap<ChangeUserInforDto, User>();
            CreateMap<User, UserReadDto>();

        }
    }
}