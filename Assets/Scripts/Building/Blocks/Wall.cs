using UnityEngine;

public class Wall : Block
{
    public override BlockType Type => BlockType.Wall;

    public override bool CanFitWith(Block other)
    {
        if (other.Type == BlockType.Foundation)
        {
            var diff = transform.position - other.transform.position;

            if (Mathf.Abs(diff.y) > Mathf.Abs(diff.z) && Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
            {
                var shift = Vector3.zero;

                if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
                {
                    shift = 0.5f * (other.transform.localScale.z - transform.localScale.z) *
                            transform.forward;
                }
                else
                {
                    shift = 0.5f * (other.transform.localScale.x - transform.localScale.z) *
                            transform.forward;
                }
                shift.y = 0.5f * (other.transform.localScale.y + transform.localScale.y);

                other.transform.position = transform.position + shift;
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
