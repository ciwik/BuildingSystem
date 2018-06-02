using UnityEngine;

public class Buildings : MonoBehaviour
{
    [SerializeField]
    private BlockItem[] _items;

    public BlockItem[] Items => _items;   
}
