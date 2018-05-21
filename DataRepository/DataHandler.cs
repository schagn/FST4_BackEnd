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

        public List<SharedBewertung> GetRatingAll()
        {
            return model.rating.Select(x => new SharedBewertung()
            {
                ArticleId = x.article_id,
                ArtikelName = x.article.description,
                PersonId = x.person_id,
                KundenName = x.person.firstname + " " + x.person.lastname,
                Sterne = x.stars,
                Kommentar = x.comment,
                Visible = x.visible
            }).ToList();
        }

        public List<SharedBewertung> GetRatingVisible()
        {
            return model.rating.Where(x => x.visible == true).Select(x => new SharedBewertung()
            {
                ArticleId = x.article_id,
                ArtikelName = x.article.description,
                PersonId = x.person_id,
                KundenName = x.person.firstname + " " + x.person.lastname,
                Sterne = x.stars,
                Kommentar = x.comment,
                Visible = x.visible
            }).ToList();
        }

        public List<SharedBewertung> GetRatingNonVisible()
        {
            return model.rating.Where(x => x.visible == false).Select(x => new SharedBewertung()
            {
                ArticleId = x.article_id,
                ArtikelName = x.article.description,
                PersonId = x.person_id,
                KundenName = x.person.firstname + " " + x.person.lastname,
                Sterne = x.stars,
                Kommentar = x.comment,
                Visible = x.visible
            }).ToList();
        }

        public void UpdateRatingVisibility(SharedBewertung tempRating)
        {
            var rating = model.rating.SingleOrDefault(x => x.article_id == tempRating.ArticleId && x.person_id == tempRating.PersonId);
            rating.visible = tempRating.Visible;
            model.SaveChanges();
        }

        public void DeleteRating(SharedBewertung selectedRating)
        {
            model.rating.Remove(model.rating.SingleOrDefault(x => x.article_id == selectedRating.ArticleId && x.person_id == selectedRating.PersonId));
            model.SaveChanges();
        }

        public List<SharedKategorie> GetIngredientCategories()
        {
            return model.category.Select(x => new SharedKategorie()
            {
                Description = x.description
            }).ToList(); ;
        }

        public List<SharedZutat> GetZutat()
        {
            return model.ingredient.Select(x => new SharedZutat()
            {
                ZutatenId = x.ingredient_id,
                Beschreibung = x.description,
                Preis = x.price,
                //Kategorie = x.category.description

            }).ToList();
        }

        public void CreateZutat(SharedZutat tempZutat)
        {
            var zutat = new SharedZutat()
            {
                ZutatenId = Guid.NewGuid(),
                Beschreibung = tempZutat.Beschreibung,
                Preis = tempZutat.Preis,
                //Kategorie = tempZutat.Kategorie,
                //IsAvailable = tempZutat.IsAvailable
            };

            //model.ingredient.Add(zutat);
            model.SaveChanges();
        }

        public void UpdateZutat(SharedZutat tempZutat)
        {
            var zutat = model.ingredient.SingleOrDefault(x => x.ingredient_id == tempZutat.ZutatenId);
            zutat.description = tempZutat.Beschreibung;
            //zutat.category = model.category.SingleOrDefault(x => x.description.Equals(tempZutat.Kategorie));
            zutat.price = tempZutat.Preis;
            //zutat.avaible
            model.SaveChanges();
        }

        public void DeleteZutat(SharedZutat selectedZutat)
        {
            model.ingredient.Remove(model.ingredient.SingleOrDefault(x => x.ingredient_id == selectedZutat.ZutatenId));
            model.SaveChanges();
        }

        public List<SharedRegelwerk> GetRegel()
        {
            return model.category.Select(x => new SharedRegelwerk()
            {
                RegelwerkId = x.category_id,
                Beschreibung = x.description
            }).ToList();
        }

        public void CreateRegel(SharedRegelwerk tempRegel)
        {
            var regel = new category()
            {
                category_id = tempRegel.RegelwerkId,
                description = tempRegel.Beschreibung
            };

            model.category.Add(regel);
            model.SaveChanges();
        }

        public void UpdateRegel(SharedRegelwerk tempRegel)
        {
            var regel = model.category.SingleOrDefault(x => x.category_id == tempRegel.RegelwerkId);

            regel.description = tempRegel.Beschreibung;
            //regel.available = tempRegel.available;
            model.SaveChanges();
        }

        public void DeleteRegel(SharedRegelwerk tempRegel)
        {

            model.category.Remove(model.category.SingleOrDefault(x => x.category_id == tempRegel.RegelwerkId));
            model.SaveChanges();
        }


        //Bestellungsverwaltung

        public List<SharedBestellung> GetAllOrders()
        {

            return model.order.Select(x => new SharedBestellung()
            {
                Artikel = x.order_has_articles.Select(y => y.article.description).ToList(),
                KundenName = x.person.firstname + x.person.lastname,
                BestellDatum = x.date,
                Bestellstatus = "Open",
                GesamtSumme = x.total_amount

            }).ToList();
            
        }

        // TODO 
        public List<SharedBestellung> GetOrdersByStatus()
        {

            throw new NotImplementedException();


        }


        #region Kundenverwaltung


        public List<SharedKunde> GetAllCustomers()
        {

            var customers = model.person.Where(x=> x.type.description.Equals("Customer")).Select(x => new SharedKunde()
            {
                KundenId = x.person_id,
                EMail = x.e_mail,
                Geburtsdatum = x.firstname,
                VorName = x.firstname,
                NachName = x.lastname,
                Land = x.country,
                PLZ = x.city.zip_code,
                Ort = x.city.name,
                Passwort = x.password,
                Strasse = x.street



            }).ToList();

            return customers;

        }

        public void AddCusomter(SharedKunde k)
        {

            CreateCityIfNotExisting(new SharedCity(k.PLZ,k.Ort));
            var customer = new person()
            {
                person_id = Guid.NewGuid(),
                firstname = k.VorName,
                lastname = k.NachName,
                e_mail = k.EMail,
                password = k.Passwort,
                birthdate = k.Geburtsdatum,
                street = k.Strasse,
                city = model.city.SingleOrDefault(x => x.zip_code.Equals(k.PLZ)),
                country = k.Land,
                type = model.type.SingleOrDefault(y => y.description.Equals("Customer"))
                

            };
            model.person.Add(customer);
            model.SaveChanges();

        }
        

        public void CreateCityIfNotExisting(SharedCity city)
        {

            if(!model.city.Any(x=> x.zip_code.Equals(city.ZipCode)))
            {
                var newCity = new city()
                {
                    name = city.Name,
                    zip_code = city.ZipCode

                };

                model.city.Add(newCity);
                model.SaveChanges();

            }

        }
        //PFUSCH
        public void CreateCustomerTypeIfNotExisting()
        {

            if (!model.type.Any(x => x.description.Equals("Customer")))
            {

                var type = new type()
                {
                    type_id = Guid.NewGuid(),
                    description = "Customer"
                };

                model.type.Add(type);
                model.SaveChanges();

            }
        }

        public void DeleteCustomer(SharedKunde k)
        {

            model.person.Remove(model.person.SingleOrDefault(x => x.person_id.Equals(k.KundenId)));
            model.SaveChanges();

        }




        #endregion
    }
}
