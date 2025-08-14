namespace DotNetIdentity.Models.CesiZenModels.ViewModels
{
    public class DiagnosticResultatVM : DiagnosticQuestionnaireVM
    {
        public int ScoreTotal { get; set; }
        public string Interpretation { get; set; } = string.Empty;
    }
}
