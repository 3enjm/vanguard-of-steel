using Godot;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace VanguardOfSteel.Mechanical
{
    public interface IMechanicalSystem : IMechanicalComponent
    {
        public Dictionary<string,MechanicalState> ComponentState { get; set; }
        
       [Signal]
       delegate void OperationalFailure(MechanicalFailureSeverity severity, string message);

       public void ChallengeOperationalStatus();
    }
}