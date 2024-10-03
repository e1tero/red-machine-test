using System;
using UnityEngine;

namespace Camera
{
    public class BoundsStorage
    {
        public Vector2 MinBounds { get; private set; }
        public Vector2 MaxBounds { get; private set; }

        public event Action<Vector2, Vector2> OnBoundsUpdated;

        public void UpdateBounds(Vector2 minBounds, Vector2 maxBounds)
        {
            MinBounds = minBounds;
            MaxBounds = maxBounds;
            OnBoundsUpdated?.Invoke(MinBounds, MaxBounds);
        }
    }
}