using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PicShare_GCPDemo.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;

namespace PicShare_GCPDemo.Controllers
{
    [Produces("application/json")]
    [Route("api/Pictures")]
    public class PicturesController : Controller
    {
        private readonly PicShareContext _context;
        private readonly IHostingEnvironment _env;
        private readonly StorageClient _storage;
        private readonly CloudStorageOptions _options;

        public PicturesController(PicShareContext context, IHostingEnvironment environment, IOptions<CloudStorageOptions> options)
        {
            _context = context;
            _env = environment;
            _storage = StorageClient.Create();
            _options = options.Value;
        }

        // POST: api/Pictures
        [HttpPost]
        public async Task<IActionResult> PostPicture(IFormFile image, string caption)
        {
            var link = await SaveImage(image);
            var picture = new Picture
            {
                AddedDate = DateTimeOffset.Now,
                Caption = caption,
                FilePath = link
            };

            _context.Picture.Add(picture);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPicture", new { id = picture.PictureId }, picture);
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            _options.ObjectName = GetFileName(file.FileName);
            var link = string.Empty;
            if (file.Length > 0)
            {
                var result = await _storage.UploadObjectAsync(_options.BucketName, _options.ObjectName, "image/*", file.OpenReadStream());
                link = result.MediaLink;
            }

            return link;
        }

        private string GetFileName(string localFilename)
        {
            // get the full path to the file
            var tempName = Path.GetRandomFileName();
            var name = tempName.Substring(0, tempName.IndexOf('.')) + 
                localFilename.Substring(localFilename.IndexOf('.'));
            return name;
        }

        // GET: api/Pictures
        [HttpGet]
        public IEnumerable<Picture> GetPicture(string search, int take = 15, int offset = 0)
        {
            if (string.IsNullOrWhiteSpace(search))
                return _context.Picture
                    .OrderByDescending(p => p.AddedDate)
                    .Take(take)
                    .Skip(offset);
            else
                return _context.Picture
                    .Where(p => p.Caption.Contains(search))
                    .OrderByDescending(p => p.AddedDate)
                    .Take(take)
                    .Skip(offset);
        }
    }
}