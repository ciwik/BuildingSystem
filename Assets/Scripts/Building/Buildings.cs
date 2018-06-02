using UnityEngine;

public class Buildings : MonoBehaviour
{
    [SerializeField]
    private BuildingItem[] _items;

    public BuildingItem[] Items => _items;   
}
