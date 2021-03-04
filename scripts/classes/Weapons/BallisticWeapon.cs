using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Godot;
using VanguardOfSteel.Mechanical;

namespace VanguardOfSteel.Weapons
{
    public class BallisticWeapon : AnimatedSprite, IMechanicalSystem
    {
        public WeaponClip Magazine { get; set; }
        
        public WeaponBarrel Barrel { get; set; }
        
        public Dictionary<string,MechanicalState> ComponentState { get; set; }
        
        public MechanicalState MechanicalStatus { get; set; }
        
        // TODO: Remove this. Having two types of statuses is confusing
        public WeaponState Status { get; set; }
        
        private Timer ReloadTimer { get; set; }
        
        [Export]
        public float ReloadingSpeed { get; set; }

        [Signal]
        delegate void Fire(WeaponRound weaponRound, float direction, Vector2 rotation);

        [Signal]
        delegate void RefreshMagazine(int reloadAmount);

        public void ChallengeOperationalStatus()
        {
            if (ComponentState["Magazine"] == MechanicalState.Operational
                && ComponentState["Barrel"] == MechanicalState.Operational)
            {
                MechanicalStatus = MechanicalState.Operational;
            }
            MechanicalStatus = MechanicalState.Damaged;
        }

        public void Shoot()
        {
            if (MechanicalStatus != MechanicalState.Operational) return;
            
            if (Magazine.IsEmpty()) {
                EmitSignal(nameof(IMechanicalSystem.OperationalFailure), 
                    MechanicalFailureSeverity.RECOVERABLE, "Reload!");
                return;
            }

            if (MechanicalStatus != MechanicalState.Operational)
            {
                EmitSignal(nameof(IMechanicalSystem.OperationalFailure),
                    MechanicalFailureSeverity.UNRECOVERABLE, "Weapon Broken");
                return;
            }
            
            Magazine--;
            EmitSignal(nameof(Fire), 
                Magazine.TypeOfRounds, Barrel.Muzzle.GlobalTransform.Rotation,
                Barrel.Muzzle.GlobalPosition);
        }

        public void Reload(ushort reloadAmount)
        {
            
            EmitSignal(nameof(RefreshMagazine), reloadAmount);
            
            // Here we decide what we want to do with 
            // the excess bullets.
            Magazine.Refresh(reloadAmount);
            throw new NotImplementedException();
        }

        private void _on_WeaponClip_StatusUpdate(MechanicalState newState)
        {
            ComponentState["Magazine"] = newState;
            
            //TODO: Challenge Mechanical Status of System
            ChallengeOperationalStatus();
        }

        private void _on_Barrel_StatusUpdate(MechanicalState newState)
        {
            ComponentState["Barrel"] = newState;
            
            //TODO: Challenge Mechanical Status of System
            ChallengeOperationalStatus();
        }
        
        public override void _Ready()
        {
            ReloadTimer.OneShot = true;
            ReloadTimer.WaitTime = ReloadingSpeed;

            ComponentState = new Dictionary<string, MechanicalState>{
                {"Magazine", Magazine.MechanicalStatus},
                {"Barrel", Barrel.MechanicalStatus}
            };
            
            Magazine.Connect("OperationalStatusUpdate", this, nameof(_on_WeaponClip_StatusUpdate));
            Barrel.Connect("OperationalStatusUpdate", this, nameof(_on_Barrel_StatusUpdate));
        }
    }
}