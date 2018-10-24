
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
    public class Yeni_kateqoriyanın_əlavə_edilməsiViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private ObservableCollection<string> Kateqoriya = new ObservableCollection<string>();
        public ObservableCollection<string> CB_Kateqoriya
        {
            get { return Kateqoriya; }
            set
            {
                Kateqoriya = value;
                OnPropertyChanged();
            }
        } 

        public Yeni_kateqoriyanın_əlavə_edilməsiViewModel()
        {
            using (var ent = new DataContex())
            {
                var CodeFromCategory = ent.Category;

                foreach (var item in CodeFromCategory)
                {
                    CB_Kateqoriya.Add(item.CatalogName);
                }

            }
        }
        private string kod;
        public string Kod_T
        {
            get => kod;
            set
            {

                kod = value;
                OnPropertyChanged();

            }
        }

        public string Kateq_Ad_TB { get; set; } 
        private RelayCommand _addNewCategory;
        public RelayCommand AddNewCategory => _addNewCategory ?? (_addNewCategory = new RelayCommand(

           x =>
           {
               try
               {
                   using (DataContex dc = new DataContex())
                   {
                       
                       if (Kateq_Ad_TB != null && Kod_T!=null)
                       {
                           Kataloq list = new Kataloq()
                           {
                               
                               CatalogName = Kateq_Ad_TB,
                               Code = Kod_T

                           };
                           dc.Category.Add(list);
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
               using (var ent = new DataContex())
               {
                   var CodeFromCategory = ent.Category;
                   var num = 0;
                   foreach (var item in CodeFromCategory)
                   {
                       num = item.Id;
                   }
                   string outputValue = String.Format("{0:D4}", num + 1);
                   Kod_T = outputValue;
               }
           }
           ));

        private RelayCommand exitCommand;
        public RelayCommand ExitCommand => exitCommand ?? (exitCommand = new RelayCommand(
           x =>
           {
               Messenger.Default.Send<RequestCloseMessage>(new RequestCloseMessage(this), this);
           }
           ));
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
