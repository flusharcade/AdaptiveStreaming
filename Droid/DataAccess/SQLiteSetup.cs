// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SQLiteSetup.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Camera.Droid.DataAccess
{
	using System;
	using System.IO;

	using SQLite.Net.Interop;

	using Camera.Portable.DataAccess.Storage;

	/// <summary>
	/// The SQLite setup object.
	/// </summary>
	public class SQLiteSetup : ISQLiteSetup
	{
		public string DatabasePath { get; set; }

		public ISQLitePlatform Platform { get; set; }

		public SQLiteSetup(ISQLitePlatform platform)
		{
			DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Camera.db3"); ;
			Platform = platform;
		}
	}
}