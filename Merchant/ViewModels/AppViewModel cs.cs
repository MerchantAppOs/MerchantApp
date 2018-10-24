using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Merchant.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchant.ViewModels
{
    public class AppViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private ViewModelBase currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get { return currentViewModel; }
            set { Set(ref currentViewModel, value); }
        }

        public AppViewModel() => Messenger.Default.Register<ViewModelBase>(this,
                param => CurrentViewModel = param);
       

    }
}
