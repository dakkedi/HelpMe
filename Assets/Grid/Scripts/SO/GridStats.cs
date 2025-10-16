using UnityEngine;

[CreateAssetMenu(fileName = "GridStats", menuName = "Scriptable Objects/GridStats")]
public class GridStats : ScriptableObject
{
    public int GridWidth = 30;
    public int GridHeight = 15;
    public int CellSize = 1;
}
