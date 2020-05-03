using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;

namespace SocialLogin
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var authenticator = new OAuth2Authenticator
                         (
                           //"198222079335-ecsj26g8tc9ve335m71h83m1ver882j0.apps.googleusercontent.com",
                           "51185825091-fgfs808gfouq54a4f25gh0b4hheuvh9d.apps.googleusercontent.com",
                           "email profile",
                            new System.Uri("https://accounts.google.com/o/oauth2/auth"),
                           new System.Uri("https://www.google.com")
                          //new System.Uri("https://www.systematic-solution.com/home")
                          );

            authenticator.AllowCancel = true;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
           
            presenter.Login(authenticator);

            authenticator.Completed += async (senders, obj) =>
            {
                if (obj.IsAuthenticated)
                {
                    var clientData = new HttpClient();

                    //call google api to fetch logged in user profile info 
                    var resData = await clientData.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo?access_token=" + obj.Account.Properties["access_token"]);
                    var jsonData = await resData.Content.ReadAsStringAsync();

                    // deserlize the jsondata and intilize in GoogleAuthClass
                    GoogleAuthClass googleObject = JsonConvert.DeserializeObject<GoogleAuthClass>(jsonData);

                    //you can access following property after login
                    string email = googleObject.email;
                    string photo = googleObject.picture;
                    string name = googleObject.name;
                    
                    lblemail.Text = email;
                    lblName.Text = name;
                }
                else
                {
                    //Authentication fail
                    // write the code to handle when auth failed
                }
            };
            authenticator.Error += onAuthError;
        }
        private void onAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            DisplayAlert("Google Authentication Error", e.Message, "OK");
        }
    }
}
