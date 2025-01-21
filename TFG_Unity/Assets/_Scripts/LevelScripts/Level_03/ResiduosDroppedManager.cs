/// <summary>
/// Clase que se encarga de almacenar que residuos ya han sido 
/// desechados en los contenedores de manera exitosa
/// </summary>
public static class ResiduosDroppedManager
{
    public static bool[] ResiduosDropped = { false, false, false };

    public static int CurrentResiduoIdx = -1;
}
