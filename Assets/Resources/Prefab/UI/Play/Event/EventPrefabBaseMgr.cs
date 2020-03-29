using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.ui
{


    public class EventPrefabBaseMgr : MonoBehaviour
    {
        private EventDisplay ed = null;
        private void Start()
        {
            ed = GetComponentInChildren<EventDisplay>();
        }

        public void DestoryShow()
        {
            ed.DestoryEvent();
        }
    }
}