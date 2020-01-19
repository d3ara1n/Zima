using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zima.Data
{
    public class Project
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }
        [Required]
        [MaxLength(128)]
        public string OperationKey { get; set; }
        public virtual ICollection<Packet> Versions { get; set; }
    }
}
