using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Models
{
    public class GardenSpace
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="제목")]
        public string Name { get; set; }
        public int SubTypeId { get; set; }
        [Display(Name="생성 날짜")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        [Display(Name="활성화 여부")]
        public bool IsActivate { get; set; }

        [ForeignKey("SubTypeId")]
        public virtual BaseSubType BaseSubType { get; set; }
        
        public virtual ICollection<GardenUser> GardenUsers { get; set; }
        public virtual ICollection<GardenTaskAttachMap> GardenTaskAttachMaps { get; set; }
        public virtual ICollection<GardenWorkTime> GardenWorkTimes { get; set; }
    }
}
