using UnityEngine;

public enum BlockType
{
    Foundation,
    Wall
}

public class Block : MonoBehaviour
{
    public BlockType Type => _type;

    [SerializeField]
    private BlockType _type;    
}
