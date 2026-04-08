using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ezAR.UI
{
    /// <summary>
    /// 按钮动画效果
    /// 悬停放大、按下缩小、点击脉冲
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Animation Settings")]
        [SerializeField] private float hoverScale = 1.1f;
        [SerializeField] private float pressScale = 0.95f;
        [SerializeField] private float animationDuration = 0.15f;

        [Header("Color Settings")]
        [SerializeField] private bool useColorAnimation = true;
        [SerializeField] private Color hoverColor = new Color(0.3f, 0.7f, 0.9f);
        [SerializeField] private Color pressColor = new Color(0.15f, 0.5f, 0.7f);

        private Vector3 originalScale;
        private Color originalColor;
        private Image buttonImage;
        private Button button;
        private Coroutine currentAnimation;
        private bool isHovering = false;
        private bool isPressed = false;

        private void Awake()
        {
            button = GetComponent<Button>();
            buttonImage = GetComponent<Image>();
            originalScale = transform.localScale;

            if (buttonImage != null)
            {
                originalColor = buttonImage.color;
            }

            Debug.Log($"[ButtonAnimation] Awake on {gameObject.name}, Button: {button != null}, Image: {buttonImage != null}");
        }

        private void OnEnable()
        {
            // 重置状态
            transform.localScale = originalScale;
            if (buttonImage != null)
            {
                buttonImage.color = originalColor;
            }
            isHovering = false;
            isPressed = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("[ButtonAnimation] OnPointerEnter");
            if (!button.interactable) return;

            isHovering = true;
            if (!isPressed)
            {
                AnimateTo(hoverScale, hoverColor);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("[ButtonAnimation] OnPointerExit");
            isHovering = false;
            if (!isPressed)
            {
                AnimateTo(1f, originalColor);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("[ButtonAnimation] OnPointerDown");
            if (!button.interactable) return;

            isPressed = true;
            AnimateTo(pressScale, pressColor);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("[ButtonAnimation] OnPointerUp");
            isPressed = false;

            if (isHovering)
            {
                AnimateTo(hoverScale, hoverColor);
            }
            else
            {
                AnimateTo(1f, originalColor);
            }
        }

        /// <summary>
        /// 播放点击脉冲动画（可由外部调用）
        /// </summary>
        public void PlayClickPulse()
        {
            if (currentAnimation != null)
            {
                StopCoroutine(currentAnimation);
            }
            currentAnimation = StartCoroutine(PulseAnimation());
        }

        private void AnimateTo(float targetScale, Color targetColor)
        {
            if (currentAnimation != null)
            {
                StopCoroutine(currentAnimation);
            }
            currentAnimation = StartCoroutine(AnimateCoroutine(targetScale, targetColor));
        }

        private IEnumerator AnimateCoroutine(float targetScale, Color targetColor)
        {
            float elapsed = 0f;
            Vector3 startScale = transform.localScale;
            Vector3 endScale = originalScale * targetScale;

            Color startColor = buttonImage != null ? buttonImage.color : Color.white;
            Color endColor = useColorAnimation ? targetColor : startColor;

            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / animationDuration;

                // 使用缓动函数
                t = EaseOutQuad(t);

                transform.localScale = Vector3.Lerp(startScale, endScale, t);

                if (buttonImage != null && useColorAnimation)
                {
                    buttonImage.color = Color.Lerp(startColor, endColor, t);
                }

                yield return null;
            }

            transform.localScale = endScale;
            if (buttonImage != null && useColorAnimation)
            {
                buttonImage.color = endColor;
            }

            currentAnimation = null;
        }

        private IEnumerator PulseAnimation()
        {
            // 快速放大再缩回
            yield return AnimateToScale(1.2f, 0.1f);
            yield return AnimateToScale(1f, 0.15f);
        }

        private IEnumerator AnimateToScale(float targetScale, float duration)
        {
            float elapsed = 0f;
            Vector3 startScale = transform.localScale;
            Vector3 endScale = originalScale * targetScale;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                t = EaseOutQuad(t);
                transform.localScale = Vector3.Lerp(startScale, endScale, t);
                yield return null;
            }

            transform.localScale = endScale;
        }

        /// <summary>
        /// 缓动函数：二次方缓出
        /// </summary>
        private float EaseOutQuad(float t)
        {
            return 1f - (1f - t) * (1f - t);
        }
    }
}
