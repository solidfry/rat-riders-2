using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using UnityEngine.UI;

namespace UI
{


    public class RageSliderUI : MonoBehaviour
    {

        [SerializeField] private Slider slider;
        [SerializeField] private float updateSpeedSeconds = 0.5f;

        [Header("Slider settings")]
        [SerializeField] private Color sliderWarningColor = Color.red;

        [SerializeField] private float sliderWarningValue = 0.3f;

        private void Awake()
        {
            if (slider == null)
                slider = GetComponentInChildren<Slider>();
        }

        private void OnEnable()
        {
            GameEvents.onChangeRageUIEvent += ChangeSlider;
        }

        private void OnDisable()
        {
            GameEvents.onChangeRageUIEvent -= ChangeSlider;
        }

        private void ChangeSlider(float normalisedValue)
        {
            StartCoroutine(AnimateSlider(normalisedValue));
        }

        private IEnumerator AnimateSlider(float normalisedValue)
        {
            Debug.Log("Coroutine is running to animate slider");
            float preChangedPercent = slider.value;
            float elapsed = 0f;
            while (elapsed < updateSpeedSeconds)
            {
                elapsed += Time.deltaTime;
                slider.value = Mathf.Lerp(preChangedPercent, normalisedValue, elapsed / updateSpeedSeconds);
                if (slider.value <= sliderWarningValue)
                    slider.fillRect.gameObject.GetComponent<Image>().color = sliderWarningColor;

                yield return null;
            }
            slider.value = normalisedValue;
        }
    }
}
