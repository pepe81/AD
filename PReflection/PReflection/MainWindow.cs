using System;
using Gtk;
using System.Reflection;
using PReflection;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

//		showInfo (typeof(Categoria));
//		showInfo (typeof(Articulo));
//		showInfo (typeof(Button));Podemos utilizarlo con cualquier clase...
//		showInfo (typeof(String));
		Categoria categoria = new Categoria (33, "Treinta y tres");

		showValues (categoria);

	}
	private void showValues (object obj)
	{
		Type type = obj.GetType ();
		FieldInfo[] fields = type.GetFields (BindingFlags.Instance | BindingFlags.NonPublic);
		foreach (FieldInfo field in fields) {
			object value = field.GetValue (obj);
			Console.WriteLine (" field.Name={0,-20} field.FieldType={1}", field.Name, field.FieldType);
		}
	}

	private void showInfo(Type type)
	{
		Console.WriteLine ("type.Name={0}", type.Name);

		PropertyInfo[] properties = type.GetProperties ();
		foreach (PropertyInfo property in properties)
			Console.WriteLine (" property.Name={0,-20} property.PropertyType={1}",property.Name,property.PropertyType);

		FieldInfo[] fields = type.GetFields (BindingFlags.Instance | BindingFlags.NonPublic);
		foreach (FieldInfo field in fields)
			//if (field.IsDefined(typeof(IdAttribute),true);
				Console.WriteLine (" field.Name={0,-20} field.FieldType={1}",field.Name,field.FieldType);
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
