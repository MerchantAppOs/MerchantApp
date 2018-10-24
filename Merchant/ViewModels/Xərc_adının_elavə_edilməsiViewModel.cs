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
    public class Xərc_adının_elavə_edilməsiViewModel : ViewModelBase, INotifyPropertyChanged
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
        private ObservableCollection<int> _CB_Amount = new ObservableCollection<int>();
        public ObservableCollection<int> CB_Amount
        {
            get { return _CB_Amount; }
            set
            {
                _CB_Amount = value;
                OnPropertyChanged();
            }
        }
        public Xərc_adının_elavə_edilməsiViewModel()
        {

            CB_Kateqoriya.Add("Ofis xərcləri");
            CB_Kateqoriya.Add("Əlavə xərclər");
            CB_Amount.Add(5);
            CB_Amount.Add(10);
            CB_Amount.Add(15);
        }
        public string Category_TB { get; set; }
        public string AmountName_TB { get; set; }
        public decimal Amount_TB { get; set; }

        private RelayCommand _addNewCost;
        public RelayCommand AddNewCost => _addNewCost ?? (_addNewCost = new RelayCommand(

           x =>
           {
               try
               {
                   using (DataContex dc = new DataContex())
                   {
                       if (Category_TB!=null && AmountName_TB != null && Amount_TB!=null)
                       {
                           Costs list = new Costs()
                           {
                               Category = Category_TB,
                               AmountName = AmountName_TB,
                               Amount = Amount_TB,
                           };
                           dc.CostName.Add(list);
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
