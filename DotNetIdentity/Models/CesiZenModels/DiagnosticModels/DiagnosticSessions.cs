using DotNetIdentity.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetIdentity.Models.CesiZenModels.DiagnosticModels
{
    public class DiagnosticSessions
    {
        public int Id { get; set; }

        
        public DateTime DateDiagnostic { get; set; } = DateTime.UtcNow;

        public int TotalPoints { get; set; }
        public string NiveauStress { get; set; }

        [ForeignKey(nameof(AppUser))]
        [Column("UserId")]
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
