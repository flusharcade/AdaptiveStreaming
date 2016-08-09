// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISQLiteSetup.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Camera.Portable.DataAccess.Storage
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Collections.Generic;
	using System.Linq;

	using SQLite.Net.Interop;
	using SQLite.Net.Async;
	using SQLite.Net;

	using Camera.Portable.Threading;
	using Camera.Portable.DataAccess.Storable;
	using Camera.Portable.DataAccess;
	using Camera.Portable.Logging;

	/// <summary>
	/// SQLite storage.
	/// </summary>
	public class SQLiteStorage : ISQLiteStorage
	{
		private readonly AsyncLock asyncLock = new AsyncLock();

		private readonly object lockObject = new object();

		private SQLiteConnectionWithLock _conn;

		private SQLiteAsyncConnection _dbAsyncConn;

		private readonly ISQLitePlatform _sqlitePlatform;

		private string _dbPath;

		private readonly ILogger _log;

		private readonly string _tag;

		public SQLiteStorage(ISQLiteSetup sqliteSetup, ILogger log)
		{
			_dbPath = sqliteSetup?.DatabasePath;
			_sqlitePlatform = sqliteSetup?.Platform;

			_log = log;
			_tag = $"{GetType()} ";
		}

		public void CreateSQLiteAsyncConnection()
		{
			var connectionFactory = new Func<SQLiteConnectionWithLock>(() =>
				{
					if (_conn == null)
					{
						_conn = new SQLiteConnectionWithLock(_sqlitePlatform, 
					                                         new SQLiteConnectionString(_dbPath, true));
					}

					return _conn;
				});

			_dbAsyncConn = new SQLiteAsyncConnection(connectionFactory);
		}

		public async Task CreateTable<T>(CancellationToken token) where T : class, IStorable, new()
		{
			using (var releaser = await asyncLock.LockAsync())
			{
				await _dbAsyncConn.CreateTableAsync<T>(token);
			}
		}

		public async Task<IList<T>> GetTable<T>(CancellationToken token) where T : class, IStorable, new()
		{
			var items = default(IList<T>);

			using (var releaser = await asyncLock.LockAsync())
			{
				try
				{
					items = await _dbAsyncConn.QueryAsync<T>(string.Format("SELECT * FROM {0};", typeof(T).Name));
				}
				catch (Exception error)
				{
					var location = string.Format("GetTable<T>() Failed to 'SELECT *' from table {0}.", typeof(T).Name);

					_log.WriteLineTime(_tag + "\n" +
						location + "\n" +
						"ErrorMessage: \n" +
						error.Message + "\n" +
						"Stacktrace: \n " +
						error.StackTrace);
				}
			}

			return items;
		}

		public async Task DropTable<T>(CancellationToken token) where T : class, IStorable, new()
		{
			using (var releaser = await asyncLock.LockAsync())
			{
				await _dbAsyncConn.DropTableAsync<T>(token);
			}
		}

		public async Task InsertObject<T>(T item, CancellationToken token) where T : class, IStorable, new()
		{
			using (var releaser = await asyncLock.LockAsync())
			{
				try
				{
					var insertOrReplaceQuery = item.CreateInsertOrReplaceQuery();
					await _dbAsyncConn.QueryAsync<T>(insertOrReplaceQuery);
				}
				catch (Exception error)
				{
					var location = string.Format("InsertObject<T>() Failed to insert or replace object with key {0}.", item.Key);

					_log.WriteLineTime(_tag + "\n" +
						location + "\n" +
						"ErrorMessage: \n" +
						error.Message + "\n" +
						"Stacktrace: \n " +
						error.StackTrace);
				}
			}
		}

		public async Task<T> GetObject<T>(string key, CancellationToken token) where T : class, IStorable, new()
		{
			using (var releaser = await asyncLock.LockAsync())
			{
				try
				{
					var items = await _dbAsyncConn.QueryAsync<T>(string.Format("SELECT * FROM {0} WHERE Key = '{1}';", typeof(T).Name, key));
					if (items != null && items.Count > 0)
					{
						return items.FirstOrDefault();
					}
				}
				catch (Exception error)
				{
					var location = string.Format("GetObject<T>() Failed to get object from key {0}.", key);

					_log.WriteLineTime(_tag + "\n" +
						location + "\n" +
						"ErrorMessage: \n" +
						error.Message + "\n" +
						"Stacktrace: \n " +
						error.StackTrace);
				}
			}

			return default(T);
		}

		public async Task ClearTable<T>(CancellationToken token) where T : class, IStorable, new()
		{
			using (var releaser = await asyncLock.LockAsync())
			{
				await _dbAsyncConn.QueryAsync<T>(string.Format("DELETE FROM {0};", typeof(T).Name));
			}
		}

		public async Task DeleteObject<T>(T item, CancellationToken token)
		{
			using (var releaser = await asyncLock.LockAsync())
			{
				await _dbAsyncConn.DeleteAsync(item, token);
			}
		}

		public async Task DeleteObjectByKey<T>(string key, CancellationToken token) where T : class, IStorable, new()
		{
			using (var releaser = await asyncLock.LockAsync())
			{
				try
				{
					await _dbAsyncConn.QueryAsync<T>(string.Format("DELETE FROM {0} WHERE Key=\'{1}\';", typeof(T).Name, key));
				}
				catch (Exception error)
				{
					var location = string.Format("DeleteObjectByKey<T>() Failed to delete object from key {0}.", key);

					_log.WriteLineTime(_tag + "\n" +
						location + "\n" +
						"ErrorMessage: \n" +
						error.Message + "\n" +
						"Stacktrace: \n " +
						error.StackTrace);
				}
			}
		}

		public void CloseConnection()
		{
			lock (lockObject)
			{
				if (_conn != null)
				{
					_conn.Close();
					_conn.Dispose();
					_conn = null;

					// Must be called as the disposal of the connection is not released until the GC runs.
					GC.Collect();
					GC.WaitForPendingFinalizers();
				}
			}
		}
	}
}