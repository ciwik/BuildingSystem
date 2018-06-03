using UnityEngine;

public class Blocks : MonoBehaviour
{
    [SerializeField]
    private BlockItem[] _items;

    public BlockItem[] Items => _items;   
}
