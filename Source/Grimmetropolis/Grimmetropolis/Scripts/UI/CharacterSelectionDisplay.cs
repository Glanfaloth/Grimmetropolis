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

    public float Cooldown = .2f;
    public float CooldownTimer = .2f;

    public PlayerType CurrentPlayerType = PlayerType.Cinderella;

    public override void Initialize()
    {
        base.Initialize();

        SetCharacterDisplayState(CharacterDisplayState.NotUsed);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (Input == null) return;

        Cooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (Cooldown <= 0f)
        {
            switch (CharacterDisplayState)
            {
                case CharacterDisplayState.SelectingCharacter:
                    Vector2 joystickDirection = Input.MoveDirection();


                    if (Input.ActionPressed())
                    {
                        Cooldown = CooldownTimer;
                        SetCharacterDisplayState(CharacterDisplayState.Ready);
                    }
                    if (Input.CancelPressed())
                    {
                        Cooldown = CooldownTimer;
                        MenuUIManager.Instance.MainMenu.RemovePlayer(Input);
                        return;
                    }
                    if (Input.CycleLeftPressed() || joystickDirection.X <= -.8f)
                    {
                        Cooldown = CooldownTimer;
                        int playerTypeCount = Enum.GetNames(typeof(PlayerType)).Length;
                        if ((int)CurrentPlayerType <= 1) CurrentPlayerType = (PlayerType)playerTypeCount - 1;
                        else CurrentPlayerType--;

                        CharacterAnimation.CharacterModel = CharacterAnimation.GetModelFromPlayerType(CurrentPlayerType);
                        CharacterAnimation.RecreateBodyParts();
                        return;
                    }
                    if (Input.CycleRightPressed() || joystickDirection.X >= .8f)
                    {
                        Cooldown = CooldownTimer;
                        int playerTypeCount = Enum.GetNames(typeof(PlayerType)).Length;
                        if ((int)CurrentPlayerType >= playerTypeCount - 1) CurrentPlayerType = (PlayerType)1;
                        else CurrentPlayerType++;

                        CharacterAnimation.CharacterModel = CharacterAnimation.GetModelFromPlayerType(CurrentPlayerType);
                        CharacterAnimation.RecreateBodyParts();
                        return;
                    }
                    break;
                case CharacterDisplayState.Ready:
                    if (MenuUIManager.Instance.MainMenu.NavigateMainMenu(Input, gameTime)) Cooldown = CooldownTimer;
                    else if (Input.CancelPressed())
                    {
                        Cooldown = CooldownTimer;
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
                Cooldown = CooldownTimer;
                if (MenuUIManager.Instance.MainMenu.CharacterDisplays.All(o => o.CharacterDisplayState != CharacterDisplayState.Ready))
                {
                    MenuUIManager.Instance.MainMenu.MainMenuState = MainMenuState.MenuInactive;
                }
                break;
            case CharacterDisplayState.Ready:
                InfoText.Text = "  Ready";
                ButtonIcon.IsShowing = false;
                LeftArrow.IsShowing = false;
                RightArrow.IsShowing = false;
                if (MenuUIManager.Instance.MainMenu.MainMenuState == MainMenuState.MenuInactive)
                {
                    MenuUIManager.Instance.MainMenu.MainMenuState = MainMenuState.MenuStart;
                }
                break;
        }
    }
}