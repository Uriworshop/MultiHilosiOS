using System;
using System.IO;
using System.Net;
using Foundation;
using UIKit;
namespace MultiTheading_iOS
{
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		public override UIWindow Window
		{
			get;
			set;
		}
		public override bool FinishedLaunching(UIApplication application,
											   NSDictionary launchOptions)
		{
			UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval
						 (UIApplication.BackgroundFetchIntervalMinimum);
			return true;
		}
		public override void OnResignActivation(UIApplication application)
		{
			Console.WriteLine("Aplicación en Estado Inactivo.");
		}
		public override void DidEnterBackground(UIApplication application)
		{
			Console.WriteLine("Aplicacion en Background.");
			Console.WriteLine("Recibiendo Localizacion en Background.");
		}
		public override void WillEnterForeground(UIApplication application)
		{
			Console.WriteLine("Aplicacion entrando a primer plano");
		}
		public override void OnActivated(UIApplication application)
		{
			Console.WriteLine("Aplicacion reactivada");
		}
		public override void WillTerminate(UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}
		public override async void PerformFetch
		(UIApplication application, Action<UIBackgroundFetchResult> EventoCompletado)
		{
			Console.WriteLine("Realizando Procesos en Background .............");
			var resultado = UIBackgroundFetchResult.NoData;
			try
			{
				var clienteWeb = new WebClient();
				var datosdeImagen = await clienteWeb.DownloadDataTaskAsync
					("https://pbs.twimg.com/media/CobB5JBUAAAGlj6.jpg");
				var carpeta = Environment.GetFolderPath
					(Environment.SpecialFolder.Personal);
				var archivoback = "foto2.jpg";
				var rutaback = Path.Combine(carpeta, archivoback);
				File.WriteAllBytes(rutaback, datosdeImagen);
				resultado = UIBackgroundFetchResult.NewData;
			}
			catch
			{
				resultado = UIBackgroundFetchResult.Failed;
			}
			finally
			{
				EventoCompletado(resultado);
			}
		}
	}
}
