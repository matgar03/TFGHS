using System;


/*
Esta clase se hace para hacer más facil el variar las pruebas pero sería 
más aconsejable que estuviesen en la clase en la que toca y aunque estén 
aquí definidas no se usan de forma global. Por ejemplo, la clase Individuo
es la única que sabe cual es el número de genes y se lo dice ella a los demás;
*/
static class EvoParameters
{
	//Numero de variables de cada individuo
    public static readonly int numGenes = 13;
	//Numero poblicación
    public static readonly int populationSize = 200;
	//Numero de partidas jugadas durante el fitness
    public static readonly int numPartidas = 100;
	//Numero padres que pasan a la generacion siguiente
    public static readonly int elitismo = 10;
	//Numero de aleatorios que se insertan en la generacion
    public static readonly int aleatoriosInsertados = 50;
	//Numero de hijos que se calculan para la generacion siguiente (numHijos + elitismo + aleatoriosInsertados = populationSize)
	public static readonly int numHijos = 140;
	//Probabilidad de mutacion
	public static readonly double probabilidadMutacion = 0.3;
	//Numero de generaciones
	public static readonly int numGeneraciones = 15;
	//Tamaño del torneo en el método de seleccion por torneo
	public static readonly int tamTorneo = 30;
	//Tamaño de la sublista que se ordena
	public static readonly int tamScramble = 4;

}
