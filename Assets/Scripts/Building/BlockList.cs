using UnityEngine;

namespace Building
{
    public class BlockList : MonoBehaviour
    {
        [SerializeField]
        private BlockItem[] _items;

        public BlockItem[] Items => _items;   
    }
}
