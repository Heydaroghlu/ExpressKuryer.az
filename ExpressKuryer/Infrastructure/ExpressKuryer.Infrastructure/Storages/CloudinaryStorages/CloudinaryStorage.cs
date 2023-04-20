using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.Storages.CloudinaryStorages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ExpressKuryer.Infrastructure.Storages.CloudinaryStorages
{
    public class CloudinaryStorage : ICloudinaryStorages
    {
        Cloudinary _cloudinary;
        public CloudinaryStorage(IConfiguration configuration)
        {
            Account account = configuration.GetSection("CloudinarySettings").Get<Account>();
            _cloudinary = new Cloudinary(account);
        }

        public async Task<bool> DeleteAsync(string pathOrContainerName, string fileName)
        {
            DeletionParams deletionParams = new("uploads/products/delete-icon-11.png");
            await _cloudinary.DestroyAsync(deletionParams);
            return true;
        }

        public bool HasFile(string pathOrContainerName, string fileName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
        {
            var result = await _cloudinary.CreateFolderAsync(pathOrContainerName);

            foreach (var file in files)
            {
                using var stream = file.OpenReadStream();
                var uploadResult = new ImageUploadResult();

                var listName = FileNames(pathOrContainerName, file.FileName);

                string fileNewName = file.FileName + Guid.NewGuid().ToString();
                var uploadParams = new ImageUploadParams()
                {

                    File = new FileDescription(fileNewName, stream),
                    Folder = result.Path,
                    PublicId = fileNewName

                };
                uploadResult = _cloudinary.Upload(uploadParams);
                Console.WriteLine(uploadResult.Url.ToString());
            }
            return new List<(string fileName, string pathOrContainerName)>();
        }

        public new List<string> FileNames(string path, string ceoFriendlyName)
        {
            return _cloudinary.Search().Expression($"public_id:{path}/{ceoFriendlyName}*").Execute().Resources.Select(x => x.PublicId).ToList();
        }
        public string GetPublicId(string imageUrl)
        {
            int startIndex = imageUrl.LastIndexOf('/') + 1;
            int endIndex = imageUrl.LastIndexOf('.');
            int length = endIndex - startIndex;
            return imageUrl.Substring(startIndex, length);
        }

        #region FileName

        //private async Task<string> FileRenameAsync(string pathOrContainerName,string fileName, List<string> fileNames)
        //{
        //    foreach (var item in FileNameContains.Split(","))
        //    {
        //        if (fileName.Contains(item)) fileName.Replace(item, "-");
        //    }


        //    foreach (var item in fileNames)
        //    {

        //    }

        //    return "";
        //}

        //private string FileNameContains = "!,@,#,$,%,^,&,*,(,),>,<,?,',{,},|";

        #endregion

    }
}
