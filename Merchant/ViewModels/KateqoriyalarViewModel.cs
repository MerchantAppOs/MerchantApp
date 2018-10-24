using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Merchant.Models;
using Merchant.Navigation;
using Merchant.Tools;
using Merchant.ViewModel;
using Merchant.Views;
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
    public class DataMessage
    {
        public int Id { get; set; }
    }

    public class KateqoriyalarViewModel : ViewModelBase, INotifyPropertyChanged
    {

        public Kataloq SelectItem { get; set; }
        private ObservableCollection<Kataloq> kataloq;
        public ObservableCollection<Kataloq> DG_Kataloq
        {
            get { return kataloq; }
            set
            {
                kataloq = value;
                OnPropertyChanged();
            }
        }
        public KateqoriyalarViewModel()
        {
            using (DataContex DGrid = new DataContex())
            {
                kataloq = new ObservableCollection<Kataloq>(DGrid.Category);

            }
        }
        private RelayCommand _addCategory;
        public RelayCommand AddCategory => _addCategory ?? (_addCategory = new RelayCommand(

           x =>
           {
               ServiceManager.GetService<IViewService>().OpenDialog(new Yeni_kateqoriyanın_əlavə_edilməsiViewModel());

               DG_Kataloq.Clear();
               using (DataContex DGrid = new DataContex())
               {

                   foreach (var item in DGrid.Category)
                   {
                       kataloq.Add(item);
                   }
               }

           }
           ));


        private RelayCommand _updateCategory;
        public RelayCommand UpdateCategory => _updateCategory ?? (_updateCategory = new RelayCommand(

            x =>
            {
                try
                {
                    if (SelectItem == null)
                    {
                        MessageBox.Show("Məlumat seçin!");
                        return;
                    }
                    else
                    {
                        ServiceManager.GetService<IViewService>().OpenDialog(new RedaktəViewModel(new Kataloq
                        {
                            Id = SelectItem.Id,
                            Code = SelectItem.Code,
                            CatalogName = SelectItem.CatalogName

                        }));
                       
                        kataloq.Clear();
                        using (var context = new DataContex())
                        {
                            foreach (var item in context.Category)
                            {

                                kataloq.Add(item);
                            }
                        }

                    }
                    kataloq.Clear();
                    using (DataContex DGrid = new DataContex())
                    {

                        foreach (var item in DGrid.Category)
                        {
                            kataloq.Add(item);
                        }
                    }
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
           ));
        private RelayCommand clearTable;
        public RelayCommand ClearTable => clearTable ?? (clearTable = new RelayCommand(

           x =>
           {
               if (MessageBox.Show("Cədvəl tamamilə silinsin?", "Təmizlə", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
               {
                   using (var context = new DataContex())
                   {
                       context.Database.ExecuteSqlCommand("TRUNCATE TABLE [Kataloqs]");
                       context.SaveChanges();
                       DG_Kataloq.Clear();
                   }
               }
               else
               {
                   return;
               }
           }
           ));

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

                               var C_Id = (SelectItem as Kataloq).Id;
                               Kataloq kataloqs = (from r in dc.Category where r.Id == C_Id select r).SingleOrDefault();
                               dc.Category.Remove(kataloqs);
                               dc.SaveChanges();
                               kataloq.Clear();
                               using (DataContex DGrid = new DataContex())
                               {

                                   foreach (var item in DGrid.Category)
                                   {
                                       kataloq.Add(item);
                                   }
                               }
                           }
                           else
                           {
                               kataloq.Clear();
                               using (DataContex DGrid = new DataContex())
                               {

                                   foreach (var item in DGrid.Category)
                                   {
                                       kataloq.Add(item);
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