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
        public int WorkingTime { get; set; }
        public int GardenTaskId { get; set; }
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
        public int SubTypeId { get; set; }

        [ForeignKey("SubTypeId")]
        public virtual BaseSubType BaseSubType { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        [ForeignKey("GardenTaskId")]
        public virtual GardenTask GardenTask { get; set; }
    }
}
