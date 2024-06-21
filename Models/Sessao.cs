using redeSocial.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace redeSocial.Models
{
    public class Sessao
    {
        [JsonIgnore]
        [NotMapped]
        public Usuario? usuarioLogado { get; set; }

        [JsonIgnore]
        [NotMapped]
        public List<Postagem>? postagens { get; set; }
    }
}
