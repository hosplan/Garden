using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Models
{
    public class Attachment
    {
        [Key]
        public int Id { get; set; }
        [Display(Name="파일 이름")]
        public string FileName { get; set; }
        [Display(Name ="파일경로")]
        public string FilePath { get; set; }
        [Display(Name ="파일 확장자")]
        public string FileExt { get; set; }
        [Display(Name ="파일크기")]
        public double FileSize { get; set; }
        [Display(Name ="등록일자")]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        public virtual ICollection<GardenTaskAttachMap> GardenTaskAttachMaps { get; set; }
    }
}
