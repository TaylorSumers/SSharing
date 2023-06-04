using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharing.Models
{
    public class RequiredString
    {
        public string Content { get; set; }

        public int Uploader { get; set; }

        public bool DeleteAfterDownload { get; set; }
    }
}
