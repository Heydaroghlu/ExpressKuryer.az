using AutoMapper;
using ExpressKuryer.Application.DTOs.AppUserDTOs;
using ExpressKuryer.Application.DTOs.BeCourierDTOs;
using ExpressKuryer.Application.DTOs.Contact;
using ExpressKuryer.Application.DTOs.CourierDTOs;
using ExpressKuryer.Application.DTOs.DeliveryDTOs;
using ExpressKuryer.Application.DTOs.JobSeekerDTOs;
using ExpressKuryer.Application.DTOs.Partner;
using ExpressKuryer.Application.DTOs.PartnerProduct;
using ExpressKuryer.Application.DTOs.ProductImageDTOs;
using ExpressKuryer.Application.DTOs.Service;
using ExpressKuryer.Application.DTOs.Setting;
using ExpressKuryer.Application.DTOs.Slider;
using ExpressKuryer.Application.DTOs.Subscribe;
using ExpressKuryer.Application.DTOs.Vacancy;
using ExpressKuryer.Application.Validators.CourierValidators;
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
            CreateMap<Partner, PartnerReturnDto>();

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

            CreateMap<PartnerProductCreateDto, PartnerProduct>();
            CreateMap<PartnerProduct, PartnerProductEditDto>();
            CreateMap<PartnerProductEditDto, PartnerProduct>();

            CreateMap<Delivery, DeliveryReturnDto>();
            CreateMap<Service, ServiceReturnDto>();
            CreateMap<PartnerProduct, PartnerProductReturnDto>();

            CreateMap<Courier, CourierReturnDto>();
            CreateMap<Courier, CourierEditDto>();
            CreateMap<CourierCreateDto, Courier>();
            CreateMap<AppUser, AppUserReturnDto>();
            CreateMap<AppUserReturnDto, AppUser>();

            CreateMap<JobSeeker, JobSeekerReturnDto>();

            CreateMap<VacancyCreateDto, Vacancy>();
            CreateMap<Vacancy, VacancyReturnDto>();
            CreateMap<Vacancy, VacancyEditDto>();

            CreateMap<ServiceCreateDto, Service>();
            CreateMap<Service, ServiceEditDto>();

            CreateMap<AppUserCreateDto, AppUser>();

            CreateMap<ProductImages, ProductImageReturnDto>();

            CreateMap<BeCouirer, BeCourierReturnDto>();

        }
    }
}
