using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharing.Models
{
    public class ResponseFile
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string DeleteAfterDownload { get; set; }

        public string FileType { get; set; }

        public string Code { get; set; }

        public string GetUrl { get; set; }
    }
}
