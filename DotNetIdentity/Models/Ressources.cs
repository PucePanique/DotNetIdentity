using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetIdentity.Models
{
    /// <summary>
    /// Représente une ressource avec ses propriétés principales, telles que le titre, la description, l'URL, la catégorie, l'image, les informations de création et de mise à jour, ainsi que le statut.
    /// </summary>
    public class Ressources
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique de la ressource.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Obtient ou définit le titre de la ressource.
        /// </summary>
        [Required]
        public required string Title { get; set; }
        /// <summary>
        /// Obtient ou définit la description de la ressource.
        /// </summary>
        [Required]
        public string? Description { get; set; }
        /// <summary>
        /// Obtient ou définit l'URL de la ressource.
        /// </summary>
        [Required]
        public string? Url { get; set; }
        /// <summary>
        /// Obtient ou définit la catégorie de la ressource.
        /// </summary>
        [Required]
        public string? Category { get; set; }
        /// <summary>
        /// Obtient ou définit l'utilisateur ayant créé la ressource.
        /// </summary>
        public string? CreatedBy { get; set; }
        /// <summary>
        /// Obtient ou définit la date de création de la ressource.
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Obtient ou définit l'utilisateur ayant mis à jour la ressource.
        /// </summary>
        public string? UpdatedBy { get; set; }
        /// <summary>
        /// Obtient ou définit la date de mise à jour de la ressource.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
        /// <summary>
        /// Obtient ou définit l'identifiant du statut de la ressource (par exemple, "Active", "Inactive", "Archived").
        /// </summary>
        [ForeignKey(nameof(status))]
        [Column("StatuId")]        
        public int StatuId { get; set; }
        /// <summary>
        /// Obtient ou définit le statut de la ressource.
        /// </summary>
        public Status status { get; set; } = new();
    }
}
