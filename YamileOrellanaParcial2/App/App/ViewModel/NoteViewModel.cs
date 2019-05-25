using System;
using System.Collections.Generic;
using System.Text;

namespace App.ViewModel
{
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;
    using global::App.Services;
    using global::App.Models;

    public class NoteViewModel:BaseViewModel
    {
        #region Attributes
        string content;
        string notes;
        string modelNotes;
        bool isrunning;
        bool isenabled;
        ApiService ApiService;
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
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                SetValue(ref this.notes, value);    // esto nos sirve para que es una programacion mas compleja
            }
        }
        public string ModelNotes
        {
            get
            {
                return this.modelNotes;
            }
            set
            {
                SetValue(ref this.modelNotes, value);    // esto nos sirve para que es una programacion mas compleja
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
                await App.Current.MainPage.DisplayAlert("Content Empty",
                                                        "Please enter your Content",
                                                        "Accept");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var conexion = await this.ApiService.CheckConnection();
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

            Response rsp2 = await ApiService.Post<Response>(         
                  "https://notesplc.azurewebsites.net/", "api/","Notes");

            
            




           //MainViewModel mainViewModel = MainViewModel.GetInstance();
           // mainViewModel.Token = token.AccessToken;
            // mainViewModel.TokenType = token.TokenType;

           // Application.Current.MainPage = new NavigationPage(new ProductPage());
            IsRunning = true;
            IsEnabled = false;
        }
        #endregion

        public NoteViewModel()
        {

        }
    }
}

