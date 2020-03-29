using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace act.ui
{
    [SerializeField]
    public class Touzi : MonoBehaviour
    {
        enum RollStarte
        {
            None = 0,
            Roll,
            Stop,
        };

        public int NumToShow = 1;
        [SerializeField] Vector2 rollPos = Vector2.zero;
        [SerializeField] int m_pip = -1;
        [SerializeField] Vector2 rollVec = Vector2.one * 200f;
        public int pip
        {
            get
            {
                return m_pip;
            }
        }
        RollStarte state;
        Animator anm;
        Rigidbody2D rb;
        AudioSource ac;
        bool toRoll;
        CallBack resetTouzi;
        CallBack continueCheck;
        // Use this for initialization
        void Awake()
        {
            anm = GetComponent<Animator>();
            ac = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody2D>();
            rb.isKinematic = true;
            toRoll = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ac.Play();
            if(rb.velocity.sqrMagnitude < 10f)
            {
                SetPip(NumToShow);
            }
        }

        public void rollTouZi(float num,CallBack continueCheck,CallBack resetTouzi)
        {
            NumToShow = (int)num;
            state = RollStarte.Roll;
            rb.isKinematic = false;
            rb.velocity = Vector2.zero;
            rb.AddForce(rollVec);
            Roll();
            this.resetTouzi = resetTouzi;
            this.continueCheck = continueCheck;
        }

        //鉴定完毕,执行回调
        public void CallBack()
        {
            resetTouzi?.Invoke();
            continueCheck?.Invoke();
            (transform as RectTransform).localPosition = rollPos;
        }
        public void OnRollButton()
        {
            toRoll = true;
        }

        /// <summary>
        /// Start roll animation
        /// </summary>
        public void Roll()
        {
            anm.Play("roll");
        }

        /// <summary>
        /// Set pip and start pip animation
        /// </summary>
        /// <param name="_pip">pip of dice</param>
        public void SetPip(int _pip)
        {
            m_pip = _pip;
            anm.Play("to" + m_pip.ToString());
        }
    }
}
