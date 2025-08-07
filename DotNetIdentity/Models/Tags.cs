namespace DotNetIdentity.Models
{
    /// <summary>
    /// Représente une étiquette utilisée pour catégoriser des éléments.
    /// </summary>
    public class Tags
    {
        /// <summary>
        /// Identifiant unique de l'étiquette.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Libellé de l'étiquette.
        /// </summary>
        public string Label { get; set; } = string.Empty;
    }
}
