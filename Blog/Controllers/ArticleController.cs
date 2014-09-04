using Blog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ArticleJSONRepository _repository;

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

        // GET: List
        public ActionResult List()
        {
            try
            {
                List<Article> liste = _repository.GetAllListArticle().ToList();
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

        //POST : Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Article article)
        {
            if (ModelState.IsValid)
            {
                _repository.AddArticle(article);
                return RedirectToAction("List", "Article");
            }
            return View(article);
        }
    }
}