using System;
using SerpisAd;
using System.Data;


namespace PArticulo
{
	public partial class CategoriasView : Gtk.Window
	{
		private object id;
		public CategoriasView () : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}
		public CategoriasView(object id):this()
		{
			this.id = id;
			IDbCommand dbCommand =
				App.Instance.DbConnection.CreateCommand();
			dbCommand.CommandText = String.Format (
				"select * from categoria where id={0}", id
				);
			IDataReader dataReader = dbCommand.ExecuteReader();
			dataReader.Read();

			entryNombreCategoria.Text = dataReader ["nombre"].ToString ();

			dataReader.Close ();
		}
		protected void OnSaveActionActivated (object sender, EventArgs e)
		{
			IDbCommand dbCommand =
				App.Instance.DbConnection.CreateCommand ();
			dbCommand.CommandText = String.Format (
				"update categoria set nombre=@nombre where id={0}", id//@ identifica que viene el nombre del parámetro
				);
			dbCommand.AddParameter("nombre", entryNombreCategoria.Text);

			dbCommand.ExecuteNonQuery ();

			Destroy ();
		}
	}
}

