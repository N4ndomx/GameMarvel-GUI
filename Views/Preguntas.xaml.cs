using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ControllersKinect.service;
using ControllersKinect.Views;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;

namespace ControllersKinect
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class Preguntas : Window
    {
        KinectSensorChooser sensorChooser;
        
        private string question;
        private string characteristic;
        List<String> characteristic_yes = new List<String>();
        List<String> characteristic_no = new List<String>();
        
        public Preguntas(KinectSensorChooser sesor)
        {
            InitializeComponent();
            sensorChooser = sesor;
            CaracteristicaAleatoria();
        }
        public void pregunta(string characteristic)
        {
            question = "Es " + characteristic.Replace("_", " ") + "?";
            Console.WriteLine(question);
            lbPregunta.Content = question;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            sensorChooser.KinectChanged += SensorChooser_KinectChanged;
            kinectSensorUI.KinectSensorChooser = sensorChooser;
            sensorChooser.Stop();
            sensorChooser.Start();

        }

        private void SensorChooser_KinectChanged(object sender, KinectChangedEventArgs e)
        {
            bool err = true;
            if (e.OldSensor == null)
            {
                try
                {
                    e.OldSensor.DepthStream.Disable();
                    e.OldSensor.SkeletonStream.Disable();
                }
                catch (Exception)
                {
                    err = false;
                }
            }
            if (e.NewSensor == null)
            {
                return;
            }
            try
            {
                e.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                e.NewSensor.SkeletonStream.Enable();
                try
                {
                    e.NewSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
                    e.NewSensor.DepthStream.Range = DepthRange.Near;
                    e.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
                }
                catch (InvalidOperationException)
                {
                    e.NewSensor.DepthStream.Range = DepthRange.Default;
                    e.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                }
            }
            catch (InvalidOperationException)
            {
                err = true;
            }
            regionKt.KinectSensor = e.NewSensor;
        }

        async void CaracteristicaAleatoria()
        {
            try
            {
                characteristic = await ApiConec.getCharacterApi();
                pregunta(characteristic);
               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error: " + ex.Message);
            }
        }
        async void PostData()
        {
            try
            {
                string list_yes = string.Join(", ", characteristic_yes);
                string list_no = string.Join(", ", characteristic_no);

                // Objeto JSON que deseas enviar en la solicitud POST
                var data = new { buscar = list_yes, arrayNo = list_no };

                // Realizar la solicitud POST y obtener la respuesta
                string response = await ApiConec.PostJson(data);

                // Hacer algo con la respuesta
                Console.WriteLine(response);

                if (!response.Contains("{") && !response.Contains("}"))
                {
                    string n = Regex.Replace(response, "(?<!_)\"", string.Empty);
                    pregunta(n);
                    characteristic = n;
                     
                }
                else
                {
                    pregunta("YAAA");
                    Resultado r = new Resultado(response);
                    r.Show();
                    this.Close();

                }

            }

            catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error: " + ex.Message);
            }
        }

        private void Click_SI(object sender, RoutedEventArgs e)
        {
            characteristic_no.Add(characteristic);
            characteristic_yes.Add(characteristic);
            PostData();
            

        }

        private void Click_NO(object sender, RoutedEventArgs e)
        {
            question = "";
            characteristic_no.Add(characteristic);
            //CaracteristicaAleatoria();
            PostData();
           // characteristic_yes.Add(characteristic);

        }
    }
}
