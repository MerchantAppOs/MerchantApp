using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Merchant.Models;
using Merchant.Navigation;
using Merchant.Tools;

namespace Merchant.ViewModels
{
    class UserRegisterViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        public UserRegisterViewModel(INavigationService navigation)
        {
            this.navigationService = navigation;
        }

        private RelayCommand loginCommand;
        public RelayCommand LoginCommand => loginCommand ?? (loginCommand = new RelayCommand(
           x =>
           {
               navigationService.NavigateTo(ViewType.UserLogin);
           }
           ));

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }

        private RelayCommand addUser;
        public RelayCommand AddUser => addUser ?? (addUser = new RelayCommand(

           x =>
           {
               try
               {
                   string email = Email;
                   Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                   Match match = regex.Match(email);
                   if (match.Success)
                   {

                       using (DataContex dc = new DataContex())
                       {
                           if (Name != null && Surname != null && Email != null &&
                           Password != null && RePassword != null)
                           {
                               if (Password.Equals(RePassword))
                               {
                                   User list = new User()
                                   {
                                       Name = Name,
                                       Surname = Surname,
                                       Email = Email,
                                       Password = Password
                                   };

                                   dc.User.Add(list);
                                   dc.SaveChanges();
                                   navigationService.NavigateTo(ViewType.UserLogin);
                               }
                           }
                           else
                           {
                               MessageBox.Show("Melumat tam doldurulmayib!!!");
                           }


                       }
                   }
                   else
                   {
                       MessageBox.Show("Email duz deyil");

                   }
                  
               }
               catch (Exception ex)
               {
                   MessageBox.Show(ex.Message);
               }

               Messenger.Default.Send<RequestCloseMessage>(new RequestCloseMessage(this), this);
           }
           ));


        private RelayCommand passwordChanged;
        public RelayCommand PasswordChangedCommand => passwordChanged ?? (passwordChanged = new RelayCommand(
            x =>
            {
                Password = ((PasswordBox)x).Password;
            }
            ));

        private RelayCommand rePasswordChanged;
        public RelayCommand RePasswordChangedCommand => rePasswordChanged ?? (rePasswordChanged = new RelayCommand(
            x =>
            {
                RePassword = ((PasswordBox)x).Password;
            }
            ));

    }
}
