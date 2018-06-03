using UnityEngine;

namespace Building.Blocks
{
    public enum BlockType
    {
        Foundation,
        Wall
    }

    public abstract class Block : MonoBehaviour
    {
        //Instead of is/as
        public abstract BlockType Type { get; }

        public virtual bool CanBePlacedOn(Block other)
        {
            return other != null && Type != other.Type;
        }

        public abstract bool CanFitWith(Block other);

        //Can be used if fitting depends on several neighbor blocks
        public abstract bool CanFitWith(Block[] others);
    }
}