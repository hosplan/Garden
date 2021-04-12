using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Models
{
    public class GardenWorkTime
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "시작 시간")]
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }
        [Display(Name = "종료 시간")]
        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }

        public int? GardenSpaceId { get; set; }
        [ForeignKey("GardenSpaceId")]
        public virtual GardenSpace GardenSpace { get; set; }
        public virtual ICollection<GardenUserTaskMap> GardenUserTaskMaps { get; set; }
    }
}
