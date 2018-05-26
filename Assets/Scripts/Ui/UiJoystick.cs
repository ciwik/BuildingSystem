using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ui
{
    [RequireComponent(typeof(Image))]
    public class UiJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        private Image _background, _knob;
        private float _knobHalfSize, _sqrInnerRadius,_sqrOutterRadius;

        public Vector3 Direction { get; private set; }
        public bool IsFocused { get; private set; }

        public void Awake()
        {
            _background = GetComponent<Image>();
            _knob = transform.GetChild(0).GetComponent<Image>();

            Direction = Vector3.zero;
        }

        public void Start()
        {
            _knobHalfSize = 0.5f * _knob.rectTransform.rect.height / _background.rectTransform.rect.height;
            _sqrInnerRadius = Mathf.Pow(0.5f - _knobHalfSize, 2);
            _sqrOutterRadius = Mathf.Pow(0.5f + 2 * _knobHalfSize, 2);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 rectPosition;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_background.rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out rectPosition))
            {
                return;
            }

            Vector2 position = new Vector2(rectPosition.x / _background.rectTransform.rect.width,
                rectPosition.y / _background.rectTransform.rect.height);

            if (position.sqrMagnitude <= _sqrOutterRadius)
            {
                Direction = 2 *new Vector3(position.x,
                                0,
                                position.y);            

                Vector2 knobPosition;
                if (position.sqrMagnitude <= _sqrInnerRadius)
                {
                    knobPosition = _background.rectTransform.rect.center +
                                   position * _background.rectTransform.rect.height;
                }
                else
                {
                    knobPosition = _background.rectTransform.rect.center +
                                   position.normalized * (0.5f - _knobHalfSize) * _background.rectTransform.rect.height;
                }
                _knob.rectTransform.localPosition = knobPosition;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
            IsFocused = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _knob.rectTransform.localPosition = _background.rectTransform.rect.center;
            Direction = Vector3.zero;
            IsFocused = false;
        }
    }
}
