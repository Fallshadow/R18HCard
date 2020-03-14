using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using act;
using act.game;
using act.ui;

public class Test : MonoBehaviour
{
    public enum TESTNAME
    {
        action,
        pushcard,
    }
    public TESTNAME testname = TESTNAME.action;
    public int id = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            switch (testname)
            {
                case TESTNAME.action:
                    act.game.ModelController.instance.ChangeAction((Action)id, false);
                    break;
                case TESTNAME.pushcard:

                    break;
                //case TESTNAME.action:
                //    break;
                default:
                    break;
            }

        }
    }
}
