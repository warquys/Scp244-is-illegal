using Exiled.API.Interfaces;
using System.ComponentModel;

namespace Scp_244_IsIllegal
{
    public class Config : IConfig
    {
        [Description("↓Indicates whether the plugin is enabled or not")]
        public bool IsEnabled { get; set; } = true;

        [Description("↓The item that will replace SCP244 in locker")]
        public ItemType ReplaceScp244a_locker { get; set; } = ItemType.None;
        public ItemType ReplaceScp244b_locker { get; set; } = ItemType.None;

        [Description("↓The item that will replace SCP244 in the floor")]
        public ItemType ReplaceScp244a_floor { get; set; } = ItemType.None;
        public ItemType ReplaceScp244b_floor { get; set; } = ItemType.None;

    }
}
