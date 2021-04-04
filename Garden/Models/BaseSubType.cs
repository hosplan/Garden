using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Models
{
    public class BaseSubType
    {
        [Key]
        public int BaseSubTypeId { get; set; }

        public string BaseTypeId { get; set; }

        [ForeignKey("BaseTypeId")]
        public virtual BaseType BaseType { get; set; }
        [Display(Name="이름")]
        public string Name { get; set; }

        [Display(Name="설명")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
