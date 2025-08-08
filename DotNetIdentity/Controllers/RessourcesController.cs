using DotNetIdentity.Data;
using DotNetIdentity.Models;
using DotNetIdentity.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetIdentity.Controllers
{
    public class RessourcesController : Controller
    {
        private readonly AppDbContext _Context;
        public RessourcesController(AppDbContext Context)
        {
            _Context = Context;
        }

        // Remplacement de la ligne problématique dans la méthode Index
        public async Task<ActionResult> Index()
        {
            // Pseudocode détaillé :
            // 1. Sélectionner toutes les ressources depuis _Context.Ressources.
            // 2. Faire un join avec _Context.RessourcesImages sur Ressources.Id == RessourcesImages.RessourceId.
            // 3. Faire un join avec _Context.Images sur RessourcesImages.ImageId == Images.Id.
            // 4. Retourner une liste de RessourcesVM contenant la ressource, l'image associée et la table de jointure.

            var ressourcesAvecImages = await (
                from res in _Context.Ressources
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
                    StatuId = res.StatuId,
                    Image = img.Image,
                    RessourceImage = resImg
                }
            ).ToListAsync();

            return View(ressourcesAvecImages);
        }

        // GET: RessourcesController/Create
        public async Task<ActionResult> Creer()
        {
            RessourcesVM rvm = new() { Title = "", Image = [] };

            List<Status> statusList = await _Context.Status.ToListAsync();
            rvm.StatusList = new SelectList(statusList, "Id", "Label");
            return View(rvm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreerRessource(RessourcesVM ressourceVm, IFormFile pic)
        {
            if (pic is not null)
            {
                string NomImg = Path.GetFileName(pic.FileName);
                string ext = Path.GetExtension(pic.FileName);

                if (ext == ".jpg" || ext == ".png" || ext == ".jpeg")
                {
                    string FilePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\assets\RessourcesImages", NomImg);
                    using (var stream = new FileStream(FilePath, FileMode.Create))
                    {                       
                        await pic.CopyToAsync(stream);
                        stream.Close();
                        ressourceVm.Image = System.IO.File.ReadAllBytes(FilePath);
                    }
                }
                else
                {
                    ModelState.AddModelError("Image", "Le format de l'image n'est pas supporté. Veuillez utiliser .jpg, .png ou .jpeg.");
                    return View("Creer", ressourceVm);
                }
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
                        StatuId = ressourceVm.StatuId
                    };

                    await _Context.Ressources.AddAsync(Ressource);

                    Images Images = new()
                    {
                        Image = ressourceVm.Image,                       
                    };

                    await _Context.Images.AddAsync(Images);
                    await _Context.SaveChangesAsync();

                    Ressource = await _Context.Ressources
                        .OrderByDescending(r => r.CreatedAt)
                        .FirstOrDefaultAsync();
                    Images = await _Context.Images
                        .OrderByDescending(i => i.Id)
                        .FirstOrDefaultAsync();
                    RessourcesImages ressourceImage = new()
                    {
                        RessourceId = Ressource.Id,
                        ImageId = Images.Id
                    };
                    await _Context.RessourcesImages.AddAsync(ressourceImage);

                    await _Context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(RessourcesController.Creer));
        }


        // GET: RessourcesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RessourcesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RessourcesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RessourcesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
