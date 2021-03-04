using System.Runtime.CompilerServices;
using Godot;
using VanguardOfSteel.Mechanical;

[assembly: InternalsVisibleTo("VanguardOfSteelTest")]
namespace VanguardOfSteel.Weapons
{
    public class WeaponClip : Node, IMechanicalComponent
    {
        public ushort Capacity { get; set; }
        
        public ushort RoundCount { get; private set; }

        // TODO: This shouldn't be public. 
        public MechanicalState MechanicalStatus { get; set; }

        public WeaponRound TypeOfRounds { get; set; }
        
        [Signal]
        public delegate void Emptied();

        public WeaponClip() { }
        
        public WeaponClip(ushort capacity, WeaponRound typeOfRounds)
        {
            Capacity = capacity;
            RoundCount = capacity;
            TypeOfRounds = typeOfRounds;
        }

        public WeaponClip(ushort capacity, ushort roundCount, WeaponRound typeOfRounds)
        {
            Capacity = capacity;
            
            if (roundCount > capacity)
                RoundCount = capacity;
            else
                RoundCount = roundCount;
            
            TypeOfRounds = typeOfRounds;
        }

        public bool IsEmpty() => RoundCount == 0;

        public int Refresh(ushort amount)
        {
            if (amount > Capacity)
            {
                RoundCount = Capacity;
                return amount - Capacity;
            }

            var diff = Capacity - RoundCount;
            if (diff == 0) return amount;

            if (diff >= amount)
            {
                RoundCount += amount;
                return 0;
            }
            
            RoundCount += (ushort)diff;
            return amount - diff;
        }

        public void Eject()
        {
            RoundCount = 0;
            EmitSignal(nameof(Emptied));
        }
        
        public static WeaponClip operator -- (WeaponClip clip)
        {
            if (clip.IsEmpty())
            {
                clip.EmitSignal(nameof(Emptied));
            }
                
            clip.RoundCount -= 1;
            return clip;
        }
    }
}