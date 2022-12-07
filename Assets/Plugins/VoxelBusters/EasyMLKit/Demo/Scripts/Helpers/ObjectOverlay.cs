using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelBusters.EasyMLKit.Demo
{
    public class ObjectOverlay : MonoBehaviour
    {
        private Rect m_rect;
        private Texture2D texture;

        private RectTransform m_rectTransform;
        //private TextRecognizerDemo

        [SerializeField]
        private Text m_label;
        private TextMeshProUGUI selectedText;
        Rect m_transformedScreenRect;
        

        private void Awake()
        {
            m_rectTransform = transform as RectTransform;
            selectedText = GameObject.Find("clickedText").GetComponent<TextMeshProUGUI>();
        }

        public void SetRect(RectTransform canvasRect, Rect screenRect)
        {
            m_transformedScreenRect = screenRect;
            m_transformedScreenRect.center = new Vector2(m_transformedScreenRect.center.x, Screen.height - screenRect.center.y);
            Vector2 minPosition, maxPosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, m_transformedScreenRect.min, null, out minPosition);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, m_transformedScreenRect.max, null, out maxPosition);

            m_rectTransform.anchoredPosition = minPosition + ((maxPosition - minPosition) / 2.0f);
            m_rectTransform.sizeDelta = new Vector2(maxPosition.x - minPosition.x, maxPosition.y - minPosition.y);
        }

        public void SetLabel(string labelText)
        {
            m_label.text = labelText;
        }

        public void OnClickBoundingBox()
        {
            selectedText.text = m_label.text;
        }

        public void ClearOverlayText()
        {
            selectedText.text = "";
        }
    }
}