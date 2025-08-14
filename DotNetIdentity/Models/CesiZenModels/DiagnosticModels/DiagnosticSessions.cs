using DotNetIdentity.Models.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetIdentity.Models.CesiZenModels.DiagnosticModels
{
    public class DiagnosticSessions
    {
        public int Id { get; set; }

        public DateTime DateDiagnostic { get; set; } = DateTime.UtcNow;

        public int TotalPoints { get; set; }

        public string NiveauStress { get; set; } = string.Empty;

        [ForeignKey(nameof(AppUser))]
        public string? UserId { get; set; } // Nullable pour permettre l’anonyme
        public virtual AppUser? User { get; set; }
    }
}
