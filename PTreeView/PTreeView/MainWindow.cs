using System;
using Gtk;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

//		opcion 1
//		treeView.AppendColumn ("precio", new CellRendererText (), "text", 0);
//
//		ListStore listStore = new ListStore (typeof(string));
//
//		object value = new Decimal (1.2).ToString();
//		listStore.AppendValues (value);
//
//		treeView.Model = listStore;

		//opcion 2
		treeView.AppendColumn ("precio", new CellRendererText (),
			new TreeCellDataFunc (delegate(TreeViewColumn tree_column, CellRenderer cell, 
		    	TreeModel tree_model, TreeIter iter) {
				CellRendererText cellRendererText = (CellRendererText)cell;

			cellRendererText.Text = "este es el valor que se visualiza";


			})
		);
		ListStore listStore = new ListStore (typeof(decimal));
		object value = new decimal (1.2);
		listStore.AppendValues(value);

		treeView.Model = listStore;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
