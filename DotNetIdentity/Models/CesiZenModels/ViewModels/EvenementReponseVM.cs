namespace DotNetIdentity.Models.CesiZenModels.ViewModels
{
    public class EvenementReponseVM
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public int Points { get; set; }
        public bool Selectionne { get; set; }
    }
}
