using Camera;
using UnityEngine;

namespace Connection
{
    public class NodeBoundsCalculator
    {
        public NodeBoundsCalculator(ColorNode[] nodes)
        {
            CalculateBounds(nodes);
        }

        private void CalculateBounds(ColorNode[] nodes)
        {
            Vector2 minBounds = Vector2.positiveInfinity;
            Vector2 maxBounds = Vector2.negativeInfinity;

            foreach (var colorNode in nodes)
            {
                var position = colorNode.transform.position;

                minBounds = Vector2.Min(minBounds, position);
                maxBounds = Vector2.Max(maxBounds, position);
            }

            var boundsStorage = CameraController.GetBoundsStorage();
            if (boundsStorage != null)
            {
                boundsStorage.UpdateBounds(minBounds, maxBounds);
            }
            else
            {
                Debug.LogWarning("CameraController или BoundsStorage не найдены.");
            }
        }
    }
}