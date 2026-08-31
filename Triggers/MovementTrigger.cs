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
            // Get the character class for the player who is triggering events
            Character character = TriggerUtils.GetCharacterForStats();
            if (character == null) return;

            double intensity = 0.0;
            float currentStamina = character.data.currentStamina;

            if (character.data.isClimbing && ConfigManager.NormalClimbingTriggerEnabled.Value)
            {
                intensity = TriggerUtils.CalculateIntensity(currentStamina - 1,
                    ConfigManager.NormalClimbingTriggerMaximumIntensity.Value, ConfigManager.NormalClimbingTriggerMinimumIntensity.Value);
            }
            else if (character.data.isRopeClimbing && ConfigManager.RopeClimbingTriggerEnabled.Value)
            {
                intensity = TriggerUtils.CalculateIntensity(currentStamina - 1,
                    ConfigManager.RopeClimbingTriggerMaximumIntensity.Value, ConfigManager.RopeClimbingTriggerMinimumIntensity.Value);
            }
            else if (character.data.isVineClimbing && ConfigManager.VineClimbingTriggerEnabled.Value)
            {
                intensity = TriggerUtils.CalculateIntensity(currentStamina - 1,
                    ConfigManager.VineClimbingTriggerMaximumIntensity.Value, ConfigManager.VineClimbingTriggerMinimumIntensity.Value);
            }
            else if (character.data.isSprinting && ConfigManager.SprintingTriggerEnabled.Value)
            {
                intensity = TriggerUtils.CalculateIntensity(currentStamina - 1,
                    ConfigManager.SprintingTriggerMaximumIntensity.Value, ConfigManager.SprintingTriggerMinimumIntensity.Value);
            }
            TriggerUtils.SetVibration("movement", intensity);
        }
    }
}