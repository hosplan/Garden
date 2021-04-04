using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Models
{
    public class GardenAttachMap
    {
        [Key]
        public int Id { get; set; }
        public int GardenId { get; set; }
        public int AttachmentId { get; set; }

        [ForeignKey("GardenId")]
        public virtual GardenSpace Garden { get; set; }
        [ForeignKey("AttachmentId")]
        public virtual Attachment Attachment { get; set; }

    }
}
