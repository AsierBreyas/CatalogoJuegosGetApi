using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Juegos.Models
{
    public class CatalogoJuegos
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int JuegoId { get; set; }
        public string title { get; set; }
        public string thumbnail { get; set; }
        public string short_description { get; set; }
        public string description { get; set; }
        public string game_url { get; set; }
        public string genre { get; set; }
        public string plataform { get; set; }
        public string publisher { get; set; }
        public string developer { get; set; }
        public DateTime release_date { get; set; }
        public string freetogame_profile_url { get; set; }
        public string screenshots { get; set; }
        public string os { get; set; }
        public string processor { get; set; }
        public string memory { get; set; }
        public string graphics { get; set; }
        public string storage { get; set; }
    }
}
