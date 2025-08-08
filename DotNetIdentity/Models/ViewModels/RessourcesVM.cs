using Microsoft.AspNetCore.Mvc.Rendering;


namespace DotNetIdentity.Models.ViewModels
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

        /// <summary>
        /// Lien entre la ressource et l'image (table de jointure).
        /// </summary>
        public RessourcesImages? RessourceImage { get; set; }

        /// <summary>
        /// Liste déroulante des statuts disponibles pour la ressource.
        /// </summary>
        public SelectList? StatusList { get; set; }
    }
}
