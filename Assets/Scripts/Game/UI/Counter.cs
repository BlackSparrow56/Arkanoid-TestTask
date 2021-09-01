using UnityEngine;
using TMPro;
using Game.Controllers;

namespace Game.UI
{
    public class Counter : MonoBehaviour
    {
        [SerializeField] private string bitTag;
        [SerializeField] private string boardTag;

        [SerializeField] private TMP_Text counterText;

        [SerializeField] private BallController targetBall;

        private int _bit = 0;
        private int _board = 0;

        private void UpdateCounter()
        {
            counterText.text = $"Bit: {_bit}\nBoard: {_board}";
        }

        private void Bump(string tag)
        {
            if (tag == bitTag)
            {
                _bit++;
                UpdateCounter();
            }
            else if (tag == boardTag)
            {
                _board++;
                UpdateCounter();
            }
        }

        private void Start()
        {
            UpdateCounter();
        }

        private void OnEnable()
        {
            targetBall.OnBallBumped += Bump;
        }

        private void OnDisable()
        {
            targetBall.OnBallBumped -= Bump;
        }
    }
}
