using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Controllers
{
    public class BallController : MonoBehaviour
    {
        public event Action<string> OnBallBumped = _ => { };

        [SerializeField] private float minStartAngle;
        [SerializeField] private float maxStartAngle;

        [SerializeField] private float speedMultiplier;
        [SerializeField] private float rotatingSpeed;

        [SerializeField] private string barrierTag;

        [SerializeField] private AnimationCurve movingSpeedCurve;

        [SerializeField] private Rigidbody rb;

        [SerializeField] private bool debugHighlight;

        private float _startTime = 0f;
        private float _currentRotation = 0f;

        private Vector3 _velocity = Vector3.zero;

        private void SetStartVelocity()
        {
            var angle = Random.Range(minStartAngle, maxStartAngle);

            _velocity = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));
        }

        private void Start()
        {
            SetStartVelocity();
        }

        private void Update()
        {
            var speed = movingSpeedCurve.Evaluate(Time.time - _startTime);
            rb.velocity = _velocity * speed * speedMultiplier;

            _currentRotation += speed;

            transform.forward = _velocity;
            transform.Rotate(Vector3.right * _currentRotation * rotatingSpeed, Space.Self);
        }

        private void OnCollisionEnter(Collision collision)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                _velocity = player.GetBallDirectionOnPoint(collision.contacts.First().point);
            }
            else
            {
                _velocity = Vector3.Reflect(_velocity, collision.contacts.First().normal);

                if (collision.gameObject.CompareTag(barrierTag))
                {
                    _startTime = Time.time;
                }
            }

            OnBallBumped.Invoke(collision.gameObject.tag);
        }

        private void OnDrawGizmos()
        {
            var minVector = new Vector3(Mathf.Cos(minStartAngle * Mathf.Deg2Rad), 0, Mathf.Sin(minStartAngle * Mathf.Deg2Rad));
            var maxVector = new Vector3(Mathf.Cos(maxStartAngle * Mathf.Deg2Rad), 0, Mathf.Sin(maxStartAngle * Mathf.Deg2Rad));

            Gizmos.color = Color.green;

            Gizmos.DrawLine(transform.position, transform.position + minVector);
            Gizmos.DrawLine(transform.position, transform.position + maxVector);
        }
    }
}
