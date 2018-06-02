using UnityEngine;

namespace Assets.Scripts.Building
{
    public static class FitValidator
    {
        public static void Fit(Block placedObject, Block newObject)
        {
            if (placedObject.Type == BlockType.Foundation && newObject.Type == BlockType.Foundation)
            {
                FitFoundations(placedObject, newObject);
            }
            if (placedObject.Type == BlockType.Wall && newObject.Type == BlockType.Wall)
            {
                FitWalls(placedObject, newObject);
            }
            if (placedObject.Type == BlockType.Foundation && newObject.Type == BlockType.Wall)
            {
                FitFoundationWall(placedObject, newObject);
            }
            if (placedObject.Type == BlockType.Wall && newObject.Type == BlockType.Foundation)
            {
                FitWallFoundation(placedObject, newObject);
            }
        }

        private static void FitFoundations(Block placedFoundation, Block newFoundation)
        {
            var diff = placedFoundation.transform.position - newFoundation.transform.position;

            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
            {
                diff.x = Mathf.Sign(diff.x) * newFoundation.transform.localScale.x;
                diff.y = 0f;
                diff.z = 0f;
            } else 
            {
                diff.x = 0f;
                diff.y = 0f;
                diff.z = Mathf.Sign(diff.z) * newFoundation.transform.localScale.z;
            }

            newFoundation.transform.position = placedFoundation.transform.position - diff;
        }

        private static void FitWalls(Block placedWall, Block newWall)
        {
            var diff = placedWall.transform.position - newWall.transform.position;

            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
            {
                diff.x = Mathf.Sign(diff.x) * newWall.transform.localScale.x;
                diff.y = 0f;
                diff.z = 0f;
            }

            newWall.transform.position = placedWall.transform.position - diff;
        }

        private static void FitFoundationWall(Block placedFoundation, Block newWall)
        {
            var diff = placedFoundation.transform.position - newWall.transform.position;

            if (Mathf.Abs(diff.y) > Mathf.Abs(diff.z) && Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
            {
                diff.y = -0.5f * (newWall.transform.localScale.y + placedFoundation.transform.localScale.y);

                if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
                {
                    newWall.transform.rotation = Quaternion.Euler(0f, (diff.x > 0) ? 90f : 270f, 0f);
                    diff.x = 0.5f * Mathf.Sign(diff.x) * (placedFoundation.transform.localScale.x - newWall.transform.localScale.z);
                    diff.z = 0f;
                }
                else
                {
                    newWall.transform.rotation = Quaternion.Euler(0f, (diff.z > 0) ? 0f : 180f, 0f);
                    diff.x = 0f;
                    diff.z = 0.5f * Mathf.Sign(diff.z) * (placedFoundation.transform.localScale.z - newWall.transform.localScale.z);
                }
            }

            newWall.transform.position = placedFoundation.transform.position - diff;
        }

        private static void FitWallFoundation(Block placedWall, Block newFoundation)
        {
            var diff = placedWall.transform.position - newFoundation.transform.position;

            if (Mathf.Abs(diff.y) > Mathf.Abs(diff.z) && Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
            {
                if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
                {
                    diff = 0.5f * (newFoundation.transform.localScale.z - placedWall.transform.localScale.z) * placedWall.transform.forward;
                }
                else
                {
                    diff = 0.5f * (newFoundation.transform.localScale.x - placedWall.transform.localScale.z) * placedWall.transform.forward;
                }

                diff.y = 0.5f * (newFoundation.transform.localScale.y + placedWall.transform.localScale.y);
            }

            newFoundation.transform.position = placedWall.transform.position + diff;
        }
    }
}
