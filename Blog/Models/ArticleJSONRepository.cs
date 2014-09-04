using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.IO;

namespace Blog.Models
{
    /// <summary>
    /// permet de gérer les articles qui sont enregistrés dans un fichier JSON
    /// </summary>
    public class ArticleJSONRepository
    {
        private readonly string _savedFile;

        /// <summary>
        /// Construit le gestionnaire d'article à partir du nom d'un fichier JSON
        /// </summary>
        /// <param name="fileName">nom du fichier json</param>
        public ArticleJSONRepository(string fileName)
        {
            _savedFile = fileName;
        }

        /// <summary>
        /// Obtient un unique article à partir de sa place dans la liste enregistrée
        /// </summary>
        /// <param name="id">la place de l'article dans la liste (commence de 0)</param>
        /// <returns>L'article désiré</returns>
        public Article GetArticleById(int id)
        {
            using (var reader = new StreamReader(_savedFile))
            {
                List<Article> list = JsonConvert.DeserializeObject<List<Article>>(reader.ReadToEnd());

                if (list.Count < id || id < 0)
                {
                    throw new ArgumentOutOfRangeException("Id incorrect");
                }
                return list[id];
            }

        }

        /// <summary>
        /// Obtient une liste d'article
        /// </summary>
        /// <param name="start">Premier article sélectionné</param>
        /// <param name="count">Nombre d'article sélectionné</param>
        /// <returns></returns>
        public IEnumerable<Article> GetListArticle(int start = 0, int count = 10)
        {
            using (var reader = new StreamReader(_savedFile))
            {
                List<Article> list = JsonConvert.DeserializeObject<List<Article>>(reader.ReadToEnd());
                return list.Skip(start).Take(count);
            }
        }

        /// <summary>
        /// Obtient une liste de tout les articles
        /// </summary>
        public IEnumerable<Article> GetAllListArticle()
        {
            using (var reader = new StreamReader(_savedFile))
            {
                List<Article> list = JsonConvert.DeserializeObject<List<Article>>(reader.ReadToEnd());
                return list;
            }
        }

        /// <summary>
        /// Ajoute un article à la liste
        /// </summary>
        /// <param name="newArticle">Le nouvel article</param>
        public void AddArticle(Article newArticle)
        {
            List<Article> list;
            using (var reader = new StreamReader(_savedFile))
            {
                list = JsonConvert.DeserializeObject<List<Article>>(reader.ReadToEnd());
            }
            using (var writter = new StreamWriter(_savedFile, false))
            {
                list.Add(newArticle);
                writter.Write(JsonConvert.SerializeObject(list));
            }
        }
    }
}