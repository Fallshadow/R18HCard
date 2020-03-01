using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.game
{
    public class LevelManager : Singleton<LevelManager>
    {
        public float ProcessValue
        {
            get { return processValue; }
            set { processValue = value; }
        }
        private float processValue;
    }
}