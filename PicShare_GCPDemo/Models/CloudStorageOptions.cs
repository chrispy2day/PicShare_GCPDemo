using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PicShare_GCPDemo.Models
{
    public class CloudStorageOptions
    {
        public string BucketName { get; set; }
        public string ObjectName { get; set; } = "sample.txt";
    }
}
