
using Microsoft.Xna.Framework;

public class Timer : TDComponent
{
    public TDText TimerText;

    public System.Action OnFinishedTimer;

    private TDAction _timerAction;

    public override void Initialize()
    {
        base.Initialize();

        TimerText.Text = "3";
        TimerText.IsShowing = false;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (TimerText.IsShowing)
        {
            foreach(TDInput input in TDInputManager.PlayerInputs)
            {
                if (input.CancelPressed())
                {
                    if (TDSceneManager.ActiveScene is MenuScene)
                    {
                        for (int i = 0; i < MenuUIManager.Instance.MainMenu.CharacterDisplays.Length; i++)
                        {
                            MenuUIManager.Instance.MainMenu.CharacterDisplays[0].Cooldown = MenuUIManager.Instance.MainMenu.CharacterDisplays[0].CooldownTimer;
                        }
                    }
                    StopTimer();
                }
            }
        }
    }

    public void StartTimer()
    {
        TimerText.Text = "3";
        TimerText.TDObject.RectTransform.Origin = .5f * new Vector2(TimerText.Width, TimerText.Height);
        TimerText.IsShowing = true;

        _timerAction = TimerText.TDObject.RunAction(1f, (p) => { }, () =>
        {
            TimerText.Text = "2";
            TimerText.TDObject.RectTransform.Origin = .5f * new Vector2(TimerText.Width, TimerText.Height);
            _timerAction = TimerText.TDObject.RunAction(1f, (p) => { }, () =>
            {
                TimerText.Text = "1";
                TimerText.TDObject.RectTransform.Origin = .5f * new Vector2(TimerText.Width, TimerText.Height);
                _timerAction = TimerText.TDObject.RunAction(1f, (p) => { }, () =>
                {
                    TimerText.IsShowing = false;
                    OnFinishedTimer?.Invoke();
                });
            });
        });
    }

    public void StopTimer()
    {
        _timerAction.Destroy();
        TimerText.IsShowing = false;
    }
}