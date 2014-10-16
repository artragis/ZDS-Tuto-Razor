using Blog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Slugify;
using System.Data.Entity.Infrastructure;

namespace Blog.Controllers
{
    public class ArticleController : Controller
    {
        /// <summary>
        /// Champ qui va permettre d'appeler des méthodes pour faire des actions sur notre fichier
        /// </summary>
        private readonly ArticleJSONRepository _repository;
        private ApplicationDbContext bdd = ApplicationDbContext.Create();//le lien vers la base de données
        private static string[] AcceptedTypes = new string[] { "image/jpeg", "image/png" };
        private static string[] AcceptedExt = new string[] { "jpeg", "jpg", "png", "gif" };

        public readonly static int ARTICLEPERPAGE = 5;

        /// <summary>
        /// Constructeur par défaut, permet d'initialiser le chemin du fichier JSON
        /// </summary>
        public ArticleController()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "liste_article_tuto_full.json");
            _repository = new ArticleJSONRepository(path);
        }

        // GET: Article
        public ActionResult Index()
        {
            return View();
        }

        #region List sans pagination

        //// GET: List
        //public ActionResult List()
        //{
        //    try
        //    {
        //        List<Article> liste = _repository.GetAllListArticle().ToList();
        //        return View(liste);
        //    }
        //    catch
        //    {
        //        return View(new List<Article>());
        //    }
        //}

        #endregion

        // GET: List
        public ActionResult List(int page = 0)
        {
            ViewBag.Page = page;
            try
            {
                //on saute un certain nombre d'article et on en prend la quantité voulue
                List<Article> liste = bdd.Articles.OrderBy(a => a.ID)
                                         .Skip(page * ARTICLEPERPAGE)
                                         .Take(ARTICLEPERPAGE)
                                         .ToList();
                
                return View(liste);
            }
            catch
            {
                return View(new List<Article>());
            }
        }

        //GET : Create
        public ActionResult Create()
        {
            return View();
        }
        private bool handleImage(ArticleCreation articleCreation, out string fileName)
        {
            bool hasError = false;
            fileName = "";
            if (articleCreation.Image != null)
            {
                
                if (articleCreation.Image.ContentLength > 1024 * 1024)
                {
                    ModelState.AddModelError("Image", "Le fichier téléchargé est trop grand.");
                    hasError = true;
                }

                if (!AcceptedTypes.Contains(articleCreation.Image.ContentType)
                       || AcceptedExt.Contains(Path.GetExtension(articleCreation.Image.FileName).ToLower()))
                {
                    ModelState.AddModelError("Image", "Le fichier doit être une image.");
                    hasError = true;
                }

                try
                {
                    string fileNameFile = Path.GetFileName(articleCreation.Image.FileName);
                    fileName = new SlugHelper().GenerateSlug(fileNameFile);
                    string imagePath = Path.Combine(Server.MapPath("~/Content/Upload"), fileName);
                    articleCreation.Image.SaveAs(imagePath);
                }
                catch
                {
                    fileName = "";
                    ModelState.AddModelError("Image", "Erreur à l'enregistrement.");
                    hasError = true;
                }
                
            }
            return !hasError;
        }
        //POST : Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticleCreation articleCreation)
        {
            if (!ModelState.IsValid)
            {
                return View(articleCreation);
            }

            string fileName = "";
            if (!handleImage(articleCreation, out fileName))
            {
                return View(articleCreation);
            }

            Article article = new Article
            {
                Contenu = articleCreation.Contenu,
                Pseudo = articleCreation.Pseudo,
                Titre = articleCreation.Titre,
                ImageName = fileName
            };

            bdd.Articles.Add(article);
            bdd.SaveChanges();
            return RedirectToAction("List", "Article");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View( bdd.Articles.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ArticleCreation articleCreation)
        {
            Article entity = bdd.Articles.Find(id);
            if (entity == null)
            {
                return RedirectToAction("List");
            }
            string fileName;
            handleImage(articleCreation, out fileName);
            DbEntityEntry<Article> entry = bdd.Entry(entity);
            entry.State = System.Data.Entity.EntityState.Modified;
            Article article = new Article
            {
                Contenu = articleCreation.Contenu,
                Pseudo = articleCreation.Pseudo,
                Titre = articleCreation.Titre,
                ImageName = fileName
            };
            entry.CurrentValues.SetValues(article);
            bdd.SaveChanges();

            return RedirectToAction("List");
        }
    }


}