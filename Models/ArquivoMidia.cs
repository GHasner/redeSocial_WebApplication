using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace redeSocial.Models
{
    [Table("ArquivoMidia")]
    public class ArquivoMidia
    {
        [Column("arquivoID")]
        [Display(Name = "Imagem ID")]
        public int arquivoID { get; set; }

        [Column("tipoArquivo")]
        [Display(Name = "Tipo de Arquivo")]
        public string? tipoArquivo { get; set; }

        [Column("nomeArmazenamento")]
        [Display(Name = "Nome do Arquivo no Armazenamento")]
        public string? nomeArmazenamento { get; set; }

        [Column("postID")]
        [Display(Name = "ID da Postagem")]
        public int postID { get; set; }

        [Column("post")]
        [Display(Name = "Postagem")]
        public Postagem? post { get; set; }
    }
}
