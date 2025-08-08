using Microsoft.AspNetCore.Mvc.Rendering;


namespace DotNetIdentity.Models.ViewModels
{
    public class RessourceAdminVM
    {
        public List<RessourcesVM> Ressources { get; set; } = new();
        public string Search { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }

}
