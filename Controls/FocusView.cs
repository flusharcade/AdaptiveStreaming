// --------------------------------------------------------------------------------------------------
//  <copyright file="FocusView.cs" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------
using System.Threading.Tasks;

namespace Camera.Controls
{
	using System;

	using Xamarin.Forms;

	using Camera.Portable.Enums;

	/// <summary>
	///  This is the control used to embed into Xamarin Forms that will yield a custom rendered for tapping focus
	/// </summary>
	public sealed class FocusView : RelativeLayout
	{
		#region Constant Properties

		/// <summary>
		/// The image target bound.
		/// </summary>
		const int IMG_TARGET_BOUND = 100;

		#endregion

		#region Private Properties

		/// <summary>
		/// The is animating.
		/// </summary>
		private bool _isAnimating;

		/// <summary>
		/// The focal target.
		/// </summary>
		private readonly CustomImage _focalTarget;

		/// <summary>
		/// The p starting orientation.
		/// </summary>
		private Point _pStartingOrientation;

		/// <summary>
		/// The p flipped orientation.
		/// </summary>
		private Point _pFlippedOrientation;

		#endregion

		#region Public Events

		/// <summary>
		/// Occurs when shutter.
		/// </summary>
		public event EventHandler Shutter;

		/// <summary>
		/// Occurs when focus.
		/// </summary>
		public event EventHandler<Point> Focus;

		#endregion

		#region Public Properties

		/// <summary>
		/// The focal target visible property.
		/// </summary>
		public static readonly BindableProperty FocalTargetVisibleProperty = BindableProperty.Create<FocusView, bool>(
			p => p.FocalTargetVisible, default(bool));

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Camera.Controls.FocusView"/> focal target visible.
		/// </summary>
		/// <value><c>true</c> if focal target visible; otherwise, <c>false</c>.</value>
		public bool FocalTargetVisible
		{
			get
			{
				return (bool)GetValue(FocalTargetVisibleProperty);
			}
			set
			{
				SetValue(FocalTargetVisibleProperty, value);
			}
		}

		/// <summary>
		/// Gets the height of the target.
		/// </summary>
		/// <value>The height of the target.</value>
		public double TargetHeight
		{
			get
			{
				return _focalTarget.Height / 2;
			}
		}

		/// <summary>
		/// Gets the width of the target.
		/// </summary>
		/// <value>The width of the target.</value>
		public double TargetWidth
		{
			get
			{
				return _focalTarget.Width / 2;
			}
		}

		/// <summary>
		/// The orientation.
		/// </summary>
		public Orientation Orientation;

		#endregion

		#region Private Methods

		/// <summary>
		/// Animates the focal target.
		/// </summary>
		/// <param name="touchPoint">Touch point.</param>
		private async void AnimateFocalTarget(Point touchPoint)
		{
			_focalTarget.TintColorString = "#007F00";

			await _focalTarget.LayoutTo(new Rectangle(touchPoint.X - (IMG_TARGET_BOUND / 2), 
			                                          touchPoint.Y - (IMG_TARGET_BOUND / 2), 
			                                          IMG_TARGET_BOUND, IMG_TARGET_BOUND), 0);

			// fade in
			await _focalTarget.FadeTo(0.7f, 25);

			// animate scale
			await _focalTarget.LayoutTo(new Rectangle(touchPoint.X - (IMG_TARGET_BOUND / 4), 
			                                          touchPoint.Y - (IMG_TARGET_BOUND / 4),
			                                          (IMG_TARGET_BOUND / 2), (IMG_TARGET_BOUND / 2)), 250);

			_focalTarget.TintOn = true;

			await Task.Delay(1000);

			_focalTarget.TintColorString = "#FFFFFF";

			_isAnimating = false;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Reset this instance.
		/// </summary>
		public void Reset()
		{
			switch (Orientation)
			{
				case Orientation.Portrait:
					NotifyFocus(_pStartingOrientation);
					break;
				case Orientation.LandscapeLeft:
				case Orientation.LandscapeRight:
					NotifyFocus(_pFlippedOrientation);
					break;
			}
		}

		/// <summary>
		/// Notifies the focus.
		/// </summary>
		/// <param name="touchPoint">Touch point.</param>
		public void NotifyFocus(Point touchPoint)
		{
			if (_isAnimating) 
			{
				return;
			}

			_focalTarget.Opacity = 0.0f;
			_focalTarget.TintOn = false;
			_isAnimating = true;

			Device.BeginInvokeOnMainThread(() => AddFocualTargetImg(touchPoint));

			if (Focus != null) 
			{
				Focus (this, touchPoint);
			}
		}

		/// <summary>
		/// Notifies the shutter.
		/// </summary>
		public void NotifyShutter()
		{
			if (Shutter != null)
			{
				Shutter(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Clear this instance.
		/// </summary>
		public void Clear()
		{
			Children.Clear();
		}

		/// <summary>
		/// Sets the focus points.
		/// </summary>
		/// <param name="pStart">P start.</param>
		/// <param name="pFlipped">P flipped.</param>
		public void SetFocusPoints(Point pStart, Point pFlipped)
		{
			_pStartingOrientation = pStart;
			_pFlippedOrientation = pFlipped;
		}

		/// <summary>
		/// Adds the focual target image.
		/// </summary>
		/// <param name="touchPoint">Touch point.</param>
		public void AddFocualTargetImg(Point touchPoint)
		{
			AnimateFocalTarget(touchPoint);
		}


		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Camera.Controls.FocusView"/> class.
		/// </summary>
		public FocusView()
		{
			_focalTarget = new CustomImage()
			{
				Path = "photo_focus.png",
				BackgroundColor = Color.Transparent,
				TintColorString = "#FFFFFFF",
				Opacity = 0.0f,
				TintOn = false
			} ;

			Children.Add(_focalTarget,
				Constraint.RelativeToParent((parent) =>
					{
						return parent.X;
					} ),
				Constraint.RelativeToParent((parent) =>
					{
						return parent.Y;
					} ),
				Constraint.RelativeToParent((parent) =>
					{
						return IMG_TARGET_BOUND;
					} ),
				Constraint.RelativeToParent((parent) =>
					{
						return IMG_TARGET_BOUND;
					} ));

			BackgroundColor = Color.Transparent;
		}

		#endregion
	}
}