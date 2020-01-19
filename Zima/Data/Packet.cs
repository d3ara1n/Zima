using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zima.Data
{
    public class Packet
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(128)]
        public string Version { get; set; }
        public string Dependencies { get; set; }
        public long UploadDate { get; set; }
        [Required]
        public virtual Project Project { get; set; }
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }
    }
}
