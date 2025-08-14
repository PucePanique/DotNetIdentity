using DotNetIdentity.Data;
using DotNetIdentity.Models.CesiZenModels.RessourcesModels;
using DotNetIdentity.Models.CesiZenModels.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;


namespace DotNetIdentity.Controllers
{
    /// <summary>
    /// Contrôleur responsable de la gestion des ressources, incluant leur création,
    /// affichage, modification de statut et gestion côté administrateur.
    /// </summary>
    public class RessourcesController : Controller
    {
        /// <summary>
        /// Contexte de base de données utilisé pour accéder aux entités.
        /// </summary>
        private readonly AppDbContext _Context;
        /// <summary>
        /// Property of type ILogger
        /// </summary>
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Initialise une nouvelle instance du contrôleur <see cref="RessourcesController"/>.
        /// </summary>
        /// <param name="Context">Le contexte de base de données.</param>
        /// <param name="logger">L'instance du logger pour le suivi des opérations.</param>
        public RessourcesController(AppDbContext Context, ILogger<UserController> logger)
        {
            _Context = Context;
            _logger = logger;
        }

        /// <summary>
        /// Affiche la liste des ressources actives côté public avec recherche optionnelle et limitation.
        /// </summary>
        /// <param name="search">Chaîne de recherche appliquée sur le titre et la description.</param>
        /// <param name="take">Nombre maximum de ressources à afficher.</param>
        /// <returns>Vue contenant les ressources filtrées.</returns>
        public async Task<IActionResult> Index(string search = "", int take = 6)
        {
            var ressourcesQuery = from res in _Context.Ressources
                                  join resImg in _Context.RessourcesImages on res.Id equals resImg.RessourceId
                                  join img in _Context.Images on resImg.ImageId equals img.Id
                                  select new RessourcesVM
                                  {
                                      Id = res.Id,
                                      Title = res.Title,
                                      Description = res.Description,
                                      Url = res.Url,
                                      Category = res.Category,
                                      CreatedBy = res.CreatedBy,
                                      CreatedAt = res.CreatedAt,
                                      UpdatedBy = res.UpdatedBy,
                                      UpdatedAt = res.UpdatedAt,
                                      Status = res.Status,
                                      ImagePath = img.Image
                                  };
            ressourcesQuery = ressourcesQuery.Where(r => r.Status);

            if (!string.IsNullOrWhiteSpace(search))
            {
                ressourcesQuery = ressourcesQuery.Where(r =>
                    r.Title.Contains(search) || r.Description.Contains(search));
            }

            var ressources = await ressourcesQuery
                .OrderByDescending(r => r.CreatedAt)
                .Take(take)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Take = take;
            ViewBag.Total = await ressourcesQuery.CountAsync();

            return View(ressources);
        }


        /// <summary>
        /// Affiche le formulaire de création d'une ressource.
        /// </summary>
        /// <returns>Vue contenant le formulaire de création.</returns>
        public async Task<IActionResult> Creer()
        {
            RessourcesVM rvm = new() { Title = "" };

            return View(rvm);
        }

        /// <summary>
        /// Traite la soumission du formulaire de création d'une ressource.
        /// </summary>
        /// <param name="ressourceVm">Objet ViewModel contenant les données de la ressource.</param>
        /// <param name="pic">Fichier image associé à la ressource.</param>
        /// <returns>Vue de confirmation ou formulaire en cas d'erreur.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreerRessource(RessourcesVM ressourceVm, IFormFile pic)
        {
            string FilePath = "";
            string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif", ".bmp" };
            string[] allowedContentTypes = new[] { "image/jpeg", "image/png", "image/webp", "image/gif", "image/bmp" };

            if (pic is not null)
            {
                string NomImg = Path.GetFileName(pic.FileName);
                string ext = Path.GetExtension(pic.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(ext) || !allowedContentTypes.Contains(pic.ContentType))
                {
                    ModelState.AddModelError("Image", "Format d'image non supporté. Formats autorisés : jpg, jpeg, png, webp, gif, bmp.");
                    return View("Creer", ressourceVm);
                }

                string relativePath = Path.Combine("/assets/RessourcesImages", NomImg);
                string absolutePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/RessourcesImages", NomImg);

                using (var stream = new FileStream(absolutePath, FileMode.Create))
                {
                    await pic.CopyToAsync(stream);
                }

                FilePath = relativePath;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Ressources Ressource = new()
                    {
                        Title = ressourceVm.Title,
                        Description = ressourceVm.Description,
                        Url = ressourceVm.Url,
                        Category = ressourceVm.Category,
                        CreatedAt = DateTime.Now,
                        CreatedBy = User.Identity?.Name ?? "Inconnu",
                        Status = ressourceVm.Status
                    };

                    await _Context.Ressources.AddAsync(Ressource);

                    Images Images = new()
                    {
                        Image = FilePath
                    };

                    await _Context.Images.AddAsync(Images);
                    await _Context.SaveChangesAsync();

                    RessourcesImages ressourceImage = new()
                    {
                        RessourceId = Ressource.Id,
                        ImageId = Images.Id
                    };
                    await _Context.RessourcesImages.AddAsync(ressourceImage);

                    await _Context.SaveChangesAsync();
                    _logger.LogWarning("CREATE: User " + User.Identity!.Name + " Ressource créé.");
                }
                catch (Exception)
                {

                }
            }

