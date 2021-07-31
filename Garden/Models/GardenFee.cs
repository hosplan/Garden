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

        [Display(Name = "할인 타입")]
        public string DiscountTypeId { get; set; }
        
        [Display(Name = "금액")]
        public int Amount { get; set; }

        [Display(Name="생성 날짜")]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        [Display(Name ="만료 날짜")]
        [DataType(DataType.Date)]
        public DateTime ExpireDate { get; set; }
        public Nullable<int> GardenUserId { get; set; }
        
        [ForeignKey("GardenUserId")]
        [Display(Name = "정원 유저")]
        public virtual GardenUser GardenUser { get; set; }

        [Display(Name = "정원")]
        public Nullable<int> GardenSpaceId { get; set; }
       
        [ForeignKey("SubTypeId")]
        public virtual BaseSubType BaseSubType { get; set; }

        [ForeignKey("DiscountTypeId")]
        public virtual BaseSubType DiscountType { get; set; }

        public string TempString { get; set; }

        public int TempInt { get; set; }

        [ForeignKey("GardenSpaceId")]
        public virtual GardenSpace GardenSpace { get; set; }
    }
}
