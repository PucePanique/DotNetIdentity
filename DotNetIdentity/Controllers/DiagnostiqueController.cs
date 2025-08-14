using DotNetIdentity.Data;
using DotNetIdentity.Models.CesiZenModels.DiagnosticModels;
using DotNetIdentity.Models.CesiZenModels.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DotNetIdentity.Controllers
{
    public class DiagnostiqueController : Controller
    {
        private readonly AppDbContext _context;

        public DiagnostiqueController(AppDbContext context)
        {
            _context = context;
        }

        // GET : afficher le questionnaire
        public async Task<IActionResult> Index()
        {
            var evenements = await _context.DiagnosticEvenements
                .OrderBy(e => e.Nom)
                .Select(e => new EvenementReponseVM
                {
                    Id = e.Id,
                    Nom = e.Nom,
                    Points = e.Points
                })
                .ToListAsync();

            var vm = new DiagnosticQuestionnaireVM
            {
                Evenements = evenements
            };

            return View(vm);
        }

        // POST : soumettre le questionnaire
        [HttpPost]
        public async Task<IActionResult> Index(DiagnosticQuestionnaireVM model)
        {
            // Calcul du total
            var totalPoints = model.TotalPoints;
            var niveauStress = CalculerNiveauStress(totalPoints);

            string? userId = User.Identity?.IsAuthenticated == true
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : "00000000-0000-0000-0000-000000000000"; // Id de l'utilisateur ANONYME

            // Créer la session
            var session = new DiagnosticSessions
            {
                DateDiagnostic = DateTime.UtcNow,
                TotalPoints = totalPoints,
                NiveauStress = niveauStress,
                UserId = userId
            };
            _context.DiagnosticSessions.Add(session);
            await _context.SaveChangesAsync();

            // Ajouter les réponses
            foreach (var ev in model.Evenements.Where(e => e.Selectionne))
            {
                _context.DiagnosticReponses.Add(new DiagnosticReponses
                {
                    DiagnosticSessionId = session.Id,
                    DiagnosticEvenementId = ev.Id
                });
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Resultats), new { id = session.Id });
        }

        public async Task<IActionResult> Resultats(int id)
        {
            var session = await _context.DiagnosticSessions
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (session == null) return NotFound();

            return View(session);
        }

        private string CalculerNiveauStress(int points)
        {
            if (points < 150) return "Faible";
            if (points < 300) return "Modéré";
            return "Élevé";
        }
    }
}
