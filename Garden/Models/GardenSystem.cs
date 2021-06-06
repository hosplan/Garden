using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Models
{
    public class GardenSystem
    {
        [Key]
        public int Id { get; set; }
        public string SysName { get; set; }
        public string License { get; set; }
        public bool IsActive { get; set; }
        public string SysLogo { get; set; }
        public string TempString { get; set; }
        public int TempInt { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }
    }
}
