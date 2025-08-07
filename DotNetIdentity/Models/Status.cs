namespace DotNetIdentity.Models
{
    /// <summary>
    /// Représente le statut avec un identifiant et un libellé.
    /// </summary>
    public class Status
    {
        /// <summary>
        /// Identifiant unique du statut.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Libellé du statut.
        /// </summary>
        public string Label { get; set; } = string.Empty;
    }
}
