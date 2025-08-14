using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetIdentity.Models.CesiZenModels.DiagnosticModels
{
    public class DiagnosticReponses
    {
        public int Id { get; set; }

        [ForeignKey(nameof(DiagnosticSessions))]
        public int DiagnosticSessionId { get; set; }
        public virtual DiagnosticSessions DiagnosticSessions { get; set; } = null!;

        [ForeignKey(nameof(DiagnosticEvenements))]
        public int DiagnosticEvenementId { get; set; }
        public virtual DiagnosticEvenements DiagnosticEvenement { get; set; } = null!;
    }
}
