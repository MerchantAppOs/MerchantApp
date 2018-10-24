using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Merchant.Models;
using Merchant.Navigation;
using Merchant.Tools;
using Merchant.Views;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Merchant.ViewModels
{

    public class Tacir_InfoViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly INavigationService navigationService;
        private decimal total;
        public decimal TotalAmount { get => total; set { total = value; OnPropertyChanged(); } }
        public Tacir_InfoViewModel(INavigationService navigation)
        {
            this.navigationService = navigation;
            NotificationLog();
        }
        private decimal refresh = 300;
        public decimal Refresh { get => refresh; set { refresh = value; OnPropertyChanged(); } }

        private RelayCommand refreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                return refreshCommand ?? (refreshCommand = new RelayCommand(

                   (x =>
                   {
                       using (DataContex dc = new DataContex())
                       {
                           var totalamount = dc.CashBoxes.OrderByDescending(p => p.Time).FirstOrDefault();
                           if (totalamount != null)
                           {
                               TotalAmount = totalamount.TotalAmount;
                           }
                           else
                           {
                               CashBox cashBox = new CashBox()
                               {
                                   Credit = 0,
                                   Debit = 0,
                                   Id = 0,
                                   Time = DateTime.Now,
                                   TotalAmount = 0
                               };
                               dc.CashBoxes.Add(cashBox);
                               dc.SaveChanges();
                           }
                       }

                       Refresh = TotalAmount + 250;
                   }
                   ))
                 );

            }
        }

        private RelayCommand listPositons;
        public RelayCommand ListPositons => listPositons ?? (listPositons = new RelayCommand(

           x =>
           {
               ServiceManager.GetService<IViewService>().OpenDialog(new VəzifələrViewModel());
           }
           ));

        private RelayCommand _listOfCategory;
        public RelayCommand ListOfCategory => _listOfCategory ?? (_listOfCategory = new RelayCommand(
           x =>
           {
               ServiceManager.GetService<IViewService>().OpenDialog(new KateqoriyalarViewModel());
           }
           ));
        private RelayCommand _nameOfProducts;
        public RelayCommand NameOfProducts => _nameOfProducts ?? (_nameOfProducts = new RelayCommand(
           x =>
           {
               ServiceManager.GetService<IViewService>().OpenDialog(new ProductsNameForMainViewModel());
           }
           ));

        private RelayCommand _listOfProducts;
        public RelayCommand ListOfProducts => _listOfProducts ?? (_listOfProducts = new RelayCommand(
           x =>
           {
               navigationService.NavigateTo(ViewType.ProductsName);
           }
           ));
        private RelayCommand _listOfAmount;
        public RelayCommand ListOfAmount => _listOfAmount ?? (_listOfAmount = new RelayCommand(
           x =>
           {
               ServiceManager.GetService<IViewService>().OpenDialog(new XərcAdlarıViewModel());
           }
           ));
        private RelayCommand _listOfEmployees;
        public RelayCommand ListOfEmployees => _listOfEmployees ?? (_listOfEmployees = new RelayCommand(
           x =>
           {
               navigationService.NavigateTo(ViewType.İşçilərin_siyahısı);
           }
           ));
        private RelayCommand import;
        public RelayCommand Import => import ?? (import = new RelayCommand(
           x =>
           {
               navigationService.NavigateTo(ViewType.Malin_Alinmasi);
           }
           ));
        private RelayCommand export;
        public RelayCommand Export => export ?? (export = new RelayCommand(
           x =>
           {
               navigationService.NavigateTo(ViewType.Malin_Satilmasi);
           }
           ));
        private RelayCommand _listOfPerson;
        public RelayCommand ListOfPerson => _listOfPerson ?? (_listOfPerson = new RelayCommand(
           x =>
           {
               navigationService.NavigateTo(ViewType.ListPersons);
           }
           ));
        public string notification;
        public string Notification { get => notification; set { notification = value; OnPropertyChanged(); } }

        public void NotificationLog()
        {
            using (DataContex dc = new DataContex())
            {
                var log = dc.LogEntries
                    .OrderByDescending(p => p.Date)
                    .FirstOrDefault();
                if (log != null)
                {

                    notification = log.Message;
                }
                var totalamount = dc.CashBoxes.OrderByDescending(p => p.Time).FirstOrDefault();
                if (totalamount != null)
                {
                    TotalAmount = totalamount.TotalAmount;
                }
                else
                {
                    CashBox cashBox = new CashBox()
                    {
                        Credit = 0,
                        Debit = 0,
                        Id = 0,
                        Time = DateTime.Now,
                        TotalAmount = 0
                    };
                    dc.CashBoxes.Add(cashBox);
                    dc.SaveChanges();
                }
            }
        }
        private RelayCommand calculator;
        public RelayCommand Calculator => calculator ?? (calculator = new RelayCommand(
           x =>
           {
               ServiceManager.GetService<IViewService>().OpenDialog(new CalculatorViewModel());
           }
           ));
        private RelayCommand _listOfDissatisfiedProduct;
        public RelayCommand ListOfDissatisfiedProduct => _listOfDissatisfiedProduct ?? (_listOfDissatisfiedProduct = new RelayCommand(
           x =>
           {
               navigationService.NavigateTo(ViewType.Təmənnasız_alış);
           }
           ));
        private RelayCommand exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                return exitCommand ?? (exitCommand = new RelayCommand(

                   (x =>
                   {
                       Environment.Exit(0);
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
