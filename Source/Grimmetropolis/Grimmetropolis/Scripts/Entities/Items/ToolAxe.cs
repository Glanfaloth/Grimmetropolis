using Microsoft.Xna.Framework;

public class ToolAxe : Item
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

        if (Character is Player player && structure is ResourceDeposit closestResourceDeposit && closestResourceDeposit.Type == ResourceDepositType.Wood)
        {
            if (closestResourceDeposit != player.LastClosestResourceDeposit)
            {
                player.NeedsToShowHarvestProgress = true;
                player.LastClosestResourceDeposit = closestResourceDeposit;
                player.Progress = 0f;
            }

            if (!player.IsShowingCooldown && player.NeedsToShowHarvestProgress)
            {
                player.NeedsToShowHarvestProgress = false;

                player.ProgressBar.MaxProgress = player.LastClosestResourceDeposit.HarvestTime;
                player.ProgressBar.Show();
            }

            player.Progress += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (player.Progress >= player.LastClosestResourceDeposit.HarvestTime)
            {
                player.LastClosestResourceDeposit.HarvestResource();
                player.Progress -= player.LastClosestResourceDeposit.HarvestTime;
            }
        }
    }
}