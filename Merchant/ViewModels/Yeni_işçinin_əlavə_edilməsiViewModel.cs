using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Merchant.Models;
using Merchant.Navigation;
using Merchant.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Merchant.ViewModels
{
    public class Yeni_işçinin_əlavə_edilməsiViewModel : ViewModelBase, INotifyPropertyChanged
    {


        public Yeni_işçinin_əlavə_edilməsiViewModel()
        {
            using (var ent = new DataContex())
            {
                var CodeFromCategory = ent.Positions;

                foreach (var item in CodeFromCategory)
                {
                    CB_Position.Add(item.PositionName);
                }
            }
        }
        private ObservableCollection<string> position = new ObservableCollection<string>();
        public ObservableCollection<string> CB_Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged();
            }
        }
        public string Named_TB { get; set; }
        public string Surname_TB { get; set; }
        public string FatherName_TB { get; set; }
        public DateTime Birthday_TB { get; set; } = DateTime.Now.Date;
        public string Position_TB { get; set; }
        public DateTime StartDay_TB { get; set; } = DateTime.Now.Date;
        public decimal Salary_TB { get; set; }
        private RelayCommand _addNewEmployee;
        public RelayCommand AddNewEmployee => _addNewEmployee ?? (_addNewEmployee = new RelayCommand(

           x =>
           {
               try
               {
                   using (DataContex dc = new DataContex())
                   {
                       if (Named_TB != null && Surname_TB != null && FatherName_TB != null &&
                       Birthday_TB != null && Position_TB != null && StartDay_TB != null && Salary_TB != null)
                       {
                           Employee list = new Employee()
                           {
                               Name = Named_TB,
                               Surname = Surname_TB,
                               FatherName = FatherName_TB,
                               Birthday = Birthday_TB,
                               Position = Position_TB,
                               StartDay = StartDay_TB,
                               Salary = Salary_TB

                           };
                           
                           dc.Employees.Add(list);
                           dc.Positions.First(e => e.PositionName.Equals(Position_TB)).CountEmployees++;
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