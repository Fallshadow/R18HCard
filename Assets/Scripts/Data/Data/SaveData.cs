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
        public int HP = 6;
        public float process = 0;
        public int num21 = 0;
        public EventInst curEvent;
        public CardInst curCard;
        public List<bool> curEventResults = new List<bool>();
        public List<EventInst> eventInsts = new List<EventInst>();
        public List<EventInst> hadSolvecardInsts = new List<EventInst>();
        public List<CardInst> cardInsts = new List<CardInst>();
        public List<CardInst> hadUsecardInsts = new List<CardInst>();

        public bool processE29Time = false;

        public bool processTwo = false;
        public int vit = 0;
        public int pleasant = 0;

        public bool FirstVit0 = true;
        public bool SecondVit0 = true;
        public bool ThrVit0 = true;
        public bool FourVit0 = true;
        public bool FirstPlea0 = true;
        public bool SecondPlea0 = true;
        public bool ThrPlea0 = true;
        public bool FourPlea0 = true;

        public float musicVoice = 0.5f;
        public float envirVoice = 0.2f;
        public float soundVoice = 1;
        public bool isPlayNewPlayer = true;
    }
}