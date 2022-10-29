using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using MapGeneration.Distributors;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scp_244_IsIllegal
{
    public class EventHandler
    {

        #region Properties & Variables
        
        private Config _config;

        #endregion

        #region Constructor & Destructor
        internal EventHandler(Config config)
        {
            _config = config;
            AttachEvent();
        }
        #endregion

        #region Methods
        public void AttachEvent()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        public void DetachEvent()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }

/* Remove bc the lockers have already spawned the items        
        public void RemoveScp244(Locker locker)
        {
            foreach (var chamber in locker.Chambers)
            {
                int index;

                index = chamber.AcceptableItems.IndexOf(ItemType.SCP244a);
                if (index != -1)
                    chamber.AcceptableItems[index] = _config.ReplaceScp244a_locker;

                index = chamber.AcceptableItems.IndexOf(ItemType.SCP244b);
                if (index != -1)
                    chamber.AcceptableItems[index] = _config.ReplaceScp244b_locker;
            }

            foreach (var loot in locker.Loot)
            {
                if (loot.TargetItem == ItemType.SCP244a)
                    loot.TargetItem = _config.ReplaceScp244a_locker;

                if (loot.TargetItem == ItemType.SCP244b)
                    loot.TargetItem = _config.ReplaceScp244b_locker;
            }
        }*/

        private void SpawnItem(Vector3 position, Quaternion rotation, ItemType itemType)
        {
            if (itemType == ItemType.None) return;

            var item = Item.Create(itemType);
            item.Spawn(position, rotation);
        }

        #endregion

        #region Events
        private unsafe void OnRoundStarted()
        {
            const float maxDistancePickupSpawnpoint = 1;

            var pickups = Map.Pickups;
            var chambers = Object.FindObjectsOfType<LockerChamber>();

            foreach (var pickup in pickups)
            {
                if (pickup is not { Type: ItemType.SCP244a or ItemType.SCP244b }) continue;

                var position = pickup.Position;
                var rotation = pickup.Rotation;

#if DEBUG
                var distance = chambers.Min(p => Vector3.Distance(p._spawnpoint.position, position));
                Log.Debug($"min {distance}");
#endif

                var inLocker = chambers.Any(p => Vector3.Distance(p._spawnpoint.position, position) < maxDistancePickupSpawnpoint);
                pickup.Destroy();

                switch (pickup.Type)
                {
                    case ItemType.SCP244a when inLocker:
                        SpawnItem(position, rotation, _config.ReplaceScp244a_locker);
                        break;
                    case ItemType.SCP244a:
                        SpawnItem(position, rotation, _config.ReplaceScp244a_floor);
                        break;
                    case ItemType.SCP244b when inLocker:
                        SpawnItem(position, rotation, _config.ReplaceScp244b_locker);
                        break;
                    case ItemType.SCP244b:
                        SpawnItem(position, rotation, _config.ReplaceScp244b_floor);
                        break;
                }
            }

            /* Remove bc the lockers have already spawned the items
            foreach (var locker in Map.Lockers)
                RemoveScp244(locker);*/
        }

#endregion
    }
}