using Gtk;
using MySql.Data.MySqlClient;
using System;

public partial class MainWindow: Gtk.Window
{	
	private MySqlConnection mySqlConnection;
	private MySqlDataReader mySqlDataReader;
	private ListStore listStore;

	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

		mySqlConnection = new MySqlConnection (
			"DataSource=localhost;Database=dbprueba;User ID=root;Password=sistemas"
		);

		mySqlConnection.Open ();

		treeView.AppendColumn ("id", new CellRendererText (), "text", 0);
		treeView.AppendColumn ("nombre", new CellRendererText (), "text", 1);
		listStore = new ListStore (typeof(ulong), typeof(string));
		treeView.Model = listStore;

		fillListStore ();

		//treeView.Selection.Mode = SelectionMode.Multiple; //seleccion multiple

		//treeView.Selection.Changed += selectionChanged;
		// metodo anonimo, acceso a variables que sten en alcance en local
		treeView.Selection.Changed += delegate 
		{
			deleteAction.Sensitive = treeView.Selection.CountSelectedRows() > 0;
		};
	}

	private void selectionChanged (object sender, EventArgs e)
	{
		Console.WriteLine ("selectionChanged");
	}

	private void fillListStore() 
	{
		MySqlCommand mySqlCommand = mySqlConnection.CreateCommand ();
		mySqlCommand.CommandText = "select * from categoria";

		mySqlDataReader = mySqlCommand.ExecuteReader ();
		while (mySqlDataReader.Read()) 
		{
			object id = mySqlDataReader ["id"];
			object nombre = mySqlDataReader ["nombre"];
			listStore.AppendValues (id, nombre);
		}
		mySqlDataReader.Close ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		mySqlConnection.Close ();
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnAddActionActivated (object sender, EventArgs e)
	{
		string insertSql = string.Format(
			"insert into categoria (nombre) values ('{0}')",
			"Nuevo " + DateTime.Now
		);
		Console.WriteLine ("insertSql={0}", insertSql);
		MySqlCommand mySqlCommand = mySqlConnection.CreateCommand ();
		mySqlCommand.CommandText = insertSql;

		mySqlCommand.ExecuteNonQuery ();
	}

	protected void OnRefreshActionActivated (object sender, EventArgs e)
	{
		listStore.Clear ();
		fillListStore ();
	}
	protected void OnDeleteActionActivated (object sender, EventArgs e)
	{
		if (!ConfirmDelete ())
			return;

		//TreeIter = iterador para arbol, movernos entre niveles.(index)
		TreeIter treeIter; // no hace falta inicializar, porque es un parámetro de salida.
		treeView.Selection.GetSelected (out treeIter);
		object id = listStore.GetValue (treeIter, 0);

		string deleteSql = string.Format("delete from categoria where id={0}", id);
		Console.WriteLine ("deleteSql={0}", deleteSql);
		MySqlCommand mySqlCommand = mySqlConnection.CreateCommand ();
		mySqlCommand.CommandText = deleteSql;

		mySqlCommand.ExecuteNonQuery ();

	}

	public bool ConfirmDelete()
	{
		return Confirm ("¿Estás seguro de eliminar el registro?");
	}

	public bool Confirm(string text)
	{
		MessageDialog messageDialog = new MessageDialog (
			this, // para la ventana en la que nos encontramos
			DialogFlags.Modal, 
			MessageType.Question, 
			ButtonsType.YesNo, 
			text
			);

		ResponseType response = (ResponseType)messageDialog.Run ();
		messageDialog.Destroy ();

		return response == ResponseType.Yes;
	}
}
