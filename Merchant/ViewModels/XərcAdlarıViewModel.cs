using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Merchant.Models;
using Merchant.Navigation;
using Merchant.Tools;
using Merchant.Views;
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
    public class XərcAdlarıViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private ObservableCollection<Costs> Cost;
        public ObservableCollection<Costs> DG_Cost
        {
            get { return Cost; }
            set
            {
                Cost = value;
                OnPropertyChanged();
            }
        }
        public Costs SelectItem { get; set; }
        public XərcAdlarıViewModel()
        {
            using (DataContex DGrid = new DataContex())
            {
                Cost = new ObservableCollection<Costs>(DGrid.CostName);
            }
        }
        private RelayCommand _addExpense;
        public RelayCommand AddExpense => _addExpense ?? (_addExpense = new RelayCommand(
           x =>
           {
               ServiceManager.GetService<IViewService>().OpenDialog(new Xərc_adının_elavə_edilməsiViewModel());
               DG_Cost.Clear();
               using (DataContex DGrid = new DataContex())
               {
                   foreach (var item in DGrid.CostName)
                   {
                       Cost.Add(item);
                   }
               }
           }
           ));
        private RelayCommand deleteRow;
        public RelayCommand DeleteRow => deleteRow ?? (deleteRow = new RelayCommand(

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

                               var C_Id = (SelectItem as Costs).Id;
                               Costs cost = (from r in dc.CostName where r.Id == C_Id select r).SingleOrDefault();
                               dc.CostName.Remove(cost);
                               dc.SaveChanges();
                               Cost.Clear();
                               using (DataContex DGrid = new DataContex())
                               {

                                   foreach (var item in DGrid.CostName)
                                   {
                                       Cost.Add(item);
                                   }
                               }
                           }
                           else
                           {
                               Cost.Clear();
                               using (DataContex DGrid = new DataContex())
                               {

                                   foreach (var item in DGrid.CostName)
                                   {
                                       Cost.Add(item);
                                   }
                               }
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

        private RelayCommand _updateExpense;
        public RelayCommand UpdateExpense => _updateExpense ?? (_updateExpense = new RelayCommand(
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
                       using (DataContex dc = new DataContex())
                       {

                           ServiceManager.GetService<IViewService>().OpenDialog(new XərcRedaktəViewModel(new Costs {
                               Id = SelectItem.Id,
                               Category = SelectItem.Category,
                               Amount = SelectItem.Amount,
                               AmountName = SelectItem.AmountName
                           }));
                       }

                   }
                   Cost.Clear();
                   using (DataContex DGrid = new DataContex())
                   {

                       foreach (var item in DGrid.CostName)
                       {
                           Cost.Add(item);
                       }
                   }
                   return;
               }
               catch (Exception ex)
               {
                   MessageBox.Show(ex.Message);
               }
             
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

