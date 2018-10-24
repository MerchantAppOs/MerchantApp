using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Merchant.Models;
using Merchant.Navigation;
using Merchant.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Merchant.ViewModels
{
    public class Y_M_redaktəsiViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private ObservableCollection<string> category = new ObservableCollection<string>();
        public ObservableCollection<string> CB_Category
        {
            get { return category; }
            set
            {
                category = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> unit = new ObservableCollection<string>();
        public ObservableCollection<string> CB_Unit
        {
            get { return unit; }
            set
            {
                unit = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<int> LimiteAmount = new ObservableCollection<int>();
        public ObservableCollection<int> CB_LimiteAmount
        {
            get { return LimiteAmount; }
            set
            {
                LimiteAmount = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> price = new ObservableCollection<string>();
        public ObservableCollection<string> CB_Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> sellingPrice = new ObservableCollection<string>();
        public ObservableCollection<string> CB_Selling
        {
            get { return sellingPrice; }
            set
            {
                sellingPrice = value;
                OnPropertyChanged();
            }
        }
        private int id;
        public int Id { get => id; set { id = value; OnPropertyChanged(); } }
        private string _category;
        public string Category { get => _category; set { _category = value; OnPropertyChanged(); } }
        private string _productCode;
        public string ProductCode { get => _productCode; set { _productCode = value; OnPropertyChanged(); } }
        private string _productName;
        public string ProductName { get => _productName; set { _productName = value; OnPropertyChanged(); } }
        private string _mark;
        public string Mark { get => _mark; set { _mark = value; OnPropertyChanged(); } }
        private string _unit;
        public string Unit { get => _unit; set { _unit = value; OnPropertyChanged(); } }
        private string _barcode;
        public string Barcode { get => _barcode; set { _barcode = value; OnPropertyChanged(); } }
        private decimal _price;
        public decimal Price { get => _price; set { _price = value; OnPropertyChanged(); } }
        private decimal _sellingPrice;
        public decimal SellingPrice { get => _sellingPrice; set { _sellingPrice = value; OnPropertyChanged(); } }
        private int _limitedAmount;
        public int LimitedAmount { get => _limitedAmount; set { _limitedAmount = value; OnPropertyChanged(); } }
        public Y_M_redaktəsiViewModel(Products x)
        {
            Id = x.Id;
            Category = x.Category;
            ProductCode = x.ProductCode;
            ProductName = x.ProductName;
            Mark = x.Mark;
            Unit = x.Unit;
            Barcode = x.Barcode;
            Price = x.Price;
            SellingPrice = x.SellingPrice;
            LimitedAmount = x.LimitedAmount;
            using (var ent = new DataContex())
            {
                var CodeFromCategory = ent.Category;
                var prd = ent.Product;

                foreach (var item in CodeFromCategory)
                {
                    CB_Category.Add(item.CatalogName);
                }
                foreach (var item in prd)
                {
                    CB_Price.Add(item.Price.ToString());
                    CB_Selling.Add(item.SellingPrice.ToString());
                }

            }
            CB_Unit.Add("ədəd");
            CB_Unit.Add("metr");
            CB_Unit.Add("kq");
            CB_Unit.Add("qutu");
            CB_LimiteAmount.Add(5);
            CB_LimiteAmount.Add(10);
            CB_LimiteAmount.Add(15);
            CB_LimiteAmount.Add(20);

        }
        private RelayCommand addUpdate;
        public RelayCommand AddUpdate
        {
            get
            {
                return addUpdate ?? (addUpdate = new RelayCommand(
                    x =>
                    {
                        using (DataContex dc = new DataContex())
                        {
                            if (MessageBox.Show("Məlumat yenilənsin?", "Yenilə", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                Products products = new Products();
                                products.Id = Id;
                                products.Category = Category;
                                products.ProductCode = ProductCode;
                                products.ProductName = ProductName;
                                products.Mark = Mark;
                                products.Unit = Unit;
                                products.Barcode = Barcode;
                                products.Price = Price;
                                products.SellingPrice = SellingPrice;
                                products.LimitedAmount = LimitedAmount;
                                dc.Entry(products).State = EntityState.Modified;
                                dc.SaveChanges();
                                Messenger.Default.Send<RequestCloseMessage>(new RequestCloseMessage(this), this);

                            }

                            else { return; }
                        }
                    }));

            }
            set => addUpdate = value;
        }
        private RelayCommand exitCommand;
        public RelayCommand ExitCommand => exitCommand ?? (exitCommand = new RelayCommand(
           x =>
           {
               Messenger.Default.Send<RequestCloseMessage>(new RequestCloseMessage(this), this);

           }
           ));
        private PropertyChangedEventHandler _PropertyChanged;
        public virtual event PropertyChangedEventHandler PropertyChanged
        {
            add { _PropertyChanged += value; }
            remove { _PropertyChanged -= value; }
        }
        protected virtual void OnPropertyChanged([CallerMemberName]string prob = "")
        {
            _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prob));
        }
    }
}