using DataRepository;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEndView.ViewModel
{
    public class KuchenverwaltungVm : ViewModelBase
    {
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; RaisePropertyChanged(); }
        }

        private double price;

        public double Price
        {
            get { return price; }
            set { price = value; RaisePropertyChanged(); }
        }

        private bool creation;

        public bool Creation
        {
            get { return creation; }
            set { creation = value; RaisePropertyChanged(); }
        }

        private bool visible;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; RaisePropertyChanged(); }
        }

        public List<string> ArticleTypes { get; set; }
        private string selectedArticleType;
        public string SelectedArticleType
        {
            get { return selectedArticleType; }
            set { selectedArticleType = value; RaisePropertyChanged(); }
        }
        public List<string> Shapes { get; set; }
        private string selectedShape;
        public string SelectedShape
        {
            get { return selectedShape; }
            set { selectedShape = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<SharedArticle> Articles { get; set; }
        private SharedArticle selectedArticle;

        public SharedArticle SelectedArticle
        {
            get { return selectedArticle; }
            set { selectedArticle = value; RaisePropertyChanged(); RefreshForm(); }
        }
        public RelayCommand BtnEditClicked { get; set; }
        public RelayCommand BtnNewClicked { get; set; }
        private DataHandler dataHandler;

        public KuchenverwaltungVm()
        {
            BtnEditClicked = new RelayCommand(Edit);
            BtnNewClicked = new RelayCommand(New);
            dataHandler = new DataHandler();

            RefreshList();
        }

        private void RefreshList()
        {
            ArticleTypes = dataHandler.GetCakeTypes();
            Shapes = dataHandler.GetShapes();
            Articles = new ObservableCollection<SharedArticle>(dataHandler.GetArticles());
            RaisePropertyChanged("Articles");
        }

        private void RefreshForm()
        {
            Description = SelectedArticle != null ? SelectedArticle.Description : null;
            Price = SelectedArticle != null ? SelectedArticle.Price : 0;
            Creation = SelectedArticle != null ? SelectedArticle.Creation : false;
            Visible = SelectedArticle != null ? SelectedArticle.Visible : false;
            SelectedArticleType = SelectedArticle != null ? SelectedArticle.ArticleTypeDescription : null;
            SelectedShape = SelectedArticle != null ? SelectedArticle.ShapeDescription : null;
        }

        private void New()
        {
            dataHandler.CreateArticle(new SharedArticle()
            {
                ArticleTypeDescription = SelectedArticleType,
                Creation = Creation,
                Description = Description,
                Price = Price,
                ShapeDescription = SelectedShape,
                Visible = Visible
            });
            RefreshList();
        }

        private void Edit()
        {
            var tempArticle = SelectedArticle;
            tempArticle.ArticleTypeDescription = SelectedArticleType;
            tempArticle.Creation = Creation;
            tempArticle.Description = Description;
            tempArticle.Price = Price;
            tempArticle.ShapeDescription = SelectedShape;
            tempArticle.Visible = Visible;
            dataHandler.UpdateArticle(tempArticle);
            RefreshList();
        }
    }
}
