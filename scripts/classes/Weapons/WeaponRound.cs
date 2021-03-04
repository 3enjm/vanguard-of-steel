using Godot;

namespace VanguardOfSteel.Weapons
{
    public readonly struct WeaponRound 
    {
        public Caliber Caliber { get; }
        public Capability Capability { get; }

        public WeaponRound(Caliber caliber, Capability capability)
        {
            Caliber = caliber;
            Capability = capability;
        }

        public WeaponRound((Caliber caliber, Capability capability) round)
        {
            (Caliber, Capability) = round;
        }

        public (Caliber, Capability) ToTuple() => (Caliber, Capability);
        
        public static bool operator == (WeaponRound a, WeaponRound b) => a.ToTuple() == b.ToTuple();
        public static bool operator != (WeaponRound a, WeaponRound b) => a.ToTuple() != b.ToTuple();

        public override bool Equals(object? obj)
        {
            return obj switch
            {
                WeaponRound round => this == round,
                _ => false
            };
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}