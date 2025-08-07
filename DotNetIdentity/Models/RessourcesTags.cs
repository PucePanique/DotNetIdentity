using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetIdentity.Models
{
    [PrimaryKey("RessourceId", "TagId")]
    public class RessourcesTags
    {
        /// <summary>
        /// Identifiant de la ressource associée.
        /// </summary>
        [ForeignKey(nameof(Ressources))]
        [Column("RessourceId")]
        public int RessourceId { get; set; }
        /// <summary>
        /// Identifiant de l'étiquette associée.
        /// </summary>
        [ForeignKey(nameof(Tags))]
        [Column("TagId")]
        public int TagId { get; set; }
        // Navigation properties can be added if needed
        public virtual Ressources Ressources { get; set; }
        public virtual Tags Tags { get; set; }
    }    
}
