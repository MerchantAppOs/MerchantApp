
using GalaSoft.MvvmLight;
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
    class ListPersonsViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private ObservableCollection<Person> people;
        public ObservableCollection<Person> People
        {
            get { return people; }
            set
            {
                people = value;
                OnPropertyChanged();
            }
        }
        private readonly INavigationService navigationService;
        public ListPersonsViewModel(INavigationService navigation)
        {
            this.navigationService = navigation;
            using (DataContex DGrid = new DataContex())
            {
                people = new ObservableCollection<Person>(DGrid.People);

            }
        }
        private RelayCommand _addPerson;
        public RelayCommand AddPerson => _addPerson ?? (_addPerson = new RelayCommand(
           x =>
           {
               ServiceManager.GetService<IViewService>().OpenDialog(new AddNewPersonViewModel());
               People.Clear();
               using (DataContex DGrid = new DataContex())
               {

                   foreach (var item in DGrid.People)
                   {
                       people.Add(item);
                   }
               }
           }
           ));

        public Person SelectItem { get; set; }

        private RelayCommand updateData;
        public RelayCommand UpdateData
        {
            get
            {
                return updateData ?? (updateData = new RelayCommand(

                   (x =>
                   {
                       try
                       {
                           if (SelectItem != null)
                           {
                               ServiceManager.GetService<IViewService>().OpenDialog(new EditPersonViewModel(new Person
                               {
                                   Activity = SelectItem.Activity,
                                   Address = SelectItem.Address,
                                   Buy = SelectItem.Buy,
                                   Code = SelectItem.Code,
                                   Discount = SelectItem.Discount,
                                   FullName = SelectItem.FullName,
                                   Id = SelectItem.Id,
                                   IndividualPerson = SelectItem.IndividualPerson,
                                   JuridicalPerson = SelectItem.JuridicalPerson,
                                   LoanLimited = SelectItem.LoanLimited,
                                   Note = SelectItem.Note,
                                   PhoneNumber = SelectItem.PhoneNumber,
                                   RegistrationTime = SelectItem.RegistrationTime,
                                   Sale = SelectItem.Sale
                               }));

                               people.Clear();
                               using (var context = new DataContex())
                               {
                                   foreach (var item in context.People)
                                   {
                                       people.Add(item);
                                   }
                               }
                           }
                       }
                       catch (Exception)
                       {


                       }
                   }
                   ))
                 );

            }
        }

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

                               var C_Id = (SelectItem as Person).Id;
                               Person person = (from r in dc.People where r.Id == C_Id select r).SingleOrDefault();
                               dc.People.Remove(person);
                               dc.SaveChanges();
                               people.Clear();
                               using (DataContex DGrid = new DataContex())
                               {

                                   foreach (var item in DGrid.People)
                                   {
                                       people.Add(item);
                                   }
                               }
                           }
                           else
                           {
                               people.Clear();
                               using (DataContex DGrid = new DataContex())
                               {

                                   foreach (var item in DGrid.People)
                                   {
                                       people.Add(item);
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

        private RelayCommand clearTable;
        public RelayCommand ClearTable => clearTable ?? (clearTable = new RelayCommand(

           x =>
           {
               if (MessageBox.Show("Cədvəl tamamilə silinsin?", "Təmizlə", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
               {
                   using (var context = new DataContex())
                   {
                       context.Database.ExecuteSqlCommand("TRUNCATE TABLE [People]");
                       context.SaveChanges();
                       People.Clear();
                   }

               }
               else
               {
                   return;
               }
           }
           ));


        private RelayCommand exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                return exitCommand ?? (exitCommand = new RelayCommand(
                    x =>
                    {
                        navigationService.NavigateTo(ViewType.Tacir_Info);
                    }));
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
