using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Merchant.Models;
using Merchant.Navigation;
using Merchant.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Merchant.ViewModels
{
    public class Malın_seçilməsiViewModel : ViewModelBase, INotifyPropertyChanged
    {

        public ObservableCollection<Products> _listOfProduct;
        public ObservableCollection<Products> ListOfProduct
        {
            get { return _listOfProduct; }
            set
            {
                _listOfProduct = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> _listOfCategory = new ObservableCollection<string>();
        public ObservableCollection<string> ListOfCategory
        {
            get { return _listOfCategory; }
            set
            {
                _listOfCategory = value;
                OnPropertyChanged();
            }
        }
        ViewModelBase CurrentView;
        public Malın_seçilməsiViewModel(ViewModelBase viewModel)
        {
            using (DataContex dc = new DataContex())
            {
                _listOfProduct = new ObservableCollection<Products>(dc.Product);
                ListOfCategory.Add("All");
                foreach (var item in dc.Category)
                {
                    ListOfCategory.Add(item.CatalogName);
                }
            }
            CurrentView = viewModel;

        }
        public string SelectCategory { get; set; }


        private RelayCommand _selectProduct;
        public RelayCommand SelectProduct => _selectProduct ?? (_selectProduct = new RelayCommand(
            x =>
            {

                ListOfProduct.Clear();
                using (DataContex dc = new DataContex())
                {

                    var obj = SelectCategory;
                    if (obj == "All")
                    {
                        foreach (var item in dc.Product)
                        {

                            ListOfProduct.Add(item);
                        }
                    }
                    else
                    {
                        List<Products> pr = (from r in dc.Product where r.Category == obj select r).ToList();
                        foreach (var item in pr)
                        {
                            ListOfProduct.Add(item);
                        }
                    }
                }
            }

            ));



        public Products Selectvalue
        {

            set
            {
                if (CurrentView.ToString() == "Merchant.ViewModels.Malin_AlinmasiViewModel")
                {
                    Messenger.Default.Send<Products, Malin_AlinmasiViewModel>(new Products
                    {
                        Price = SelectItem.Price,
                        ProductName = SelectItem.ProductName,
                        SellingPrice = SelectItem.SellingPrice,
                        ProductCode = SelectItem.ProductCode
                    });
                }
                else if (CurrentView.ToString() == "Merchant.ViewModels.Təmənnasız_alışViewModel")
                {
                    Messenger.Default.Send<Products, Təmənnasız_alışViewModel>(new Products
                    {
                        Price = SelectItem.Price,
                        ProductName = SelectItem.ProductName,
                        SellingPrice = SelectItem.SellingPrice,
                        ProductCode = SelectItem.ProductCode

                    });
                }
                else
                {
                    Messenger.Default.Send<Products, Malin_SatilmasiViewModel>(new Products
                    {
                        Price = SelectItem.Price,
                        ProductName = SelectItem.ProductName,
                        SellingPrice = SelectItem.SellingPrice,
                        ProductCode = SelectItem.ProductCode

                    });
                }
                Messenger.Default.Send<RequestCloseMessage>(new RequestCloseMessage(this), this);
            }
        }
        public Products SelectItem { get; set; }

        private string _textPrintersFilter;
        public string TextPrintersFilter
        {
            get { return _textPrintersFilter; }
            set
            {
                _textPrintersFilter = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand filterTextChangedCommand;
        public RelayCommand FilterTextChangedCommand
        {
            get
            {
                if (this.filterTextChangedCommand == null)
                {
                    this.filterTextChangedCommand =
                      new RelayCommand(param => this.OnRequestFilterTextChanged());
                }
                return this.filterTextChangedCommand;
            }
        }
        private void OnRequestFilterTextChanged()
        {
            string txtOrig = TextPrintersFilter;
            string upper = txtOrig.ToUpper();
            string lower = txtOrig.ToLower();
            using (DataContex dc = new DataContex())
            {
                var ProdFiltered = (from product in dc.Product
                                    let w = product.ProductName
                                    where w.StartsWith(lower) ||
                                    w.StartsWith(upper) ||
                                    w.Contains(txtOrig)
                                    select product).ToList();
                ListOfProduct.Clear();
                foreach (var item in ProdFiltered)
                {

                    ListOfProduct.Add(item);
                }
            }
        }

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
