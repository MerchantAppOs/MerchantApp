using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Merchant.Models;
using Merchant.Navigation;
using Merchant.Tools;
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

namespace Merchant.ViewModels
{
    public class RedaktəViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private int id;
        public int Id { get => id; set { id = value; OnPropertyChanged(); } }

        private string code;
        public string Cod { get => code; set { code = value; OnPropertyChanged(); } }

        private string catalog_Name;
        public string Catalog_Name { get => catalog_Name; set { catalog_Name = value; OnPropertyChanged(); } }

        public RedaktəViewModel(Kataloq x)
        {
            Id = x.Id;
            Cod = x.Code;
            Catalog_Name = x.CatalogName;


        }
        private RelayCommand addUpdate;
        public RelayCommand AddUpdate
        {
            get
            {
                return addUpdate ?? (addUpdate = new RelayCommand(
                    x =>
                    {
                        using (DataContex dc = new DataContex())
                        {
                            if (MessageBox.Show("Məlumat yenilənsin?", "Yenilə", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                Kataloq kataloq = new Kataloq();
                                kataloq.Id = Convert.ToInt32(Id);
                                kataloq.Code = Cod;
                                kataloq.CatalogName = Catalog_Name;
                                dc.Entry(kataloq).State = EntityState.Modified;
                                dc.SaveChanges();
                                Messenger.Default.Send<RequestCloseMessage>(new RequestCloseMessage(this), this);

                            }

                            else { return; }
                        }
                    }));

            }
            set => addUpdate = value;
        }
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