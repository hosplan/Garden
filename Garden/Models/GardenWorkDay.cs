using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Models
{
    public class GardenWorkDay
    {
        [Key]
        public int Id { get; set; }
        public Nullable<int> GardenSpaceId { get; set; }
        [Display(Name ="월요일")]
        public bool IsMon { get; set; }
        [Display(Name = "화요일")]
        public bool IsTue { get; set; }
        [Display(Name = "수요일")]
        public bool IsWed { get; set; }
        [Display(Name = "목요일")]
        public bool IsThu { get; set; }
        [Display(Name = "금요일")]
        public bool IsFri { get; set; }
        [Display(Name = "토요일")]
        public bool IsSat { get; set; }
        [Display(Name = "일요일")]
        public bool IsSun { get; set; }
        public Nullable<int> SubTypeId { get; set; }

        [ForeignKey("SubTypeId")]
        public virtual BaseSubType BaseSubType { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        [ForeignKey("GardenSpaceId")]
        public virtual GardenSpace GardenSpace { get; set; }
        public virtual ICollection<GardenUserTaskMap> GardenUserTaskMaps { get; set; }
    }
}
