using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetIdentity.Models.CesiZenModels.RessourcesModels
{
    [PrimaryKey("RessourceId", "ImageId")]
    public class RessourcesTags
    {
        [ForeignKey(nameof(Ressources))]        
        [Column("RessourceId")]
        public required int RessourceId { get; set; }

        [ForeignKey(nameof(Tags))]
        [Column("TagId")]
        public required int TagId { get; set; }

        // Navigation properties can be added if needed
        public virtual Ressources Ressources { get; set; }
        public virtual Tags Tags { get; set; }       
    }
}
