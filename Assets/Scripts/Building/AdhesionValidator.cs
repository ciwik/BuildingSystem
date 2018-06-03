using UnityEngine;

namespace Building
{
    public static class AdhesionValidator
    {
        public static bool TryFit(Block placedObject, Block newObject)
        {
            if (placedObject.Type == BlockType.Foundation && newObject.Type == BlockType.Foundation)
            {
                return TryFitFoundations(placedObject, newObject);
            }
            if (placedObject.Type == BlockType.Wall && newObject.Type == BlockType.Wall)
            {
                return TryFitWalls(placedObject, newObject);
            }
            if (placedObject.Type == BlockType.Foundation && newObject.Type == BlockType.Wall)
            {
                return TryFitFoundationWall(placedObject, newObject);
            }
            if (placedObject.Type == BlockType.Wall && newObject.Type == BlockType.Foundation)
            {
                return TryFitWallFoundation(placedObject, newObject);
            }

            return false;
        }

        private static bool TryFitFoundations(Block placedFoundation, Block newFoundation)
        {
            var diff = placedFoundation.transform.position - newFoundation.transform.position;

            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
            {
                newFoundation.transform.position = placedFoundation.transform.position - Mathf.Sign(diff.x) *
                                                   0.5f * (placedFoundation.transform.localScale.x + newFoundation.transform.localScale.x) *
                                                   placedFoundation.transform.right;
                return true;
            } else 
            {
                newFoundation.transform.position = placedFoundation.transform.position - Mathf.Sign(diff.z) *
                                                   0.5f * (placedFoundation.transform.localScale.z + newFoundation.transform.localScale.z) *
                                                   placedFoundation.transform.forward;
                return true;
            }
        }

        private static bool TryFitWalls(Block placedWall, Block newWall)
        {
            var diff = placedWall.transform.position - newWall.transform.position;

            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
            {
                newWall.transform.position = placedWall.transform.position - Mathf.Sign(diff.x) *
                                             0.5f * (placedWall.transform.localScale.x + newWall.transform.localScale.x) *
                                             placedWall.transform.right;
                return true;
            }

            return false;
        }

        private static bool TryFitFoundationWall(Block placedFoundation, Block newWall)
        {
            var diff = placedFoundation.transform.position - newWall.transform.position;

            if (Mathf.Abs(diff.y) > Mathf.Abs(diff.z) && Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
            {
                var shift = Vector3.zero;
                shift.y = -0.5f * (newWall.transform.localScale.y + placedFoundation.transform.localScale.y);

                if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
                {
                    newWall.transform.rotation = Quaternion.Euler(0f, (diff.x > 0) ? 90f : 270f, 0f);
                    shift.x = 0.5f * Mathf.Sign(diff.x) *
                              (placedFoundation.transform.localScale.x - newWall.transform.localScale.z);
                }
                else
                {
                    newWall.transform.rotation = Quaternion.Euler(0f, (diff.z > 0) ? 0f : 180f, 0f);
                    shift.z = 0.5f * Mathf.Sign(diff.z) *
                              (placedFoundation.transform.localScale.z - newWall.transform.localScale.z);
                }

                newWall.transform.position = placedFoundation.transform.position - shift;
                return true;
            }

            return false;            
        }

        private static bool TryFitWallFoundation(Block placedWall, Block newFoundation)
        {
            var diff = placedWall.transform.position - newFoundation.transform.position;

            if (Mathf.Abs(diff.y) > Mathf.Abs(diff.z) && Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
            {
                var shift = Vector3.zero;

                if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
                {
                    shift = 0.5f * (newFoundation.transform.localScale.z - placedWall.transform.localScale.z) *
                            placedWall.transform.forward;
                }
                else
                {
                    shift = 0.5f * (newFoundation.transform.localScale.x - placedWall.transform.localScale.z) *
                            placedWall.transform.forward;
                }
                shift.y = 0.5f * (newFoundation.transform.localScale.y + placedWall.transform.localScale.y);

                newFoundation.transform.position = placedWall.transform.position + shift;
                return true;
            }

            return false;
        }
    }
}
