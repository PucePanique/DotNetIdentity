using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FuturoCheques.Tools
{
    /// <summary>
    /// TagHelper personnalisé permettant de générer dynamiquement une balise <label></label> avec mise en forme conditionnelle
    /// selon que le champ associé est requis ou non, en se basant sur les métadonnées du modèle.
    /// </summary>
    /// <remarks>
    /// Ce tag helper ajoute automatiquement une classe CSS <c>fw-bold</c> et un astérisque rouge <c>*</c> pour
    /// indiquer les champs obligatoires définis via l’attribut <c>[Required]</c> dans le modèle.
    /// 
    /// Utilisation dans une vue Razor :
    /// <code>
    /// &lt;required-label asp-for="Nom_Client" /&gt;
    /// Ajout dans _ViewImport.cshtml : @addTagHelper *, [Nom du fichier .csproj sans l'éxention]
    /// </code>
    /// </remarks>
    public class RequiredLabelTagHelper : TagHelper
    {
        /// <summary>
        /// Représente l'expression du modèle associée au champ pour lequel générer le label.
        /// Cette propriété est automatiquement remplie lorsque l'attribut <c>asp-for</c> est utilisé dans la vue.
        /// </summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; } = default!;

        /// <summary>
        /// Méthode principale appelée à la compilation de la vue Razor.
        /// Elle construit dynamiquement le HTML de la balise &lt;label&gt; en fonction des métadonnées du champ.
        /// </summary>
        /// <param name="context">Le contexte d'exécution du TagHelper (informations sur la balise et l'environnement).</param>
        /// <param name="output">L'objet utilisé pour générer le HTML de sortie à injecter dans la page.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Récupère les métadonnées associées au champ ciblé (type, validations, annotations, etc.)
            var metadata = For.Metadata;

            // Détermine si le champ est marqué comme requis dans le modèle
            var isRequired = metadata.IsRequired;

            // Définit que ce TagHelper génère une balise <label> (au lieu de la balise d'origine)
            output.TagName = "label";

            // Force le rendu de la balise complète (avec balise ouvrante et fermante) pour y inclure du contenu HTML
            output.TagMode = TagMode.StartTagAndEndTag;

            // Ajoute l'attribut "for" au label pour lier correctement au champ input avec le même nom
            output.Attributes.SetAttribute("for", For.Name);

            // Applique une classe CSS conditionnelle : en gras si requis
            output.Attributes.SetAttribute("class", isRequired ? "form-label fw-bold" : "form-label");

            // Texte du label : utilise DisplayName si défini, sinon le nom brut du champ
            var labelText = metadata.DisplayName ?? For.Name;

            // Si le champ est requis, ajoute un astérisque rouge dans la balise <label>
            if (isRequired)
            {
                output.Content.SetHtmlContent($@"
                    {labelText}
                    <span class=""text-danger"">*</span>
                ");
            }
            else
            {
                // Sinon, juste le nom du champ
                output.Content.SetContent(labelText);
            }
        }
    }
}
