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
        [Display(Name ="담당자")]
        public Nullable<int> GardenManagerId { get; set; }

        [Display(Name ="참여자")]
        public Nullable<int> GardenUserId { get; set; }

        [Display(Name ="업무")]
        public Nullable<int> GardenTaskId { get; set; }

        [Display(Name ="업무 시간")]
        public Nullable<int> GardenWorkTimeId { get; set; }

        [ForeignKey("GardenWorkTimeId")]
        public virtual GardenWorkTime GardenWorkTime { get; set; }

        [ForeignKey("GardenManagerId")]
        public virtual GardenUser GardenManagerTask { get; set; }

        [ForeignKey("GardenUserId")]
        public virtual GardenUser GardenUserTask { get; set; }

        [ForeignKey("GardenTaskId")]
        public virtual GardenTask GardenTask { get; set; }
    }
}
