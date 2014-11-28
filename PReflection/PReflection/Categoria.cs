using System;

namespace PReflection
{
	public class Categoria
	{
		public Categoria(ulong id, string nombre)
		{
			this.id = id;
			this.nombre = nombre;
		}

		[Id]
		public ulong id;
		public string nombre;

		public ulong Id 
		{ 
			get {return id;}
			set {id = value;}
		}

		public string Nombre 
		{
			get {return nombre;}
			set {nombre = value;}
		}
	}
}

