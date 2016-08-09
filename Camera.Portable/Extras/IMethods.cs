// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMethods.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Camera.Portable.Extras
{
	using System.Threading.Tasks;

	/// <summary>
	/// The methods interface
	/// </summary>
	public interface IMethods
	{
		#region Methods

		/// <summary>
		/// Exit this instance.
		/// </summary>
		void Exit();

		/// <summary>
		/// Displaies the entry alert.
		/// </summary>
		/// <returns>The entry alert.</returns>
		void DisplayEntryAlert(TaskCompletionSource<string> tcs, string message);

		#endregion
	}
}