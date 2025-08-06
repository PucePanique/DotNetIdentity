using System.Collections;

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
        int Id { get; set; }

        /// <summary>
        /// Données binaires de l'image.
        /// </summary>
        BitArray Image { get; set; }
    }
}
