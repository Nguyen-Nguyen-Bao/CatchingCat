using UnityEngine;

[CreateAssetMenu(fileName = "GameManager", menuName = "Scriptable Objects/GameManager")]
public class GameManager : ScriptableObject
{
    public bool clicking;
    public Vector3 move_dir;
    public bool grounded;
}
