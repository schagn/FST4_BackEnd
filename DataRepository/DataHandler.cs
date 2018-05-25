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

        #region Kuchenverwaltung+Zutaten

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
            var list = model.article.Select(x => new SharedArticle()
            {
                ArticleId = x.article_id,
                ArticleTypeDescription = x.article_type.description,
                Creation = x.creation.Value,
                Description = x.description,
                Price = x.price.Value,
                ShapeDescription = x.shape.description,
                Visible = x.visible.Value,
                Ingredients = x.article_has_ingredient.Select(y => y.ingredient.description).ToList()
            }).ToList();
            foreach(var item in list)
            {
                item.IngredientString = string.Join(", ", item.Ingredients);
            }
            return list;
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

        public void DeleteArticle(Guid articleId)
        {
            model.article.Remove(model.article.SingleOrDefault(x => x.article_id == articleId));
            model.SaveChanges();
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

        #endregion

        #region Bewertung

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

        #endregion

        #region Zutatenverwaltung

        public List<string> GetIngredientCategories()
        {
            return model.category.Select(x => x.description).ToList();
        }

        public List<SharedZutat> GetZutat()
        {
            return model.ingredient.Select(x => new SharedZutat()
            {
                ZutatenId = x.ingredient_id,
                Beschreibung = x.description,
                Preis = x.price,
                IsAvailable = x.ing_available,
                //Kategorie = model.category.description
            }).ToList();
        }

        public void CreateZutat(SharedZutat tempZutat)
        {
            var zutat = new ingredient()
            {
                ingredient_id = Guid.NewGuid(),
                description = tempZutat.Beschreibung,
                price = tempZutat.Preis,
                //category.d = tempZutat.Kategorie,
                ing_available = tempZutat.IsAvailable
            };

            model.ingredient.Add(zutat);
            model.SaveChanges();
        }

        public void UpdateZutat(SharedZutat tempZutat)
        {
            var zutat = model.ingredient.SingleOrDefault(x => x.ingredient_id == tempZutat.ZutatenId);
            zutat.description = tempZutat.Beschreibung;
            //zutat.category = model.category.SingleOrDefault(x => x.description.Equals(tempZutat.Kategorie));
            zutat.price = tempZutat.Preis;
            zutat.ing_available = tempZutat.IsAvailable;

            model.SaveChanges();
        }

        public void DeleteZutat(SharedZutat selectedZutat)
        {
            model.ingredient.Remove(model.ingredient.SingleOrDefault(x => x.ingredient_id == selectedZutat.ZutatenId));
            model.SaveChanges();
        }

        #endregion

        #region Regelwerk

        public List<SharedRegelwerk> GetRegel()
        {
            return model.category.Select(x => new SharedRegelwerk()
            {
                RegelwerkId = x.category_id,
                Beschreibung = x.description,
                IsAvailable = x.cat_active
            }).ToList();
        }

        public List<SharedRegelwerk> GetRegelAvailable()
        {
            return model.category.Where(x => x.cat_active == true).Select(x => new SharedRegelwerk()
            {
                RegelwerkId = x.category_id,
                Beschreibung = x.description,
                IsAvailable = x.cat_active
            }).ToList();
        }

        public List<SharedRegelwerk> GetRegelNonAvailable()
        {
            return model.category.Where(x => x.cat_active == false).Select(x => new SharedRegelwerk()
            {
                RegelwerkId = x.category_id,
                Beschreibung = x.description,
                IsAvailable = x.cat_active
            }).ToList();
        }

        public void CreateRegel(SharedRegelwerk tempRegel)
        {
            var regel = new category()
            {
                category_id = tempRegel.RegelwerkId,
                description = tempRegel.Beschreibung,
                cat_active = tempRegel.IsAvailable
            };

            model.category.Add(regel);
            model.SaveChanges();
        }

        public void UpdateRegel(SharedRegelwerk tempRegel)
        {
            var regel = model.category.SingleOrDefault(x => x.category_id == tempRegel.RegelwerkId);

            regel.description = tempRegel.Beschreibung;
            regel.cat_active = tempRegel.IsAvailable;
            model.SaveChanges();
        }

        public void DeleteRegel(SharedRegelwerk tempRegel)
        {

            model.category.Remove(model.category.SingleOrDefault(x => x.category_id == tempRegel.RegelwerkId));
            model.SaveChanges();
        }

        #endregion

        #region  Bestellungsverwaltung

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

        #endregion

        #region Kundenverwaltung


        public List<SharedKunde> GetAllCustomers()
        {

            var customers = model.person.Where(x=> x.type.description.Equals("Kunde") || x.type.description.Equals("Firmenkunde")).Select(x => new SharedKunde()
            {
                KundenId = x.person_id,
                EMail = x.e_mail,
                Geburtsdatum = x.birthdate,
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
                type = model.type.SingleOrDefault(y => y.description.Equals("Kunde"))
                

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
      

        public void DeleteCustomer(SharedKunde k)
        {
            model.person.Remove(model.person.SingleOrDefault(x => x.person_id.Equals(k.KundenId)));
            model.SaveChanges();
        }

        public void UpdateCustomer(SharedKunde k)
        {
            var customer = model.person.SingleOrDefault(x => x.person_id.Equals(k.KundenId));

            customer.firstname = k.VorName;
            customer.lastname = k.NachName;
            customer.e_mail = k.EMail;
            customer.password = k.Passwort;
            customer.street = k.Strasse;
            customer.birthdate = k.Geburtsdatum;

            if (!(k.PLZ.Equals(customer.city.zip_code) && k.Ort.Equals(customer.city.name)))
            {
                if (!k.Land.Equals(customer.country))
                {
                    customer.country = k.Land;
                }

                CreateCityIfNotExisting(new SharedCity(k.PLZ, k.Ort));
                customer.city = model.city.SingleOrDefault(x => x.zip_code.Equals(k.PLZ));

            }
            model.SaveChanges();
        }

        public List<SharedKunde> FilterCustomersByType(String typeName)
        {
            return model.person.Where(x => x.type.description.Equals(typeName)).Select(x => new SharedKunde()
            {
                KundenId = x.person_id,
                EMail = x.e_mail,
                Geburtsdatum = x.birthdate,
                VorName = x.firstname,
                NachName = x.lastname,
                Land = x.country,
                PLZ = x.city.zip_code,
                Ort = x.city.name,
                Passwort = x.password,
                Strasse = x.street

            }).ToList();



        }

        #endregion
    }
}
