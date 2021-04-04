using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Models
{
    public class GardenRole
    {
        [Key]
        public int GardenRoleId { get; set; }
        public int GardenId { get; set; }
        public string SubTypeId { get; set; }
        [ForeignKey("SubTypeId")]
        public virtual BaseSubType BaseSubType { get; set; }
        [ForeignKey("GardenId")]
        public virtual Garden Garden { get; set; }
    }
}
