using GalaSoft.MvvmLight;
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
using System.Windows.Controls;

namespace Merchant.ViewModels
{
    class UserLoginViewModel : ViewModelBase, INotifyPropertyChanged
    {
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

        private readonly INavigationService navigationService;
        public UserLoginViewModel(INavigationService navigation)
        {
            this.navigationService = navigation;
        }

        private RelayCommand registerCommand;
        public RelayCommand RegisterCommand => registerCommand ?? (registerCommand = new RelayCommand(
           x =>
           {
               navigationService.NavigateTo(ViewType.UserRegister);
           }
           ));

        public string Email { get; set; }
        public string Password { get; set; }

        private RelayCommand loginCommand;
        public RelayCommand LoginCommand => loginCommand ?? (loginCommand = new RelayCommand(
            x =>
            {
                using (DataContex DGrid = new DataContex())
                {

                    foreach (var item in DGrid.User)
                    {
                        
                        if (item.Password == Password && item.Email == Email)
                        {
                        
                            navigationService.NavigateTo(ViewType.Tacir_Info);
                            User.StaticName = item.Name;
                        }
                    }
                    
                }
            }
            ));


        private RelayCommand passwordChanged;
        public RelayCommand PasswordChangedCommand => passwordChanged ?? (passwordChanged = new RelayCommand(
            x =>
            {
                Password = ((PasswordBox)x).Password;
            }
            ));
    }
}
