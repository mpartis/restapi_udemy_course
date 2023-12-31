﻿using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.Dto
{
    public class VillaDTO
    {
        // ta DTO (data transfer object) ta exoume gia na ta xrhsimopoioume
        // sta requests kai na mhn vazoume to kanoniko object pou exei
        // oles tis plhrofories, edw vazoume mono oti theloume na fainetai
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Details { get; set; }
        [Required]
        public double Rate { get; set; }
        public int Occupancy { get; set; }
        public int Sqft { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }

    }
}
