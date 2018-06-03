using System;
using System.Linq;
using Building.Blocks;
using UnityEngine;

namespace Building
{
    public class Builder : MonoBehaviour
    {
        private BlockItem _currentbuildingItem;
        private Block _block;
        private MeshRenderer _buildingRenderer;

        [SerializeField]
        private Color _permissionColor;
        [SerializeField]
        private Color _refuseColor;
        [SerializeField]
        private float _normalThreshold = 0.95f;
        [SerializeField]
        private int _blocksLayer;    
        [SerializeField]
        private int _terrainLayer;

        public void StartBuilding(BlockItem blockItem)
        {
            if (_block != null)
            {
                Destroy(_block.gameObject);
                _block = null;
            }

            _currentbuildingItem = blockItem;

            _block = Instantiate(_currentbuildingItem.Prefab);
            _buildingRenderer = _block.GetComponent<MeshRenderer>();
        }

        public void ResetBuilding()
        {
            if (_block != null)
            {
                Destroy(_block.gameObject);
                _block = null;
            }

            _currentbuildingItem = null;        
        }

        public bool PlaceBlock(RaycastHit raycastHit)
        {        
            //If user try to build block on hill then refuse.
            //'raycastHit.normal.y < _normalThreshold' is the same as 'Vector3.Dot(raycastHit.normal, Vector3.up) < _normalThreshold',
            //because 'raycastHit.normal' is normalized
            if (raycastHit.normal.y < _normalThreshold)
            {
                return FillPreview(false);
            }

            _block.transform.position = raycastHit.point + 0.5f * _block.transform.localScale.y * raycastHit.normal;
            _block.transform.up = raycastHit.normal;

            //Get heighbour blocks
            Collider[] sphereIntersections = Physics.OverlapSphere(raycastHit.point, _block.transform.localScale.magnitude, 1 << _blocksLayer);
            if (sphereIntersections.Length != 0)
            {
                Func<Collider, float> getDistance = col => Vector3.Distance(col.transform.position, raycastHit.point);
                //Get block with minimum distance to current block
                var nearestBlock = sphereIntersections
                    .Aggregate((col1, col2) => getDistance(col1) < getDistance(col2) ? col1 : col2)
                    .gameObject.GetComponent<Block>();

                var canBeFitted = nearestBlock.CanFitWith(_block);
                return FillPreview(canBeFitted);
            }

            var canBePlacedOn = CanBePlacedOn(raycastHit.transform.gameObject);
            return FillPreview(canBePlacedOn);
        }

        public void Build()
        {
            _buildingRenderer.material.color = Color.white;
            _block.gameObject.layer = _blocksLayer;
            _block.transform.rotation = Quaternion.Euler(0f, _block.transform.eulerAngles.y, 0f);
            _currentbuildingItem = null;
            _block = null;
        }

        private bool FillPreview(bool canBePlaced)
        {
            _buildingRenderer.material.color = canBePlaced ? _permissionColor : _refuseColor;
            return canBePlaced;
        }

        private bool CanBePlacedOn(GameObject otherObject)
        {
            if (otherObject.layer == _terrainLayer)
            {
                return _block.Type == BlockType.Foundation;
            }

            var otherBlock = otherObject.GetComponent<Block>();
            return _block.CanBePlacedOn(otherBlock);
        }
    }
}
