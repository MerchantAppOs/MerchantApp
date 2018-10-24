using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
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
    public class Təmənnasız_alışViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _productName;
        public string ProductName { get => _productName; set { _productName = value; OnPropertyChanged(); } }
        private decimal _price;
        public decimal Price { get => _price; set { _price = value; OnPropertyChanged(); } }
        private decimal _sellingPrice;
        public decimal SellingPrice { get => _sellingPrice; set { _sellingPrice = value; OnPropertyChanged(); } }
        private int _limitedAmount = 0;
        public int LimitedAmount { get => _limitedAmount; set { _limitedAmount = value; OnPropertyChanged(); } }
        private decimal _amount;
        public decimal Amount { get => _amount; set { _amount = value; OnPropertyChanged(); } }
        private readonly INavigationService navigationService;
        public Təmənnasız_alışViewModel(INavigationService navigation)
        {
            this.navigationService = navigation;
            decimal amount = 0;
            Messenger.Default.Register<Products>(this, x =>
            {
                EnabledButton = true;
                ProductName = x.ProductName;
                Price = x.Price;
                SellingPrice = x.SellingPrice;

            });
            //using (DataContex DGrid = new DataContex())
            //{
            //    foreach (var item in DGrid.Waherhouses)
            //    {
            //        amount += item.Amount;
            //    }
            //    _amountLabel = amount;
            //}
        }

        public ObservableCollection<Waherhouse> _listOfProduct = new ObservableCollection<Waherhouse>();
        public ObservableCollection<Waherhouse> ListOfProduct
        {
            get { return _listOfProduct; }
            set
            {
                _listOfProduct = value;
                OnPropertyChanged();
            }
        }

        public DateTime ImportTime { get; set; } = DateTime.Now.Date;
        public decimal _amountLabel;
        public decimal AmountLabel
        {
            get => _amountLabel;
            set
            {
                _amountLabel = value;
                OnPropertyChanged();
            }
        }


        public Təmənnasız_alışViewModel()
        {

        }

        private RelayCommand _chooseProduct;
        public RelayCommand ChooseProduct
        {
            get
            {
                return _chooseProduct ?? (_chooseProduct = new RelayCommand(

                   (x =>
                   {
                       ServiceManager.GetService<IViewService>().OpenDialog(new Malın_seçilməsiViewModel(this));
                   }
                   ))
                 );

            }
        }
        public bool _enabledButton;
        public bool EnabledButton { get => _enabledButton; set { _enabledButton = value; OnPropertyChanged(); } }
        public Waherhouse SelectItem { get; set; }

        private RelayCommand addNewProduct;
        public RelayCommand AddNewProduct
        {
            get => addNewProduct ?? (addNewProduct = new RelayCommand(
                   x =>
                   {
                       try
                       {
                           decimal amount = 0;
                           using (DataContex dc = new DataContex())
                           {

                               var obj = dc.Product.Where(q => q.ProductName.Equals(ProductName)).FirstOrDefault<Products>();

                               Waherhouse waherhouse = new Waherhouse()
                               {
                                   ProductName = obj.ProductName,
                                   ProductCode = obj.ProductCode,
                                   Category = obj.Category,
                                   Mark = obj.Mark,
                                   Unit = obj.Unit,
                                   Quantity = LimitedAmount,
                                   Price = Price,
                                   SellingPrice = SellingPrice,
                                   Id = obj.Id,
                                   ImportProduct = ImportTime

                               };

                               if (waherhouse.Quantity == 0)
                               {
                                   MessageBox.Show("Miqdar düzgün qeyd edilməyib!!!");
                                   return;
                               }
                               Amount = LimitedAmount * Price;
                               LimitedAmount = 0;
                               Price = 0;
                               SellingPrice = 0;
                               ProductName = string.Empty;
                               EnabledButton = false;
                               _listOfProduct.Add(waherhouse);
                               foreach (var item in ListOfProduct)
                               {
                                   amount += item.Amount;
                               }

                               AmountLabel = amount;
                           }

                       }
                       catch (Exception)
                       {
                           MessageBox.Show("Melumat tam yazilmayib!!!");
                       }
                   }
                   ));

        }
        private RelayCommand _confirm;
        public RelayCommand Confirm => _confirm ?? (_confirm = new RelayCommand(
            x =>
            {
                if (MessageBox.Show("Məlumat Təsdiqlənsin?", "Təsdiqlə", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (DataContex db = new DataContex())
                    {
                        foreach (var item in _listOfProduct)
                        {

                            var obj = db.Waherhouses.Where(q => q.ProductCode == item.ProductCode).FirstOrDefault<Waherhouse>();
                            if (obj != null)
                            {
                                obj.Quantity += item.Quantity;
                                db.Entry(obj).State = EntityState.Modified;
                            }
                            else
                            {
                                Waherhouse waherhouse = new Waherhouse()
                                {
                                    ProductName = item.ProductName,
                                    ProductCode = item.ProductCode,
                                    Category = item.Category,
                                    Mark = item.Mark,
                                    Unit = item.Unit,
                                    Quantity = item.Quantity,
                                    Price = item.Price,
                                    SellingPrice = item.SellingPrice,
                                    Id = item.Id,
                                    ImportProduct = item.ImportProduct

                                };
                                db.Waherhouses.Add(waherhouse);
                            }
                        }
                        db.SaveChanges();
                    }
                    _listOfProduct.Clear();
                    AmountLabel = 0;
                    navigationService.NavigateTo(ViewType.Tacir_Info);
                }
            }
        ));
        private RelayCommand _updateProduct;
        public RelayCommand UpdateProduct => _updateProduct ?? (_updateProduct = new RelayCommand(

            x =>
            {
                try
                {
                    if (SelectItem == null)
                    {
                        MessageBox.Show("Məlumat seçin!");
                        return;
                    }
                    else
                    {
                        ServiceManager.GetService<IViewService>().OpenDialog(new EditWaherHouseViewModel(new Waherhouse
                        {
                            Id = SelectItem.Id,
                            Category = SelectItem.Category,
                            ProductCode = SelectItem.ProductCode,
                            ProductName = SelectItem.ProductName,
                            Mark = SelectItem.Mark,
                            Unit = SelectItem.Unit,
                            Quantity = SelectItem.Quantity,
                            Price = SelectItem.Price,
                            SellingPrice = SelectItem.SellingPrice,
                            ImportProduct = SelectItem.ImportProduct

                        }));
                        //_listOfProduct.Clear();
                        //    using (var context = new DataContex())
                        //    {
                        //        foreach (var item in context.Waherhouses)
                        //        {

                        //            _listOfProduct.Add(item);
                        //        }
                        //        decimal amount = 0;
                        //        foreach (var item in context.Waherhouses.ToList())
                        //        {
                        //            _listOfProduct.Add(item);
                        //            amount += item.Amount;
                        //        }
                        //        AmountLabel = amount;
                        //    }

                        //}
                        //_listOfProduct.Clear();
                        //using (DataContex DGrid = new DataContex())
                        //{

                        //    foreach (var item in DGrid.Waherhouses)
                        //    {
                        //        _listOfProduct.Add(item);
                        //    }
                        //}
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
           ));

        private RelayCommand deleteRow;
        public RelayCommand DeleteRow
        {
            get
            {
                return deleteRow ?? (deleteRow = new RelayCommand(

                   x =>
                   {
                       try
                       {

                           using (DataContex dc = new DataContex())
                           {
                               if (SelectItem == null)
                               {
                                   MessageBox.Show("Məlumat seçin!");
                                   return;
                               }
                               else
                               {
                                   if (MessageBox.Show("Məlumat silinsin?", "Sil", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                   {

                                       var C_Id = (SelectItem as Waherhouse).Id;
                                       Waherhouse ProdP = (from r in ListOfProduct where r.Id == C_Id select r).SingleOrDefault();

                                       ListOfProduct.Remove(ProdP);
                                       //dc.SaveChanges();
                                       _listOfProduct.Clear();
                                       using (DataContex data = new DataContex())
                                       {
                                           decimal amount = 0;
                                           foreach (var item in ListOfProduct)
                                           {
                                               amount += item.Amount;
                                           }

                                           AmountLabel = amount;
                                       }
                                   }
                                   else
                                   {
                                       SelectItem = null;
                                       return;
                                   }
                               }
                           }
                       }
                       catch (Exception ex)
                       {
                           MessageBox.Show(ex.Message);
                       }
                   }
                   ));
            }
        }

        private RelayCommand exitcommand;
        public RelayCommand Exitcommand
        {
            get
            {
                return exitcommand ?? (exitcommand = new RelayCommand(

                   (x =>
                   {
                       ProductName = null;
                       Price = 0;
                       SellingPrice = 0;
                       AmountLabel = 0;
                       ListOfProduct.Clear();
                       navigationService.NavigateTo(ViewType.Tacir_Info);
                   }
                   ))
                 );

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
