using Android.App;
using Android.Widget;
using Android.OS;

namespace PhoneApp
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            var botonValidador = FindViewById<Button>(Resource.Id.btValidar);
            botonValidador.Click += (object sender, System.EventArgs e) =>
            {
                try
                {
                    ValidateJC();
                }
                catch (System.Exception)
                {
                    var miResultado = FindViewById<TextView>(Resource.Id.lbValidateResult);
                    miResultado.Text = "Ocurrio un Error en APP";
                }
            };

        }

        private async void ValidateJC()
        {
            var miCorreo = FindViewById<EditText>(Resource.Id.txtEmail);
            var miPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            var miResultado = FindViewById<TextView>(Resource.Id.lbValidateResult);
            
            string StudentEmail = miCorreo.Text.Trim();
            string PasswordStudent = miPassword.Text.Trim();
            string resultadoFin = "";

            string myDevice = Android.Provider.Settings.Secure.GetString(
                    ContentResolver,
                    Android.Provider.Settings.Secure.AndroidId);

            var ServiceClient = new SALLab07.ServiceClient();
            var Result = await ServiceClient.ValidateAsync(StudentEmail, PasswordStudent, myDevice);
            resultadoFin  = $"{Result.Status}\n{Result.Fullname}\n{Result.Token}";   //resultadoFin = "prueba";
            miResultado.Text = "";

            if ((Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop))
            {
                var Builder = new Notification.Builder(this)
                    .SetContentTitle("Validacion de Actividad")
                    .SetContentText(resultadoFin)  //resultadoFin  
                    .SetSmallIcon(Resource.Drawable.Icon);

                Builder.SetCategory(Notification.CategoryMessage); 
                var ObjectNotification = Builder.Build();
                var Manager = GetSystemService(
                    Android.Content.Context.NotificationService) as NotificationManager;
                Manager.Notify(0, ObjectNotification);
            }
            else
            {
                miResultado.Text = resultadoFin;
            }
        }
    }
}

