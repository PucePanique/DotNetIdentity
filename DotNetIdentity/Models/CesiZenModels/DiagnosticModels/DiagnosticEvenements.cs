using System.ComponentModel.DataAnnotations;

namespace DotNetIdentity.Models.CesiZenModels.DiagnosticModels
{
    public class DiagnosticEvenements
    {
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; } = string.Empty;

        [Required]
        public int Points { get; set; }
    }
}
