using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Merchant.Models;
using Merchant.Navigation;
using Merchant.Tools;
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
    class AddNewPersonViewModel : ViewModelBase, INotifyPropertyChanged
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

        private DateTime registrationTime = DateTime.Now.Date;
        public DateTime RegistrationTime { get => registrationTime; set { registrationTime = value; OnPropertyChanged(); } }



       


        private bool activity;
        public bool Activity { get => activity; set { activity = value; OnPropertyChanged(); } }



        public AddNewPersonViewModel()
        {
            using (DataContex dc=new DataContex())
            {
                var a = dc.People.OrderByDescending(x => x.Id).FirstOrDefault();
                if (a != null)
                {

                  Id = ++a.Id;
                }
            }
        }



        private RelayCommand _addNewPerson;
        public RelayCommand AddNewPerson => _addNewPerson ?? (_addNewPerson = new RelayCommand(

           x =>
           {
               try
               {
                   using (DataContex dc = new DataContex())
                   {
                       if (FullName != null && Code != null && JuridicalPerson != null &&
                       IndividualPerson != null && PhoneNumber != null && Address != null && LoanLimited != null && Discount != null
                       && Sale != null && Buy != null && Note != null && RegistrationTime != null  && Activity != null)
                       {
                           Person list = new Person()
                           {
                               FullName = FullName,
                               Activity = Activity,
                               Address = Address,
                               Buy = Buy,
                               Code = Code,
                               Discount = Discount,
                              
                               IndividualPerson = IndividualPerson,
                               JuridicalPerson = JuridicalPerson,
                               LoanLimited = LoanLimited,
                               Note = Note,
                               PhoneNumber = PhoneNumber,
                               RegistrationTime = RegistrationTime,
                               Sale = Sale
                           };

                           dc.People.Add(list);
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

        private RelayCommand _defaultCode;
        public RelayCommand DefaultCode => _defaultCode ?? (_defaultCode = new RelayCommand(
           x =>
           {
               Random random = new Random();
               int randomNumber;
               String Sb = "";
               for (int i = 0; i < 2; i++)
               {
                   randomNumber = random.Next(10, 99);
                   Sb += randomNumber;
               }
               randomNumber = random.Next(1, 9);
               Sb += randomNumber;
               Code = Sb;
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
