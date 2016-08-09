// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStorable.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Camera.Portable.DataAccess.Storable
{
	/// <summary>
	/// The storable interface.
	/// </summary>
	public interface IStorable
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        string Key { get; set; }

		#endregion
	}
}