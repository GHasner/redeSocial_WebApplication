using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace redeSocial.Models
{
    [Table("Postagem")]
    public class Postagem
    {
        [Column("postID")]
        [Display(Name = "PostID")]
        public int postID { get; set; }

        [Column("conteudoTxt")]
        [Display(Name = "Conteudo")]
        public string? conteudoTxt { get; set; }

        [Column("usuarioID")]
        [Display(Name = "ID do Usuário")]
        public int usuarioID { get; set; }

        [Column("usuario")]
        [Display(Name = "Usuário")]
        public Usuario? usuario { get; set; }

        [JsonIgnore]
        [NotMapped]
        public List<Comentario> comentarios { get; set; }
    }
}
