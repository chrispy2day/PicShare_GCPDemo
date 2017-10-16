using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PicShare_GCPDemo.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace PicShare_GCPDemo.Controllers
{
    [Produces("application/json")]
    [Route("api/Pictures")]
    public class PicturesController : Controller
    {
        private readonly PicShareContext _context;
        private readonly IHostingEnvironment _env;

        public PicturesController(PicShareContext context, IHostingEnvironment environment)
        {
            _context = context;
            _env = environment;
        }

        // POST: api/Pictures
        [HttpPost]
        public async Task<IActionResult> PostPicture(IFormFile image, string caption)
        {
            var filename = await SaveImage(image);
            var picture = new Picture
            {
                AddedDate = DateTimeOffset.Now,
                Caption = caption,
                FilePath = "/images/pics/" + filename
            };

            _context.Picture.Add(picture);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPicture", new { id = picture.PictureId }, picture);
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            var path = GetFilePath(file.FileName);

            if (file.Length > 0)
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar) + 1);
        }

        private string GetFilePath(string localFilename)
        {
            // get the full path to the file
            var tempName = Path.GetRandomFileName();
            var name = tempName.Substring(0, tempName.IndexOf('.')) + 
                localFilename.Substring(localFilename.IndexOf('.'));

            // create the path
            var sep = Path.DirectorySeparatorChar;
            var filePath = _env.WebRootPath + sep + "images" + sep + "pics" + sep + name;
            return filePath;
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

        // GET: api/Pictures/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetPicture([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var picture = await _context.Picture.SingleOrDefaultAsync(m => m.PictureId == id);

        //    if (picture == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(picture);
        //}

        // PUT: api/Pictures/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPicture([FromRoute] int id, [FromBody] Picture picture)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != picture.PictureId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(picture).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PictureExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Pictures
        //[HttpPost]
        //public async Task<IActionResult> PostPicture([FromBody] Picture picture)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.Picture.Add(picture);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetPicture", new { id = picture.PictureId }, picture);
        //}

        //// DELETE: api/Pictures/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeletePicture([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var picture = await _context.Picture.SingleOrDefaultAsync(m => m.PictureId == id);
        //    if (picture == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Picture.Remove(picture);
        //    await _context.SaveChangesAsync();

        //    return Ok(picture);
        //}

        private bool PictureExists(int id)
        {
            return _context.Picture.Any(e => e.PictureId == id);
        }
    }
}