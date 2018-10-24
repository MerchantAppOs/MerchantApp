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
    public class XərcRedaktəViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
        private string categoryName;
        public string CategoryName
        {
            get => categoryName;
            set
            {
                categoryName = value;
                OnPropertyChanged();
            }
        }
        private string amountName;
        public string AmountName
        {
            get => amountName;
            set
            {
                amountName = value;
                OnPropertyChanged();
            }
        }
        private decimal amount;
        public decimal Amount
        {
            get => amount;
            set
            {
                amount = value;
                OnPropertyChanged();
            }
        }
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
        public XərcRedaktəViewModel(Costs costs)
        {
            id = costs.Id;
            categoryName = costs.Category;
            amount = costs.Amount;
            amountName = costs.AmountName;
            CB_Amount.Add(5);
            CB_Amount.Add(10);
            CB_Amount.Add(15);
            CB_Kateqoriya.Add("Ofis xərcləri");
            CB_Kateqoriya.Add("Əlavə xərclər");
        }

        private RelayCommand _addExpense;
        public RelayCommand AddExpense => _addExpense ?? (_addExpense = new RelayCommand(
           x =>
           {
               using (DataContex dc = new DataContex())
               {
                   if (MessageBox.Show("Məlumat yenilənsin?", "Yenilə", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                   {
                       Costs costs = new Costs();
                       costs.Id = Convert.ToInt32(Id);
                       costs.Amount = Amount;
                       costs.AmountName = AmountName;
                       costs.Category = CategoryName;
                       dc.Entry(costs).State = EntityState.Modified;
                       dc.SaveChanges();
                       Messenger.Default.Send<RequestCloseMessage>(new RequestCloseMessage(this), this);

                   }

                   else { return; }
               }
           }
           ));
        private RelayCommand _exitCommand;
        public RelayCommand ExitCommand => _exitCommand ?? (_exitCommand = new RelayCommand(
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
