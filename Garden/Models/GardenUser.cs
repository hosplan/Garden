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
        public Nullable<int> GardenSpaceId { get; set; }

        //해당 부분은 회원가입 기능을 false 했을경우 
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }

        [Display(Name = "생일")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> BirthDay { get; set; }
        //끝

        [Display(Name ="등록날짜")]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        [ForeignKey("GardenSpaceId")]
        public virtual GardenSpace GardenSpace { get; set; }

        public Nullable<int> GardenRoleId { get; set; }

        [ForeignKey("GardenRoleId")]
        public virtual GardenRole GardenRole { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        [InverseProperty("GardenUser")]
        public virtual ICollection<GardenUserTaskMap> GardenUserTasks { get; set; }
        [InverseProperty("GardenManager")]
        public virtual ICollection<GardenUserTaskMap> GardenManagerTasks { get; set; }
    }
}
