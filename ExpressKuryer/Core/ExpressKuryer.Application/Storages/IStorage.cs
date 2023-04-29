using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.Storages
{
	public interface IStorage
	{
		Task<List<(string fileName, string pathOrContainerName)>> UploadRangeAsync(string pathOrContainerName, IFormFileCollection files, string? contentType = null);
		Task<(string fileName, string pathOrContainerName)> UploadAsync(string pathOrContainerName, IFormFile file, string? contentType = null);
		Task<bool> DeleteAsync(string pathOrContainerName, string fileName);
		bool HasFile(string pathOrContainerName, string fileName);
		List<string> FileNames(string path, string ceoFriendlyName);
		string GetPublicId(string imageUrl);
    }
}
