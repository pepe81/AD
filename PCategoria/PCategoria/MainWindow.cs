using Gtk;
using System.Data;
using System;
using PCategoria;
using SerpisAd;

public partial class MainWindow: Gtk.Window
{	
	private IDbConnection dbConnection;
	private ListStore listStore;

	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

		deleteAction.Sensitive = false;
		editAction.Sensitive = false;

		dbConnection = App.Instance.DbConnection;


		treeView.AppendColumn ("id", new CellRendererText (), "text", 0);
		treeView.AppendColumn ("nombre", new CellRendererText (), "text", 1);
		listStore = new ListStore (typeof(ulong), typeof(string));
		treeView.Model = listStore;

		fillListStore ();

		//treeView.Selection.Mode = SelectionMode.Multiple; //seleccion multiple

		//treeView.Selection.Changed += selectionChanged;
		// metodo anonimo, acceso a variables que sten en alcance en local
		treeView.Selection.Changed += delegate //Podemos añadir varios metodos para que sean llamados con +=
		{
			deleteAction.Sensitive = treeView.Selection.CountSelectedRows() > 0;
			editAction.Sensitive = treeView.Selection.CountSelectedRows() > 0;
		};
	}

	private void selectionChanged (object sender, EventArgs e)
	{
		Console.WriteLine ("selectionChanged");
		bool hasSelected = treeView.Selection.CountSelectedRows () > 0;
		deleteAction.Sensitive = hasSelected;
		editAction.Sensitive = hasSelected;
	}

	private void fillListStore() 
	{
		IDbCommand dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = "select * from categoria";

		IDataReader dataReader = dbCommand.ExecuteReader ();
		while (dataReader.Read()) 
		{
			object id = dataReader ["id"];
			object nombre = dataReader ["nombre"];
			listStore.AppendValues (id, nombre);
		}
		dataReader.Close ();
	}

	protected void OnAddActionActivated (object sender, EventArgs e)
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

	protected void OnRefreshActionActivated (object sender, EventArgs e)
	{
		listStore.Clear ();
		fillListStore ();
	}
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		dbConnection.Close ();
		Application.Quit ();
		a.RetVal = true;
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
		IDbCommand dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = deleteSql;

		dbCommand.ExecuteNonQuery ();

	}

	public bool ConfirmDelete()
	{
		return Confirm ("¿Estás seguro de eliminar el registro?");
	}

	public bool Confirm(string text)
	{
		MessageDialog messageDialog = new MessageDialog (
			this, //Para la ventana en la que nos encontramos
			DialogFlags.Modal, 
			MessageType.Question, 
			ButtonsType.YesNo, 
			text
			);

		messageDialog.Title = "Confirmación eliminación";
		ResponseType response = (ResponseType)messageDialog.Run ();//Hacemos un cast ya que run devuelve un int
		messageDialog.Destroy ();

		return response == ResponseType.Yes;
	}
	protected void OnEditActionActivated (object sender, EventArgs e)
	{
		TreeIter treeIter;
		treeView.Selection.GetSelected (out treeIter);
		object id = listStore.GetValue (treeIter, 0);
		CategoriaView categoriaView = new CategoriaView (id);

	}

}
