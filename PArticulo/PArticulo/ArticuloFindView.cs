using System;
using System.Data;
using Gtk;
using SerpisAd;
using PArticulo;

namespace PArticulo
{
	public partial class ArticuloFindView : Gtk.Window
	{
		private object id;
		private IDbConnection dbConnection;
		private ListStore listStoreArticulo;

		public ArticuloFindView () : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build ();

			treeviewBuscarArticulo.AppendColumn ("Id", new CellRendererText (), "text", 0);
			treeviewBuscarArticulo.AppendColumn ("Nombre", new CellRendererText (), "text", 1);
			treeviewBuscarArticulo.AppendColumn ("Categoría", new CellRendererText (), "text", 2);
			treeviewBuscarArticulo.AppendColumn ("Precio", new CellRendererText (), "text", 3);
			listStoreArticulo = new ListStore (typeof(ulong), typeof(string),typeof(ulong),typeof(string));
			treeviewBuscarArticulo.Model = listStoreArticulo;
		}
		public ArticuloFindView(object id):this()
		{
			this.id = id;
			IDbCommand dbCommand = App.Instance.DbConnection.CreateCommand();
			dbCommand.CommandText = String.Format (
				"select * from articulo");

			IDataReader dataReader = dbCommand.ExecuteReader();
			dataReader.Read ();

			dataReader.Close ();
		}
		private void fillListStoreArticuloId() 
		{
			string idBuscar = entryIdArticulo.Text;
			IDbCommand dbCommand = dbConnection.CreateCommand ();
			dbCommand.CommandText = string.Format("select * from articulo where id=@idBuscar",idBuscar);

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
		private void fillListStoreArticuloNombre() 
		{
			IDbCommand dbCommand = dbConnection.CreateCommand ();
			dbCommand.CommandText = "select * from articulo where nombre=@nombre";

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

		protected void OnButtonListarNombreActivated (object sender, EventArgs e)
		{
			fillListStoreArticuloNombre ();
		}

		protected void OnButtonListarIdClicked (object sender, EventArgs e)
		{
			fillListStoreArticuloId ();
		}
	}
}

