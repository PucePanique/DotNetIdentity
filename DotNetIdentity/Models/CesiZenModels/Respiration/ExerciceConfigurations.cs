using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetIdentity.Models.CesiZenModels.Respiration
{
    public class ExerciceConfigurations
    {
        public int Id { get; set; }
        public string Label { get; set; } = string.Empty;
        public int InhaleDuration { get; set; }
        public int HoldDuration { get; set; }
        public int ExhaleDuration { get; set; }

        [ForeignKey(nameof(Exercice))]
        [Column("ExerciceId ")]
        public int ExerciceId { get; set; }
        public virtual Exercices Exercice { get; set; } = null!;
    }
}
