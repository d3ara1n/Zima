using System;
using System.IO;
using System.Linq;
using Zima.Data;

namespace Zima.Models
{
    public static class PackageExtension
    {
        public static Package ToModel(this Packet packet)
        {
            var p = new Package() { Name = packet.Project.Name, Version = packet.Version, UploadDate = packet.UploadDate };
            p.Dependencies = packet.Dependencies?.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList();
            return p;
        }

        public static string Locate(this Package package)
        {
            return Path.Combine(Environment.CurrentDirectory, "Packages", $"{package.Name}_{package.Version}.zpkg");
        }
    }
}
