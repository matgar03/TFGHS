using System;
using System.Collections.Generic;
using System.Text;

namespace SabberStoneCoreAi.src
{
	class ArbolGener<T>
	{
		private T nodo;
		private ArbolGener<T> padre;
		private List<ArbolGener<T>> hijos;
		private int hijoCount;

		public ArbolGener(T raiz)
		{
			this.nodo = raiz;
			this.hijos = new List<ArbolGener<T>>();
			this.hijoCount = 0;
			this.padre = null;
		}

		public ArbolGener(T raiz,ArbolGener<T> padre)
		{
			this.nodo = raiz;
			this.hijos = new List<ArbolGener<T>>();
			this.hijoCount = 0;
			this.padre = padre;
		}

		public void AddHijo(T nuevo)
		{
			this.hijos.Add(new ArbolGener<T>(nuevo,this));
			++hijoCount;
		}

		public ArbolGener<T> getHijo(int i ){
			foreach(ArbolGener<T> h in hijos){
				if (--i == 0) return h;
			}
			return null;
		}
		public int getHijoCount()
		{
			return hijoCount;
		}

		public T getNodo()
		{
			return nodo;
		}
		public ArbolGener<T> getPadre()
		{
			return padre;
		}
	}
}
