using AutoMapper;
using ExpressKuryer.Application.DTOs.Contact;
using ExpressKuryer.Application.DTOs.Partner;
using ExpressKuryer.Application.DTOs.Setting;
using ExpressKuryer.Application.DTOs.Subscribe;
using ExpressKuryer.Application.DTOs.Vacancy;
using ExpressKuryer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.Profiles
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<ContactDto, Contact>();	
			CreateMap<Contact, ContactReturnDto>();

			CreateMap<Setting, SettingReturnDto>();
			CreateMap<PartnerCreateDto, Partner>();
			CreateMap<Partner,PartnerReturnDto>();

			CreateMap<VacancyDto, Vacancy>();
			CreateMap<SubscribeDto, Subscribe>();
		}
	}
}
