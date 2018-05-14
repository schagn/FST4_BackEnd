using SharedClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository
{
    public class DataHandler
    {
        FST4Entities model = new FST4Entities();
        public List<string> GetCakeTypes()
        {
            return model.article_type.Select(x => x.description).ToList();
        }

        public List<string> GetShapes()
        {
            return model.shape.Select(x => x.description).ToList();
        }

        public List<SharedArticle> GetArticles()
        {
            return model.article.Select(x => new SharedArticle()
            {
                ArticleId = x.article_id,
                ArticleTypeDescription = x.article_type.description,
                Creation = x.creation.Value,
                Description = x.description,
                Price = x.price.Value,
                ShapeDescription = x.shape.description,
                Visible = x.visible.Value
            }).ToList();
        }

        public void CreateArticle(SharedArticle sharedArticle)
        {
            var article = new article()
            {
                article_id = Guid.NewGuid(),
                article_type = model.article_type.SingleOrDefault(x => x.description.Equals(sharedArticle.ArticleTypeDescription)),
                creation = sharedArticle.Creation,
                description = sharedArticle.Description,
                price = sharedArticle.Price,
                shape = model.shape.SingleOrDefault(x => x.description.Equals(sharedArticle.ShapeDescription)),
                visible = sharedArticle.Visible
            };
            model.article.Add(article);
            model.SaveChanges();
        }

        public List<SharedIngredient> GetIngredients()
        {
                return model.ingredient.Select(x => new SharedIngredient()
                {
                    Description = x.description,
                    IngredientId = x.ingredient_id,
                    Price = x.price.Value
                }).ToList();
        }

        public List<SharedArticleIngredient> GetArticleIngredients(string selectedArticle)
        {
            return model.article_has_ingredient.Where(x => x.article.description.Equals(selectedArticle)).Select(x => new SharedArticleIngredient()
            {
                Ingredient = new SharedIngredient()
                {
                    Description = x.ingredient.description,
                    IngredientId = x.ingredient.ingredient_id,
                    Price = x.ingredient.price.Value
                },
                Amount = x.amount.Value
            }).ToList();
        }

        public void DeleteArticleIngredient(string selectedArticle, string selectedIngredient)
        {
            model.article_has_ingredient.Remove(model.article_has_ingredient.SingleOrDefault(x => x.article.description.Equals(selectedArticle) && x.ingredient.description.Equals(selectedIngredient)));
            model.SaveChanges();
        }

        public void CreateArticleIngredient(string selectedArticle, string selectedIngredient, double amount)
        {
            model.article_has_ingredient.Add(new article_has_ingredient()
            {
                amount = amount,
                article = model.article.SingleOrDefault(x => x.description.Equals(selectedArticle)),
                ingredient = model.ingredient.SingleOrDefault(x => x.description.Equals(selectedIngredient))
            });
            model.SaveChanges();
        }

        public void UpdateArticleIngredient(string selectedArticle, string selectedIngredient, double amount)
        {
            var item = model.article_has_ingredient.SingleOrDefault(x => x.article.description.Equals(selectedArticle) && x.ingredient.description.Equals(selectedIngredient));
            item.amount = amount;
            model.SaveChanges();
        }

        public void UpdateArticle(SharedArticle tempArticle)
        {
            var article = model.article.SingleOrDefault(x => x.article_id == tempArticle.ArticleId);
            article.article_type = model.article_type.SingleOrDefault(x => x.description.Equals(tempArticle.ArticleTypeDescription));
            article.creation = tempArticle.Creation;
            article.description = tempArticle.Description;
            article.price = tempArticle.Price;
            article.shape = model.shape.SingleOrDefault(x => x.description.Equals(tempArticle.ShapeDescription));
            article.visible = tempArticle.Visible;
            model.SaveChanges();
        }
    }
}
