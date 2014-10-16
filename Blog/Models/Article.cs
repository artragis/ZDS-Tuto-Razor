using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Blog.Models
{
    /// <summary>
    /// Notre modèle de base pour représenter un article
    /// </summary>
    public class Article
    {
        public int ID { get; set; }
        /// <summary>
        /// Le pseudo de l'auteur
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(128)]
        [RegularExpression(@"^[^,\.\^]+$")]
        [DataType(DataType.Text)]
        public string Pseudo { get; set; }

        /// <summary>
        /// Le titre de l'article
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(128)]
        [DataType(DataType.Text)]
        public string Titre { get; set; }

        /// <summary>
        /// Le contenu de l'article
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.MultilineText)]
        public string Contenu { get; set; }

        /// <summary>
        /// Le chemin de l'image
        /// </summary>
        public string ImageName { get; set; }
    }
}