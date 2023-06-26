using ExpressKuryer.Application.Storages.LocalStorages;
using ExpressKuryer.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
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

        IWebHostEnvironment _env;

        public LocalStorage(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<bool> DeleteAsync(string pathOrContainerName, string fileName)
        {
            var check = await Task.Run<bool>(() =>
            {
                string path = _env.WebRootPath + pathOrContainerName + fileName;
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return true;
            });
            return true;
        }

        //todo getUrl LocalStorage
        public string GetUrl(string pathOrContainerName, string fileName)
        {
            return HttpService.StorageUrl(pathOrContainerName, fileName);
        }

        public bool HasFile(string pathOrContainerName, string fileName)
        {
            string path = _env.WebRootPath + pathOrContainerName + fileName;
            return System.IO.File.Exists(path);
        }

        public async Task<(string fileName, string pathOrContainerName)> UploadAsync(string pathOrContainerName, IFormFile file)
        {
            var name = await Task.Run<string>(() =>
            {
                string newfilename = Guid.NewGuid().ToString() + file.FileName;
                string path = _env.WebRootPath + pathOrContainerName + newfilename;
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return newfilename;
            });

            return (name, pathOrContainerName);
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadRangeAsync(string pathOrContainerName, List<IFormFile> files)
        {
            List<(string fileName, string pathOrContainerName)> fileNameList = new();

            var list = await Task<List<(string fileName, string pathOrContainerName)>>.Run(() =>
            {
                files.ForEach(file =>
                {
                    string newfilename = Guid.NewGuid().ToString() + file.FileName;
                    string path = _env.WebRootPath + pathOrContainerName + newfilename;
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    fileNameList.Add((newfilename, pathOrContainerName));
                });
                return fileNameList;
            });

            return list;
        }

       
    }
}
