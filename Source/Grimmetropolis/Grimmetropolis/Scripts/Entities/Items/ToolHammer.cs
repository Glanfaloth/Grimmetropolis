using Microsoft.Xna.Framework;
public class ToolHammer : Item
{
    public override void InteractWithCharacter(GameTime gameTime, Character character)
    {
        base.InteractWithCharacter(gameTime, character);

        character.Health -= Config.PLAYER_DAMAGE;
        Character.Cooldown = 1.5f * Config.PLAYER_ATTACK_DURATION;

        if (Character is Player player)
        {
            player.ResetProgressBarForProgress();
            player.SetProgressForCooldown();
        }
    }

    public override void InteractWithStructure(GameTime gameTime, Structure structure)
    {
        base.InteractWithStructure(gameTime, structure);

        if (Character is Player player && structure.Mesh.IsPreview)
        {
            if (structure is Building building) player.Build(gameTime, building);
            player.Animation.UseAnimation();
        }
    }
}