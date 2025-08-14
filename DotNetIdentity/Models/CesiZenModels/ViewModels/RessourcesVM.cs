using DotNetIdentity.Models.CesiZenModels.RessourcesModels;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace DotNetIdentity.Models.CesiZenModels.ViewModels
{
    /// <summary>
    /// ViewModel combinant une ressource et son image associée.
    /// </summary>
    public class RessourcesVM : Ressources
    {
        /// <summary>
        /// Les données binaires de l'image associée à la ressource.
        /// </summary>
        public string? ImagePath { get; set; }
        public int? ImageId { get; set; }
    }
}
