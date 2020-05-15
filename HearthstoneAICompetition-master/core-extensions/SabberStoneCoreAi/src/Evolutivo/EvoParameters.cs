using System;


/*
Esta clase se hace para hacer más facil el variar las pruebas pero sería 
más aconsejable que estuviesen en la clase en la que toca y aunque estén 
aquí definidas no se usan de forma global. Por ejemplo, la clase Individuo
es la única que sabe cual es el número de genes y se lo dice ella a los demás;
*/
static class EvoParameters
{

    public static readonly int numGenes = 12;

    public static readonly int populationSize = 200;

    public static readonly int numPartidas = 100;

    public static readonly int elitismo = 20;

    public static readonly int aleatoriosInsertados = 50;

	public static readonly int numHijos = 130;

	public static readonly double probabilidadMutacion = 0.3;

	public static readonly int numGeneraciones = 15;

	public static readonly int tamTorneo = 15;

}
