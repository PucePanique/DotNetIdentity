using DotNetIdentity.Models.CesiZenModels.DiagnosticModels;
using System.ComponentModel.DataAnnotations;

namespace DotNetIdentity.Models.CesiZenModels.ViewModels
{
    public class DiagnosticQuestionnaireVM
    {
        public List<EvenementReponseVM> Evenements { get; set; } = new();
        public int TotalPoints => Evenements.Where(e => e.Selectionne).Sum(e => e.Points);
    }
}
