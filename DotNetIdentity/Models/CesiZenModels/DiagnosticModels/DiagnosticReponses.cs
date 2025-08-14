using DotNetIdentity.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetIdentity.Models.CesiZenModels.DiagnosticModels
{
    public class DiagnosticReponses
    {
        public int Id { get; set; }

        [ForeignKey(nameof(DiagnosticSessions))]
        [Column("DiagnosticSessionId")]
        public int DiagnosticSessionId { get; set; }
        public DiagnosticSessions DiagnosticSessions { get; set; } = null!;

        [ForeignKey(nameof(DiagnosticEvenements))]
        [Column("DiagnosticEvenementId")]
        public int DiagnosticEvenementId { get; set; }
        public DiagnosticEvenements DiagnosticEvenement { get; set; } = null!;
    }
}
