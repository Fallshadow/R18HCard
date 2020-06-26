using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using act.game;

namespace act.data
{
    [Serializable]
    public class SaveData
    {
        public int RoundNum = 1;
        public int HP = 100;
        public float process = 0;
        public EventInst curEvent;
        public CardInst curCard;
        public List<bool> curEventResults = new List<bool>();
        public List<EventInst> eventInsts = new List<EventInst>();
        public List<EventInst> hadSolvecardInsts = new List<EventInst>();
        public List<CardInst> cardInsts = new List<CardInst>();
        public List<CardInst> hadUsecardInsts = new List<CardInst>();
    }
}