﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelBusters.CoreLibrary.NativePlugins.DemoKit
{
	public class ConsoleRect : MonoBehaviour 
	{
        #region Properties

        //[SerializeField]
        //private Text m_text = null;

        [SerializeField]
        private     ScrollRect      m_textScroller = null;

		[SerializeField] public List<string> strArr = new List<string>();

		#endregion

		#region Unity methods

		private void Awake()
		{
			Reset();
		}

		#endregion

		#region Public methods

		public void Log(string message, bool append)
		{
			if (append)
			{
               // m_text.text    = m_text.text + "\n" + message;
				strArr.Add(message);
			}
            //else
            //{
            //  //  m_text.text = message;
            //    strArr.Add(message);
            //}

            StartCoroutine(MoveScrollerToBottom());
		}

        #endregion

        #region Private methods

        private IEnumerator MoveScrollerToBottom()
        {
            yield return null;
                 
            // set position
            m_textScroller.verticalNormalizedPosition   = 0;
        }

		private void Reset()
		{
            //if (m_text)
            //{
            //    m_text.text = "Console";
            //}
    //        if (strArr != null)
    //        {
				//strArr = null;
    //        }
		}

		#endregion
	}

}