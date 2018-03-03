// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Multihilos
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIImageView Imagen { get; set; }

		[Outlet]
		UIKit.UIImageView Imagen2 { get; set; }

		[Outlet]
		MapKit.MKMapView Mapa { get; set; }

		[Outlet]
		UIKit.UIProgressView Progreso { get; set; }

		[Outlet]
		UIKit.UITextView Texto { get; set; }

		[Outlet]
		UIKit.UIWebView VisorWeb { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Imagen != null) {
				Imagen.Dispose ();
				Imagen = null;
			}

			if (Imagen2 != null) {
				Imagen2.Dispose ();
				Imagen2 = null;
			}

			if (Mapa != null) {
				Mapa.Dispose ();
				Mapa = null;
			}

			if (Progreso != null) {
				Progreso.Dispose ();
				Progreso = null;
			}

			if (Texto != null) {
				Texto.Dispose ();
				Texto = null;
			}

			if (VisorWeb != null) {
				VisorWeb.Dispose ();
				VisorWeb = null;
			}
		}
	}
}
