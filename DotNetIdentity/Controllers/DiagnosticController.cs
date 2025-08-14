using DotNetIdentity.Data;
using DotNetIdentity.Models.CesiZenModels.DiagnosticModels;
using DotNetIdentity.Models.CesiZenModels.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DotNetIdentity.Controllers
{
    public class DiagnosticController : Controller
    {
        private readonly AppDbContext _context;

        public DiagnosticController(AppDbContext context)
        {
            _context = context;
        }

        //// GET : Affiche le questionnaire
        //public async Task<IActionResult> Index()
        //{
        //    var vm = new DiagnosticQuestionnaireVM
        //    {
        //        Evenements = await _context.DiagnosticEvenement.OrderBy(e => e.Nom).ToListAsync()
        //    };
        //    return View(vm);
        //}

        //// POST : Traite le questionnaire
        //[HttpPost]
        //public async Task<IActionResult> Index(DiagnosticQuestionnaireVM model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        model.Evenements = await _context.DiagnosticEvenement.OrderBy(e => e.Nom).ToListAsync();
        //        return View(model);
        //    }

        //    var points = await _context.DiagnosticEvenement
        //        .Where(e => model.EvenementsSelectionnes.Contains(e.Id))
        //        .SumAsync(e => e.Points);

        //    var userId = User.Identity?.IsAuthenticated == true
        //        ? User.FindFirstValue(ClaimTypes.NameIdentifier)
        //        : "00000000-0000-0000-0000-000000000000"; // ID anonyme

        //    var resultat = new DiagnosticSessions
        //    {
        //        UserId = userId,
        //        TotalPoints = points,
        //        DateDiagnostic = DateTime.UtcNow
        //    };

        //    _context.DiagnosticResultat.Add(resultat);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Resultat), new { id = resultat.Id });
        //}

        //// GET : Affiche le résultat
        //public async Task<IActionResult> Resultat(int id)
        //{
        //    var resultat = await _context.DiagnosticResultat.FindAsync(id);
        //    if (resultat == null) return NotFound();

        //    var vm = new DiagnosticResultatVM
        //    {
        //        ScoreTotal = resultat.TotalPoints,
        //        Interpretation = InterpreterScore(resultat.TotalPoints)
        //    };

        //    return View(vm);
        //}

        //private string InterpreterScore(int score)
        //{
        //    if (score < 150)
        //        return "Niveau de stress faible (faible probabilité de problème lié au stress).";
        //    if (score <= 299)
        //        return "Niveau de stress modéré (risque modéré de problèmes de santé liés au stress).";
        //    return "Niveau de stress élevé (risque important de problèmes de santé liés au stress).";
        //}
    }
}
