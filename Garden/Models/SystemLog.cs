using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Models
{
    public class SystemLog
    {
        [Key]
        public int Id { get; set; }
        public string ControllerName { get; set; }
        public string ViewName { get; set; }
        public string SubTypeId { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }
        [ForeignKey("SubTypeId")]
        public virtual BaseSubType BaseSubType { get; set; }
    }
}
