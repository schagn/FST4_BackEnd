using DataRepository;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace BackEndView.ViewModel
{
    public class KuchenZutatenVm : ViewModelBase
    {
        public List<string> Articles { get; set; }
        private string selectedArticle;

        public string SelectedArticle
        {
            get { return selectedArticle; }
            set { selectedArticle = value; RaisePropertyChanged(); RefreshList(); RefreshForm(); }
        }
        public List<string> Ingredients { get; set; }
        private string selectedIngredient;

        public string SelectedIngredient
        {
            get { return selectedIngredient; }
            set { selectedIngredient = value; RaisePropertyChanged(); }
        }
        private bool enabledIngredient;

        public bool EnabledIngredient
        {
            get { return enabledIngredient; }
            set { enabledIngredient = value; RaisePropertyChanged(); }
        }

        private double amount;

        public double Amount
        {
            get { return amount; }
            set { amount = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<SharedArticleIngredient> ArticleIngredients { get; set; }
        private SharedArticleIngredient selectedArticleIngredient;

        public SharedArticleIngredient SelectedArticleIngredient
        {
            get { return selectedArticleIngredient; }
            set { selectedArticleIngredient = value; RaisePropertyChanged(); RefreshForm(); }
        }


        public RelayCommand BtnDeleteClicked { get; set; }
        public RelayCommand BtnEditClicked { get; set; }
        public RelayCommand BtnNewClicked { get; set; }

        private DataHandler dataHandler;
        private Visibility visibleError;

        public Visibility VisibleError
        {
            get { return visibleError; }
            set { visibleError = value; RaisePropertyChanged(); }
        }
        private string errorText;

        public string ErrorText
        {
            get { return errorText; }
            set { errorText = value; RaisePropertyChanged(); }
        }


        public KuchenZutatenVm()
        {
            dataHandler = new DataHandler();
            BtnDeleteClicked = new RelayCommand(Delete, () => { return SelectedArticleIngredient != null; });
            BtnEditClicked = new RelayCommand(Edit, () => { return SelectedArticleIngredient != null; });
            BtnNewClicked = new RelayCommand(New, () => { return SelectedIngredient != null && SelectedArticleIngredient == null; });

            EnabledIngredient = true;
            VisibleError = Visibility.Hidden;
            Articles = dataHandler.GetArticles().Select(x => x.Description).ToList();
            Ingredients = dataHandler.GetIngredients().Select(x => x.Description).ToList();
        }

        private void RefreshList()
        {
            ArticleIngredients = new ObservableCollection<SharedArticleIngredient>(dataHandler.GetArticleIngredients(SelectedArticle));
            RaisePropertyChanged("ArticleIngredients");
        }

        private void RefreshForm()
        {
            Amount = SelectedArticleIngredient != null ? SelectedArticleIngredient.Amount : 0;
            SelectedIngredient = SelectedArticleIngredient != null ? SelectedArticleIngredient.Ingredient.Description : null;
            EnabledIngredient = SelectedArticleIngredient == null;
        }

        private void Delete()
        {
            try
            {
                dataHandler.DeleteArticleIngredient(SelectedArticle, SelectedIngredient);
            }
            catch (Exception e)
            {
                ErrorText = e.Message;
                VisibleError = Visibility.Visible;
                Thread thread = new Thread(new ThreadStart(DisappearErrorMessage));
                thread.IsBackground = true;
                thread.Start();
            }
            RefreshList();
        }

        private void Edit()
        {
            try
            {
                dataHandler.UpdateArticleIngredient(SelectedArticle, SelectedIngredient, Amount);
            }
            catch (Exception e)
            {
                ErrorText = e.Message;
                VisibleError = Visibility.Visible;
                Thread thread = new Thread(new ThreadStart(DisappearErrorMessage));
                thread.IsBackground = true;
                thread.Start();
            }
            RefreshList();
        }

        private void New()
        {
            try
            {
                dataHandler.CreateArticleIngredient(SelectedArticle, SelectedIngredient, Amount);
            }
            catch (Exception e)
            {
                ErrorText = e.Message;
                VisibleError = Visibility.Visible;
                Thread thread = new Thread(new ThreadStart(DisappearErrorMessage));
                thread.IsBackground = true;
                thread.Start();
            }
            RefreshList();
        }

        private void DisappearErrorMessage()
        {
            Thread.Sleep(5000);
            App.Current.Dispatcher.Invoke(() => { VisibleError = Visibility.Hidden; });
        }

    }
}
