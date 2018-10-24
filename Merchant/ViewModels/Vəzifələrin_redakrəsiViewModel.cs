using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Merchant.Models;
using Merchant.Navigation;
using Merchant.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Merchant.ViewModels
{
    public class Vəzifələrin_redakrəsiViewModel : ViewModelBase, INotifyPropertyChanged
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
        private int _countEmployees;
        public int CountEmployees
        {
            get => _countEmployees;
            set
            {
                _countEmployees = value;
                OnPropertyChanged();
            }
        }
        private string _positionName;
        public string PositionName
        {
            get => _positionName;
            set
            {
                _positionName = value;
                OnPropertyChanged();
            }
        }
        public Vəzifələrin_redakrəsiViewModel(Position position)
        {
            id = position.Id;
            _countEmployees = position.CountEmployees;
            _positionName = position.PositionName;
        }

        private RelayCommand _addExpense;
        public RelayCommand AddExpense => _addExpense ?? (_addExpense = new RelayCommand(
           x =>
           {
               using (DataContex dc = new DataContex())
               {
                   if (MessageBox.Show("Məlumat yenilənsin?", "Yenilə", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                   {
                       Position  position = new Position();
                       position.Id = Id;
                       position.PositionName = PositionName;
                       position.CountEmployees = CountEmployees;
                       dc.Entry(position).State = EntityState.Modified;
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
