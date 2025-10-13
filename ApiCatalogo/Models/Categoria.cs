using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;


namespace ApiCatalogo.Models;

[Table("Categorias")]
public class Categoria
{
    public Categoria()
    {
        Produtos = new Collection<Produto>();
    }

    [Key] public int CategoriaId { get; set; }

    [Required] [StringLength(80)] public string? Nome { get; set; }
    [Required] [StringLength(300)] public string? ImageUrl { get; set; }

    [JsonIgnore]
    public ICollection<Produto>? Produtos { get; set; }
}