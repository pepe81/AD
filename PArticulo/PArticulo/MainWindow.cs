using System;
using System.Data;
using Gtk;
using SerpisAd;
using PArticulo;

public partial class MainWindow: Gtk.Window
{	
	private IDbConnection dbConnection;
	private ListStore listStore;
	private ListStore listStoreCategoria;
	private ListStore listStoreArticulo;

	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

		dbConnection = App.Instance.DbConnection;

		treeViewCategoria.AppendColumn ("Id", new CellRendererText (), "text", 0);
		treeViewCategoria.AppendColumn ("Nombre", new CellRendererText (), "text", 1);
		listStoreCategoria = new ListStore (typeof(ulong), typeof(string));
		treeViewCategoria.Model = listStoreCategoria;

		fillListStoreCategoria ();

		treeViewArticulo.AppendColumn ("Id", new CellRendererText (), "text", 0);
		treeViewArticulo.AppendColumn ("Nombre", new CellRendererText (), "text", 1);
		treeViewArticulo.AppendColumn ("Categoría", new CellRendererText (), "text", 2);
		treeViewArticulo.AppendColumn ("Precio", new CellRendererText (), "text", 3);
		listStoreArticulo = new ListStore (typeof(ulong), typeof(string),typeof(ulong),typeof(string));
		treeViewArticulo.Model = listStoreArticulo;

		fillListStoreArticulo ();
	}

	private void fillListStoreCategoria() 
	{
		IDbCommand dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = "select * from categoria";

		IDataReader dataReader = dbCommand.ExecuteReader ();
		while (dataReader.Read()) 
		{
			object id = dataReader ["id"];
			object nombre = dataReader ["nombre"];
			listStoreCategoria.AppendValues (id, nombre);
		}
		dataReader.Close ();
	}
	private void fillListStoreArticulo() 
	{
		IDbCommand dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = "select * from articulo";

		IDataReader dataReader = dbCommand.ExecuteReader ();
		while (dataReader.Read()) 
		{
			object id = dataReader ["id"];
			object nombre = dataReader ["nombre"];
			object categoria = dataReader ["categoria"];
			object precio = dataReader ["precio"].ToString();
			listStoreArticulo.AppendValues (id, nombre, categoria, precio);
		}
		dataReader.Close ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}


	protected void OnNewActionArticuloActivated (object sender, EventArgs e)
	{
		throw new NotImplementedException ();
	}
	
	protected void OnNewActionCategoriaActivated (object sender, EventArgs e)
	{
		string insertSql = string.Format(
			"insert into categoria (nombre) values ('{0}')",
			"Nuevo " + DateTime.Now
			);
		Console.WriteLine ("insertSql={0}", insertSql);
		IDbCommand dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = insertSql;

		dbCommand.ExecuteNonQuery ();
	}
}