using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PicShare_GCPDemo.Models
{
    public class PictureUpload
    {
        public IFormFile file { get; set; }
        public string Caption { get; set; }
    }
}
