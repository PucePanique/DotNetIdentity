using DotNetIdentity.Models.CesiZenModels.DiagnosticModels;
using System.ComponentModel.DataAnnotations;

namespace DotNetIdentity.Models.CesiZenModels.ViewModels
{
    public class DiagnosticQuestionnaireVM
    {
        public List<DiagnosticEvenements> Evenements { get; set; } = new();

        [Required(ErrorMessage = "Vous devez sélectionner au moins un événement ou aucun si rien ne s'applique.")]
        public List<int> EvenementsSelectionnes { get; set; } = new();
    }
}
