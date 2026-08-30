using BepInEx.Logging;

namespace PeakIntiface.Triggers
{
    public class StaminaTrigger
    {
        private readonly ManualLogSource logger;
        private float lastStamina = -999f;

        public StaminaTrigger(ManualLogSource logger)
        {
            this.logger = logger;
        }

        public void Update()
        {
            Character character = Character.localCharacter;

            if (character == null) return;

            if (character.data == null) return;

            float currentStamina = character.data.currentStamina;

            if (System.Math.Abs(currentStamina - lastStamina) > 0.01f)
            {
                lastStamina = currentStamina;
                logger.LogInfo($"Stamina changed: {currentStamina}");
            }

            double intensity = 1 - currentStamina;
            _ = Plugin.ToyController?.StartVibrationAsync(intensity);
        }
    }
}
