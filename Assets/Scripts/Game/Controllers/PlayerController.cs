using UnityEngine;

namespace Game.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float minBounceAngle;
        [SerializeField] private float maxBounceAngle;

        [SerializeField] private Vector3 minBitCorner;
        [SerializeField] private Vector3 maxBitCorner;

        [SerializeField] private Vector3 minMovementCorner;
        [SerializeField] private Vector3 maxMovementCorner;

        [SerializeField] private Camera worldCamera;
        [SerializeField] private GameObject playerObject;

        [SerializeField] private bool debugHighlight;

        public Vector3 GetBallDirectionOnPoint(Vector3 point)
        {
            var localMin = transform.TransformPoint(minBitCorner);
            var localMax = transform.TransformPoint(maxBitCorner);

            float magnitudeToMin = (point - localMin).magnitude;
            float magnitudeToMax = (point - localMax).magnitude;

            float sum = magnitudeToMin + magnitudeToMax;

            float ratio = magnitudeToMax / sum;

            float angle = Mathf.Lerp(minBounceAngle, maxBounceAngle, ratio);

            var direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));

            return direction;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var screenMin = worldCamera.WorldToScreenPoint(minMovementCorner);
                var screenMax = worldCamera.WorldToScreenPoint(maxMovementCorner);

                var average = Vector3.Lerp(screenMin, screenMax, 0.5f);

                var delta = screenMax - screenMin;

                if ((average - Input.mousePosition).magnitude > delta.magnitude / 1.9f) return;

                var ratio = (Input.mousePosition - screenMin).magnitude / delta.magnitude;

                playerObject.transform.position = Vector3.Lerp(minMovementCorner, maxMovementCorner, ratio);
            }
        }

        private void OnDrawGizmos()
        {
            if (!debugHighlight) return;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(minMovementCorner, maxMovementCorner);

            var localMin = transform.TransformPoint(minBitCorner);
            var localMax = transform.TransformPoint(maxBitCorner);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(localMin, localMax);
        }
    }
}
