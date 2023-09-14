using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.Dto
{
    public class VillaDTO
    {
        // ta DTO (data transfer object) ta exoume gia na ta xrhsimopoioume
        // sta requests kai na mhn vazoume to kaoniko object pou exei
        // oles tis plhrofories, edw vazoume mono oti theloume na fainetai
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public int Occupancy { get; set; }
        public int Sqft { get; set; }

    }
}
