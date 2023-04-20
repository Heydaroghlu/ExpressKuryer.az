using ExpressKuryer.Application.Storages.LocalStorages;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Infrastructure.Storages.LocalStorages
{
    public class LocalStorage : ILocalStorage
    {
        public Task<bool> DeleteAsync(string pathOrContainerName, string fileName)
        {
            throw new NotImplementedException();
        }

        public List<string> FileNames(string path, string ceoFriendlyName)
        {
            throw new NotImplementedException();
        }

        public string GetPublicId(string imageUrl)
        {
            throw new NotImplementedException();
        }

        public bool HasFile(string pathOrContainerName, string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
        {
            throw new NotImplementedException();
        }
    }
}
