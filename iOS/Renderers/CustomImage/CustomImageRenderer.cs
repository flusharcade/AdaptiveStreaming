// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomImageRenderer.cs" company="Flush Arcade">
//   Copyright (c) 2015 Flush Arcade All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

[assembly: Xamarin.Forms.ExportRenderer(typeof(Camera.Controls.CustomImage), 
                                        typeof(Camera.iOS.Renderers.CustomImage.CustomImageRenderer))]

namespace Camera.iOS.Renderers.CustomImage
{
	using System;
	using System.Threading.Tasks;
	using System.IO;

	using Foundation;

	using Xamarin.Forms.Platform.iOS;
	using Xamarin.Forms;

	using UIKit;

	using Camera.Controls;
	using Camera.iOS.Extensions;
	using Camera.iOS.Helpers;

	/// <summary>
	/// Custom image renderer.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class CustomImageRenderer : ViewRenderer<CustomImage, UIView>
	{
		/// <summary>
		/// The image view.
		/// </summary>
		private UIImageView _imageView;

		/// <summary>
		/// The system version.
		/// </summary>
		private int _systemVersion = Convert.ToInt16 (UIDevice.CurrentDevice.SystemVersion.Split ('.') [0]);

		/// <summary>
		/// Initializes a new instance of the <see cref="T:LogIt.iOS.Renderers.CustomImageRenderer"/> class.
		/// </summary>
		public CustomImageRenderer()
		{
			_imageView = new UIImageView();
		}

		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged (ElementChangedEventArgs<CustomImage> e)
		{
			base.OnElementChanged (e);

			if (Element != null) 
			{
				// make sure we only set native control once
				if (Control == null)
				{
					base.SetNativeControl(_imageView);
				}

				loadImage ();

				if (Element.CircleOn)
				{
					createCircle();
				}

				Element.CustomPropertyChanged -= handleCustomPropertyChanged;
				Element.CustomPropertyChanged += handleCustomPropertyChanged;
			}
		}

		/// <summary>
		/// Raises the element property changed event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			if (Element != null && _imageView != null && Element.CircleOn)
			{
				if (e.PropertyName == VisualElement.HeightProperty.PropertyName ||
					e.PropertyName == VisualElement.WidthProperty.PropertyName ||
					e.PropertyName == CustomImage.BorderColorProperty.PropertyName ||
					e.PropertyName == CustomImage.BorderThicknessProperty.PropertyName ||
					e.PropertyName == CustomImage.FillColorProperty.PropertyName)
				{
					createCircle();
				}
			}
		}

		/// <summary>
		/// Creates the circle.
		/// </summary>
		private void createCircle()
		{
			try
			{
				double min = Math.Min(Element.Width, Element.Height);

				Control.Layer.CornerRadius = (float)(min / 2.0);
				Control.Layer.MasksToBounds = false;
				Control.Layer.BorderColor = Element.BorderColor.ToCGColor();
				Control.Layer.BorderWidth = Element.BorderThickness;
				Control.BackgroundColor = Element.FillColor.ToUIColor();
				Control.ClipsToBounds = true;
			}
			catch (Exception ex)
			{
			}
		}

		/// <summary>
		/// Handles the custom property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="propertyName">Property name.</param>
		private void handleCustomPropertyChanged (object sender, string propertyName)
		{
			switch (propertyName)
			{
				case "TintColorString":
				case "TintOn":
					updateControlColor();
					break;
				case "Path":
					InvokeOnMainThread(() => loadImage());
					break;
				case "UseData":
				case "Data":
					InvokeOnMainThread(() => updateData());
					break;
			}
		}

		/// <summary>
		/// Loads the image.
		/// </summary>
		private async void loadImage()
		{
			try 
			{
				if (Element != null)
				{
					if (!string.IsNullOrEmpty(Element.Path))
					{
						_imageView.Image = await ReadBitmapImageFromStorage (Element.Path);

						if (_imageView.Image != null)
						{
							if (_systemVersion >= 7 && Element.TintOn)
							{
								_imageView.Image = _imageView.Image.ImageWithRenderingMode (UIImageRenderingMode.AlwaysTemplate);
							}

							updateControlColor();

							_imageView.ContentMode = setAspect();
						}
					}
				}
			}
			catch (Exception e) 
			{
			}
		}

		/// <summary>
		/// Sets the aspect.
		/// </summary>
		/// <returns>The aspect.</returns>
		private UIViewContentMode setAspect()
		{
			if (Element != null)
			{
				switch (Element.Aspect) 
				{
				case Aspect.AspectFill:
					return UIViewContentMode.ScaleAspectFill;
				case Aspect.AspectFit:
					return UIViewContentMode.ScaleAspectFit;
				case Aspect.Fill:
					return UIViewContentMode.ScaleToFill;
				default:
					return UIViewContentMode.ScaleAspectFit;
				}
			}

			return UIViewContentMode.ScaleAspectFit;
		}

		/// <summary>
		/// Reads the bitmap image from storage.
		/// </summary>
		/// <returns>The bitmap image from storage.</returns>
		/// <param name="fn">Fn.</param>
		public async Task<UIImage> ReadBitmapImageFromStorage(string fn)
		{
			var docsPath = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
			string filePath = Element.UseCustomDirectory ? Path.Combine (docsPath, fn) : Path.Combine (Environment.CurrentDirectory, fn);

			try 
			{
				using (Stream stream = File.OpenRead(filePath))
				{
					NSData data = NSData.FromStream (stream);
					return UIImage.LoadFromData (data);
				}
			}
			catch (Exception e) 
			{
			}

			return UIImage.FromFile (Path.Combine (Environment.CurrentDirectory, "loading.png"));
		}

		/// <summary>
		/// Reads the bitmap image from storage.
		/// </summary>
		/// <returns>The bitmap image from storage.</returns>
		/// <param name="imageData">Image data.</param>
		public async Task<UIImage> ReadBitmapImageFromStorage(byte[] imageData)
		{
			try 
			{
				using (Stream stream = new MemoryStream(imageData))
				{
					NSData data = NSData.FromStream (stream);
					return UIImage.LoadFromData (data);
				}
			}
			catch (Exception e) 
			{
			}

			return UIImage.FromFile (Path.Combine (Environment.CurrentDirectory, "loading.png"));
		}

		/// <summary>
		/// Updates the color of the control.
		/// </summary>
		private void updateControlColor()
		{
			if (Element.TintOn && !string.IsNullOrEmpty(Element.TintColorString)) 
			{
				var color = UIColor.Clear.FromHex (Element.TintColorString, 1.0f);

				if (_systemVersion >= 7)
				{
					Control.TintColor = color;
				}
				else
				{
					_imageView.Image = UIImageEffects.GetColoredImage (_imageView.Image, color);
				}
			}
		}

		/// <summary>
		/// Handles the use data property changed.
		/// </summary>
		private async void updateData()
		{
			if (Element.UseData) 
			{
				_imageView.Image = await ReadBitmapImageFromStorage (Element.Data ?? new byte[]{});
			}
			else
			{
				_imageView.Image = await ReadBitmapImageFromStorage (Element.Path ?? string.Empty);
			}
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose (bool disposing)
		{
			if (Element != null) 
			{
				Element.CustomPropertyChanged -= handleCustomPropertyChanged;
			}

			base.Dispose (disposing);
		}
	}
}