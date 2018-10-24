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
    class EditPersonViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private int id;
        public int Id { get => id; set { id = value; OnPropertyChanged(); } }


        private string fullName;
        public string FullName { get => fullName; set { fullName = value; OnPropertyChanged(); } }


        private string code;
        public string Code { get => code; set { code = value; OnPropertyChanged(); } }

        private bool juridicalPerson;
        public bool JuridicalPerson { get => juridicalPerson; set { juridicalPerson = value; OnPropertyChanged(); } }

        private bool individualPerson;
        public bool IndividualPerson { get => individualPerson; set { individualPerson = value; OnPropertyChanged(); } }

        private int phoneNumber;
        public int PhoneNumber { get => phoneNumber; set { phoneNumber = value; OnPropertyChanged(); } }

        private string address;
        public string Address { get => address; set { address = value; OnPropertyChanged(); } }

        private decimal loanLimited;
        public decimal LoanLimited { get => loanLimited; set { loanLimited = value; OnPropertyChanged(); } }

        private decimal discount;
        public decimal Discount { get => discount; set { discount = value; OnPropertyChanged(); } }


        private bool sale;
        public bool Sale { get => sale; set { sale = value; OnPropertyChanged(); } }

        private bool buy;
        public bool Buy { get => buy; set { buy = value; OnPropertyChanged(); } }

        private string note;
        public string Note { get => note; set { note = value; OnPropertyChanged(); } }

        private DateTime registrationTime;
        public DateTime RegistrationTime { get => registrationTime; set { registrationTime = value; OnPropertyChanged(); } }



        private string difference;
        public string Difference { get => difference; set { difference = value; OnPropertyChanged(); } }


        private bool activity;
        public bool Activity { get => activity; set { activity = value; OnPropertyChanged(); } }

        public EditPersonViewModel(Person person)
        {
            Activity = person.Activity;
            Address = person.Address;
            Buy = person.Buy;
            Code = person.Code;
            
            Discount = person.Discount;
            FullName = person.FullName;
            Id = person.Id;
            IndividualPerson = person.IndividualPerson;
            JuridicalPerson = person.JuridicalPerson;
            LoanLimited = person.LoanLimited;
            Note = person.Note;
            PhoneNumber = person.PhoneNumber;
            RegistrationTime = person.RegistrationTime;
            Sale = person.Sale;
        }

        private RelayCommand addUpdate;
        public RelayCommand AddUpdate
        {
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
                                    Person person = new Person();

                                    person.Activity = Activity;
                                    person.Address = Address;
                                    person.Buy = Buy;
                                    person.Code = Code;
                                    person.Discount = Discount;
                                    person.FullName = FullName;
                                    person.Id = Id;
                                    person.IndividualPerson = IndividualPerson;
                                    person.JuridicalPerson = JuridicalPerson;
                                    person.LoanLimited = LoanLimited;
                                    person.Note = Note;
                                    person.PhoneNumber = PhoneNumber;
                                    person.RegistrationTime = RegistrationTime;
                                    person.Sale= Sale;
                                    dc.Entry(person).State = EntityState.Modified;
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
