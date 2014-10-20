using System;
using System.Data;

namespace SerpisAd
{
	public class App
	{
		public App ()
		{
		}

		private static App instance = new App();

		public static App Instance 
		{
			get{return instance;}
		}

		private IDbConnection dbConnection;
		public IDbConnection DbConnection
		{
			get{return dbConnection;}
			set{dbConnection = value;}
		}
	}
}

