using System;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private int count;
        [SerializeField]
        [ReadOnly]
        private int maxCount;

        public Sprite empty, full;
        public List<Image> images = new();

        [SerializeField]
        private GameObject objPrefab;

        private void Awake()
        {
            var testImages = GetComponentsInChildren<Image>();
            foreach (var toRemove in testImages)
            {
                Destroy(toRemove.gameObject);
            }
        }

        private void OnEnable()
        {
            GameEvents.onSetHealthCountEvent += SetUp;
            GameEvents.onHealthChangeEvent += ChangeCount;
        }

        private void OnDisable()
        {
            GameEvents.onSetHealthCountEvent -= SetUp;
            GameEvents.onHealthChangeEvent -= ChangeCount;
        }

        void ChangeCount(int currentCount)
        {
            count = currentCount;
            if (currentCount <= maxCount)
                for (int i = 0; i < images.Count; i++)
                {
                    images[i].sprite = i < count ? full : empty;
                }
        }

        void SetUp(int loopCount)
        {
            count = loopCount;
            maxCount = loopCount;
            Debug.Log($"Object max count is {count}");
            for (int i = 0; i < loopCount; i++)
            {
                GameObject obj = Instantiate(objPrefab, transform);
                images.Add(obj.GetComponentInChildren<Image>());
            }
        }
    }
}