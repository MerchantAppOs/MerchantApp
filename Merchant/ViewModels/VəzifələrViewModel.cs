using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Merchant.Models;
using Merchant.Navigation;
using Merchant.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Merchant.ViewModels
{
    public class VəzifələrViewModel : ViewModelBase, INotifyPropertyChanged
    {
        //melumat silindikde id artmagda davam edir
        public Position SelectItem { get; set; }
        private ObservableCollection<Position> positions;
        public ObservableCollection<Position> DG_Position
        {
            get { return positions; }
            set
            {
                positions = value;
                OnPropertyChanged();
            }
        }
        private int _countEmp;
        public int CountEmp { get { return _countEmp; } set { _countEmp = value; OnPropertyChanged(); } }
        private int employee;
        public int Employee
        {
            get => employee;
            set
            {
                employee = value;
                OnPropertyChanged();
            }
        }
        public VəzifələrViewModel()
        {
            using (DataContex DGrid = new DataContex())
            {
                positions = new ObservableCollection<Position>(DGrid.Positions);

                CountEmp = DG_Position.Count;
                foreach (var item in DGrid.Employees)
                {
                    Employee += 1;
                }

            }

        }

        private RelayCommand _addPosition;
        public RelayCommand AddPosition => _addPosition ?? (_addPosition = new RelayCommand(
           x =>
           {
               ServiceManager.GetService<IViewService>().OpenDialog(new Yeni_vəzifənin_əlavə_edilməsiViewModel());

               DG_Position.Clear();
               using (DataContex DGrid = new DataContex())
               {

                   foreach (var item in DGrid.Positions)
                   {
                       positions.Add(item);
                   }
                   CountEmp = DG_Position.Count;
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

                               var C_Id = (SelectItem as Position).Id;
                               Position position = (from r in dc.Positions where r.Id == C_Id select r).SingleOrDefault();
                               dc.Positions.Remove(position);
                               dc.SaveChanges();
                               positions.Clear();
                               using (DataContex DGrid = new DataContex())
                               {

                                   foreach (var item in DGrid.Positions)
                                   {
                                       positions.Add(item);
                                   }
                                   CountEmp = DG_Position.Count;
                               }
                           }
                           else
                           {
                               positions.Clear();
                               using (DataContex DGrid = new DataContex())
                               {

                                   foreach (var item in DGrid.Positions)
                                   {
                                       positions.Add(item);
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
        private RelayCommand updateData;
        public RelayCommand UpdateData
        {
            get
            {
                return updateData ?? (updateData = new RelayCommand(

                   (x =>
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
                               using (DataContex dc = new DataContex())
                               {

                                   ServiceManager.GetService<IViewService>().OpenDialog(new Vəzifələrin_redakrəsiViewModel(new Position
                                   {
                                       Id = SelectItem.Id,
                                       CountEmployees = SelectItem.CountEmployees,
                                       PositionName = SelectItem.PositionName
                                   }));

                               }

                           }
                           positions.Clear();
                           using (DataContex DGrid = new DataContex())
                           {

                              
                               foreach (var item in DGrid.Positions)
                               {
                                   positions.Add(item);
                                   Employee = item.CountEmployees;
                               }
                           }
                           return;
                       }
                       catch (Exception ex)
                       {
                           MessageBox.Show(ex.Message);
                       }
                   }
                   ))
                 );

            }
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