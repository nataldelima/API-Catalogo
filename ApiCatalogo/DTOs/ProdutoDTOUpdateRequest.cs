using System.ComponentModel.DataAnnotations;

namespace ApiCatalogo.DTOs;

public class ProdutoDTOUpdateRequest : IValidatableObject
{
    [Range(1, 9999, ErrorMessage = "O estoque deve estar entre 1 e 9999")]
    public float Estoque { get; set; }

    public DateTime DataCadastro { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DataCadastro.Date <= DateTime.Now)
        {
            yield return new ValidationResult("A data deve ser maior que a data atual",
                new[] { nameof(this.DataCadastro) });
        }
    }
}