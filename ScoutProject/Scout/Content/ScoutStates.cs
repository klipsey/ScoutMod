using ScoutMod.Modules.BaseStates;
using ScoutMod.Scout.SkillStates;

namespace ScoutMod.Scout.Content
{
    public static class ScoutStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(BaseScoutSkillState));
            Modules.Content.AddEntityState(typeof (MainState));
            Modules.Content.AddEntityState(typeof(BaseScoutState));
            Modules.Content.AddEntityState(typeof(Shoot));
            Modules.Content.AddEntityState(typeof(ThrowCleaver));
            Modules.Content.AddEntityState(typeof(HitBaseball));
            Modules.Content.AddEntityState(typeof(ActivateAtomic));
            Modules.Content.AddEntityState(typeof(Swing));
            Modules.Content.AddEntityState(typeof(SwapWeapon));
            Modules.Content.AddEntityState(typeof(Reload));
        }
    }
}
