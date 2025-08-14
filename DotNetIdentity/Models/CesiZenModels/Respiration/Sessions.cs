using DotNetIdentity.Models.CesiZenModels.RessourcesModels;
using DotNetIdentity.Models.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetIdentity.Models.CesiZenModels.Respiration
{
    public class Sessions
    {
        public int Id { get; set; }
        public int Cycles { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }                
        
        [ForeignKey(nameof(User))]
        [Column("UserId")]
        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }

        [ForeignKey(nameof(ExerciceConfigurations))]
        [Column("ExerciceConfigurationId")]
        public int ExerciceConfigurationId { get; set; }
        public virtual ExerciceConfigurations ExerciceConfigurations { get; set; }
    }
}
