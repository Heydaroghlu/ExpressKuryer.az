using AutoMapper;
using ExpressKuryer.Application.DTOs.Contact;
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
		}
	}
}
