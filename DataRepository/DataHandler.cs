using DataRepository.CreateWebServiceReference;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
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

        public void CreateArticle(SharedArticle sharedArticle, string filePath)
        {
            Guid tempGuid = Guid.NewGuid();
            var tempArticleType = model.article_type.SingleOrDefault(x => x.description.Equals(sharedArticle.ArticleTypeDescription));
            var tempShape = model.shape.SingleOrDefault(x => x.description.Equals(sharedArticle.ShapeDescription));
            var article = new article()
            {
                article_id = tempGuid,
                article_type = tempArticleType,
                creation = sharedArticle.Creation,
                description = sharedArticle.Description,
                price = sharedArticle.Price,
                shape = tempShape,
                visible = sharedArticle.Visible
            };
            model.article.Add(article);
            model.SaveChanges();

            if (!String.IsNullOrEmpty(filePath) && File.Exists(filePath) && (Path.GetExtension(filePath) == ".jpg" || Path.GetExtension(filePath) == ".png"))
            {
                if (!Directory.Exists(ConfigurationManager.AppSettings["imageFolder"]))
                {
                    Directory.CreateDirectory(ConfigurationManager.AppSettings["imageFolder"]);
                }
                string tempDestFile = Path.Combine(ConfigurationManager.AppSettings["imageFolder"], tempGuid + Path.GetExtension(filePath));
                File.Copy(filePath, tempDestFile);
            }

            CreateWebServiceSoapClient client = new CreateWebServiceSoapClient();
            StatementModel statementModel = new StatementModel();
            statementModel.Type = "Insert";
            statementModel.TableName = "article";
            statementModel.PrimaryKeyNames = new ArrayOfString() { "article_id" };
            statementModel.PrimaryKeyValues = new ArrayOfString() { tempGuid.ToString() };
            statementModel.Columns = new ArrayOfString();
            statementModel.Values = new ArrayOfString();
            statementModel.Columns.Add("article_type_id");
            statementModel.Values.Add(tempArticleType.article_type_id.ToString());
            statementModel.Columns.Add("creation");
            statementModel.Values.Add(sharedArticle.Creation.ToString());
            statementModel.Columns.Add("description");
            statementModel.Values.Add(sharedArticle.Description);
            statementModel.Columns.Add("price");
            statementModel.Values.Add(sharedArticle.Price.ToString());
            statementModel.Columns.Add("shape_id");
            statementModel.Values.Add(tempShape.shape_id.ToString());
            statementModel.Columns.Add("visible");
            statementModel.Values.Add(sharedArticle.Visible.ToString());
            statementModel.Sender = "Backend";
            string response = client.InsertStatement(statementModel);
        }

        public void DeleteArticle(Guid articleId)
        {
            foreach(var item in model.package.Where(x => x.article.Count(y => y.article_id == articleId) > 0))
            {
                item.article.Remove(item.article.SingleOrDefault(x => x.article_id == articleId));
            }
            model.order_has_articles.RemoveRange(model.order_has_articles.Where(x => x.article_id == articleId));
            model.rating.RemoveRange(model.rating.Where(x => x.article_id == articleId));
            model.article_has_ingredient.RemoveRange(model.article_has_ingredient.Where(x => x.article_id == articleId));
            model.article_has_mass.RemoveRange(model.article_has_mass.Where(x => x.fk_article_id == articleId));
            model.article.Remove(model.article.SingleOrDefault(x => x.article_id == articleId));
            model.SaveChanges();

            CreateWebServiceSoapClient client = new CreateWebServiceSoapClient();
            StatementModel statementModel = new StatementModel();
            statementModel.Type = "Delete";
            statementModel.TableName = "article";
            statementModel.PrimaryKeyNames = new ArrayOfString() { "article_id" };
            statementModel.PrimaryKeyValues = new ArrayOfString() { articleId.ToString() };
            statementModel.Sender = "Backend";
            string response = client.InsertStatement(statementModel);
        }

        public void UpdateArticle(SharedArticle tempArticle, string filePath)
        {
            var article = model.article.SingleOrDefault(x => x.article_id == tempArticle.ArticleId);
            var tempArticleType = model.article_type.SingleOrDefault(x => x.description.Equals(tempArticle.ArticleTypeDescription));
            var tempShape = model.shape.SingleOrDefault(x => x.description.Equals(tempArticle.ShapeDescription));
            article.article_type = tempArticleType;
            article.creation = tempArticle.Creation;
            article.description = tempArticle.Description;
            article.price = tempArticle.Price;
            article.shape = tempShape;
            article.visible = tempArticle.Visible;
            model.SaveChanges();

            if (!String.IsNullOrEmpty(filePath) && File.Exists(filePath) && (Path.GetExtension(filePath) == ".jpg" || Path.GetExtension(filePath) == ".png"))
            {
                if (!Directory.Exists(ConfigurationManager.AppSettings["imageFolder"]))
                {
                    Directory.CreateDirectory(ConfigurationManager.AppSettings["imageFolder"]);
                }
                string tempDestFile = Path.Combine(ConfigurationManager.AppSettings["imageFolder"], article.article_id + Path.GetExtension(filePath));
                File.Copy(filePath, tempDestFile, true);
            }

            CreateWebServiceSoapClient client = new CreateWebServiceSoapClient();
            StatementModel statementModel = new StatementModel();
            statementModel.Type = "Update";
            statementModel.TableName = "article";
            statementModel.PrimaryKeyNames = new ArrayOfString() { "article_id" };
            statementModel.PrimaryKeyValues = new ArrayOfString() { tempArticle.ArticleId.ToString() };
            statementModel.Columns = new ArrayOfString();
            statementModel.Values = new ArrayOfString();
            statementModel.Columns.Add("article_type_id");
            statementModel.Values.Add(tempArticleType.article_type_id.ToString());
            statementModel.Columns.Add("creation");
            statementModel.Values.Add(tempArticle.Creation.ToString());
            statementModel.Columns.Add("description");
            statementModel.Values.Add(tempArticle.Description);
            statementModel.Columns.Add("price");
            statementModel.Values.Add(tempArticle.Price.ToString());
            statementModel.Columns.Add("shape_id");
            statementModel.Values.Add(tempShape.shape_id.ToString());
            statementModel.Columns.Add("visible");
            statementModel.Values.Add(tempArticle.Visible.ToString());
            statementModel.Sender = "Backend";
            string response = client.InsertStatement(statementModel);
        }

        public void CreateArticleIngredient(string selectedArticle, string selectedIngredient, double amount)
        {
            var tempArticle = model.article.SingleOrDefault(x => x.description.Equals(selectedArticle));
            var tempIngredient = model.ingredient.SingleOrDefault(x => x.description.Equals(selectedIngredient));
            model.article_has_ingredient.Add(new article_has_ingredient()
            {
                amount = amount,
                article = tempArticle,
                ingredient = tempIngredient
            });
            model.SaveChanges();

            CreateWebServiceSoapClient client = new CreateWebServiceSoapClient();
            StatementModel statementModel = new StatementModel();
            statementModel.Type = "Insert";
            statementModel.TableName = "article_has_ingredient";
            statementModel.PrimaryKeyNames = new ArrayOfString() { "article_id", "ingredient_id" };
            statementModel.PrimaryKeyValues = new ArrayOfString() { tempArticle.article_id.ToString(), tempIngredient.ingredient_id.ToString() };
            statementModel.Columns = new ArrayOfString();
            statementModel.Values = new ArrayOfString();
            statementModel.Columns.Add("amount");
            statementModel.Values.Add(amount.ToString());
            statementModel.Sender = "Backend";
            string response = client.InsertStatement(statementModel);
        }

        public void DeleteArticleIngredient(string selectedArticle, string selectedIngredient)
        {
            var tempArticle = model.article.SingleOrDefault(x => x.description.Equals(selectedArticle));
            var tempIngredient = model.ingredient.SingleOrDefault(x => x.description.Equals(selectedIngredient));
            model.article_has_ingredient.Remove(model.article_has_ingredient.SingleOrDefault(x => x.article_id == tempArticle.article_id && x.ingredient_id == tempIngredient.ingredient_id));
            model.SaveChanges();

            CreateWebServiceSoapClient client = new CreateWebServiceSoapClient();
            StatementModel statementModel = new StatementModel();
            statementModel.Type = "Delete";
            statementModel.TableName = "article_has_ingredient";
            statementModel.PrimaryKeyNames = new ArrayOfString() { "article_id", "ingredient_id" };
            statementModel.PrimaryKeyValues = new ArrayOfString() { tempArticle.article_id.ToString(), tempIngredient.ingredient_id.ToString() };
            statementModel.Sender = "Backend";
            string response = client.InsertStatement(statementModel);
        }

        public void UpdateArticleIngredient(string selectedArticle, string selectedIngredient, double amount)
        {
            var tempArticle = model.article.SingleOrDefault(x => x.description.Equals(selectedArticle));
            var tempIngredient = model.ingredient.SingleOrDefault(x => x.description.Equals(selectedIngredient));
            var item = model.article_has_ingredient.SingleOrDefault(x => x.article_id == tempArticle.article_id && x.ingredient_id == tempIngredient.ingredient_id);
            item.amount = amount;
            model.SaveChanges();

            CreateWebServiceSoapClient client = new CreateWebServiceSoapClient();
            StatementModel statementModel = new StatementModel();
            statementModel.Type = "Update";
            statementModel.TableName = "article_has_ingredient";
            statementModel.PrimaryKeyNames = new ArrayOfString() { "article_id", "ingredient_id" };
            statementModel.PrimaryKeyValues = new ArrayOfString() { tempArticle.article_id.ToString(), tempIngredient.ingredient_id.ToString() };
            statementModel.Columns = new ArrayOfString();
            statementModel.Values = new ArrayOfString();
            statementModel.Columns.Add("amount");
            statementModel.Values.Add(amount.ToString());
            statementModel.Sender = "Backend";
            string response = client.InsertStatement(statementModel);
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

            CreateWebServiceSoapClient client = new CreateWebServiceSoapClient();
            StatementModel statementModel = new StatementModel();
            statementModel.Type = "Update";
            statementModel.TableName = "rating";
            statementModel.PrimaryKeyNames = new ArrayOfString() { "article_id", "person_id" };
            statementModel.PrimaryKeyValues = new ArrayOfString() { tempRating.ArticleId.ToString(), tempRating.PersonId.ToString() };
            statementModel.Columns = new ArrayOfString();
            statementModel.Values = new ArrayOfString();
            statementModel.Columns.Add("visible");
            statementModel.Values.Add(tempRating.Visible.ToString());
            statementModel.Sender = "Backend";
            string response = client.InsertStatement(statementModel);
        }

        public void DeleteRating(SharedBewertung selectedRating)
        {
            model.rating.Remove(model.rating.SingleOrDefault(x => x.article_id == selectedRating.ArticleId && x.person_id == selectedRating.PersonId));
            model.SaveChanges();

            CreateWebServiceSoapClient client = new CreateWebServiceSoapClient();
            StatementModel statementModel = new StatementModel();
            statementModel.Type = "Delete";
            statementModel.TableName = "rating";
            statementModel.PrimaryKeyNames = new ArrayOfString() { "article_id", "person_id" };
            statementModel.PrimaryKeyValues = new ArrayOfString() { selectedRating.ArticleId.ToString(), selectedRating.PersonId.ToString() };
            statementModel.Columns = new ArrayOfString();
            statementModel.Values = new ArrayOfString();
            statementModel.Sender = "Backend";
            string response = client.InsertStatement(statementModel);
        }

        #endregion

        #region Zutatenverwaltung

        public List<string> GetCategories()
        {
            return model.category.Select(x => x.description).ToList();
        }

        public List<SharedZutat> GetZutat(string filter)
        {
            if (filter.Equals("Available"))
            {
                var list = model.ingredient.Where(x => x.ing_available == true).Select(x => new SharedZutat()
                {
                    ZutatenId = x.ingredient_id,
                    Beschreibung = x.description,
                    Preis = x.price,
                    IsAvailable = x.ing_available,
                    Kategorie = x.category.Select(y => y.description).ToList()
                }).ToList();
                foreach (var item in list)
                {
                    item.KategorieString = string.Join(", ", item.Kategorie);
                }
                return list;
            }
            else if(filter.Equals("Not available"))
            {
                var list = model.ingredient.Where(x => x.ing_available == false).Select(x => new SharedZutat()
                {
                    ZutatenId = x.ingredient_id,
                    Beschreibung = x.description,
                    Preis = x.price,
                    IsAvailable = x.ing_available,
                    Kategorie = x.category.Select(y => y.description).ToList()
                }).ToList();
                foreach (var item in list)
                {
                    item.KategorieString = string.Join(", ", item.Kategorie);
                }
                return list;
            }
            else
            {
                var list = model.ingredient.Select(x => new SharedZutat()
                {
                    ZutatenId = x.ingredient_id,
                    Beschreibung = x.description,
                    Preis = x.price,
                    IsAvailable = x.ing_available,
                    Kategorie = x.category.Select(y => y.description).ToList()
                }).ToList();
                foreach (var item in list)
                {
                    item.KategorieString = string.Join(", ", item.Kategorie);
                }
                return list;
            }
        }

        public void CreateZutat(SharedZutat tempZutat)
        {
            var zutat = new ingredient()
            {
                ingredient_id = Guid.NewGuid(),
                description = tempZutat.Beschreibung,
                price = tempZutat.Preis,
                ing_available = tempZutat.IsAvailable
            };          

            model.ingredient.Add(zutat);

            foreach (var item in tempZutat.Kategorie)
            {
                var kategorie = model.category.SingleOrDefault(x => x.description.Equals(item));
                zutat.category.Add(kategorie);
            }

            model.SaveChanges();
        }

        public void UpdateZutat(SharedZutat tempZutat)
        {
            var zutat = model.ingredient.SingleOrDefault(x => x.ingredient_id == tempZutat.ZutatenId);
            zutat.description = tempZutat.Beschreibung;
            zutat.price = tempZutat.Preis;
            zutat.ing_available = tempZutat.IsAvailable;

            foreach (var item in tempZutat.Kategorie)
            {
                var kategorie = model.category.SingleOrDefault(x => x.description.Equals(item));
                zutat.category.Add(kategorie);
            }

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

            CreateWebServiceSoapClient client = new CreateWebServiceSoapClient();
            StatementModel statementModel = new StatementModel();
            statementModel.Type = "Create";
            statementModel.TableName = "category";
            statementModel.PrimaryKeyNames = new ArrayOfString() { "category_id" };
            statementModel.PrimaryKeyValues = new ArrayOfString() { tempRegel.RegelwerkId.ToString() };
            statementModel.Columns = new ArrayOfString();
            statementModel.Values = new ArrayOfString();
            statementModel.Columns.Add("description");
            statementModel.Values.Add(tempRegel.Beschreibung);
            statementModel.Columns.Add("cat_active");
            statementModel.Values.Add(tempRegel.IsAvailable.ToString());
            statementModel.Sender = "Backend";
            string response = client.InsertStatement(statementModel);
        }

        public void UpdateRegel(SharedRegelwerk tempRegel)
        {
            var regel = model.category.SingleOrDefault(x => x.category_id == tempRegel.RegelwerkId);

            regel.description = tempRegel.Beschreibung;
            regel.cat_active = tempRegel.IsAvailable;
            model.SaveChanges();

            CreateWebServiceSoapClient client = new CreateWebServiceSoapClient();
            StatementModel statementModel = new StatementModel();
            statementModel.Type = "Update";
            statementModel.TableName = "category";
            statementModel.PrimaryKeyNames = new ArrayOfString() { "category_id" };
            statementModel.PrimaryKeyValues = new ArrayOfString() { tempRegel.RegelwerkId.ToString() };
            statementModel.Columns = new ArrayOfString();
            statementModel.Values = new ArrayOfString();
            statementModel.Columns.Add("description");
            statementModel.Values.Add(tempRegel.Beschreibung);
            statementModel.Columns.Add("cat_active");
            statementModel.Values.Add(tempRegel.IsAvailable.ToString());
            statementModel.Sender = "Backend";
            string response = client.InsertStatement(statementModel);
        }

        public void DeleteRegel(SharedRegelwerk tempRegel)
        {

            model.category.Remove(model.category.SingleOrDefault(x => x.category_id == tempRegel.RegelwerkId));
            model.SaveChanges();

            CreateWebServiceSoapClient client = new CreateWebServiceSoapClient();
            StatementModel statementModel = new StatementModel();
            statementModel.Type = "Delete";
            statementModel.TableName = "category";
            statementModel.PrimaryKeyNames = new ArrayOfString() { "category_id" };
            statementModel.PrimaryKeyValues = new ArrayOfString() { tempRegel.RegelwerkId.ToString() };
            statementModel.Columns = new ArrayOfString();
            statementModel.Values = new ArrayOfString();
            statementModel.Sender = "Backend";
            string response = client.InsertStatement(statementModel);
        }

        #endregion

        #region  Bestellungsverwaltung

        public List<SharedBestellung> GetAllOrders()
        {
            
            double? voucherValue;

            

            var orders =  model.order.Select(x => new SharedBestellung()
            {
                BestellId = x.order_id,
                Artikel = x.order_has_articles.Select(y => new SharedOrderArticle()
                {
                    id = y.article_id,
                    name = y.article.description

                }).ToList(),
                KundenName = x.person.firstname + " " + x.person.lastname,
                BestellDatum = x.order_date,
                LieferDatum = x.delivery_date,
                Bestellstatus = x.status,
                GesamtSumme = x.total_amount

            }).ToList();

            foreach (var item in orders)
            {

                voucherValue = GetOrderVaucherValue(item.BestellId);

                if(voucherValue != 0)
                {
                    item.GutscheinUsed = true;
                }
                else
                {
                    item.GutscheinUsed = false;
                }

                item.GutscheinWert = voucherValue;
            }

            return orders;
            
        }

        private double? GetOrderVaucherValue(Guid orderId)
        {
            var voucher = model.order.Where(x => x.order_id.Equals(orderId)).Select(id=> id.voucher_id).SingleOrDefault();
            double? voucherValue = 0;
            double? orderValue;
            double difference;
            

            if(voucher != null)
            {
                orderValue = model.order.Where(z=> z.order_id.Equals(orderId)).Select(p=> p.total_amount).SingleOrDefault();
                voucherValue = model.order.Where(y => y.order_id.Equals(orderId)).Select(w => w.voucher.amount).SingleOrDefault();

                difference = (double)(voucherValue - orderValue);

                if(difference < 0)
                {
                    return voucherValue;
                }
                else
                {
                    return orderValue;
                }
            }
            return (double?)0;
        }

        public void UpdateOrder(SharedBestellung ord)
        {
            var order = model.order.Where(x => x.order_id.Equals(ord.BestellId)).SingleOrDefault();

            order.status = ord.Bestellstatus;
            order.delivery_date = ord.LieferDatum;

            model.SaveChanges();

            CreateWebServiceSoapClient client = new CreateWebServiceSoapClient();
            StatementModel statementModel = new StatementModel();
            statementModel.Type = "Update";
            statementModel.TableName = "order";
            statementModel.PrimaryKeyNames = new ArrayOfString() { "order_id" };
            statementModel.PrimaryKeyValues = new ArrayOfString() { ord.BestellId.ToString() };
            statementModel.Columns = new ArrayOfString();
            statementModel.Values = new ArrayOfString();
            statementModel.Columns.Add("delivery_date");
            statementModel.Values.Add(ord.LieferDatum.ToString());
            statementModel.Columns.Add("status");
            statementModel.Values.Add(ord.Bestellstatus);
            statementModel.Sender = "Backend";
            string response = client.InsertStatement(statementModel);
        }

         
        public List<SharedBestellung> GetOrdersByStatus(string selectedFilterMethode)
        {

            double? voucherValue;



            var orders = model.order.Where(order=> order.status.Equals(selectedFilterMethode)).Select(x => new SharedBestellung()
            {
                BestellId = x.order_id,
                Artikel = x.order_has_articles.Select(y => new SharedOrderArticle()
                {
                    id = y.article_id,
                    name = y.article.description

                }).ToList(),
                KundenName = x.person.firstname + " " + x.person.lastname,
                BestellDatum = x.order_date,
                Bestellstatus = x.status,
                GesamtSumme = x.total_amount

            }).ToList();

            foreach (var item in orders)
            {

                voucherValue = GetOrderVaucherValue(item.BestellId);

                if (voucherValue != 0)
                {
                    item.GutscheinUsed = true;
                }
                else
                {
                    item.GutscheinUsed = false;
                }

                item.GutscheinWert = voucherValue;
            }

            return orders;


        }

        public SharedEmailCustomer GetEmailCustomerForOrder(SharedBestellung ord)
        {

            var mailCusomter = model.order.Where(x => x.order_id.Equals(ord.BestellId)).Select(y => new SharedEmailCustomer()
            {
                email = y.person.e_mail,
                firstname = y.person.firstname,
                lastname = y.person.lastname
                


            }).SingleOrDefault();

            return mailCusomter;

        }

        public void DeleteProductFromOrder(Guid orderId, Guid productId)
        {

            var amount = model.order_has_articles.SingleOrDefault(x => x.order_id.Equals(orderId) && x.article_id.Equals(productId)).amount;
            model.order_has_articles.Remove(model.order_has_articles.SingleOrDefault(x=> x.order_id.Equals(orderId) && x.article_id.Equals(productId)));

            
            var order = model.order.Where(x => x.order_id.Equals(orderId)).SingleOrDefault();
            var product = model.article.Where(y => y.article_id.Equals(productId)).SingleOrDefault();

            order.total_amount = order.total_amount - (product.price * amount);


            model.SaveChanges();

            CreateWebServiceSoapClient client = new CreateWebServiceSoapClient();
            StatementModel statementModel = new StatementModel();
            statementModel.Type = "Delete";
            statementModel.TableName = "order_has_articles";
            statementModel.PrimaryKeyNames = new ArrayOfString() { "order_id", "article_id" };
            statementModel.PrimaryKeyValues = new ArrayOfString() { orderId.ToString(), productId.ToString() };
            statementModel.Columns = new ArrayOfString();
            statementModel.Values = new ArrayOfString();
            statementModel.Sender = "Backend";
            string response = client.InsertStatement(statementModel);

            StatementModel statementModel1 = new StatementModel();
            statementModel1.Type = "Update";
            statementModel1.TableName = "order";
            statementModel1.PrimaryKeyNames = new ArrayOfString() { "order_id",};
            statementModel1.PrimaryKeyValues = new ArrayOfString() { orderId.ToString() };
            statementModel1.Columns = new ArrayOfString();
            statementModel1.Values = new ArrayOfString();
            statementModel1.Columns.Add("total_amount");
            statementModel1.Values.Add(order.total_amount.ToString());
            statementModel1.Sender = "Backend";
            string response1 = client.InsertStatement(statementModel1);
        }
            
        
            
        #endregion

        #region Kundenverwaltung

        public List<SharedKunde> GetAllCustomers()
        {

            var customers = model.person.Where(x=> x.type.description.Equals("Kunde") || x.type.description.Equals("Firmenkunde")).Select(x => new SharedKunde()
            {
                KundenId = x.person_id,
                EMail = x.e_mail,
                Geburtsdatum = x.birthdate.Value,
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

            CreateWebServiceSoapClient client = new CreateWebServiceSoapClient();
            StatementModel statementModel = new StatementModel();
            statementModel.Type = "Create";
            statementModel.TableName = "person";
            statementModel.PrimaryKeyNames = new ArrayOfString() { "person_id" };
            statementModel.PrimaryKeyValues = new ArrayOfString() { k.KundenId.ToString() };
            statementModel.Columns = new ArrayOfString();
            statementModel.Values = new ArrayOfString();
            statementModel.Columns.Add("firstname");
            statementModel.Values.Add(k.VorName);
            statementModel.Columns.Add("lastname");
            statementModel.Values.Add(k.NachName);
            statementModel.Columns.Add("e-mail");
            statementModel.Values.Add(k.EMail);
            //statementModel.Columns.Add("phone_number");
            //statementModel.Values.Add(k.);
            statementModel.Columns.Add("password");
            statementModel.Values.Add(k.Passwort);
            statementModel.Columns.Add("birthdate");
            statementModel.Values.Add(k.Geburtsdatum.ToString());
            statementModel.Columns.Add("street");
            statementModel.Values.Add(k.Strasse);
            statementModel.Columns.Add("country");
            statementModel.Values.Add(k.Land);
            statementModel.Columns.Add("zip_code");
            statementModel.Values.Add(k.PLZ.ToString());
            statementModel.Columns.Add("type_id");
            statementModel.Values.Add(customer.type.ToString());
            statementModel.Sender = "Backend";
            string response = client.InsertStatement(statementModel);

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

                CreateWebServiceSoapClient client = new CreateWebServiceSoapClient();
                StatementModel statementModel = new StatementModel();
                statementModel.Type = "Create";
                statementModel.TableName = "city";
                statementModel.PrimaryKeyNames = new ArrayOfString() { "zip_code" };
                statementModel.PrimaryKeyValues = new ArrayOfString() { city.ZipCode.ToString() };
                statementModel.Columns = new ArrayOfString();
                statementModel.Values = new ArrayOfString();
                statementModel.Columns.Add("name");
                statementModel.Values.Add(city.Name);
                statementModel.Sender = "Backend";
                string response = client.InsertStatement(statementModel);
            }            
        }
      

        public void DeleteCustomer(SharedKunde k)
        {
            model.person.Remove(model.person.SingleOrDefault(x => x.person_id.Equals(k.KundenId)));
            model.SaveChanges();

            CreateWebServiceSoapClient client = new CreateWebServiceSoapClient();
            StatementModel statementModel = new StatementModel();
            statementModel.Type = "Delete";
            statementModel.TableName = "person";
            statementModel.PrimaryKeyNames = new ArrayOfString() { "person_id" };
            statementModel.PrimaryKeyValues = new ArrayOfString() { k.KundenId.ToString() };
            statementModel.Columns = new ArrayOfString();
            statementModel.Values = new ArrayOfString();
            statementModel.Sender = "Backend";
            string response = client.InsertStatement(statementModel);
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
                Geburtsdatum = x.birthdate.Value,
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

        #region Packages_verwalten

        public List<string> GetCakeTypes(string art)
        {
            if (art.Equals("Kuchenkreationen"))
            {
                return model.article.Where(x => x.creation == true && x.article_type.description.Equals("Kuchen")).Select(x => x.description).ToList();
            }
            else
            {
                return model.article.Where(x => x.creation == false && x.article_type.description.Equals("Kuchen")).Select(x => x.description).ToList();
            }
        }

        public List<SharedPackage> GetPackages(string filterVisible, string filterCreation)
        {
            if (filterVisible.Equals("Sichtbar & Nicht Sichtbar"))
            {
                if (filterCreation.Equals("Kreation"))
                {
                    var list = model.package.Where(x => x.creation == true).Select(x => new SharedPackage()
                    {
                        PackageId = x.package_id,
                        Beschreibung = x.description,
                        Preis = x.price,
                        Visible = x.pack_active,
                        Creation = x.creation,
                        Kuchen = x.article.Select(y => y.description).ToList()
                    }).ToList();
                    foreach (var item in list)
                    {
                        item.KuchenString = string.Join(", ", item.Kuchen);
                    }
                    return list;
                }else if (filterCreation.Equals("Nicht Kreation"))
                {
                    var list = model.package.Where(x => x.creation == false).Select(x => new SharedPackage()
                    {
                        PackageId = x.package_id,
                        Beschreibung = x.description,
                        Preis = x.price,
                        Visible = x.pack_active,
                        Creation = x.creation,
                        Kuchen = x.article.Select(y => y.description).ToList()
                    }).ToList();
                    foreach (var item in list)
                    {
                        item.KuchenString = string.Join(", ", item.Kuchen);
                    }
                    return list;
                }
                else
                {
                    var list = model.package.Select(x => new SharedPackage()
                    {
                        PackageId = x.package_id,
                        Beschreibung = x.description,
                        Preis = x.price,
                        Visible = x.pack_active,
                        Creation = x.creation,
                        Kuchen = x.article.Select(y => y.description).ToList()
                    }).ToList();
                    foreach (var item in list)
                    {
                        item.KuchenString = string.Join(", ", item.Kuchen);
                    }
                    return list;
                }
            }else if (filterVisible.Equals("Sichtbar"))
            {
                if (filterCreation.Equals("Kreation"))
                {
                    var list = model.package.Where(x => x.creation == true && x.pack_active == true).Select(x => new SharedPackage()
                    {
                        PackageId = x.package_id,
                        Beschreibung = x.description,
                        Preis = x.price,
                        Visible = x.pack_active,
                        Creation = x.creation,
                        Kuchen = x.article.Select(y => y.description).ToList()
                    }).ToList();
                    foreach (var item in list)
                    {
                        item.KuchenString = string.Join(", ", item.Kuchen);
                    }
                    return list;
                }
                else if (filterCreation.Equals("Nicht Kreation"))
                {
                    var list = model.package.Where(x => x.creation == false && x.pack_active == true).Select(x => new SharedPackage()
                    {
                        PackageId = x.package_id,
                        Beschreibung = x.description,
                        Preis = x.price,
                        Visible = x.pack_active,
                        Creation = x.creation,
                        Kuchen = x.article.Select(y => y.description).ToList()
                    }).ToList();
                    foreach (var item in list)
                    {
                        item.KuchenString = string.Join(", ", item.Kuchen);
                    }
                    return list;
                }
                else
                {
                    var list = model.package.Where(x => x.pack_active == true).Select(x => new SharedPackage()
                    {
                        PackageId = x.package_id,
                        Beschreibung = x.description,
                        Preis = x.price,
                        Visible = x.pack_active,
                        Creation = x.creation,
                        Kuchen = x.article.Select(y => y.description).ToList()
                    }).ToList();
                    foreach (var item in list)
                    {
                        item.KuchenString = string.Join(", ", item.Kuchen);
                    }
                    return list;
                }
            }else if(filterVisible.Equals("Nicht Sichtbar"))
            {
                if (filterCreation.Equals("Kreation"))
                {
                    var list = model.package.Where(x => x.creation == true && x.pack_active == false).Select(x => new SharedPackage()
                    {
                        PackageId = x.package_id,
                        Beschreibung = x.description,
                        Preis = x.price,
                        Visible = x.pack_active,
                        Creation = x.creation,
                        Kuchen = x.article.Select(y => y.description).ToList()
                    }).ToList();
                    foreach (var item in list)
                    {
                        item.KuchenString = string.Join(", ", item.Kuchen);
                    }
                    return list;
                }
                else if (filterCreation.Equals("Nicht Kreation"))
                {
                    var list = model.package.Where(x => x.creation == false && x.pack_active == false).Select(x => new SharedPackage()
                    {
                        PackageId = x.package_id,
                        Beschreibung = x.description,
                        Preis = x.price,
                        Visible = x.pack_active,
                        Creation = x.creation,
                        Kuchen = x.article.Select(y => y.description).ToList()
                    }).ToList();
                    foreach (var item in list)
                    {
                        item.KuchenString = string.Join(", ", item.Kuchen);
                    }
                    return list;
                }
                else
                {
                    var list = model.package.Where(x => x.pack_active == false).Select(x => new SharedPackage()
                    {
                        PackageId = x.package_id,
                        Beschreibung = x.description,
                        Preis = x.price,
                        Visible = x.pack_active,
                        Creation = x.creation,
                        Kuchen = x.article.Select(y => y.description).ToList()
                    }).ToList();
                    foreach (var item in list)
                    {
                        item.KuchenString = string.Join(", ", item.Kuchen);
                    }
                    return list;
                }
            }
            else
            {
                var list = model.package.Select(x => new SharedPackage()
                {
                    PackageId = x.package_id,
                    Beschreibung = x.description,
                    Preis = x.price,
                    Visible = x.pack_active,
                    Creation = x.creation,
                    Kuchen = x.article.Select(y => y.description).ToList()
                }).ToList();
                foreach (var item in list)
                {
                    item.KuchenString = string.Join(", ", item.Kuchen);
                }
                return list;
            }
        }

        public void CreatePackage(SharedPackage tempPackage)
        {
            model.package.Add(new package()
            {
                package_id = tempPackage.PackageId,
                description = tempPackage.Beschreibung,
                price = tempPackage.Preis,
                pack_active = tempPackage.Visible,
                creation = tempPackage.Creation
            });
            model.SaveChanges();
        }

        public void UpdatePackage(SharedPackage tempPackage)
        {
            var package = model.package.SingleOrDefault(x => x.package_id == tempPackage.PackageId);

            package.description = tempPackage.Beschreibung;
            package.price = tempPackage.Preis;
            package.pack_active = tempPackage.Visible;
            package.creation = tempPackage.Creation;

            model.SaveChanges();
        }

        public void CreatePackageItem(string selectedPackage, string kuchen)
        {
            var tempPackage = model.package.SingleOrDefault(x => x.description.Equals(selectedPackage));
            var tempKuchen = model.article.SingleOrDefault(x => x.description.Equals(kuchen));

            tempPackage.article.Add(tempKuchen);
            model.SaveChanges();
        }

        public void DeletePackageItem(string selectedPackage, string kuchen)
        {
            var tempPackage = model.package.SingleOrDefault(x => x.description.Equals(selectedPackage));
            var tempKuchen = model.article.SingleOrDefault(x => x.description.Equals(kuchen));

            tempPackage.article.Remove(tempKuchen);
            model.SaveChanges();
        }

        public void DeletePackage(SharedPackage tempPackage)
        {
            model.package.Remove(model.package.SingleOrDefault(x => x.package_id == tempPackage.PackageId));
            model.SaveChanges();
        }

        #endregion

        #region Verpackungsverwaltung

        public List<string> GetMaschen()
        {
            return model.ingredient.Where(x => x.ingredient_type.description.Equals("Masche")).Select(x => x.description).ToList();
        }

        public List<string> GetKarton()
        {
            return model.ingredient.Where(x => x.ingredient_type.description.Equals("Karton")).Select(x => x.description).ToList();
        }

        public List<string> GetSticker()
        {
            return model.ingredient.Where(x => x.ingredient_type.description.Equals("Sticker")).Select(x => x.description).ToList();
        }

        public List<SharedVerpackung> GetPackaging(string filterVisible, string filterCreation)
        {
            if (filterVisible.Equals("Sichtbar & Nicht Sichtbar"))
            {
                if (filterCreation.Equals("Kreation"))
                {
                    var list = model.article.Where(x => x.article_type.description.Equals("Verpackung") && x.creation == true).Select(x => new SharedVerpackung()
                    {
                        VerpackungsId = x.article_id,
                        Description = x.description,
                        Creation = x.creation,
                        Price = x.price,
                        Visible = x.visible,
                        Komponenten = x.article_has_ingredient.Select(y => y.ingredient.description).ToList()
                    }).ToList();
                    foreach (var item in list)
                    {
                        item.KomponentenString = string.Join(", ", item.Komponenten);
                    }
                    return list;
                }
                else if(filterCreation.Equals("Nicht Kreation"))
                {
                    var list = model.article.Where(x => x.article_type.description.Equals("Verpackung") && x.creation == false).Select(x => new SharedVerpackung()
                    {
                        VerpackungsId = x.article_id,
                        Description = x.description,
                        Creation = x.creation,
                        Price = x.price,
                        Visible = x.visible,
                        Komponenten = x.article_has_ingredient.Select(y => y.ingredient.description).ToList()
                    }).ToList();
                    foreach (var item in list)
                    {
                        item.KomponentenString = string.Join(", ", item.Komponenten);
                    }
                    return list;
                }
                else
                {
                    var list = model.article.Where(x => x.article_type.description.Equals("Verpackung")).Select(x => new SharedVerpackung()
                    {
                        VerpackungsId = x.article_id,
                        Description = x.description,
                        Creation = x.creation,
                        Price = x.price,
                        Visible = x.visible,
                        Komponenten = x.article_has_ingredient.Select(y => y.ingredient.description).ToList()
                    }).ToList();
                    foreach (var item in list)
                    {
                        item.KomponentenString = string.Join(", ", item.Komponenten);
                    }
                    return list;
                }
            }else if (filterVisible.Equals("Sichtbar"))
            {
                if (filterCreation.Equals("Kreation"))
                {
                    var list = model.article.Where(x => x.article_type.description.Equals("Verpackung") && x.visible == true && x.creation == true).Select(x => new SharedVerpackung()
                    {
                        VerpackungsId = x.article_id,
                        Description = x.description,
                        Creation = x.creation,
                        Price = x.price,
                        Visible = x.visible,
                        Komponenten = x.article_has_ingredient.Select(y => y.ingredient.description).ToList()
                    }).ToList();
                    foreach (var item in list)
                    {
                        item.KomponentenString = string.Join(", ", item.Komponenten);
                    }
                    return list;
                }
                else if (filterCreation.Equals("Nicht Kreation"))
                {
                    var list = model.article.Where(x => x.article_type.description.Equals("Verpackung") && x.visible == true && x.creation == false).Select(x => new SharedVerpackung()
                    {
                        VerpackungsId = x.article_id,
                        Description = x.description,
                        Creation = x.creation,
                        Price = x.price,
                        Visible = x.visible,
                        Komponenten = x.article_has_ingredient.Select(y => y.ingredient.description).ToList()
                    }).ToList();
                    foreach (var item in list)
                    {
                        item.KomponentenString = string.Join(", ", item.Komponenten);
                    }
                    return list;
                }
                else
                {
                    var list = model.article.Where(x => x.article_type.description.Equals("Verpackung") && x.visible == true).Select(x => new SharedVerpackung()
                    {
                        VerpackungsId = x.article_id,
                        Description = x.description,
                        Creation = x.creation,
                        Price = x.price,
                        Visible = x.visible,
                        Komponenten = x.article_has_ingredient.Select(y => y.ingredient.description).ToList()
                    }).ToList();
                    foreach (var item in list)
                    {
                        item.KomponentenString = string.Join(", ", item.Komponenten);
                    }
                    return list;
                }
            }else if (filterVisible.Equals("Nicht Sichtbar"))
            {
                if (filterCreation.Equals("Kreation"))
                {
                    var list = model.article.Where(x => x.article_type.description.Equals("Verpackung") && x.visible == false && x.creation == true).Select(x => new SharedVerpackung()
                    {
                        VerpackungsId = x.article_id,
                        Description = x.description,
                        Creation = x.creation,
                        Price = x.price,
                        Visible = x.visible,
                        Komponenten = x.article_has_ingredient.Select(y => y.ingredient.description).ToList()
                    }).ToList();
                    foreach (var item in list)
                    {
                        item.KomponentenString = string.Join(", ", item.Komponenten);
                    }
                    return list;
                }
                else if (filterCreation.Equals("Nicht Kreation"))
                {
                    var list = model.article.Where(x => x.article_type.description.Equals("Verpackung") && x.visible == false && x.creation == false).Select(x => new SharedVerpackung()
                    {
                        VerpackungsId = x.article_id,
                        Description = x.description,
                        Creation = x.creation,
                        Price = x.price,
                        Visible = x.visible,
                        Komponenten = x.article_has_ingredient.Select(y => y.ingredient.description).ToList()
                    }).ToList();
                    foreach (var item in list)
                    {
                        item.KomponentenString = string.Join(", ", item.Komponenten);
                    }
                    return list;
                }
                else
                {
                    var list = model.article.Where(x => x.article_type.description.Equals("Verpackung") && x.visible == false).Select(x => new SharedVerpackung()
                    {
                        VerpackungsId = x.article_id,
                        Description = x.description,
                        Creation = x.creation,
                        Price = x.price,
                        Visible = x.visible,
                        Komponenten = x.article_has_ingredient.Select(y => y.ingredient.description).ToList()
                    }).ToList();
                    foreach (var item in list)
                    {
                        item.KomponentenString = string.Join(", ", item.Komponenten);
                    }
                    return list;
                }
            }
            else
            {
                var list = model.article.Where(x => x.article_type.description.Equals("Verpackung")).Select(x => new SharedVerpackung()
                {
                    VerpackungsId = x.article_id,
                    Description = x.description,
                    Creation = x.creation,
                    Price = x.price,
                    Visible = x.visible,
                    Komponenten = x.article_has_ingredient.Select(y => y.ingredient.description).ToList()
                }).ToList();
                foreach (var item in list)
                {
                    item.KomponentenString = string.Join(", ", item.Komponenten);
                }
                return list;
            }
        }

        
        //db nicht auf selben stand bzw. noch nicht befüllt, deshalb funktioniert das noch nicht
        public void CreateVerpackung(SharedVerpackung tempVerpackung)
        {
            var tempArticleType = model.article_type.SingleOrDefault(x => x.description.Equals("Verpackung"));
            var tempShape = model.shape.SingleOrDefault(x => x.description.Equals("keine Form"));
            var verpackung = new article()
             {
                 article_id = tempVerpackung.VerpackungsId,
                 article_type = tempArticleType,
                 description = tempVerpackung.Description,
                 price = tempVerpackung.Price,
                 creation = tempVerpackung.Creation,
                 shape = tempShape,
                 visible = tempVerpackung.Visible
             };
             model.article.Add(verpackung);
             model.SaveChanges();        
        }

        public void UpdateVerpackung(SharedVerpackung tempVerpackung)
        {
            var tempArticleType = model.article_type.SingleOrDefault(x => x.description.Equals("Verpackung"));
            var tempShape = model.shape.SingleOrDefault(x => x.description.Equals("keine Form"));

            var verpackung = model.article.SingleOrDefault(x => x.article_id == tempVerpackung.VerpackungsId);
            verpackung.article_id = tempVerpackung.VerpackungsId;
            verpackung.article_type = tempArticleType;
            verpackung.description = tempVerpackung.Description;
            verpackung.price = tempVerpackung.Price;
            verpackung.creation = tempVerpackung.Creation;
            verpackung.shape = tempShape;
            verpackung.visible = tempVerpackung.Visible;

            model.SaveChanges();
        }

        public void CreateVerpackungskomponenten(Guid Id, string Komponente)
        {
            model.article_has_ingredient.Add(new article_has_ingredient()
            {
                article = model.article.SingleOrDefault(x => x.article_id == Id),
                ingredient = model.ingredient.SingleOrDefault(x => x.description.Equals(Komponente)),
                amount = 1
            });
            model.SaveChanges();
        }

        public void DeleteVerpackungskomponenten(Guid Id, string Komponente)
        {
            model.article_has_ingredient.Remove(model.article_has_ingredient.SingleOrDefault(x => x.article_id == Id && x.ingredient.description.Equals(Komponente)));
            model.SaveChanges();
        }

        public void DeleteVerpackung(SharedVerpackung tempVerpackung)
        {
            model.article.Remove(model.article.SingleOrDefault(x => x.article_id == tempVerpackung.VerpackungsId));
            model.SaveChanges();
        }

        #endregion

        #region Angebotsverwaltung

        public List<SharedAngebot> GetAllSpecialOffers()
        {
            var offers = model.special_offer.Select(x => new SharedAngebot()
            {
                AngebotId = x.special_offer_id,
                Code = x.code,
                StartDatum = x.start_date,
                EndDatum = x.end_date,
                Prozent = x.percentage


            }).ToList();

            return offers;

        }

        public void AddNewSpecialOffer(SharedAngebot sOffer)
        {

            var specialOffer = new special_offer()
            {
                special_offer_id = Guid.NewGuid(),
                code = sOffer.Code,
                start_date = sOffer.StartDatum,
                end_date = sOffer.EndDatum,
                percentage = sOffer.Prozent

            };

            model.special_offer.Add(specialOffer);
            model.SaveChanges();
        }

        public void UpdateSpecialOffer(SharedAngebot sOffer)
        {

            var specialOffer = model.special_offer.SingleOrDefault(x => x.special_offer_id.Equals(sOffer.AngebotId));

            specialOffer.code = sOffer.Code;
            specialOffer.start_date = sOffer.StartDatum;
            specialOffer.end_date = sOffer.EndDatum;
            specialOffer.percentage = sOffer.Prozent;

            model.SaveChanges();
        }

        public void DeleteSpecialOffer(SharedAngebot sOffer)
        {

            model.special_offer.Remove(model.special_offer.SingleOrDefault(x => x.special_offer_id.Equals(sOffer.AngebotId)));
            model.SaveChanges();
        }
        #endregion

        #region Berichte


        public List<SharedShortOrder> GetOfOrdersInCurrentMonthByType(string orderstatus)
        {

            if (orderstatus != "alle") { 

                return model.order.Where(x => x.order_date.Value.Month.Equals(DateTime.Now.Month) && x.status.Equals(orderstatus)).Select(y => new SharedShortOrder
                {

                    OrderID = y.order_id


                }).ToList();

            }else{

                return model.order.Where(x => x.order_date.Value.Month.Equals(DateTime.Now.Month)).Select(y => new SharedShortOrder
                {

                    OrderID = y.order_id


                }).ToList();

            }
        }


        public List<SharedShortOrder> GetDoneOrdersByMonth(int month)
        {

            return model.order.Where(x=> x.order_date.Value.Month.Equals(month) && x.status.Equals("abgeschlossen")).Select(y=> new SharedShortOrder{

                OrderID = y.order_id


             }).ToList();


        }


        public List<SharedOrderArticle> GetArticelsforOrder(Guid orderId)
        {

            var order = model.order_has_articles.Where(x => x.order_id.Equals(orderId)).Select(y => new SharedOrderArticle
            {

                id = y.article_id,
                name = y.article.description,
                quantity = y.amount.Value
            }).ToList();

            return order;
        }

        public List<SharedOrderIngridient> getRawMaterialForOrderProduct(Guid pID, int amountMultiplier)
        {

           var rawMat =  model.article_has_ingredient.Where(x => x.article_id.Equals(pID)).Select(y => new SharedOrderIngridient
            {

                name = y.ingredient.description,
                unit = y.ingredient.unit,
                amount = y.amount.Value * amountMultiplier

            }).ToList();

            return rawMat;

        }
        

#endregion

    }
}
