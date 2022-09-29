using AutoMapper;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Dtos.Invitation;
using LmsBeApp_Group06.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Profiles
{
    public class InvitationProfiles: Profile
    {
        public InvitationProfiles()
        {
            CreateMap<Invitation, InvitationReadDto>();
            CreateMap<InvitationReadDto, Invitation>();       
        }
    }
}
