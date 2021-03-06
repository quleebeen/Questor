﻿// ------------------------------------------------------------------------------
//   <copyright from='2010' to='2015' company='THEHACKERWITHIN.COM'>
//     Copyright (c) TheHackerWithin.COM. All Rights Reserved.
//
//     Please look in the accompanying license.htm file for the license that
//     applies to this source code. (a copy can also be found at:
//     http://www.thehackerwithin.com/license.htm)
//   </copyright>
// -------------------------------------------------------------------------------

namespace Questor.Modules.Caching
{
    using System;
    //using System.Collections.Generic;
    using DirectEve;
    using Questor.Modules.Lookup;
    using Questor.Modules.Logging;

    public class ModuleCache
    {
        private readonly DirectModule _module;
        private double _reloadTimeThisMission;
        private DateTime _activatedTimeStamp;

        public ModuleCache(DirectModule module, double reloadTimeThisMission = 0, DateTime activatedTimeStamp = default(DateTime))
        {
            _module = module;
            _reloadTimeThisMission = reloadTimeThisMission;
            _activatedTimeStamp = activatedTimeStamp;
        }

        public double ReloadTimeThisMission
        {
            get { return _reloadTimeThisMission; }
            set
            {
                _reloadTimeThisMission = value;
                return;
            }
        }

        public DateTime ActivatedTimeStamp
        {
            get { return _activatedTimeStamp; }
            set
            {
                _activatedTimeStamp = value;
                return;
            }
        }

        public int TypeId
        {
            get { return _module.TypeId; }
        }

        public int GroupId
        {
            get { return _module.GroupId; }
        }

        public double Damage
        {
            get { return _module.Damage; }
        }

        public bool ActivatePlex //do we need to make sure this is ONLY valid on a PLEX?
        {
            get { return _module.ActivatePLEX(); }
        }

        public bool AssembleShip // do we need to make sure this is ONLY valid on a packaged ship?
        {
            get { return _module.AssembleShip(); }
        }

        //public double Attributes
        //{
        //    get { return _module.Attributes; }
        //}

        public double AveragePrice
        {
            get { return _module.AveragePrice(); }
        }

        public double Duration
        {
            get { return _module.Duration ?? 0; }
        }

        public double FallOff
        {
            get { return _module.FallOff ?? 0; }
        }

        public double MaxRange
        {
            get
            {
                try
                {
                    double? _maxRange = null;
                    //_maxRange = _module.Attributes.TryGet<double>("maxRange");

                    if (_maxRange == null || _maxRange == 0)
                    {
                        //
                        // if we could not find the max range via EVE use the XML setting for RemoteRepairers
                        //
                        if (_module.GroupId == (int)Group.RemoteArmorRepairer || _module.GroupId == (int)Group.RemoteShieldRepairer || _module.GroupId == (int)Group.RemoteHullRepairer)
                        {
                            return Settings.Instance.RemoteRepairDistance;
                        }
                        //
                        // if we could not find the max range via EVE use the XML setting for Nos/Neuts
                        //
                        if (_module.GroupId == (int)Group.NOS || _module.GroupId == (int)Group.Neutralizer)
                        {
                            return Settings.Instance.NosDistance;
                        }
                        //
                        // Add other types of modules here?
                        //
                        return 0;
                    }

                    return (double)_maxRange;
                }
                catch(Exception ex)
                {
                    Logging.Log("ModuleCache.RemoteRepairDistance", "Exception [ " + ex + " ]", Logging.Debug);
                }

                return 0;
            }            
        }

        public double Hp
        {
            get { return _module.Hp; }
        }

        public bool IsOverloaded
        {
            get { return _module.IsOverloaded; }
        }

        public bool IsPendingOverloading
        {
            get { return _module.IsPendingOverloading; }
        }

        public bool IsPendingStopOverloading
        {
            get { return _module.IsPendingStopOverloading; }
        }

        public bool ToggleOverload
        {
            get { return _module.ToggleOverload(); }
        }

