
namespace App.ViewModel
{
    //using GalaSoft.MvvmLight.Command;
    using System;
    using System.Windows.Input;
    using Services;
    using Xamarin.Forms;
    using global::App.Models;
    using global::App.Views;

    public class NoteViewModel:BaseViewModel
    {
        #region Attributes
        string content;
        bool isrunning;
        bool isenabled;
        ApiService apiService;
        #endregion

        #region Properties
        public string Content
        {
            get
            {
                return this.content;
            }
            set
            {
                SetValue(ref this.content, value);    // esto nos sirve para que es una programacion mas compleja
            }
        }
        
        public bool IsRunning
        {
            get
            {
                return this.isrunning;
            }
            set
            {
                SetValue(ref this.isrunning, value);
            }
        }
        public bool IsEnabled
        {
            get
            {
                return this.isenabled;
            }
            set
            {
                SetValue(ref this.isenabled, value);
            }
        }
        #endregion

        #region Commands
        public ICommand InsertarCommand
        {
            get
            {
                return new RelayCommand(Insertar);      //el cdmlogin nos sirve para pregunte si esta bien o falta algun dato
            }
        }

        private async void Insertar()
        {
            if (String.IsNullOrEmpty(Content))
            {
                await App.Current.MainPage.DisplayAlert("Email Empty",
                                                        "Please enter your email",
                                                        "Accept");
                return;
            }


            IsRunning = true;
            IsEnabled = false;

            var conexion = await this.apiService.CheckConnection();
            if (!conexion.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                   "ERROR",
                   conexion.Message,
                   "Accept");
                return;
            }

            TokenResponse token = await this.apiService.GetToken(           //copiamos todo esto de slack
                  "https://notesplc.azurewebsites.net",
                  this.Email,
                  this.Password);
            if (token == null)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                   "ERROR",
                   "Something was wrong, please try later.",
                   "Accept");
                return;
            }

            if (string.IsNullOrEmpty(token.AccessToken))
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                   "ERROR",
                   token.ErrorDescription,
                   "Accept");
                this.Password = String.Empty;

                return;
            }

            MainViewModel mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token.AccessToken;
            mainViewModel.TokenType = token.TokenType;

            Application.Current.MainPage = new NavigationPage(new ProductPage());
            IsRunning = true;
            IsEnabled = false;
        }
        #endregion

        public NoteViewModel()
        {

        }
    }
}