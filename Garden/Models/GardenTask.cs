﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Models
{
    public class GardenTask
    {
        [Key]
        public int Id { get; set;}
        [Display(Name="제목")]
        public string Name { get; set; }
        [Display(Name="내용")]
        public string Description { get; set; }
        public int SubTypeId { get; set; }
        [Display(Name="활성화 여부")]
        public bool IsActivate { get; set; }
        [Display(Name="생성 날짜")]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }
        public int GardenId { get; set; }
        [ForeignKey("SubTypeId")]
        public virtual BaseSubType BaseSubType { get; set; }
        public virtual GardenSpace GardenSpace { get; set; }
        public ICollection<GardenWorkTime> GardenWorkTimes { get; set; }
        public ICollection<GardenUserTaskMap> GardenUserTaskMaps { get; set; }
    }
}
