using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect;
using ControllersKinect.modelos;
using Newtonsoft.Json;

namespace ControllersKinect.Views
{
    /// <summary>
    /// Lógica de interacción para Resultado.xaml
    /// </summary>
    public partial class Resultado : Window
    {
        KinectSensorChooser sensorChooser;
        BitmapImage imagen; Personaje respuestaObj;
        public Resultado(string data )
        {

            InitializeComponent();
            respuestaObj = JsonConvert.DeserializeObject<Personaje>(data); // Deserializar el JSON
            asignarDatos();
           
        }
        public void asignarDatos()
        {
            imagen = new BitmapImage(new Uri(respuestaObj.url_personaje));
            imgPersonaje.Source = imagen;
            txtDatos.Text = respuestaObj.descripcion_personaje;
            lblnombre.Content = respuestaObj.nombre_personaje.Replace("_", " ").ToUpper();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sensorChooser = new KinectSensorChooser();
            sensorChooser.KinectChanged += SensorChooser_KinectChanged;
            kinectSensorUI.KinectSensorChooser = sensorChooser;
            sensorChooser.Start();
        }

        private void SensorChooser_KinectChanged(object sender, KinectChangedEventArgs e)
        {
            //throw new NotImplementedException();
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

        private void KinectTileButton_Click(object sender, RoutedEventArgs e)
        {
            MainWin n = new MainWin();
            n.Show();
            this.Close();
        }
    }
}
