using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Merchant.Models;
using Merchant.Navigation;
using Merchant.Tools;
using Merchant.Views;
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
using System.Windows.Data;

namespace Merchant.ViewModels
{
    public class İşçilərin_redaktəsiViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private int id;
        public int Id { get => id; set { id = value;OnPropertyChanged(); } }
        private string name;
        public string Name { get => name; set { name = value;OnPropertyChanged(); } }
        private string surname;
        public string Surname { get => surname; set { surname = value; OnPropertyChanged(); } }
        private string fatherName;
        public string FatherName { get => fatherName; set { fatherName = value; OnPropertyChanged(); } }
        private decimal salary;
        public decimal Salary { get => salary; set { salary = value; OnPropertyChanged(); } }
        private DateTime birthday;
        public DateTime Birthday { get => birthday; set { birthday = value; OnPropertyChanged(); } }
        private string position;
        public string Position { get => position; set { position = value; OnPropertyChanged(); } }
        private DateTime startDay;
        public DateTime StartDay { get => startDay; set { startDay = value; OnPropertyChanged(); } }
        private ObservableCollection<string> positions = new ObservableCollection<string>();
        public ObservableCollection<string> CB_Positions
        {
            get { return positions; }
            set
            {
                positions = value;
                OnPropertyChanged();
            }
        }
        public İşçilərin_redaktəsiViewModel()
        {
            using (var ent = new DataContex())
            {
                var CodeFromCategory = ent.Positions;

                foreach (var item in CodeFromCategory)
                {
                    CB_Positions.Add(item.PositionName);
                }

            }
        }
        public İşçilərin_redaktəsiViewModel(Employee x)
        {
           
                Id = x.Id;
                Name = x.Name;
                Surname = x.Surname;
                FatherName = x.FatherName;
                Salary = x.Salary;
                Birthday = x.Birthday;
                Position = x.Position;
                StartDay = x.StartDay;
            
        }
        private RelayCommand addUpdate;
        public  RelayCommand AddUpdate {
            get 
            {
                return addUpdate ?? (addUpdate = new RelayCommand(
                    x =>
                    {
                        try
                        {
                            using (DataContex dc = new DataContex())
                            {
                                if (MessageBox.Show("Məlumat yenilənsin?", "Yenilə", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                {
                                    Employee emp = new Employee();
                                    emp.Id = Convert.ToInt32(Id);
                                    emp.Surname = Surname;
                                    emp.Name = Name;
                                    emp.FatherName = FatherName;
                                    emp.Birthday = Birthday;
                                    emp.Position = Position;
                                    emp.StartDay = StartDay;
                                    emp.Salary = Convert.ToDecimal(Salary);
                                    dc.Entry(emp).State = EntityState.Modified;
                                    dc.SaveChanges();
                                    Messenger.Default.Send<RequestCloseMessage>(new RequestCloseMessage(this), this);
                                    
                                }

                                else { return; }
                            }
                        }
                        catch (Exception)
                        {

                            MessageBox.Show("basma");
                        }
                       
                       

                    }));
            }
        }
        private RelayCommand exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                return exitCommand ?? (exitCommand = new RelayCommand(
                    x =>
                    {

                        Messenger.Default.Send<RequestCloseMessage>(new RequestCloseMessage(this), this);
                        


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
