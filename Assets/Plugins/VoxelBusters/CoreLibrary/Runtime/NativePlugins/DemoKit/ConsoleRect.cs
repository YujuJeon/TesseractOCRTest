using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelBusters.CoreLibrary.NativePlugins.DemoKit
{
	public class ConsoleRect : MonoBehaviour 
	{
		[SerializeField] private ScrollRect	m_textScroller = null;
		[SerializeField] public List<string> strArr = new List<string>();

		private void Awake()
		{
			Reset();
		}

        public void Log(string message, bool append)
        {
            if (append)
            {
                strArr.Add(message);
            }

            StartCoroutine(MoveScrollerToBottom());
        }

        private IEnumerator MoveScrollerToBottom()
        {
            yield return null;
                 
            // set position
            m_textScroller.verticalNormalizedPosition   = 0;
        }

		private void Reset()
		{
            if (strArr != null)
            {
				strArr.Clear();
            }
		}
	}
}