            return View(nameof(RessourcesController.Creer));
        }

        /// <summary>
        /// Liste les ressources côté administrateur avec pagination et recherche.
        /// </summary>
        /// <param name="search">Chaîne de recherche appliquée sur le titre, la description et l'URL.</param>
        /// <param name="page">Numéro de page actuelle.</param>
        /// <returns>Vue affichant les ressources paginées et filtrées.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Liste(string search = "", int page = 1)
        {
            const int pageSize = 10;

            var baseQuery = _Context.Ressources
                .Include(r => r.RessourcesImages)
                    .ThenInclude(ri => ri.Images)
                .AsQueryable();

            // Option de recherche
            if (!string.IsNullOrWhiteSpace(search))
            {
                baseQuery = baseQuery.Where(r =>
                    r.Title.Contains(search) ||
                    r.Description.Contains(search) ||
                    r.Url.Contains(search));
            }

            int totalItems = await baseQuery.CountAsync();

            // Projection vers RessourcesVM
            var ressources = await baseQuery
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RessourcesVM
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    Url = r.Url,
                    Category = r.Category,
                    CreatedBy = r.CreatedBy,
                    CreatedAt = r.CreatedAt,
                    UpdatedBy = r.UpdatedBy,
                    UpdatedAt = r.UpdatedAt,
                    Status = r.Status,
                    ImagePath = r.RessourcesImages
                        .Select(ri => ri.Images.Image)
                        .FirstOrDefault() // Première image liée
                })
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return View(ressources);

        }

        /// <summary>
        /// Active ou désactive une ressource selon son identifiant.
        /// </summary>
        /// <param name="id">Identifiant unique de la ressource à modifier.</param>
        /// <param name="search">Chaîne de recherche courante à conserver.</param>
        /// <param name="page">Page courante à conserver.</param>
        /// <returns>Redirection vers la liste des ressources avec les paramètres préservés.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleStatut(int id, string search = "", int page = 1)
        {
            var ressource = await _Context.Ressources.FindAsync(id);
            if (ressource == null)
                return NotFound();

            ressource.Status = !ressource.Status;
            ressource.UpdatedAt = DateTime.Now;
            ressource.UpdatedBy = User.Identity?.Name ?? "System";

            await _Context.SaveChangesAsync();
            _logger.LogWarning("UPD: User " + User.Identity!.Name + ", Ressource " + ressource.Id + " Status de ressource mis à jours.");
            return RedirectToAction(nameof(Liste), new { search, page });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Modifiaction(int id)
        {
            RessourcesVM ressource = await _Context.Ressources
                .Include(r => r.RessourcesImages)
                    .ThenInclude(ri => ri.Images)
                    .Where(r => r.Id == id)
                    .Select(r => new RessourcesVM
                    {
                        Title = r.Title,
                        Description = r.Description,
                        Url = r.Url,
                        Category = r.Category,
                        ImageId = r.RessourcesImages.FirstOrDefault().ImageId,
                        ImagePath = r.RessourcesImages
                        .Select(ri => ri.Images.Image)
                        .FirstOrDefault()
                    }).SingleOrDefaultAsync() ?? new RessourcesVM { Title = "" };
            ;

            return View(ressource);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ModifierRessource(RessourcesVM ressourcesVM)
        {
            var ressource = await _Context.Ressources.FindAsync(ressourcesVM.Id);


            if (ressource == null)
                return NotFound();

            ressource.Title = ressourcesVM.Title;
            ressource.Description = ressourcesVM.Description;
            ressource.Url = ressourcesVM.Url;
            ressource.Category = ressourcesVM.Category;

            _Context.Ressources.Update(ressource);

            var Image = await _Context.Images.FindAsync(ressourcesVM.ImageId);
            if (Image == null)
                return NotFound();

            Image.Image = ressourcesVM.ImagePath;
            _Context.Images.Update(Image);

            return View(nameof(Modifiaction),ressourcesVM);
        }
    }
}
