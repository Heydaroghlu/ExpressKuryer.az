using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.HelperManager
{
    public static class ContentTypeManager
    {
        public static List<string> ImageContentTypes = new List<string>()
        {
            new string("image/png"),
            new string("image/jpg"),
            new string("image/jpeg"),
            new string("image/gif"),
        };

        public static List<string> PDFContentTypes = new List<string>()
        {
            new string("application/pdf"),
        };


        public static string ImageContentMessage()
        {
            string message = "type must be ";

            for (int i = 0; i < ContentTypeManager.ImageContentTypes.Count; i++)
            {
                if (i == ContentTypeManager.ImageContentTypes.Count - 1)
                {
                    message += ContentTypeManager.ImageContentTypes[i];
                }
                else
                {
                    message += ContentTypeManager.ImageContentTypes[i] + " or ";
                }
            }
            
            return message;
        }

        public static string PDFContentMessage()
        {
            string message = "type must be ";

            for (int i = 0; i < ContentTypeManager.PDFContentTypes.Count; i++)
            {
                if (i == ContentTypeManager.PDFContentTypes.Count - 1)
                {
                    message += ContentTypeManager.PDFContentTypes[i];
                }
                else
                {
                    message += ContentTypeManager.PDFContentTypes[i] + " or ";
                }
            }

            return message;
        }
    }
}
