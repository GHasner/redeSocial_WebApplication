using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace redeSocial.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Column("usuarioID")]
        [Display(Name = "UsuarioID")]
        public int usuarioID { get; set; }

        [Column("nomeUsuario")]
        [Display(Name = "Usuário")]
        public string? nomeUsuario { get; set; }

        [Column("nome")]
        [Display(Name = "Nome")]
        public string? nome { get; set; }

        [Column("telefone")]
        [Display(Name = "Telefone")]
        public string? telefone { get; set; }

        [Column("email")]
        [Display(Name = "E-mail")]
        public string? email { get; set; }

        [Column("senha")]
        [Display(Name = "Senha")]
        public string? senha { get; set; }

        [JsonIgnore]
        [NotMapped]
        public List<Postagem>? postagens { get; set; }

        [JsonIgnore]
        [NotMapped]
        public List<int>? idsBloqueados { get; set; }
    }
}
