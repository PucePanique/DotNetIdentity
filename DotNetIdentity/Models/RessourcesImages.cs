using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetIdentity.Models
{
    [PrimaryKey("RessourceId", "ImageId")]
    public class RessourcesImages
    {
        [ForeignKey(nameof(Ressources))]        
        [Column("RessourceId")]
        public required int RessourceId { get; set; }

        [ForeignKey(nameof(Images))]
        [Column("ImageId")]
        public required int ImageId { get; set; }

        // Navigation properties can be added if needed
        public virtual Ressources Ressources { get; set; }
        public virtual Images Images { get; set; }       
    }
}
