// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomImageRenderer.cs" company="Flush Arcade">
//   Copyright (c) 2016 Flush Arcade All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

[assembly: Xamarin.Forms.ExportRenderer(typeof(Camera.Controls.CustomImage), 
                                        typeof(Camera.Droid.Renderers.CustomImageRenderer))]

namespace Camera.Droid.Renderers
{
	using System;
	using System.Threading.Tasks;
	using System.IO;
	using System.Threading;
	using System.Linq;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.Android;

	using Android.Graphics;
	using Android.Widget;

	using Camera.Controls;
	using Camera.Portable.Ioc;
	using Camera.Portable.Logging;

	/// <summary>
	/// Custom image renderer.
	/// </summary>
	public class CustomImageRenderer : ViewRenderer<CustomImage, ImageView> 
	{
		/// <summary>
		/// The tag.
		/// </summary>
		private readonly string _tag;

		/// <summary>
		/// The image view.
		/// </summary>
		private ImageView _imageView;

		/// <summary>
		/// The custom image.
		/// </summary>
		private CustomImage _customImage;

		/// <summary>
		/// The token source.
		/// </summary>
		private CancellationTokenSource _tokenSource = new CancellationTokenSource();

		/// <summary>
		/// The log.
		/// </summary>
		private ILogger _log;

		/// <summary>
		/// The bitmap.
		/// </summary>
		private Bitmap _bitmap;

		/// <summary>
		/// Initializes a new instance of the <see cref="LogIt.Droid.Renderers.CustomImageRenderer"/> class.
		/// </summary>
		public CustomImageRenderer()
		{
			_imageView = new ImageView(Context);

			_log = IoC.Resolve<ILogger> ();
			_tag = string.Format ("{0} ", GetType ());
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
				_customImage = Element;

				// make sure we only set native control once
				if (Control == null) 
				{
					setAspect ();
					base.SetNativeControl (_imageView);
				}

				updateControlColor ();
				loadImage ();

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
					//CreateCircle();
				}
			}
		}

		/// <summary>
		/// Sets the aspect.
		/// </summary>
		private void setAspect()
		{
			if (Element != null)
			{
				switch (Element.Aspect) 
				{
				case Aspect.AspectFill:
					_imageView.SetScaleType (ImageView.ScaleType.FitXy);
					break;
				case Aspect.AspectFit:
					_imageView.SetScaleType (ImageView.ScaleType.FitCenter);
					break;
				case Aspect.Fill:
					_imageView.SetScaleType (ImageView.ScaleType.FitXy);
					break;
				default:
					_imageView.SetScaleType (ImageView.ScaleType.FitCenter);
					break;
				}
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
				loadImage();
				break;
			case "UseData":
			case "Data":
				updateData ();
				break;
			}
		}

		/// <summary>
		/// Loads the image.
		/// </summary>
		private void loadImage()
		{
			Task.Factory.StartNew (async () => 
				{
					try
					{
						_bitmap = await ReadBitmapImageFromStorage (Element.Path);

						if (_imageView != null && !_tokenSource.IsCancellationRequested && _bitmap != null)
						{
							// set bitmap
							if (Element.UseMainThread)
							{
								// use this for anuimated images as Device.StartTimer runs on a different thread, we need to force this on the UI thread.
								Android.App.Application.SynchronizationContext.Post (state => _imageView.SetImageBitmap(_bitmap), null);
							}
							else
							{
								// for animated images we have to force
								_imageView.SetImageBitmap(_bitmap);
							}
						}
					}
					catch (ArgumentException error)
					{
						_log.WriteLineTime (_tag + "\n" +
							"LoadAsync() Failed to load view model.  \n " +
							"ErrorMessage: \n" + 
							error.Message + "\n" + 
							"Stacktrace: \n " + 
							error.StackTrace);
					}
					catch (Exception error)
					{
						_log.WriteLineTime (_tag + "\n" +
							"LoadAsync() Failed to load view model.  \n " +
							"ErrorMessage: \n" + 
							error.Message + "\n" + 
							"Stacktrace: \n " + 
							error.StackTrace);
					}

				}, _tokenSource.Token);
		}

		/// <summary>
		/// Cancel this instance.
		/// </summary>
		private void cancel()
		{
			_tokenSource.Cancel ();
		}

		/// <summary>
		/// Reads the bitmap image from storage.
		/// </summary>
		/// <returns>The bitmap image from storage.</returns>
		/// <param name="fn">Fn.</param>
		public async Task<Bitmap> ReadBitmapImageFromStorage(string fn)
		{
			try
			{
				if (!string.IsNullOrEmpty(fn))
				{
					var file = fn.Split('.').FirstOrDefault();

					var id = Resources.GetIdentifier(file, "drawable", Context.PackageName);

					using (Stream stream = Resources.OpenRawResource(id))
					{
						if (stream != null)
						{
							return await BitmapFactory.DecodeResourceAsync(Resources, id);
						}
					}
				}
			}
			catch (Exception error) 
			{
				_log.WriteLineTime (
					"MyCareManager.Droid.Renderers.CustomImageRenderer; \n" +
					"ErrorMessage: Failed to load image " + fn + "\n " + 
					"Stacktrace: Login Error  \n " + 
					error);
				// catch failed stream exceptions
			}

			return null;
		}

		/// <summary>
		/// Updates the color of the control.
		/// </summary>
		private void updateControlColor()
		{
			try 
			{
				if (_customImage.TintOn && !string.IsNullOrEmpty(_customImage.TintColorString))
				{
					var color = Android.Graphics.Color.ParseColor(_customImage.TintColorString.Replace(" ", ""));

					_imageView.SetColorFilter (color, PorterDuff.Mode.SrcAtop);
				}
			}
			catch (Exception e) 
			{
				_log.WriteLineTime ("CustomImageRenderer: " + e);
			}
		}

		/// <summary>
		/// Handles the use data property changed.
		/// </summary>
		private async void updateData()
		{
			if (Element.UseData) 
			{
				var imageData = Element.Data ?? new byte[]{ };
				_bitmap = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
				_imageView.SetImageBitmap(_bitmap);
			}
			else
			{
				_bitmap = await ReadBitmapImageFromStorage(Element.Path ?? string.Empty);
				_imageView.SetImageBitmap(_bitmap);
			}
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose (bool disposing)
		{
			// cancel any loading images
			cancel ();

			if (Element != null) 
			{
				Element.CustomPropertyChanged -= handleCustomPropertyChanged;
			}

			if (_bitmap != null) 
			{
				_bitmap.Recycle ();
				_bitmap.Dispose ();
			}

			base.Dispose (disposing);
		}
	}
}