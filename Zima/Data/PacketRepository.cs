using System.Collections.Generic;
using System.Linq;
using Zima.Models;

namespace Zima.Data
{
    public class PacketRepository
    {
        private readonly ApplicationDbContext _context;

        public PacketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Package> All()
        {
            return _context.Packets.ToList().Select(p => p.ToModel());
        }

        public IEnumerable<Package> List(string name)
        {
            return _context.Packets.Where(p => p.Project.Name == name).ToList().Select(p => p.ToModel());
        }

        public bool Add(Package package, string op)
        {
            Project project = _context.Projects.Where(p => p.Name == package.Name).FirstOrDefault();
            if (project == null)
            {
                project = new Project() { Name = package.Name, OperationKey = op, Versions = new List<Packet>() };
                _context.Add(project);
            }
            else
            {
                if (_context.Packets.Any(p => p.Project.Name == package.Name && p.Version == package.Version))
                {
                    return false;
                }
            }
            if (project.OperationKey != op)
            {
                return false;
            }
            Packet packet = new Packet() { Version = package.Version, UploadDate = package.UploadDate };
            packet.Dependencies = package.Dependencies == null ? "" : string.Join(';', package.Dependencies);
            project.Versions.Add(packet);
            _context.Add(packet);
            _context.SaveChanges();
            return true;
        }

        public Package FindLatest(string name)
        {
            return _context.Packets.Where(p => p.Project.Name == name).OrderByDescending(p => p.UploadDate).FirstOrDefault()?.ToModel();
        }

        public Package Find(string name, string version)
        {
            return _context.Packets.FirstOrDefault(p => p.Project.Name == name && p.Version == version)?.ToModel();
        }
    }
}
