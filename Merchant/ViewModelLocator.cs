using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Merchant.Navigation;
using Merchant.ViewModels;
using Merchant.Views;
using System;
using System.Windows;

namespace Merchant.ViewModel
{
    class ViewModelLocator
    {
       private NavigationService navigationService=new NavigationService() ;

        private IViewService viewService = ServiceManager.RegisterService<IViewService>(new ViewService());
     
        //TODO: Add VMs
        public AppViewModel AppViewModel;
        public Tacir_InfoViewModel TacirViewModel;
        public Təmənnasız_alışViewModel TəmənnasızViewModel;
        public ProductsNameViewModel ProductsNameViewModel;
        public İşçilərin_siyahısiViewModel İşçilərin_siyahısiViewModel;
        public Malin_AlinmasiViewModel Malin_AlinmasiViewModel;
        public Malin_SatilmasiViewModel Malin_SatilmasiViewModel;
        public ListPersonsViewModel ListPersonsViewModel;
        public UserRegisterViewModel UserRegisterViewModel;
        public UserLoginViewModel UserLoginViewModel;


        public ViewModelLocator()
        {

            AppViewModel = new AppViewModel();
            TacirViewModel = new Tacir_InfoViewModel(navigationService);
            TəmənnasızViewModel = new Təmənnasız_alışViewModel(navigationService);
            ProductsNameViewModel = new ProductsNameViewModel(navigationService);
            İşçilərin_siyahısiViewModel = new İşçilərin_siyahısiViewModel(navigationService);
            Malin_AlinmasiViewModel = new Malin_AlinmasiViewModel(navigationService);
            Malin_SatilmasiViewModel = new Malin_SatilmasiViewModel(navigationService);
            ListPersonsViewModel = new ListPersonsViewModel(navigationService);
            UserRegisterViewModel = new UserRegisterViewModel(navigationService);
            UserLoginViewModel = new UserLoginViewModel(navigationService);

            navigationService.AddPage(AppViewModel, ViewType.AppView);
            navigationService.AddPage(TacirViewModel, ViewType.Tacir_Info);
            navigationService.AddPage(TəmənnasızViewModel, ViewType.Təmənnasız_alış);
            navigationService.AddPage(ProductsNameViewModel, ViewType.ProductsName);
            navigationService.AddPage(İşçilərin_siyahısiViewModel, ViewType.İşçilərin_siyahısı);
            navigationService.AddPage(Malin_AlinmasiViewModel, ViewType.Malin_Alinmasi);
            navigationService.AddPage(Malin_SatilmasiViewModel, ViewType.Malin_Satilmasi);
            navigationService.AddPage(ListPersonsViewModel, ViewType.ListPersons);
            navigationService.AddPage(UserRegisterViewModel, ViewType.UserRegister);
            navigationService.AddPage(UserLoginViewModel, ViewType.UserLogin);



            navigationService.NavigateTo(ViewType.UserLogin);



            viewService.RegisterView(typeof(İşçilərin_redaktəsi), typeof(İşçilərin_redaktəsiViewModel));
            viewService.RegisterView(typeof(Kateqoriyalar), typeof(KateqoriyalarViewModel));
            viewService.RegisterView(typeof(Malın_seçilməsi), typeof(Malın_seçilməsiViewModel));
            viewService.RegisterView(typeof(Redaktə), typeof(RedaktəViewModel));
            viewService.RegisterView(typeof(Vəzifələr), typeof(VəzifələrViewModel));
            viewService.RegisterView(typeof(Vəzifələrin_redakrəsi), typeof(Vəzifələrin_redakrəsiViewModel));
            viewService.RegisterView(typeof(Xərc_adının_elavə_edilməsi), typeof(Xərc_adının_elavə_edilməsiViewModel));
            viewService.RegisterView(typeof(XərcAdları), typeof(XərcAdlarıViewModel));
            viewService.RegisterView(typeof(XərcRedaktə), typeof(XərcRedaktəViewModel));
            viewService.RegisterView(typeof(Y_M_redaktəsi), typeof(Y_M_redaktəsiViewModel));
            viewService.RegisterView(typeof(Yeni_işçinin_əlavə_edilməsi), typeof(Yeni_işçinin_əlavə_edilməsiViewModel));
            viewService.RegisterView(typeof(Yeni_kateqoriyanın_əlavə_edilməsi), typeof(Yeni_kateqoriyanın_əlavə_edilməsiViewModel));
            viewService.RegisterView(typeof(Yeni_mal_adının_əlavə_edilməsi), typeof(Yeni_mal_adının_əlavə_edilməsiViewModel));
            viewService.RegisterView(typeof(Yeni_vəzifənin_əlavə_edilməsi), typeof(Yeni_vəzifənin_əlavə_edilməsiViewModel));
            viewService.RegisterView(typeof(ProductsNameForMain), typeof(ProductsNameForMainViewModel));
            viewService.RegisterView(typeof(Calculator), typeof(CalculatorViewModel));
            viewService.RegisterView(typeof(AddNewPerson), typeof(AddNewPersonViewModel));
            viewService.RegisterView(typeof(EditPerson), typeof(EditPersonViewModel));
            viewService.RegisterView(typeof(EditWaherhouse), typeof(EditWaherHouseViewModel));
            


        }
    }
}