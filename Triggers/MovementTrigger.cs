using BepInEx.Logging;

namespace PeakIntiface.Triggers
{
    public class MovementTrigger
    {
        private readonly ManualLogSource logger;

        public MovementTrigger(ManualLogSource logger)
        {
            this.logger = logger;
        }

        public void Update()
        {
            if (Plugin.ToyController == null) return;
            // Get the local character data and check if it's null (Could be the case when the player is in a menu or loading screen)
            Character character = Character.localCharacter;
            if (character == null) return;
            if (character.data == null) return;

            if (character.data.isClimbing && ConfigManager.NormalClimbingTriggerEnabled.Value)
            {
                float currentStamina = character.data.currentStamina;
                double intensity = ConfigManager.NormalClimbingTriggerMinimumIntensity.Value + 
                    (1 - currentStamina) * (ConfigManager.NormalClimbingTriggerMaximumIntensity.Value - ConfigManager.NormalClimbingTriggerMinimumIntensity.Value);

                SetVibration(intensity);
            }
            else if (character.data.isRopeClimbing && ConfigManager.RopeClimbingTriggerEnabled.Value)
            {
                float currentStamina = character.data.currentStamina;
                double intensity = ConfigManager.RopeClimbingTriggerMinimumIntensity.Value + 
                    (1 - currentStamina) * (ConfigManager.RopeClimbingTriggerMaximumIntensity.Value - ConfigManager.RopeClimbingTriggerMinimumIntensity.Value);

                SetVibration(intensity);
            }
            else if (character.data.isVineClimbing && ConfigManager.VineClimbingTriggerEnabled.Value)
            {
                float currentStamina = character.data.currentStamina;
                double intensity = ConfigManager.VineClimbingTriggerMinimumIntensity.Value + 
                    (1 - currentStamina) * (ConfigManager.VineClimbingTriggerMaximumIntensity.Value - ConfigManager.VineClimbingTriggerMinimumIntensity.Value);

                SetVibration(intensity);
            }
            else if (character.data.isSprinting && ConfigManager.SprintingTriggerEnabled.Value)
            {
                float currentStamina = character.data.currentStamina;
                double intensity = ConfigManager.SprintingTriggerMinimumIntensity.Value + 
                    (1 - currentStamina) * (ConfigManager.SprintingTriggerMaximumIntensity.Value - ConfigManager.SprintingTriggerMinimumIntensity.Value);

                 SetVibration(intensity);
            }
            else
            {
                SetVibration(0.0);
            }
        }

        private void SetVibration(double intensity)
        {
            Plugin.ToyController.SetSourceIntensity("movement", intensity);
        }
    }
}
