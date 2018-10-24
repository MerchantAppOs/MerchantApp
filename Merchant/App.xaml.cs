using Merchant.ViewModel;
using Merchant.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Merchant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        //MainWindow app = new MainWindow();
        public App()
        {
            var viewModelLocator = new ViewModelLocator();
            var app = new AppView();
            app.DataContext = viewModelLocator.AppViewModel;
            app.ShowDialog();
        }
       

        
    }
}

