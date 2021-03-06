using Microsoft.Xna.Framework;

public class ToolPickaxe : Item
{
    public override void InteractWithStructure(GameTime gameTime, Structure structure)
    {
        if (Character is Player player)
        {
            if (structure is ResourceDeposit closestResourceDeposit && closestResourceDeposit.Type == ResourceDepositType.Stone)
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

                if (player.LastClosestResourceDeposit.CurrentStorage > 0) player.Progress += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (player.Progress >= player.LastClosestResourceDeposit.HarvestTime)
                {
                    player.LastClosestResourceDeposit.HarvestResource();
                    player.Progress -= player.LastClosestResourceDeposit.HarvestTime;
                }

                Character.Animation.UseAnimation();
            }

            else if (structure is Building closestBuilding)
            {
                if (!closestBuilding.Mesh.IsPreview)
                {
                    closestBuilding.Health -= Config.PLAYER_DAMAGE;
                    Character.Cooldown = Config.PLAYER_ATTACK_DURATION;
                }

                player.ResetProgressBarForProgress();
                player.SetProgressForCooldown();
            }

            Character.Animation.UseAnimation();
        }
    }
}
