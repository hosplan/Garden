using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Models
{
    public class GardenFee
    {
        [Key]
        public int Id { get; set;}       
        
        [Display(Name = "타입")]
        public string SubTypeId { get; set; }
        
        [Display(Name = "금액")]
        public int Amount { get; set; }

        [Display(Name="생성 날짜")]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }
        
        public Nullable<int> GardenUserId { get; set; }
        
        [ForeignKey("GardenUserId")]
        public virtual GardenUser GardenUser { get; set; }
        
        public Nullable<int> GardenSpaceId { get; set; }
        
        [ForeignKey("SubTypeId")]
        public virtual BaseSubType BaseSubType { get; set; }

        public string TempString { get; set; }

        public int TempInt { get; set; }

        [ForeignKey("GardenSpaceId")]
        public virtual GardenSpace GardenSpace { get; set; }
    }
}
