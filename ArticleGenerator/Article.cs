using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleGenerator
{
    /// <summary>
    /// Notre modèle de base pour représenter un article
    /// </summary>
    public class Article
    {
        /// <summary>
        /// Le pseudo de l'auteur
        /// </summary>
        public string Pseudo { get; set; }
        /// <summary>
        /// Le titre de l'article
        /// </summary>
        public string Titre { get; set; }
        /// <summary>
        /// Le contenu de l'article
        /// </summary>
        public string Contenu { get; set; }
    }
}
