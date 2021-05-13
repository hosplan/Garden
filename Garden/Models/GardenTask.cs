using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Models
{
    public class GardenTask
    {
        [Key]
        public int Id { get; set;}
        [Display(Name="제목")]
        public string Name { get; set; }
        [Display(Name="내용")]
        public string Description { get; set; }
        [Display(Name = "타입")]
        public string SubTypeId { get; set; }
        [Display(Name="활성화 여부")]
        public bool IsActivate { get; set; }
        [Display(Name="생성 날짜")]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        public string RegUserId { get; set; }
        [ForeignKey("RegUserId")]
        public virtual ApplicationUser RegUser { get; set; }
        public Nullable<int> GardenSpaceId { get; set; }
        [ForeignKey("SubTypeId")]
        public virtual BaseSubType BaseSubType { get; set; }
        [ForeignKey("GardenSpaceId")]
        public virtual GardenSpace GardenSpace { get; set; }
        public virtual ICollection<GardenUserTaskMap> GardenUserTaskMaps { get; set; }


        //public int GetTodayTask
        //{
        //    get
        //    {
        //        if (GardenUserTaskMaps == null)
        //            return 0;

        //        List<GardenUserTaskMap> todayTask_list 
        //            = GardenUserTaskMaps
        //                .Where(z => z.TaskDate.ToShortDateString() == DateTime.Now.ToShortDateString())
        //                .ToList();

        //        if (todayTask_list == null)
        //            return 0;

        //        return todayTask_list.Count();
        //    }
        //}
    }
}
