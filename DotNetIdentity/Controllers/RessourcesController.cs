using DotNetIdentity.Data;
using DotNetIdentity.Models;
using DotNetIdentity.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;


namespace DotNetIdentity.Controllers
{
    public class RessourcesController : Controller
    {
        private readonly AppDbContext _Context;
        /// <summary>
        /// Property of type ILogger
        /// </summary>
        private readonly ILogger<UserController> _logger;
        public RessourcesController(AppDbContext Context, ILogger<UserController> logger)
        {
            _Context = Context;
            _logger = logger;
        }

        // Remplacement de la ligne problématique dans la méthode Index
        public async Task<IActionResult> Index(string search = "", int take = 6)
        {
            var ressourcesQuery = from res in _Context.Ressources
                                  join resImg in _Context.RessourcesImages on res.Id equals resImg.RessourceId
                                  join img in _Context.Images on resImg.ImageId equals img.Id
                                  join status in _Context.Status on res.StatuId equals status.Id
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
                                      StatuId = res.StatuId,
                                      status = status,
                                      ImagePath = img.Image
                                  };
            ressourcesQuery = ressourcesQuery.Where(r => r.status.Label == "Actif");

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


        // GET: RessourcesController/Create
        public async Task<IActionResult> Creer()
        {
            RessourcesVM rvm = new() { Title = "" };
            List<Status> statusList = await _Context.Status.ToListAsync();
            rvm.StatusList = new SelectList(statusList, "Id", "Label");
            return View(rvm);
        }

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
                        StatuId = ressourceVm.StatuId,
                        status = await _Context.Status.FindAsync(ressourceVm.StatuId)
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Liste(string search = "", int page = 1)
        {
            const int pageSize = 10;

            // Requête de base incluant le status (via navigation) et les ressources
            var baseQuery = _Context.Ressources
                .Include(r => r.status)
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

            // Requête paginée + projection vers RessourcesVM avec jointure explicite sur RessourceImage
            var ressources = await (
                from res in baseQuery
                join ri in _Context.RessourcesImages on res.Id equals ri.RessourceId into riJoin
                from ri in riJoin.DefaultIfEmpty()
                join img in _Context.Images on ri.ImageId equals img.Id into imgJoin
                from img in imgJoin.DefaultIfEmpty()
                orderby res.CreatedAt descending
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
                    StatuId = res.StatuId,
                    status = res.status,
                    ImagePath = img != null ? img.Image : null
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return View(ressources);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleStatut(int id)
        {
            var ressource = await _Context.Ressources.FindAsync(id);
            if (ressource == null)
                return NotFound();

            var statutActif = await _Context.Status.FirstOrDefaultAsync(s => s.Label == "Actif");
            var statutInactif = await _Context.Status.FirstOrDefaultAsync(s => s.Label == "Inactif");

            if (statutActif == null || statutInactif == null)
                return BadRequest("Les statuts nécessaires n'existent pas.");

            ressource.StatuId = (ressource.StatuId == statutActif.Id)
                ? statutInactif.Id
                : statutActif.Id;

            ressource.UpdatedAt = DateTime.Now;
            ressource.UpdatedBy = User.Identity?.Name ?? "System";

            await _Context.SaveChangesAsync();
            _logger.LogWarning("UPD: User " + User.Identity!.Name + ", Ressource " + ressource.Id + " Status de ressource mis à jours.");
            return RedirectToAction(nameof(Liste));
        }


    }
}
