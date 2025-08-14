using DotNetIdentity.Data;
using DotNetIdentity.Models.CesiZenModels.Respiration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DotNetIdentity.Controllers
{
    public class RespirationController : Controller
    {
        private readonly AppDbContext _context;

        public RespirationController(AppDbContext context)
        {
            _context = context;
        }

        // Liste des configurations (748, 55, 46)
        public async Task<IActionResult> Index()
        {
            var configurations = await _context.ExerciceConfigurations
                .Include(c => c.Exercice)
                .ToListAsync();

            return View(configurations);
        }

        // Lance une session
        [HttpPost]
        public async Task<IActionResult> StartSession(int configurationId, int cycles = 6)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // AspNetUsers._Id_

            var session = new Sessions
            {
                ExerciceConfigurationId = configurationId,
                UserId = userId,
                Cycles = cycles,
                StartedAt = DateTime.UtcNow
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            return RedirectToAction("Run", new { sessionId = session.Id });
        }

        // Vue pour exécuter l'exercice
        public async Task<IActionResult> Run(int sessionId)
        {
            var session = await _context.Sessions
                .Include(s => s.ExerciceConfigurations) 
                .FirstOrDefaultAsync(s => s.Id == sessionId);

            if (session == null)
                return NotFound();

            return View(session);
        }
    }
}
