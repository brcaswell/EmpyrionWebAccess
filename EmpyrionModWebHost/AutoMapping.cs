﻿using AutoMapper;
using Eleon.Modding;
using EmpyrionModWebHost.Models;
using static EmpyrionModWebHost.Controllers.StructureController;

namespace EmpyrionModWebHost
{
    public class AutoMapping : Profile
    {
        public class RoleFormatter : IValueConverter<Role, string>
        {
            public string Convert(Role source, ResolutionContext context)
                => source.ToString();
        }

        public AutoMapping()
        {
            CreateMap<User, UserDto>()
                .ForMember(d => d.Role, opt => opt.ConvertUsing(new RoleFormatter()));

            CreateMap<PVector3, Vector3Data>();
            CreateMap<GlobalStructureInfo, GlobalStructureInfoData>();
            CreateMap<GlobalStructureList, GlobalStructureListData>();

        }
    }
}
