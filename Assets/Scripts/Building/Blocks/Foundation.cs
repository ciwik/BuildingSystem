using UnityEngine;

namespace Building.Blocks
{
    public class Foundation : Block
    {
        public override BlockType Type => BlockType.Foundation;

        public override bool CanFitWith(Block other)
        {
            if (other.Type == BlockType.Foundation)
            {
                var diff = transform.position - other.transform.position;

                if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
                {
                    other.transform.position = transform.position - Mathf.Sign(diff.x) *
                                               0.5f * (transform.localScale.x + other.transform.localScale.x) *
                                               transform.right;
                    return true;
                }
                else
                {
                    other.transform.position = transform.position - Mathf.Sign(diff.z) *
                                               0.5f * (transform.localScale.z + other.transform.localScale.z) *
                                               transform.forward;
                    return true;
                }
            }

            if (other.Type == BlockType.Wall)
            {
                var diff = transform.position - other.transform.position;

                if (Mathf.Abs(diff.y) > Mathf.Abs(diff.z) && Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
                {
                    var shift = Vector3.zero;
                    shift.y = -0.5f * (other.transform.localScale.y + transform.localScale.y);

                    if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
                    {
                        other.transform.rotation = Quaternion.Euler(0f, (diff.x > 0) ? 90f : 270f, 0f);
                        shift.x = 0.5f * Mathf.Sign(diff.x) *
                                  (transform.localScale.x - other.transform.localScale.z);
                    }
                    else
                    {
                        other.transform.rotation = Quaternion.Euler(0f, (diff.z > 0) ? 0f : 180f, 0f);
                        shift.z = 0.5f * Mathf.Sign(diff.z) *
                                  (transform.localScale.z - other.transform.localScale.z);
                    }

                    other.transform.position = transform.position - shift;
                    return true;
                }

                return false;
            }

            return false;
        }

        public override bool CanFitWith(Block[] others)
        {
            throw new System.NotImplementedException();
        }
    }
}
