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
    public class İşçilərin_siyahısiViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly INavigationService navigationService;
        public İşçilərin_siyahısiViewModel(INavigationService navigation)
        {
            this.navigationService = navigation;
            using (DataContex DGrid = new DataContex())
            {
                employees = new ObservableCollection<Employee>(DGrid.Employees);

            }
        }

        private ObservableCollection<Employee> employees;
        public ObservableCollection<Employee> DG_Employees
        {
            get { return employees; }
            set
            {
                employees = value;
                OnPropertyChanged();
            }
        }

        public İşçilərin_siyahısiViewModel()
        {
           

        }

        private RelayCommand _addEmployee;
        public RelayCommand AddEmployee
        {
            get
            {
                return _addEmployee ?? (_addEmployee = new RelayCommand(

                   (x =>
                   {

                       ServiceManager.GetService<IViewService>().OpenDialog(new Yeni_işçinin_əlavə_edilməsiViewModel());
                       DG_Employees.Clear();
                       using (DataContex DGrid = new DataContex())
                       {

                           foreach (var item in DGrid.Employees)
                           {
                               employees.Add(item);
                           }
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

                               var C_Id = (SelectItem as Employee).Id;
                               Employee employee = (from r in dc.Employees where r.Id == C_Id select r).SingleOrDefault();
                               dc.Employees.Remove(employee);
                               dc.Positions.First(e => e.PositionName.Equals(SelectItem.Position)).CountEmployees--;
                               dc.SaveChanges();
                               employees.Clear();
                               using (DataContex DGrid = new DataContex())
                               {

                                   foreach (var item in DGrid.Employees)
                                   {
                                       employees.Add(item);
                                   }
                               }
                           }
                           else
                           {
                               employees.Clear();
                               using (DataContex DGrid = new DataContex())
                               {

                                   foreach (var item in DGrid.Employees)
                                   {
                                       employees.Add(item);
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
        private bool visiable=true ;
        public bool Visiable { get => visiable; set { visiable = value; OnPropertyChanged(); } }
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
                               ServiceManager.GetService<IViewService>().OpenDialog(new İşçilərin_redaktəsiViewModel(new Employee
                               {
                                   Id = SelectItem.Id,
                                   Name = SelectItem.Name,
                                   FatherName = SelectItem.FatherName,
                                   Surname = SelectItem.Surname,
                                   Salary = SelectItem.Salary,
                                   Birthday = SelectItem.Birthday,
                                   Position = SelectItem.Position,
                                   StartDay = SelectItem.StartDay
                               }));
                             
                               employees.Clear();
                               using (var context = new DataContex())
                               {
                                   foreach (var item in context.Employees)
                                   {

                                     employees.Add(item);
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
        private RelayCommand clearTable;
        public RelayCommand ClearTable => clearTable ?? (clearTable = new RelayCommand(

           x =>
           {
               if (MessageBox.Show("Cədvəl tamamilə silinsin?", "Təmizlə", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
               {
                   using (var context = new DataContex())
                   {
                       context.Database.ExecuteSqlCommand("TRUNCATE TABLE [Employees]");
                       context.SaveChanges();
                       DG_Employees.Clear();
                   }

               }
               else
               {
                   return;
               }
           }
           ));
        private RelayCommand exitCommand;
        public RelayCommand ExitCommand => exitCommand ?? (exitCommand = new RelayCommand(
           x =>
           {
               navigationService.NavigateTo(ViewType.Tacir_Info);
           }
           ));

        public Employee SelectItem { get; set; }

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