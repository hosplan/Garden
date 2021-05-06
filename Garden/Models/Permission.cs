using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Models
{
    public class Permission
    {
        [Key]
        public string Id { get; set; }
        [Display(Name ="역할")]
        public string RoleId { get; set; }

        [ForeignKey("RoleId")]
        [Display(Name ="역할")]
        public virtual ApplicationRole Role { get; set; }

        [Display(Name ="컨트롤러 명")]
        public string ControllerName { get; set; }
        
        [Display(Name ="페이지 명")]
        public string ActionName { get; set; }
        
        [Display(Name ="읽기")]
        public bool IsRead { get; set; }
        
        [Display(Name ="쓰기")]
        public bool IsCreate { get; set; }
        
        [Display(Name ="수정")]
        public bool IsUpdate { get; set; }
        
        [Display(Name ="삭제")]
        public bool IsDelete { get; set; }
    }
}
