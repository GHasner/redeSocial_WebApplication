using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace redeSocial.Models
{
    [Table("Comentario")]
    public class Comentario
    {
        [Column("comentID")]
        [Display(Name = "ComentarioID")]
        public int comentID { get; set; }

        [Column("postID")]
        [Display(Name = "ID da Postagem")]
        public int postID { get; set; }

        [Column("post")]
        [Display(Name = "Postagem")]
        public Postagem? post { get; set; }

        [Column("usuarioID")]
        [Display(Name = "ID do Usuário")]
        public int usuarioID { get; set; }

        [Column("usuario")]
        [Display(Name = "Usuário")]
        public Usuario? usuario { get; set; }

        [Column("comentario")]
        [Display(Name = "Comentário")]
        public string? comentario { get; set; }

        [Column("visible")]
        [Display(Name = "Visível")]
        public bool visible { get; set; }
    }
}
