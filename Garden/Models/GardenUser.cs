using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Models
{
    public class GardenUser
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } 
        public bool IsActivate { get; set; }
        public int GardenSpaceId { get; set; }
        [Display(Name ="등록날짜")]
        [DataType(DataType.Date)]
        public DateTime RegDate { get; set; }

        [ForeignKey("GardenSpaceId")]
        public virtual GardenSpace GardenSpace { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public ICollection<GardenUserTaskMap> GardenUserTaskMaps { get; set; }
    }
}
