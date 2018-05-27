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
            set { selectedArticle = value; RaisePropertyChanged(); RefreshList(); }
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
            set { selectedArticleIngredient = value; RaisePropertyChanged(); }
        }


        public RelayCommand BtnCancelClicked { get; set; }
        public RelayCommand BtnDeleteClicked { get; set; }
        public RelayCommand BtnEditClicked { get; set; }
        public RelayCommand BtnSaveClicked { get; set; }

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
        private bool IsEditing;
        private SharedArticleIngredient EditedArticleIngredient;


        public KuchenZutatenVm()
        {
            dataHandler = new DataHandler();
            BtnCancelClicked = new RelayCommand(Cancel);
            BtnDeleteClicked = new RelayCommand(Delete);
            BtnEditClicked = new RelayCommand(Edit);
            BtnSaveClicked = new RelayCommand(Save);

            EnabledIngredient = true;
            VisibleError = Visibility.Hidden;

            RefreshForm();
        }

        private void Cancel()
        {
            IsEditing = false;
            EditedArticleIngredient = null;
            Amount = 0;
            SelectedIngredient = null;
            EnabledIngredient = false;
        }

        private void Delete()
        {
            try
            {
                dataHandler.DeleteArticleIngredient(SelectedArticle, SelectedArticleIngredient.Ingredient.Description);
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
            IsEditing = true;
            EditedArticleIngredient = SelectedArticleIngredient;
            Amount = SelectedArticleIngredient != null ? SelectedArticleIngredient.Amount : 0;
            SelectedIngredient = SelectedArticleIngredient != null ? SelectedArticleIngredient.Ingredient.Description : null;
            EnabledIngredient = SelectedArticleIngredient == null;
        }

        private void Save()
        {
            if (IsEditing)
            {
                dataHandler.UpdateArticleIngredient(SelectedArticle, SelectedIngredient, Amount);
            }
            else
            {
                dataHandler.CreateArticleIngredient(SelectedArticle, SelectedIngredient, Amount);
            }
            RefreshList();
            Cancel();
        }

        public void RefreshForm()
        {
            Articles = dataHandler.GetArticles().Where(x => x.ArticleTypeDescription.Equals("Kuchen")).Select(x => x.Description).ToList();
            Ingredients = dataHandler.GetIngredients().Select(x => x.Description).ToList();
        }

        public void RefreshList()
        {
            ArticleIngredients = new ObservableCollection<SharedArticleIngredient>(dataHandler.GetArticleIngredients(SelectedArticle));
            RaisePropertyChanged("ArticleIngredients");
        }

        private void DisappearErrorMessage()
        {
            Thread.Sleep(5000);
            App.Current.Dispatcher.Invoke(() => { VisibleError = Visibility.Hidden; });
        }

    }
}
