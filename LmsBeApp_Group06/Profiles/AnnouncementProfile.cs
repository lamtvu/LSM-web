using AutoMapper;
using LmsBeApp_Group06.Dtos.Announcement;
using LmsBeApp_Group06.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Profiles
{
    public class AnnouncementProfile:Profile
    {
        public AnnouncementProfile()
        {
            CreateMap<Announcement, AnnouncementReadDto>();
            CreateMap<AnnouncementReadDto, Announcement>();
            CreateMap<Announcement, AnnouncementCreateDto>();
            CreateMap<AnnouncementCreateDto, Announcement>();
            CreateMap<Announcement, AnnouncementEditDto>();
            CreateMap<AnnouncementEditDto, Announcement>();
        }
    }
}
