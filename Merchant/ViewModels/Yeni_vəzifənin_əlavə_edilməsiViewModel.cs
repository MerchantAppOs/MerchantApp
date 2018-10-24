using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Merchant.Models;
using Merchant.Navigation;
using Merchant.Tools;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Merchant.ViewModels
{
    public class Yeni_vəzifənin_əlavə_edilməsiViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public Yeni_vəzifənin_əlavə_edilməsiViewModel()
        {

        }
        public string PositionName_TB { get; set; }
        public int CountEmployee_L { get; set; }
        private RelayCommand _addNewPosition;
        public RelayCommand AddNewPosition => _addNewPosition ?? (_addNewPosition = new RelayCommand(

           x =>
           {
               try
               {
                 
                   using (DataContex dc = new DataContex())
                   {
                       if (PositionName_TB != null)
                       {
                           Position list = new Position()
                           {
                               PositionName = PositionName_TB,
                               CountEmployees = CountEmployee_L
                           };

                           
                           dc.Positions.Add(list);
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