using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Models
{
    public class BaseType
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name="이름")]
        public string Name { get; set; }
        
        [Display(Name="설명")]
        public string Description { get; set; }

        [Display(Name="하위항목 수정가능 여부")]
        public bool IsSubTypeEditable { get; set; }

        public virtual ICollection<BaseSubType> baseSubTypes { get; set; }
    }
}
