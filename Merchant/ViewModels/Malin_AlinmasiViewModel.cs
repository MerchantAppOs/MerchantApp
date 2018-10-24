using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Merchant.Models;
using Merchant.Navigation;
using Merchant.Tools;
using NLog;
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
    class Malin_AlinmasiViewModel : ViewModelBase, INotifyPropertyChanged
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

        private decimal _paidamount;
        public decimal Paidamount { get => _paidamount; set { _paidamount = value; OnPropertyChanged(); } }

        private decimal _compromise;
        public decimal Compromise { get => _compromise; set { _compromise = value; OnPropertyChanged(); } }

        private string _note;
        public string Note { get => _note; set { _note = value; OnPropertyChanged(); } }

        public DateTime ImportTime { get; set; } = DateTime.Now.Date;

        public ObservableCollection<string> _personList = new ObservableCollection<string>();
        public ObservableCollection<string> PersonList
        {
            get { return _personList; }
            set
            {
                _personList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Purchase> _listOfProduct = new ObservableCollection<Purchase>();
        public ObservableCollection<Purchase> ListOfProduct
        {
            get { return _listOfProduct; }
            set
            {
                _listOfProduct = value;
                OnPropertyChanged();
            }
        }

        public Malin_AlinmasiViewModel()
        {

        }
        private readonly INavigationService navigationService;
        public Malin_AlinmasiViewModel(INavigationService navigation)
        {
            this.navigationService = navigation;
            Messenger.Default.Register<Products>(this, x =>
            {
                EnabledButton = true;
                ProductName = x.ProductName;
                Price = x.Price;
                SellingPrice = x.SellingPrice;
                using (DataContex db = new DataContex())
                {
                    var obj = db.Waherhouses.Where(q => q.ProductCode == x.ProductCode).FirstOrDefault<Waherhouse>();
                    if (obj != null)
                    {
                        Residual = obj.Quantity.ToString();
                    }
                    else
                    {
                        Residual = "0";
                    }
                }

            });

        }
        private string _residual;
        public string Residual { get => _residual; set { _residual = value; OnPropertyChanged(); } }

        public decimal _debt;
        public decimal Debt
        {
            get => _debt;
            set
            {
                _debt = value;
                OnPropertyChanged();
            }
        }
        public string SelectItem { get; set; } = "Negd Alis";

        private RelayCommand selectCommand;
        public RelayCommand SelectCommand => selectCommand ?? (selectCommand = new RelayCommand(
           x =>
           {
               using (DataContex db = new DataContex())
               {
                   var obj = db.People.Where(q => q.FullName.Equals(SelectItem)).FirstOrDefault<Person>();
                   if (obj != null)
                   {
                       Debt = obj.DebtFromUs;
                   }
                   else
                   {
                       Debt = 0;
                   }
               }
           }
           ));

        decimal amount = 0;
        decimal save = 0;
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
        private decimal _saveLabel;
        public decimal SaveLabel
        {
            get => _saveLabel;
            set
            {
                _saveLabel = value;
                OnPropertyChanged();
            }
        }

        public bool _enabledButton;
        public bool EnabledButton { get => _enabledButton; set { _enabledButton = value; OnPropertyChanged(); } }
        //
        private RelayCommand _selectPerson;
        public RelayCommand SelectPerson => _selectPerson ?? (_selectPerson = new RelayCommand(
           x =>
           {
               _personList.Clear();
               _personList.Add("Negd Alis");
               using (DataContex db = new DataContex())
               {
                   foreach (var item in db.People)
                   {
                       _personList.Add(item.FullName);
                       ;
                   }
               }

           }
           ));

        private RelayCommand _personsCommand;
        public RelayCommand PersonsCommand => _personsCommand ?? (_personsCommand = new RelayCommand(
           x =>
           {

               navigationService.NavigateTo(ViewType.ListPersons);

           }
           ));
        private RelayCommand _addPersonCommand;
        public RelayCommand AddPersonCommand => _addPersonCommand ?? (_addPersonCommand = new RelayCommand(
           x =>
           {
               _personList.Clear();
               ServiceManager.GetService<IViewService>().OpenDialog(new AddNewPersonViewModel());
               _personList.Add("Negd Alis");
               using (DataContex db = new DataContex())
               {
                   foreach (var item in db.People)
                   {
                       _personList.Add(item.FullName);
                       ;
                   }
               }

           }
           ));

        private RelayCommand _chooseProduct;
        public RelayCommand ChooseProduct => _chooseProduct ?? (_chooseProduct = new RelayCommand(
           x =>
           {

               ServiceManager.GetService<IViewService>().OpenDialog(new Malın_seçilməsiViewModel(this));
           }
           ));

        private RelayCommand _addNewProduct;
        public RelayCommand AddNewProduct => _addNewProduct ?? (_addNewProduct = new RelayCommand(
           x =>
           {

               ServiceManager.GetService<IViewService>().OpenDialog(new Yeni_mal_adının_əlavə_edilməsiViewModel());

           }
           ));
        private RelayCommand _confirmCommand;
        public RelayCommand ConfirmCommand => _confirmCommand ?? (_confirmCommand = new RelayCommand(
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
                       var person = db.People.Where(q => q.FullName.Equals(SelectItem)).FirstOrDefault<Person>();
                       if (person != null)
                       {
                           person.DebtFromUs += Amount - Paidamount - Compromise;
                           db.Entry(person).State = EntityState.Modified;
                       }

                       decimal total = 0;
                       total = Paidamount- Compromise;
                       foreach (var item in db.CashBoxes)
                       {
                           total -= item.Debit;
                           total += item.Credit;
                       }
                       CashBox cashBox = new CashBox()
                       {
                           Time = DateTime.Now,
                           Credit = Paidamount - Compromise,
                           TotalAmount = -total
                       };
                       db.CashBoxes.Add(cashBox);
                       db.SaveChanges();

                   }
                   var logger = LogManager.GetLogger(User.StaticName);
                   logger.Info("------");
                   SelectItem = "Negd Alis";
                   AmountLabel = 0;
                   SellingPrice = 0;
                   Price = 0;
                   ProductName = string.Empty;
                   SaveLabel = 0;
                   Amount = 0;
                   Debt = 0;
                   Residual = "0";
                   Compromise = 0;
                   LimitedAmount = 0;
                   amount = 0;
                   save = 0;
                   Paidamount = 0;
                   _listOfProduct.Clear();
                   navigationService.NavigateTo(ViewType.Tacir_Info);
               }


           }
           ));

        private RelayCommand _addButtonCommand;
        public RelayCommand AddButtonCommand
        {
            get => _addButtonCommand ?? (_addButtonCommand = new RelayCommand(
                   x =>
                   {
                       try
                       {

                           using (DataContex dc = new DataContex())
                           {

                               var obj = dc.Product.Where(q => q.ProductName.Equals(ProductName)).FirstOrDefault<Products>();
                               Purchase purchase = new Purchase()
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
                                   ImportProduct = ImportTime,
                                   Amount = LimitedAmount * Price - Compromise,
                                   Save = (LimitedAmount * SellingPrice) - (LimitedAmount * Price) + Compromise
                               };

                               if (purchase.Quantity == 0)
                               {
                                   MessageBox.Show("Miqdar düzgün qeyd edilməyib!!!");
                                   return;
                               }
                               Amount = LimitedAmount * Price;

                               if (SelectItem.Equals("Negd Alis"))
                               {

                                   Paidamount = purchase.Amount;
                               }

                               var totalamount = dc.CashBoxes.OrderByDescending(p => p.Time).FirstOrDefault();

                               if (totalamount != null && Paidamount > totalamount.TotalAmount)
                               {
                                   MessageBox.Show($"Kassada o qeder mebleg yoxdur! sizin balansiniz: {totalamount.TotalAmount}");
                                   return;
                               }
                               else
                               {
                                   _listOfProduct.Add(purchase);
                                   amount += Amount;
                                   save += (LimitedAmount * SellingPrice) - (LimitedAmount * Price) + Compromise;
                                   LimitedAmount = 0;
                                   Price = 0;
                                   SellingPrice = 0;
                                   ProductName = string.Empty;
                                   EnabledButton = false;
                                   AmountLabel = amount;
                                   SaveLabel = save;
                               }


                           }

                       }
                       catch (Exception)
                       {
                           MessageBox.Show("Melumat tam yazilmayib!!!");
                       }
                   }
                   ));

        }


        private RelayCommand backCommand;
        public RelayCommand BackCommand => backCommand ?? (backCommand = new RelayCommand(
           x =>
           {

               AmountLabel = 0;
               SellingPrice = 0;
               Price = 0;
               ProductName = string.Empty;
               SaveLabel = 0;
               Amount = 0;
               Debt = 0;
               Residual = "0";
               Compromise = 0;
               LimitedAmount = 0;
               amount = 0;
               save = 0;
               Paidamount = 0;
               _listOfProduct.Clear();
               navigationService.NavigateTo(ViewType.Tacir_Info);

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
