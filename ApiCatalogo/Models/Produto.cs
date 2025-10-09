using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ApiCatalogo.Validations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ApiCatalogo.Models;

[Table("Produtos")]
public class Produto : IValidatableObject
{
    [Key] public int ProdutoId { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório!")]
    [StringLength(80, MinimumLength = 5, ErrorMessage = "O nome deve ter entre {2} e {1} caracteres!")]
    // [PrimeiraLetraMaiuscula]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300, ErrorMessage = "A descrição deve ter no máximo {1} caracteres!")]
    public string? Descricao { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    [Range(1, 10000, ErrorMessage = "O preço deve estar entre {1} e {2}!")]
    public decimal Preco { get; set; }

    [Required] [StringLength(300)] public string? ImageUrl { get; set; }

    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }
    public int CategoriaId { get; set; }

    [JsonIgnore] public Categoria? Categoria { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var primeiraLetra = this.Nome.ToString();
        if (primeiraLetra != primeiraLetra.ToUpper())
            yield return new ValidationResult("A primeira letra do nome do produto deve ser maiúscula",
                new[] { nameof(this.Nome) });
        if (this.Estoque <= 0)
        {
            yield return new ValidationResult("O estoque deve ser maior que zero",
                new[] { nameof(this.Estoque) });
        }
    }
}