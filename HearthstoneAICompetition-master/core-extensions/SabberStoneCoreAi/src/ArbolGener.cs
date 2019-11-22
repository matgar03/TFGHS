using System;
using System.Collections.Generic;
using System.Text;

namespace SabberStoneCoreAi.src
{
	class ArbolGener<T>
	{
		private T nodo;
		private List<ArbolGener<T>> hijos;
		private int hijoCount;

		public ArbolGener(T raiz)
		{
			this.nodo = raiz;
			this.hijos = new List<ArbolGener<T>>();
			this.hijoCount = 0;
		}

		public void AddHijo(T nuevo)
		{
			this.hijos.Add(new ArbolGener<T>(nuevo));
			++hijoCount;
		}

		public ArbolGener<T> getHijo(int i ){
			foreach(ArbolGener<T> h in hijos){
				if (--i == 0) return h;
			}
			return null;
		}
	}
}
