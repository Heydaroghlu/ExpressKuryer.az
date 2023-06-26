using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.Storages.CloudinaryStorages
{
	public interface ICloudinaryStorages : IStorage
	{
        string GetPublicId(string imageUrl);
        void CheckFileType(IFormFile file, List<string> contentTypes);
    }
}
