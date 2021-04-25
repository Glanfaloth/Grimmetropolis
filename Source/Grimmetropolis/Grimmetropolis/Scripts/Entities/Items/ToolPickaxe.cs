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

                player.Progress += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (player.Progress >= player.LastClosestResourceDeposit.HarvestTime)
                {
                    player.LastClosestResourceDeposit.HarvestResource();
                    player.Progress -= player.LastClosestResourceDeposit.HarvestTime;
                }
            }

            else if (structure is Building closestBuilding)
            {
                if (!closestBuilding.Mesh.IsBlueprint)
                {
                    closestBuilding.Health -= Config.PLAYER_DAMAGE;
                    Character.Cooldown = Config.PLAYER_ATTACK_DURATION;
                }
                else
                {
                    player.Build(gameTime);
                }

                player.ResetProgressBarForProgress();
                player.SetProgressForCooldown();
            }

            Character.Animation.UseArm();
        }
    }
}
