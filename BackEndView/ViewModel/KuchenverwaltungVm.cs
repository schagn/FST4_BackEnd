﻿using DataRepository;
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
            set { selectedArticle = value; RaisePropertyChanged(); }
        }
        public RelayCommand BtnCancelClicked { get; set; }
        public RelayCommand BtnDeleteClicked { get; set; }
        public RelayCommand BtnEditClicked { get; set; }
        public RelayCommand BtnSaveClicked { get; set; }
        public List<string> VisibilityFilter { get; set; }
        public List<string> CreationFilter { get; set; }
        private string selectedVisibilityFilter;

        public string SelectedVisibilityFilter
        {
            get { return selectedVisibilityFilter; }
            set { selectedVisibilityFilter = value; RaisePropertyChanged(); RefreshList(); }
        }
        private string selectedCreationFilter;

        public string SelectedCreationFilter
        {
            get { return selectedCreationFilter; }
            set { selectedCreationFilter = value; RaisePropertyChanged(); RefreshList(); }
        }

        private DataHandler dataHandler;
        private bool IsEditing;
        private SharedArticle EditedArticle;

        public KuchenverwaltungVm()
        {
            VisibilityFilter = new List<string>() { "Visible & Non-Visible", "Visible", "Non-Visible" };
            CreationFilter = new List<string>() { "Creation & Non-Creation", "Creation", "Non-Creation" };
            selectedVisibilityFilter = "Visible & Non-Visible";
            selectedCreationFilter = "Creation & Non-Creation";

            IsEditing = false;
            BtnCancelClicked = new RelayCommand(Cancel);
            BtnDeleteClicked = new RelayCommand(Delete);
            BtnEditClicked = new RelayCommand(Edit);
            BtnSaveClicked = new RelayCommand(Save);
            dataHandler = new DataHandler();

            RefreshList();
        }

        private void Cancel()
        {
            IsEditing = false;
            EditedArticle = null;
            Description = null;
            Price = 0;
            Creation = false;
            Visible = false;
            SelectedShape = null;
        }

        private void Delete()
        {
            dataHandler.DeleteArticle(SelectedArticle.ArticleId);
            RefreshList();
        }

        private void Edit()
        {
            IsEditing = true;
            EditedArticle = SelectedArticle;
            Description = SelectedArticle != null ? SelectedArticle.Description : null;
            Price = SelectedArticle != null ? SelectedArticle.Price : 0;
            Creation = SelectedArticle != null ? SelectedArticle.Creation : false;
            Visible = SelectedArticle != null ? SelectedArticle.Visible : false;
            SelectedShape = SelectedArticle != null ? SelectedArticle.ShapeDescription : null;
        }

        private void Save()
        {
            if (IsEditing)
            {
                var tempArticle = EditedArticle;
                tempArticle.Creation = Creation;
                tempArticle.Description = Description;
                tempArticle.Price = Price;
                tempArticle.ShapeDescription = SelectedShape;
                tempArticle.Visible = Visible;
                dataHandler.UpdateArticle(tempArticle);
            }
            else
            {
                dataHandler.CreateArticle(new SharedArticle()
                {
                    ArticleTypeDescription = "Kuchen",
                    Creation = Creation,
                    Description = Description,
                    Price = Price,
                    ShapeDescription = SelectedShape,
                    Visible = Visible
                });
            }
            RefreshList();
            Cancel();
        }

        private void RefreshList()
        {
            Shapes = dataHandler.GetShapes();
            Articles = new ObservableCollection<SharedArticle>(dataHandler.GetArticles().Where(x => x.ArticleTypeDescription.Equals("Kuchen")));
            switch (SelectedVisibilityFilter)
            {
                case "Visible":
                    Articles = new ObservableCollection<SharedArticle>(Articles.Where(x => x.Visible));
                    break;
                case "Non-Visible":
                    Articles = new ObservableCollection<SharedArticle>(Articles.Where(x => !x.Visible));
                    break;
                default:
                    break;
            }
            switch (SelectedCreationFilter)
            {
                case "Creation":
                    Articles = new ObservableCollection<SharedArticle>(Articles.Where(x => x.Creation));
                    break;
                case "Non-Creation":
                    Articles = new ObservableCollection<SharedArticle>(Articles.Where(x => !x.Creation));
                    break;
                default:
                    break;
            }
            RaisePropertyChanged("Articles");
        }
    }
}
