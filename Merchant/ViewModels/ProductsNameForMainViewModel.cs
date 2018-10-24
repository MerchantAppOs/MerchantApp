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
    class ProductsNameForMainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private ObservableCollection<Products> ProductName;
        public ObservableCollection<Products> DG_ProductName
        {
            get { return ProductName; }
            set
            {
                ProductName = value;
                OnPropertyChanged();
            }
        }



        public Products SelectItem { get; set; }
        public string SelectCategory { get; set; }

        public ProductsNameForMainViewModel()
        {
            using (DataContex DGrid = new DataContex())
            {
                ProductName = new ObservableCollection<Products>(DGrid.Product);
            }
        }


        private RelayCommand _addProductName;
        public RelayCommand AddProductName => _addProductName ?? (_addProductName = new RelayCommand(
           x =>
           {
               ServiceManager.GetService<IViewService>().OpenDialog(new Yeni_mal_adının_əlavə_edilməsiViewModel());
               DG_ProductName.Clear();
               using (DataContex DGrid = new DataContex())
               {

                   foreach (var item in DGrid.Product)
                   {
                       ProductName.Add(item);
                   }
               }
           }
           ));
        private RelayCommand _updateProduct;
        public RelayCommand UpdateProduct => _updateProduct ?? (_updateProduct = new RelayCommand(

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
                        ServiceManager.GetService<IViewService>().OpenDialog(new Y_M_redaktəsiViewModel(new Products
                        {
                            Id = SelectItem.Id,
                            Category = SelectItem.Category,
                            ProductCode = SelectItem.ProductCode,
                            ProductName = SelectItem.ProductName,
                            Mark = SelectItem.Mark,
                            Unit = SelectItem.Unit,
                            Barcode = SelectItem.Barcode,
                            Price = SelectItem.Price,
                            SellingPrice = SelectItem.SellingPrice,
                            LimitedAmount = SelectItem.LimitedAmount

                        }));
                        ProductName.Clear();
                        using (var context = new DataContex())
                        {
                            foreach (var item in context.Product)
                            {

                                ProductName.Add(item);
                            }
                        }

                    }
                    ProductName.Clear();
                    using (DataContex DGrid = new DataContex())
                    {

                        foreach (var item in DGrid.Product)
                        {
                            ProductName.Add(item);
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

                               var C_Id = (SelectItem as Products).Id;
                               Products product = (from r in dc.Product where r.Id == C_Id select r).SingleOrDefault();
                               dc.Product.Remove(product);
                               dc.SaveChanges();
                               ProductName.Clear();
                               using (DataContex DGrid = new DataContex())
                               {

                                   foreach (var item in DGrid.Product)
                                   {
                                       ProductName.Add(item);
                                   }
                               }
                           }
                           else
                           {
                               ProductName.Clear();
                               using (DataContex DGrid = new DataContex())
                               {

                                   foreach (var item in DGrid.Product)
                                   {
                                       ProductName.Add(item);
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
        private RelayCommand clearTable;
        public RelayCommand ClearTable => clearTable ?? (clearTable = new RelayCommand(

           x =>
           {
               if (MessageBox.Show("Cədvəl tamamilə silinsin?", "Təmizlə", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
               {
                   using (var context = new DataContex())
                   {
                       context.Database.ExecuteSqlCommand("TRUNCATE TABLE [Products]");
                       context.SaveChanges();
                       DG_ProductName.Clear();
                   }

               }
               else
               {
                   return;
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
