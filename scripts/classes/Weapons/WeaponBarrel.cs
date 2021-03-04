using Godot;
using VanguardOfSteel.Mechanical;

namespace VanguardOfSteel.Weapons
{
    public class WeaponBarrel : Node, IMechanicalComponent
    {
        [Export]
        public Position2D Muzzle { get; set; }

        public MechanicalState MechanicalStatus { get; set; }

        public bool IsOperational() => MechanicalStatus == MechanicalState.Operational;

        [Export]
        public Caliber CaliberCompatability { get; set; }
        
        public WeaponBarrel() { }

        public WeaponBarrel(Caliber cal)
        {
            CaliberCompatability = cal;
        }
        
        public bool IsCompatibleWith(Caliber cal) 
            => cal == CaliberCompatability;
    }
}