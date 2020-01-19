using System.Collections.Generic;

namespace Zima.Models
{
    public class Package
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public long UploadDate { get; set; }
        public List<string> Denpendencies { get; set; }
    }
}
