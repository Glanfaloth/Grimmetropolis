using Microsoft.Xna.Framework;

public class SpeechBubble : TDComponent
{
    public TDSprite Background;
    public TDSprite SpeakerIcon;
    public TDText Message;
    public TDSprite ButtonIcon;

    public bool IsShowing = true;

    private float _time = .5f;

    public override void Initialize()
    {
        base.Initialize();

        Hide();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);


        if (IsShowing)
        {
            _time -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_time <= 0f)
            {
                foreach (TDInput input in TDInputManager.PlayerInputs)
                {
                    if (input.ActionPressed()) Hide();
                }
            }
        }
    }

    public void Show(string message = "")
    {
        if (IsShowing) return;
        IsShowing = true;

        _time = .5f;

        if (message != "")
        {
            Message.Text = message;
        }

        foreach (Player player in GameManager.Instance.Players)
        {
            player.ActiveInput = false;
        }

        Background.IsShowing = true;
        SpeakerIcon.IsShowing = true;
        Message.IsShowing = true;
        ButtonIcon.IsShowing = true;
    }

    public void Hide()
    {
        if (!IsShowing) return;
        IsShowing = false;

        foreach (Player player in GameManager.Instance.Players)
        {
            player.ActiveInput = true;
        }

        Background.IsShowing = false;
        SpeakerIcon.IsShowing = false;
        Message.IsShowing = false;
        ButtonIcon.IsShowing = false;
    }
}