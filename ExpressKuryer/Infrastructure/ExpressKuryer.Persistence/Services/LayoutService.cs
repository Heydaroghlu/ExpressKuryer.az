using ExpressKuryer.Application.Enums;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Persistence.Services
{
    public class LayoutService
    {

        readonly IUnitOfWork _unitOfWork;

        public LayoutService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Setting>> GetSettingsAsync()
        {
            return await _unitOfWork.RepositorySetting.GetAllAsync(x => true,false);
        }

        public async Task<AppUser> GetUserAsync(string name)
        {
            return await _unitOfWork.RepositoryUser.GetAsync(x => x.UserName.Equals(name));
        }

    }
}
