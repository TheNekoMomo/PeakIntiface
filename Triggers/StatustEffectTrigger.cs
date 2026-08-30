using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BepInEx.Logging;

namespace PeakIntiface.Triggers
{
    public class StatustEffectTrigger
    {
        private readonly ManualLogSource logger;

        private Character subscribedCharacter = null;
        private readonly Dictionary<CharacterAfflictions.STATUSTYPE, CancellationTokenSource> afflictionIntensities = 
            new Dictionary<CharacterAfflictions.STATUSTYPE, CancellationTokenSource>();

        public StatustEffectTrigger(ManualLogSource logger)
        {
            this.logger = logger;
        }

        internal void Update()
        {
            // Check if the local character is valid and has afflictions
            Character character = Character.localCharacter;
            if (character == null || character.refs.afflictions == null) return;
            // Check if the character has changed
            if (subscribedCharacter == character) return;

            // Unsubscribe from the previous character's affliction events
            if (subscribedCharacter != null)
            {
                subscribedCharacter.refs.afflictions.OnAddedIncrementalStatus -= OnAfflictionAdded;
            }

            subscribedCharacter = character;
            subscribedCharacter.refs.afflictions.OnAddedIncrementalStatus += OnAfflictionAdded;
            logger.LogInfo("Subscribed to status effects.");
        }

        private void OnAfflictionAdded(CharacterAfflictions.STATUSTYPE sTATUSTYPE, float amount)
        {
            switch (sTATUSTYPE)
            {
                case CharacterAfflictions.STATUSTYPE.Injury:
                    double InjuryIntensity = Plugin.InjuryTriggerMinimumIntensity.Value +
                        amount * (Plugin.InjuryTriggerMaximumIntensity.Value - Plugin.InjuryTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, InjuryIntensity, Plugin.InjuryTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Hunger:
                    double hungerIntensity = Plugin.HungerTriggerMinimumIntensity.Value +
                        amount * (Plugin.HungerTriggerMaximumIntensity.Value - Plugin.HungerTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, hungerIntensity, Plugin.HungerTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Cold:
                    double coldIntensity = Plugin.ColdTriggerMinimumIntensity.Value +
                        amount * (Plugin.ColdTriggerMaximumIntensity.Value - Plugin.ColdTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, coldIntensity, Plugin.ColdTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Poison:
                    double poisonIntensity = Plugin.PoisonTriggerMinimumIntensity.Value +
                        amount * (Plugin.PoisonTriggerMaximumIntensity.Value - Plugin.PoisonTriggerMinimumIntensity.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Curse:
                    double curseIntensity = Plugin.CurseTriggerMinimumIntensity.Value +
                        amount * (Plugin.CurseTriggerMaximumIntensity.Value - Plugin.CurseTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, curseIntensity, Plugin.CurseTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Drowsy:
                    double drowsyIntensity = Plugin.DrowsyTriggerMinimumIntensity.Value +
                        amount * (Plugin.DrowsyTriggerMaximumIntensity.Value - Plugin.DrowsyTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, drowsyIntensity, Plugin.DrowsyTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Weight:
                    break;
                case CharacterAfflictions.STATUSTYPE.Hot:
                    double hotIntensity = Plugin.HotTriggerMinimumIntensity.Value +
                        amount * (Plugin.HotTriggerMaximumIntensity.Value - Plugin.HotTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, hotIntensity, Plugin.HotTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Thorns:
                    double thornsIntensity = Plugin.ThornsTriggerMinimumIntensity.Value +
                        amount * (Plugin.ThornsTriggerMaximumIntensity.Value - Plugin.ThornsTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, thornsIntensity, Plugin.ThornsTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Spores:
                    double sporesIntensity = Plugin.SporesTriggerMinimumIntensity.Value +
                        amount * (Plugin.SporesTriggerMaximumIntensity.Value - Plugin.SporesTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, sporesIntensity, Plugin.SporesTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Web:
                    double webIntensity = Plugin.WebTriggerMinimumIntensity.Value +
                        amount * (Plugin.WebTriggerMaximumIntensity.Value - Plugin.WebTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, webIntensity, Plugin.WebTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Arrow:
                    double arrowIntensity = Plugin.ArrowTriggerMinimumIntensity.Value +
                        amount * (Plugin.ArrowTriggerMaximumIntensity.Value - Plugin.ArrowTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, arrowIntensity, Plugin.ArrowTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Petrify:
                    double PetrifyIntensity = Plugin.PetrifyTriggerMinimumIntensity.Value +
                        amount * (Plugin.PetrifyTriggerMaximumIntensity.Value - Plugin.PetrifyTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, PetrifyIntensity, Plugin.PetrifyTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.FlyTrap:
                    break;
            }
        }

        private async Task HandleAfflictionIntensity(CharacterAfflictions.STATUSTYPE sTATUSTYPE, double intensity, float durationMilliseconds)
        {
            if (afflictionIntensities.TryGetValue(sTATUSTYPE, out CancellationTokenSource oldTimer))
            {
                oldTimer.Cancel();
                oldTimer.Dispose();
            }

            CancellationTokenSource timer = new CancellationTokenSource();
            afflictionIntensities[sTATUSTYPE] = timer;

            string source = $"StatustEffectTrigger-{sTATUSTYPE}";

            Plugin.ToyController.SetSourceIntensity(source, intensity);

            try
            {
                await Task.Delay((int)durationMilliseconds, timer.Token);
            }
            catch (System.Exception)
            {
                return;
            }

            Plugin.ToyController.SetSourceIntensity(source, 0);

            timer.Dispose();
            afflictionIntensities.Remove(sTATUSTYPE);
        }
    }
}
