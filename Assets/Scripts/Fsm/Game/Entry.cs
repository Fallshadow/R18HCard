namespace act
{
    namespace fsm
    {
        public class Entry<T> : State<T>
        {
            public override void Enter()
            {
                InitializeGame();
                m_fsm.SwitchToState((int)fsm.GameFsmState.MAINMENU);
            }

            public override void Exit()
            {
            }

            public override void Update()
            {
            }

            private void InitializeGame()
            {
                // 暫時先放在這邊讀取
                //localization.LocalizationManager.instance.LoadModule("example");
                //存在顺序依赖！！
                game.ExampleMgr.instance.InitConfigData();
                game.ConditionMgr.instance.InitConfigData();
                game.EffectMgr.instance.InitConfigData();
                game.GameFlowMgr.instance.InitConfigData();
                game.ProcessMgr.instance.InitConfigData();
                game.CardMgr.instance.InitConfigData();
                game.EventMgr.instance.InitConfigData();
            }
        }
    }
}