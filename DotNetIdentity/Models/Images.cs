namespace DotNetIdentity.Models
{
    /// <summary>
    /// Représente une image avec un identifiant et des données binaires.
    /// </summary>
    public class Images
    {
        /// <summary>
        /// Identifiant unique de l'image.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Données binaires de l'image.
        /// </summary>
        public string? Image { get; set; }

        // Navigation to join table
        public ICollection<RessourcesImages> RessourcesImages { get; set; } = new List<RessourcesImages>();
    }
}
