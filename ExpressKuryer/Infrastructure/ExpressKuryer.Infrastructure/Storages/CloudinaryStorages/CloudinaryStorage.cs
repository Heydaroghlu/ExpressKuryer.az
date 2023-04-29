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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using ExpressKuryer.Application.Exceptions;
using System.Net.Mime;

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
            var deletionParams = new DeletionParams(pathOrContainerName+fileName);
            var deletionResult = _cloudinary.DestroyAsync(deletionParams);
            return true;
        }

        public bool HasFile(string pathOrContainerName, string fileName)
        {
            var getResource = new GetResourceParams(pathOrContainerName+fileName)
            {
                ResourceType = ResourceType.Image
            };
            var info = _cloudinary.GetResource(getResource);

            if (info.Error == null)
                return true;
            else
                return false;

        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadRangeAsync(string pathOrContainerName, IFormFileCollection files, string? contentType = null)
        {
            var result = await _cloudinary.CreateFolderAsync(pathOrContainerName);

            List<(string fileName, string pathOrContainerName)> fileNameList = new();

            foreach (var file in files)
            {
                string fileNewName = UploadImageAction(pathOrContainerName,result.Path,file);
                fileNameList.Add(new(fileNewName, pathOrContainerName));
            }
            return fileNameList;
        }

        public async Task<(string fileName, string pathOrContainerName)> UploadAsync(string pathOrContainerName, IFormFile file, string? contentType = null)
        {

            CheckFileType(file,contentType);

            var result = await _cloudinary.CreateFolderAsync(pathOrContainerName);
            
            string fileNewName = UploadImageAction(pathOrContainerName,result.Path,file);

            return (fileNewName, pathOrContainerName);
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


        public string UploadImageAction(string pathOrContainerName,string resultPath, IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var uploadResult = new ImageUploadResult();

            var listName = FileNames(pathOrContainerName,file.FileName);

            //todo change image name

            string fileNewName = file.FileName;
            var uploadParams = new ImageUploadParams()
            {

                File = new FileDescription(fileNewName, stream),
                Folder = resultPath,
                PublicId = fileNewName,

            };
            uploadResult = _cloudinary.Upload(uploadParams);
            Console.WriteLine(uploadResult.Url.ToString());
            return fileNewName;
        }



        //todo add to local storage
        public void CheckFileType(IFormFile file , string contentType)
        {
            if (file.ContentType.ToLower().Equals(contentType.ToLower()) == false)
            {
                throw new ContentTypeException("type must be pdf");
            }
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
