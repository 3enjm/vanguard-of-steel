using System;
using Godot;

namespace VanguardOfSteel.Mechanical
{
    public interface IMechanicalComponent
    {
       public MechanicalState MechanicalStatus { get; set; }

       [Signal]
       public delegate void OperationalStatusUpdate(string componentName, MechanicalState state);
    }
}