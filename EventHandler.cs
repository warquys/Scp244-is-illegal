using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using MapGeneration.Distributors;
using System.Collections.Generic;
using System.Linq;

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
            Exiled.Events.Handlers.Server.RoundStarted += OnWhaitForPlayer;
        }

        public void DetachEvent()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWhaitForPlayer;
        }

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

                foreach (var loot in locker.Loot)
                {
                    if (loot.TargetItem == ItemType.SCP244a)
                        loot.TargetItem = _config.ReplaceScp244a_locker;
                    
                    if (loot.TargetItem == ItemType.SCP244b)
                        loot.TargetItem = _config.ReplaceScp244b_locker;
                }
            }
        }

        private void DoorSetOpen(ICollection<Door> doors, bool open = true)
        {
            foreach (var door in doors)
                door.IsOpen = open;
        }

        private void RemoveScp244(ICollection<Pickup> pickups)
        {
            foreach (var pickup in pickups)
            {
                if (pickup.Type == ItemType.SCP244a)
                {
                    if (_config.ReplaceScp244a_floor != ItemType.None)
                    {
                        var item = Item.Create(_config.ReplaceScp244a_floor);
                        item.Spawn(pickup.Position, pickup.Rotation);
                    }
                    pickup.Destroy();
                }
                if (pickup.Type == ItemType.SCP244b)
                {
                    if (_config.ReplaceScp244b_floor != ItemType.None)
                    {
                        var item = Item.Create(_config.ReplaceScp244b_floor);
                        item.Spawn(pickup.Position, pickup.Rotation);
                    }
                    pickup.Destroy();
                }
            }
            
        }
        #endregion

        #region Events
        private void OnWhaitForPlayer()
        {
            var doors = Room.List.SelectMany(p => p.Doors).Where(p => !p.IsOpen).ToList();
            DoorSetOpen(doors);
            RemoveScp244(Map.Pickups);
            DoorSetOpen(doors, false);

            foreach (var locker in Map.Lockers)
                RemoveScp244(locker);
        }

        #endregion
    }
}