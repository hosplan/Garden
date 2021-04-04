using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Models
{
    public class GardenUserTaskMap
    {
        [Key]
        public int Id { get; set; }
        public int GardenUserId { get; set; }
        public int GardenTaskId { get; set; }
    
        [ForeignKey("GardenUserId")]
        public virtual GardenUser GardenUser { get; set; }
        [ForeignKey("GardenTaskId")]
        public virtual GardenTask GardenTask { get; set; }
    }
}
