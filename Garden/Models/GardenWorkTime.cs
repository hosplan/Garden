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
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }
        
        [Display(Name = "종료 시간")]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        [Display(Name = "수행날짜")]
        [DataType(DataType.Date)]
        public DateTime TaskDate { get; set; }
        [Display(Name = "완료 여부")]
        public bool IsComplete { get; set; }

        [Display(Name = "주 단위")]
        public int TaskWeek { get; set; }

        [Display(Name = "업무")]
        public Nullable<int> GardenTaskId { get; set; }

        [ForeignKey("GardenTaskId")]
        public virtual GardenTask GardenTask { get; set; }

        public Nullable<int> GardenSpaceId { get; set; }

        [ForeignKey("GardenSpaceId")]
        public virtual GardenSpace GardenSpace { get; set; }

        public Nullable<int> GardenUserId { get; set; }
        [ForeignKey("GardenUserId")]
        public virtual GardenUser GardenUser { get; set; }
        public virtual ICollection<GardenUserTaskMap> GardenUserTaskMaps { get; set; }

        public Weekend weekend { get; set; }
    }

    [NotMapped]
    public class Weekend
    {
        public bool IsMon { get; set; }
        public bool IsTue { get; set; }
        public bool IsWed { get; set; }
        public bool IsThr { get; set; }
        public bool IsFri { get; set; }
        public bool IsSat { get; set; }
        public bool IsSun { get; set; }
    }
}
