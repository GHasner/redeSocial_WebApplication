using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace redeSocial.Models
{
    [Table("Banimentos")]
    public class Banimentos
    {
        [Column("banID")]
        [Display(Name = "BanID")]
        public int banID { get; set; }

        [Column("usuarioID")]
        [Display(Name = "ID do Usuário")]
        public int usuarioID { get; set; }

        [Column("usuario")]
        [Display(Name = "Usuário")]
        public Usuario? usuario { get; set; }

        [Column("usuarioBanID")]
        [Display(Name = "ID do Usuário Banido")]
        public int usuarioBanID { get; set; }

        [Column("usuarioBan")]
        [Display(Name = "Usuário Banido")]
        public Usuario? usuarioBan { get; set; }
    }
}
