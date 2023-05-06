using UnityEngine;

using TMPro;


namespace Drop
{
    public class Drop_MessageController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textField;

        private string[] _helpMessages = new string[] { "Q/E" };

        private float _expireTimer;
        private const float FadeTimer = 2;
        private const float ExpireTimerMax = 5;

        private void Awake()
        {
            GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        }

        private void FixedUpdate()
        {
            _expireTimer = Mathf.Max(_expireTimer - Time.deltaTime, 0);

            Color tempColor = _textField.color;
            tempColor.a = _expireTimer / FadeTimer;

            _textField.color = tempColor;
        }

        public void ShowMessage(string message)
        {
            _expireTimer = ExpireTimerMax;

            _textField.text = message;
        }

        public void ShowHelpMessage(int messageID)
        {
            ShowMessage(_helpMessages[messageID]);
        }
    }
}