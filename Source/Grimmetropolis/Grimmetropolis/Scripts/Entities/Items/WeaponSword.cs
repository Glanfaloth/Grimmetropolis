using Microsoft.Xna.Framework;

public class WeaponSword : Item
{
    public override void InteractWithCharacter(GameTime gameTime, Character character)
    {
        base.InteractWithCharacter(gameTime, character);

        character.Health -= 2f * Config.PLAYER_DAMAGE;
        Character.Cooldown = .5f * Config.PLAYER_ATTACK_DURATION;

        if (Character is Player player)
        {
            player.ResetProgressBarForProgress();
            player.SetProgressForCooldown();
        }

        Character.Animation.UseArm();
    }
}
