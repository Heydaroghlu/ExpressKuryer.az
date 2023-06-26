using ExpressKuryer.Application.Abstractions.File;
using ExpressKuryer.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Infrastructure.Services.File
{
    public class FileService : IFileService
    {
        public void CheckFileType(IFormFile file, List<string> contentTypes)
        {
            bool check = false;

            foreach (var contentType in contentTypes)
            {
                if (file.ContentType.ToLower().Equals(contentType.ToLower()) == true)
                {
                    check = true;
                    break;
                }
            }

            string message = "type must be ";
            for (int i = 0; i < contentTypes.Count; i++)
            {
                if (i == contentTypes.Count - 1)
                {
                    message += contentTypes[i];
                }
                else
                {
                    message += contentTypes[i] + " or ";
                }
            }


            if (check == false)
            {
                throw new ContentTypeException();
            }
        }

    }
}
