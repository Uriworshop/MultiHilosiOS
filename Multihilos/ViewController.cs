using System;
using CoreLocation;
using MapKit;
using System.Threading.Tasks;
using Foundation;
using System.IO;
using System.Net;
using CoreGraphics;
using UIKit;

namespace Multihilos
{
	public partial class ViewController : UIViewController
	{

		protected LoadingOverlay loadPop = null;
		public static LocationManager Localizacion { get; set; }
		MKCoordinateSpan RecibeAltura;
		MKCoordinateRegion RecibeRegion;
		string rutaback;
		string archivoBack = "foto2.jpg";

		protected ViewController(IntPtr handle) : base(handle)
		{
			Localizacion = new LocationManager();
			Localizacion.StartLocationUpdates();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			UIApplication.Notifications.ObserveDidBecomeActive
						 ((sender, args) =>
			{
				Localizacion.UbicacionActualizada += CambiodeUbicacion;
			});
			UIApplication.Notifications.ObserveDidEnterBackground
						 ((sender, args) =>
			{
				Localizacion.UbicacionActualizada -= CambiodeUbicacion;
			});
			Texto.Text = "";
			var Archivos = Directory.GetFiles
			 			(Environment.GetFolderPath
						 (Environment.SpecialFolder.Personal));
			foreach (var entry in Archivos)
			{
				Texto.Text += entry + Environment.NewLine;
			}

			ArchivoLibro();
			System.Threading.Thread.Sleep(3000);
			ArchivoImagen();

		}

		public void CambiodeUbicacion(object sender, LocationUpdatedEventArgs e)
		{
			CLLocation ubicacion = e.Location;
			Task.Factory.StartNew(() =>
					{
						var RecibeCentrar = new CLLocationCoordinate2D
				(ubicacion.Coordinate.Latitude,
				 ubicacion.Coordinate.Longitude);
						var carpeta = Environment.GetFolderPath
				(Environment.SpecialFolder.Personal);
						rutaback = Path.Combine(carpeta, archivoBack);
						RecibeAltura = new MKCoordinateSpan(.002, .002);
						RecibeRegion = new MKCoordinateRegion
				(RecibeCentrar, RecibeAltura);
					}).ContinueWith(t =>
		{
				Mapa.SetRegion(RecibeRegion, true);
				Mapa.ShowsUserLocation = true;
				Imagen2.Image = UIImage.FromFile(rutaback);
			}, TaskScheduler.FromCurrentSynchronizationContext()
				);
			Console.WriteLine("Aplicación Actualizada");
		}
		async void ArchivoImagen()
		{
			var ruta = await DescargarImagen();
			Imagen.Image = UIImage.FromFile(ruta);
		}
		public async Task<string> DescargarImagen()
		{
			var clienteWEB = new WebClient();
			var datosImagen = await clienteWEB.DownloadDataTaskAsync("https://i.ytimg.com/vi/W3q8Od5qJio/maxresdefault.jpg");
			var carpeta = Environment.GetFolderPath
				(Environment.SpecialFolder.Personal);
			string archivo = "foto1.jpg";
			var rutalocal = Path.Combine(carpeta, archivo);
			File.WriteAllBytes(rutalocal, datosImagen);
			return rutalocal;
		}
		async void ArchivoLibro()
		{
			var libro = await DescargaPDF();
			var ruta = NSUrl.FromFilename(libro);
			var urlrequest = new NSUrlRequest(ruta);
			VisorWeb.LoadRequest(urlrequest);
			Progreso.SetProgress(1.0f, true);
		}
		public async Task<string> DescargaPDF()
		{
			var clienteWEB = new WebClient();
			var datosImagen = await clienteWEB.DownloadDataTaskAsync
			("http://lasalle.edu.mx/wp-content/uploads/2011/08/b.-Ampliacion-SU-VIDA-Y-OBRA.pdf");
			var carpeta = Environment.GetFolderPath
				(Environment.SpecialFolder.Personal);
			string archivo = "Libro.pdf";
			var rutalocal = Path.Combine(carpeta, archivo);
			File.WriteAllBytes(rutalocal, datosImagen);
			return rutalocal;
		}
	}
	public class LocationManager
	{
		protected CLLocationManager Ubicacion;
		public event EventHandler<LocationUpdatedEventArgs> UbicacionActualizada = delegate { };
		public LocationManager()
		{
			Ubicacion = new CLLocationManager();
			Ubicacion.PausesLocationUpdatesAutomatically = false;
			Ubicacion.RequestAlwaysAuthorization();
			Ubicacion.StartUpdatingLocation();
			Ubicacion.AllowsBackgroundLocationUpdates = true;
			UbicacionActualizada += ImprimeUbicacion;
		}
		public CLLocationManager Ubica
		{
			get { return Ubicacion; }
		}
		public void StartLocationUpdates()
		{
			if (CLLocationManager.LocationServicesEnabled)
			{
				Ubica.DesiredAccuracy = 1;
				Ubica.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
				{
					UbicacionActualizada(this, new LocationUpdatedEventArgs
									(e.Locations[e.Locations.Length - 1]));
				};
				Ubica.StartUpdatingLocation();
			}
		}
		public void ImprimeUbicacion(object sender, LocationUpdatedEventArgs e)
		{
			CLLocation ubicacion = e.Location;
			Console.WriteLine("Longitude: " + ubicacion.Coordinate.Longitude);
			Console.WriteLine("Latitude: " + ubicacion.Coordinate.Latitude);
		}
	}
	public class LocationUpdatedEventArgs : EventArgs
	{
		CLLocation ubicacion;
		public LocationUpdatedEventArgs(CLLocation ubicacion)
		{
			this.ubicacion = ubicacion;
		}
		public CLLocation Location
		{
			get { return ubicacion; }

		}
	}
	public class LoadingOverlay : UIView
	{
		UIActivityIndicatorView Indicador;
		UILabel Mensaje;
		public LoadingOverlay(CGRect frame) : base(frame)
		{
			BackgroundColor = UIColor.Black;
			Alpha = 0.75f;
			AutoresizingMask = UIViewAutoresizing.All;

			nfloat labelAlto = 22;
			nfloat labelAncho = Frame.Width - 20;

			nfloat centerX = Frame.Width / 2;
			nfloat centerY = Frame.Height / 2;

			Indicador = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
			Indicador.Frame = new CGRect(
				centerX - (Indicador.Frame.Width / 2),
				centerY - Indicador.Frame.Height - 20,
				Indicador.Frame.Width,
				Indicador.Frame.Height);
			Indicador.AutoresizingMask = UIViewAutoresizing.All;
			AddSubview(Indicador);
			Indicador.StartAnimating();

			Mensaje = new UILabel(new CGRect(
				centerX - (labelAncho / 2),
				centerY + 20,
				labelAncho,
				labelAlto
				));
			Mensaje.BackgroundColor = UIColor.Clear;
			Mensaje.TextColor = UIColor.White;
			Mensaje.Text = "Cargando Datos...";
			Mensaje.TextAlignment = UITextAlignment.Center;
			Mensaje.AutoresizingMask = UIViewAutoresizing.All;
			AddSubview(Mensaje);

		}
		public void Hide()
		{
			UIView.Animate(
				0.5, // duration
				() => { Alpha = 0; },
				() => { RemoveFromSuperview(); }
			);
		}
	}
}
