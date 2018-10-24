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
using System.Windows;

namespace Merchant.ViewModels
{

    public class Yeni_mal_adının_əlavə_edilməsiViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private ObservableCollection<string> Kateqoriya = new ObservableCollection<string>();
        public ObservableCollection<string> CB_Kateqoriya
        {
            get { return Kateqoriya; }
            set
            {
                Kateqoriya = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> Unit = new ObservableCollection<string>();
        public ObservableCollection<string> CB_Unit
        {
            get { return Unit; }
            set
            {
                Unit = value;
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
        public Yeni_mal_adının_əlavə_edilməsiViewModel()
        {
            using (var ent = new DataContex())
            {
                var CodeFromCategory = ent.Category;

                foreach (var item in CodeFromCategory)
                {
                    CB_Kateqoriya.Add(item.CatalogName);
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
        private string kod;
        public string Kod_T
        {
            get => kod;
            set
            {

                kod = value;
                OnPropertyChanged();

            }
        }
        private string barcode;
        public string Barcode_T
        {
            get => barcode;
            set
            {

                barcode = value;
                OnPropertyChanged();

            }
        }
        public string Kateq_Ad_TB { get; set; }
        public string ProductName_TB { get; set; }
        public string Mark_TB { get; set; }
        public string Unit_TB { get; set; }
        public decimal Price_TB { get; set; }
        public decimal SelPrice_TB { get; set; }
        public int LimiteAmount_TB { get; set; }

        private RelayCommand _addNewProductName;
        public RelayCommand AddNewProductName => _addNewProductName ?? (_addNewProductName = new RelayCommand(

           x =>
           {
               try
               {
                   using (DataContex dc = new DataContex())
                   {
                       if (Kateq_Ad_TB != null && Kod_T != null && Barcode_T != null && ProductName_TB != null &&
                            Unit_TB != null && Price_TB != null && SelPrice_TB != null & LimiteAmount_TB != null)
                       {
                           Products list = new Products()
                           {
                               Category = Kateq_Ad_TB,
                               ProductCode = Kod_T,
                               ProductName = ProductName_TB,
                               Mark = Mark_TB,
                               Unit = Unit_TB,
                               Barcode = Barcode_T,
                               Price = Price_TB,
                               SellingPrice = SelPrice_TB,
                               LimitedAmount = LimiteAmount_TB
                           };
                           dc.Product.Add(list);
                           dc.SaveChanges();
                       }
                       else
                       {
                           MessageBox.Show("Melumat tam doldurulmayib!!!");
                       }


                   }
               }
               catch (Exception ex)
               {
                   MessageBox.Show(ex.Message);
               }

               Messenger.Default.Send<RequestCloseMessage>(new RequestCloseMessage(this), this);
           }
           ));
        private RelayCommand _defaultCode;
        public RelayCommand DefaultCode => _defaultCode ?? (_defaultCode = new RelayCommand(
           x =>
           {
               using (var ent = new DataContex())
               {
                   var CodeFromProduct = ent.Product;
                   var num = 0;
                   foreach (var item in CodeFromProduct)
                   {
                       num = item.Id;
                   }
                   string outputValue = String.Format("{0:D5}", num + 1);
                   Kod_T = outputValue;
               }
           }
           ));
        private RelayCommand _defaultBarcode;
        public RelayCommand DefaultBarcode => _defaultBarcode ?? (_defaultBarcode = new RelayCommand(
           x =>
           {
               Random random = new Random();
               int randomNumber;
               String Sb = "";
               for (int i = 0; i < 2; i++)
               {
                   randomNumber = random.Next(10000, 99999);
                   Sb += randomNumber;
               }
               randomNumber = random.Next(100, 999);
               Sb += randomNumber;
               Barcode_T = Sb;
           }
           ));

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
