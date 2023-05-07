using AutoMapper;
using ExpressKuryer.Application.DTOs.Contact;
using ExpressKuryer.Application.DTOs.Partner;
using ExpressKuryer.Application.DTOs.PartnerProduct;
using ExpressKuryer.Application.DTOs.Service;
using ExpressKuryer.Application.DTOs.Setting;
using ExpressKuryer.Application.DTOs.Slider;
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
            CreateMap<Subscribe, SubscribeReturnDto>();
            CreateMap<Subscribe, SubscribeEditDto>();

            CreateMap<Partner, PartnerEditDto>();

			CreateMap<SliderCreateDto, Slider>();	
			CreateMap<Slider, SliderReturnDto>();
			CreateMap<Slider, SliderEditDto>();


			CreateMap<Vacancy, VacancyReturnDto>();

			CreateMap<Contact, ContactReturnDto>();
			CreateMap<Contact, ContactEditDto>();

            CreateMap<PartnerProduct, PartnerProductReturnDto>();
			CreateMap<PartnerProductCreateDto, PartnerProduct>();
			CreateMap<PartnerProduct, PartnerProductEditDto>();
			CreateMap<PartnerProductEditDto, PartnerProduct>();

			CreateMap<Service, ServiceReturnDto>();
        }
    }
}
