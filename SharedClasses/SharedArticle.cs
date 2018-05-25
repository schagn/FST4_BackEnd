using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class SharedArticle
    {
        public Guid ArticleId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool Creation { get; set; }
        public bool Visible { get; set; }
        //public Guid ArticleTypeId { get; set; }
        public string ArticleTypeDescription { get; set; }
        //public Guid ShapeId { get; set; }
        public string ShapeDescription { get; set; }
        public List<string> Ingredients { get; set; }
        public string IngredientString { get; set; }
    }
}
