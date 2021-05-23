using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Linq;

public enum CharacterDisplayState
{
    NotUsed,
    SelectingCharacter,
    Ready
}

public class CharacterSelectionDisplay : TDComponent
{
    public TDText InfoText;
    public TDSprite ButtonIcon;

    public TDSprite LeftArrow;
    public TDSprite RightArrow;

    public CharacterAnimation CharacterAnimation;
    public TDInput Input;
    public CharacterDisplayState CharacterDisplayState = CharacterDisplayState.Ready;

    private float _cooldown = .2f;
    private float _cooldownTimer = .2f;

    private PlayerType _currentPlayerType = PlayerType.Cinderella;

    public override void Initialize()
    {
        base.Initialize();

        SetCharacterDisplayState(CharacterDisplayState.NotUsed);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (Input == null) return;

        _cooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_cooldown <= 0f)
        {
            switch (CharacterDisplayState)
            {
                case CharacterDisplayState.SelectingCharacter:
                    Vector2 joystickDirection = Input.MoveDirection();


                    if (Input.ActionPressed())
                    {
                        _cooldown = _cooldownTimer;
                        SetCharacterDisplayState(CharacterDisplayState.Ready);
                    }
                    if (Input.CancelPressed())
                    {
                        _cooldown = _cooldownTimer;
                        MenuUIManager.Instance.MainMenu.RemovePlayer(Input);
                        return;
                    }
                    if (Input.CycleLeftPressed() || joystickDirection.X <= -.8f)
                    {
                        _cooldown = _cooldownTimer;
                        int playerTypeCount = Enum.GetNames(typeof(PlayerType)).Length;
                        if ((int)_currentPlayerType <= 1) _currentPlayerType = (PlayerType)playerTypeCount - 1;
                        else _currentPlayerType--;

                        CharacterAnimation.CharacterModel = CharacterAnimation.GetModelFromPlayerType(_currentPlayerType);
                        CharacterAnimation.RecreateBodyParts();
                        return;
                    }
                    if (Input.CycleRightPressed() || joystickDirection.X >= .8f)
                    {
                        _cooldown = _cooldownTimer;
                        int playerTypeCount = Enum.GetNames(typeof(PlayerType)).Length;
                        if ((int)_currentPlayerType >= playerTypeCount - 1) _currentPlayerType = (PlayerType)1;
                        else _currentPlayerType++;

                        CharacterAnimation.CharacterModel = CharacterAnimation.GetModelFromPlayerType(_currentPlayerType);
                        CharacterAnimation.RecreateBodyParts();
                        return;
                    }
                    break;
                case CharacterDisplayState.Ready:
                    if (Input.ActionPressed())
                    {
                        _cooldown = _cooldownTimer;
                        if (MenuUIManager.Instance.MainMenu.CharacterDisplays.All(o => o.CharacterDisplayState != CharacterDisplayState.SelectingCharacter))
                        {
                            for (int i = 0; i < GameManager.PlayerTypes.Length; i++)
                            {
                                GameManager.PlayerTypes[i] = MenuUIManager.Instance.MainMenu.CharacterDisplays[i].CharacterDisplayState == CharacterDisplayState.Ready
                                    ? GameManager.PlayerTypes[i] = MenuUIManager.Instance.MainMenu.CharacterDisplays[i]._currentPlayerType : PlayerType.None;
                            }
                            TDSceneManager.LoadScene(new GameScene());
                        }
                        return;
                    }

                    if (Input.CancelPressed())
                    {
                        _cooldown = _cooldownTimer;
                        SetCharacterDisplayState(CharacterDisplayState.SelectingCharacter);
                        return;
                    }
                    break;
            }    
        }
    }

    public void SetCharacterDisplayState(CharacterDisplayState state)
    {
        if (CharacterDisplayState == state) return;
        CharacterDisplayState = state;

        switch (CharacterDisplayState)
        {
            case CharacterDisplayState.NotUsed:
                InfoText.Text = "Press   ";
                ButtonIcon.IsShowing = true;
                LeftArrow.IsShowing = false;
                RightArrow.IsShowing = false;
                break;
            case CharacterDisplayState.SelectingCharacter:
                InfoText.Text = " Choose";
                ButtonIcon.IsShowing = false;
                LeftArrow.IsShowing = true;
                RightArrow.IsShowing = true;
                _cooldown = _cooldownTimer;
                break;
            case CharacterDisplayState.Ready:
                InfoText.Text = "  Ready";
                ButtonIcon.IsShowing = false;
                LeftArrow.IsShowing = false;
                RightArrow.IsShowing = false;
                break;
        }
    }
}