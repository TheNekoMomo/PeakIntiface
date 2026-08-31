namespace PeakIntiface.Triggers
{
    internal static class TriggerUtils
    {
        public static double CalculateIntensity(float intensity, float maximum, float minimum, float multiplier = 1)
        {
            return minimum + (intensity * multiplier) * (maximum - minimum);
        }

        public static void SetVibration(string source, double intensity)
        {
            Plugin.ToyController.SetSourceIntensity(source, intensity);
        }

        public static Character GetCharacterForStats()
        {
            Character character = Character.localCharacter;
            if (character == null) return null;
            if (character.data == null) return null;

            if (ConfigManager.UseCarried.Value && character.data.isCarried && character.data.carrier != null)
            {
                character = character.data.carrier;
            }
            else if (ConfigManager.UseSpectate.Value && character.data.dead)
            {
                character = Character.observedCharacter;
            }

            return character;
        }
    }
}