        public bool IsActivatable
        {
            get { return _module.IsActivatable; }
        }

        public long ItemId
        {
            get { return _module.ItemId; }
        }

        public bool IsActive
        {
            get { return _module.IsActive; }
        }

        public bool IsOnline
        {
            get { return _module.IsOnline; }
        }

        public bool IsGoingOnline
        {
            get { return _module.IsGoingOnline; }
        }

        public bool IsReloadingAmmo
        {
            get { return _module.IsReloadingAmmo; }
        }

        public bool IsDeactivating
        {
            get { return _module.IsDeactivating; }
        }

        public bool IsChangingAmmo
        {
            get { return _module.IsChangingAmmo; }
        }

        public bool IsTurret
        {
            get
            {
                if (GroupId == (int)Group.EnergyWeapon) return true;
                if (GroupId == (int)Group.ProjectileWeapon) return true;
                if (GroupId == (int)Group.HybridWeapon) return true;
                return false;
            }
        }

        public bool IsMissileLauncher
        {
            get
            {
                if (GroupId == (int)Group.AssaultMissilelaunchers) return true;
                if (GroupId == (int)Group.CruiseMissileLaunchers) return true;
                if (GroupId == (int)Group.TorpedoLaunchers) return true;
                if (GroupId == (int)Group.StandardMissileLaunchers) return true;
                if (GroupId == (int)Group.AssaultMissilelaunchers) return true;
                if (GroupId == (int)Group.HeavyMissilelaunchers) return true;
                if (GroupId == (int)Group.DefenderMissilelaunchers) return true;
                return false;
            }
        }

        public bool IsEnergyWeapon
        {
            get { return GroupId == (int)Group.EnergyWeapon; }
        }

        public long TargetId
        {
            get { return _module.TargetId ?? -1; }
        }

        public long LastTargetId
        {
            get
            {
                if (Cache.Instance.LastModuleTargetIDs.ContainsKey(ItemId))
                {
                    return Cache.Instance.LastModuleTargetIDs[ItemId];
                }

                return -1;
            }
        }

        //public IEnumerable<DirectItem> MatchingAmmo
        //{
        //    get { return _module.MatchingAmmo; }
        //}

        public DirectItem Charge
        {
            get { return _module.Charge; }
        }

        public int CurrentCharges
        {
            get
            {
                if (_module.Charge != null)
                    return _module.Charge.Quantity;

                return -1;
            }
        }

        public int MaxCharges
        {
            get { return _module.MaxCharges; }
        }

        public double OptimalRange
        {
            get { return _module.OptimalRange ?? 0; }
        }

        public void ReloadAmmo(DirectItem charge)
        {
            _module.ReloadAmmo(charge);
        }

        public void ChangeAmmo(DirectItem charge)
        {
            _module.ChangeAmmo(charge);
        }

        public bool InLimboState
        {
            get
            {
                bool result = false;
                result |= !IsActivatable;
                result |= !IsOnline;
                result |= IsDeactivating;
                result |= IsGoingOnline;
                result |= IsReloadingAmmo;
                result |= IsChangingAmmo;
                result |= !Cache.Instance.InSpace;
                result |= Cache.Instance.InStation;
                result |= Cache.Instance.LastInStation.AddSeconds(7) > DateTime.UtcNow; 
                return result;
            }
        }

        public void Click()
        {
            if (InLimboState)
                return;

            _module.Click();
        }

        public void Activate()
        {
            if (InLimboState || IsActive)
                return;

            _module.Activate();
        }

        public void Activate(long entityId)
        {
            if (InLimboState || IsActive)
                return;

            _module.Activate(entityId);
            ActivatedTimeStamp = DateTime.UtcNow;
            Cache.Instance.LastModuleTargetIDs[ItemId] = entityId;
        }

        public void Deactivate()
        {
            if (InLimboState || !IsActive)
                return;

            _module.Deactivate();
        }
    }
}