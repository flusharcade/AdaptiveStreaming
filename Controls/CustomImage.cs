// --------------------------------------------------------------------------------------------------
//  <copyright file="CustomImage.cs" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace Camera.Controls
{
	using System;

	using Xamarin.Forms;

	/// <summary>
	/// Custom image.
	/// </summary>
	public class CustomImage : View
	{
		/// <summary>
		/// The tint color string property.
		/// </summary>
		/// <summary>
		/// The tint on property.
		/// </summary>
		public static readonly BindableProperty TintColorStringProperty = BindableProperty.Create ((CustomImage o) => o.TintColorString, string.Empty,
			propertyChanged: (bindable, oldvalue, newValue) => 
			{
				var eh = ((CustomImage)bindable).CustomPropertyChanged;

				if (eh != null)
				{
					eh (bindable, TintColorStringProperty.PropertyName);
				}
			});

		/// <summary>
		/// Gets or sets the tint color string.
		/// </summary>
		/// <value>The tint color string.</value>
		public string TintColorString
		{
			get
			{
				return (string)GetValue(TintColorStringProperty);
			}
			set
			{
				this.SetValue(TintColorStringProperty, value);
			}
		}

		/// <summary>
		/// The tint on property.
		/// </summary>
		public static readonly BindableProperty TintOnProperty = BindableProperty.Create ((CustomImage o) => o.TintOn, default(bool),
			propertyChanged: (bindable, oldvalue, newValue) => 
			{
				var eh = ((CustomImage)bindable).CustomPropertyChanged;

				if (eh != null)
				{
					eh (bindable, TintOnProperty.PropertyName);
				}
			});

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MyCareManager.XamForms.Controls.CustomImage"/> tint on.
		/// </summary>
		/// <value><c>true</c> if tint on; otherwise, <c>false</c>.</value>
		public bool TintOn 
		{
			get 
			{
				return (bool)GetValue (TintOnProperty);
			}
			set 
			{				 
				SetValue (TintOnProperty, value);
			}
		}

		/// <summary>
		/// The circle on property.
		/// </summary>
		public static readonly BindableProperty CircleOnProperty = BindableProperty.Create ((CustomImage o) => o.CircleOn, false,
			propertyChanged: (bindable, oldvalue, newValue) => 
			{
				var eh = ((CustomImage)bindable).CustomPropertyChanged;

				if (eh != null)
				{
					eh (bindable, CircleOnProperty.PropertyName);
				}
			});

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MyCareManager.XamForms.Controls.CustomImage"/> use main thread.
		/// </summary>
		/// <value><c>true</c> if use main thread; otherwise, <c>false</c>.</value>
		public bool UseMainThread 
		{
			get 
			{
				return (bool)GetValue (UseMainThreadProperty);
			}
			set 
			{				 
				SetValue (UseMainThreadProperty, value);
			}
		}

		/// <summary>
		/// The use main thread property.
		/// </summary>
		public static readonly BindableProperty UseMainThreadProperty = BindableProperty.Create ((CustomImage o) => o.UseMainThread, false,
			propertyChanged: (bindable, oldvalue, newValue) => 
			{
				var eh = ((CustomImage)bindable).CustomPropertyChanged;

				if (eh != null)
				{
					eh (bindable, CircleOnProperty.PropertyName);
				}
			});

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MyCareManager.XamForms.Controls.CustomImage"/> circle on.
		/// </summary>
		/// <value><c>true</c> if circle on; otherwise, <c>false</c>.</value>
		public bool CircleOn
		{
			get 
			{
				return (bool)GetValue (CircleOnProperty);
			}
			set 
			{				 
				SetValue (CircleOnProperty, value);
			}
		}

		/// <summary>
		/// The use custom directory property.
		/// </summary>
		public static readonly BindableProperty UseCustomDirectoryProperty = BindableProperty.Create ((CustomImage o) => o.UseCustomDirectory, default(bool),
			propertyChanged: (bindable, oldvalue, newValue) => 
			{
				var eh = ((CustomImage)bindable).CustomPropertyChanged;

				if (eh != null)
				{
					eh (bindable, UseCustomDirectoryProperty.PropertyName);
				}
			});

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MyCareManager.XamForms.Controls.CustomImage"/> use custom directory.
		/// </summary>
		/// <value><c>true</c> if use custom directory; otherwise, <c>false</c>.</value>
		public bool UseCustomDirectory 
		{
			get 
			{
				return (bool)GetValue (UseCustomDirectoryProperty);
			}
			set 
			{
				SetValue (UseCustomDirectoryProperty, value);
			}
		}

		/// <summary>
		/// The image source property.
		/// </summary>
		public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create((CustomImage o) => o.Source, default(ImageSource),
			propertyChanged: (bindable, oldvalue, newValue) => 
			{
				var eh = ((CustomImage)bindable).CustomPropertyChanged;

				if (eh != null)
				{
					eh (bindable, ImageSourceProperty.PropertyName);
				}
			});

		/// <summary>
		/// Gets or sets the source.
		/// </summary>
		/// <value>The source.</value>
		public ImageSource Source
		{
			get
			{
				return (ImageSource)GetValue(ImageSourceProperty);
			}
			set
			{
				SetValue(ImageSourceProperty, value);
			}
		}

		/// <summary>
		/// The path property.
		/// </summary>
		public static readonly BindableProperty PathProperty = BindableProperty.Create((CustomImage o) => o.Path, default(string),
			propertyChanged: (bindable, oldvalue, newValue) =>
			{
				var eh = ((CustomImage)bindable).CustomPropertyChanged;

				if (eh != null)
				{
					eh (bindable, PathProperty.PropertyName);
				}
			});

		/// <summary>
		/// Gets or sets the path.
		/// </summary>
		/// <value>The path.</value>
		public string Path
		{
			get
			{
				return (string)GetValue(PathProperty);
			}
			set
			{
				SetValue(PathProperty, value);
			}
		}

		/// <summary>
		/// The aspect property.
		/// </summary>
		public static readonly BindableProperty AspectProperty = BindableProperty.Create((CustomImage o) => o.Aspect, default(Aspect),
			propertyChanged: (bindable, oldvalue, newValue) =>
			{
				var eh = ((CustomImage)bindable).CustomPropertyChanged;

				if (eh != null)
				{
					eh (bindable, AspectProperty.PropertyName);
				}
			});

		/// <summary>
		/// Gets or sets the aspect.
		/// </summary>
		/// <value>The aspect.</value>
		public Aspect Aspect
		{
			get
			{
				return (Aspect)GetValue(AspectProperty);
			}
			set
			{
				SetValue(AspectProperty, value);
			}
		}

		/// <summary>
		/// The data property.
		/// </summary>
		public static readonly BindableProperty DataProperty = BindableProperty.Create((CustomImage o) => o.Data, default(byte[]), 
			propertyChanged: (bindable, oldvalue, newValue) => 
			{
				var eh = ((CustomImage)bindable).CustomPropertyChanged;

				if (eh != null)
				{
					eh (bindable, DataProperty.PropertyName);
				}
			});

		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>The data.</value>
		public byte[] Data 
		{
			get 
			{
				return (byte[])GetValue (DataProperty);
			}
			set 
			{
				SetValue (DataProperty, value);
			}
		}

		/// <summary>
		/// The use data property.
		/// </summary>
		public static readonly BindableProperty UseDataProperty = BindableProperty.Create((CustomImage o) => o.UseData, default(bool),
			propertyChanged: (bindable, oldvalue, newValue) => 
			{
				var eh = ((CustomImage)bindable).CustomPropertyChanged;

				if (eh != null)
				{
					eh (bindable, UseDataProperty.PropertyName);
				}
			});

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MyCareManager.XamForms.Controls.CustomImage"/> use data.
		/// </summary>
		/// <value><c>true</c> if use data; otherwise, <c>false</c>.</value>
		public bool UseData
		{
			get
			{
				return (bool)GetValue(UseDataProperty);
			}
			set
			{
				SetValue(UseDataProperty, value);
			}
		}

		/// <summary>
		/// Thickness property of border
		/// </summary>
		public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create<CustomImage, int>(p => p.BorderThickness, 0);

		/// <summary>
		/// Border thickness of circle image
		/// </summary>
		public int BorderThickness
		{
			get 
			{ 
				return (int)GetValue(BorderThicknessProperty); 
			}
			set 
			{ 
				SetValue(BorderThicknessProperty, value); 
			}
		}

		/// <summary>
		/// Color property of border
		/// </summary>
		public static readonly BindableProperty BorderColorProperty = BindableProperty.Create<CustomImage, Color>(p => p.BorderColor, Color.White);

		/// <summary>
		/// Border Color of circle image
		/// </summary>
		public Color BorderColor
		{
			get 
			{ 
				return (Color)GetValue(BorderColorProperty); 
			}
			set 
			{ 
				SetValue(BorderColorProperty, value);
			}
		}

		/// <summary>
		/// Color property of fill
		/// </summary>
		public static readonly BindableProperty FillColorProperty = BindableProperty.Create<CustomImage, Color>(p => p.FillColor, Color.Transparent);

		/// <summary>
		/// Fill color of circle image
		/// </summary>
		public Color FillColor
		{
			get
			{ 
				return (Color)GetValue(FillColorProperty); 
			}
			set 
			{ 
				SetValue(FillColorProperty, value); 
			}
		}

		/// <summary>
		/// Occurs when custom property changed.
		/// </summary>
		public event EventHandler<string> CustomPropertyChanged;

		/// <param name="propertyName">The name of the property that changed.</param>
		/// <summary>
		/// Call this method from a child class to notify that a change happened on a property.
		/// </summary>
		protected override void OnPropertyChanged (string propertyName)
		{
			base.OnPropertyChanged (propertyName);

			if (propertyName == CustomImage.TintColorStringProperty.PropertyName ||
				propertyName == CustomImage.CircleOnProperty.PropertyName ||
				propertyName == CustomImage.TintOnProperty.PropertyName || 
				propertyName == CustomImage.UseCustomDirectoryProperty.PropertyName ||
				propertyName == CustomImage.ImageSourceProperty.PropertyName ||
				propertyName == CustomImage.PathProperty.PropertyName ||
				propertyName == CustomImage.AspectProperty.PropertyName ||
				propertyName == CustomImage.DataProperty.PropertyName ||
				propertyName == CustomImage.UseDataProperty.PropertyName)
			{
				if (CustomPropertyChanged != null) 
				{
					this.CustomPropertyChanged (this, propertyName);
				}
			}
		}
	}
}