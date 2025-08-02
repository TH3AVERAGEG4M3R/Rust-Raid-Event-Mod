using System;
using System.Collections.Generic;
using System.Linq;
using Oxide.Core;
using Oxide.Core.Plugins;
using Oxide.Game.Rust.Cui;
using UnityEngine;
using Newtonsoft.Json;

namespace Oxide.Plugins
{
    [Info("Raid Event Mod", "TH3AVERAGEG4M3R", "1.0.0")]
    [Description("Manages raid events where players can spawn raiding items that get removed when event ends")]
    class RaidEventMod : RustPlugin
    {
        #region Fields
        private bool eventActive = false;
        private DateTime eventStartTime;
        private DateTime eventEndTime;
        private Timer eventTimer;
        private Timer reminderTimer;
        private List<uint> eventItems = new List<uint>();
        private Dictionary<ulong, List<uint>> playerEventItems = new Dictionary<ulong, List<uint>>();
        #endregion

        #region Configuration
        private Configuration config;

        public class Configuration
        {
            [JsonProperty("Event Duration (minutes)")]
            public int EventDuration = 60;

            [JsonProperty("Reminder Intervals (minutes)")]
            public List<int> ReminderIntervals = new List<int> { 30, 15, 5, 1 };

            [JsonProperty("Available Raid Items")]
            public Dictionary<string, RaidItem> RaidItems = new Dictionary<string, RaidItem>
            {
                ["rocket"] = new RaidItem { ShortName = "ammo.rocket.basic", DisplayName = "Rocket", MaxAmount = 20 },
                ["hvrocket"] = new RaidItem { ShortName = "ammo.rocket.hv", DisplayName = "HV Rocket", MaxAmount = 10 },
                ["incendiary"] = new RaidItem { ShortName = "ammo.rocket.fire", DisplayName = "Incendiary Rocket", MaxAmount = 10 },
                ["c4"] = new RaidItem { ShortName = "explosive.timed", DisplayName = "Timed Explosive Charge", MaxAmount = 15 },
                ["satchel"] = new RaidItem { ShortName = "explosive.satchel", DisplayName = "Satchel Charge", MaxAmount = 20 },
                ["rpg"] = new RaidItem { ShortName = "rocket.launcher", DisplayName = "Rocket Launcher", MaxAmount = 2 },
                ["mlrs"] = new RaidItem { ShortName = "rocket.mlrs", DisplayName = "MLRS Rocket", MaxAmount = 5 },
                ["expammo"] = new RaidItem { ShortName = "ammo.rifle.explosive", DisplayName = "Explosive 5.56 Rifle Ammo", MaxAmount = 100 },
                ["40mmhe"] = new RaidItem { ShortName = "40mm_grenade_he", DisplayName = "40mm HE Grenade", MaxAmount = 30 },
                ["40mmsmoke"] = new RaidItem { ShortName = "40mm_grenade_smoke", DisplayName = "40mm Smoke Grenade", MaxAmount = 20 },
                ["beancan"] = new RaidItem { ShortName = "grenade.beancan", DisplayName = "Beancan Grenade", MaxAmount = 40 },
                ["f1"] = new RaidItem { ShortName = "grenade.f1", DisplayName = "F1 Grenade", MaxAmount = 30 },
                ["gl"] = new RaidItem { ShortName = "multiplegrenadelauncher", DisplayName = "Multiple Grenade Launcher", MaxAmount = 1 }
            };

            [JsonProperty("Admin Permission")]
            public string AdminPermission = "raidevent.admin";

            [JsonProperty("Use Permission")]
            public string UsePermission = "raidevent.use";

            [JsonProperty("Chat Command")]
            public string ChatCommand = "raidevent";

            [JsonProperty("Chat Prefix")]
            public string ChatPrefix = "<color=#ff6b6b>[Raid Event]</color>";

            [JsonProperty("Auto Start Events")]
            public bool AutoStartEvents = false;

            [JsonProperty("Auto Event Times (24h format, e.g., 18:00)")]
            public List<string> AutoEventTimes = new List<string> { "18:00", "20:00" };
        }

        public class RaidItem
        {
            [JsonProperty("Item Short Name")]
            public string ShortName;

            [JsonProperty("Display Name")]
            public string DisplayName;

            [JsonProperty("Maximum Amount Per Player")]
            public int MaxAmount;
        }

        protected override void LoadDefaultConfig()
        {
            config = new Configuration();
        }

        protected override void LoadConfig()
        {
            base.LoadConfig();
            try
            {
                config = Config.ReadObject<Configuration>();
                if (config == null)
                {
                    throw new JsonException();
                }
            }
            catch
            {
                PrintWarning("Configuration file is corrupt, using default values");
                LoadDefaultConfig();
            }
        }

        protected override void SaveConfig()
        {
            Config.WriteObject(config, true);
        }
        #endregion

        #region Oxide Hooks
        void Init()
        {
            permission.RegisterPermission(config.AdminPermission, this);
            permission.RegisterPermission(config.UsePermission, this);
            
            cmd.AddChatCommand(config.ChatCommand, this, "ChatCommand");
            
            LoadConfig();
            SaveConfig();
        }

        void OnServerInitialized()
        {
            if (config.AutoStartEvents)
            {
                SetupAutoEvents();
            }
        }

        void Unload()
        {
            if (eventActive)
            {
                EndEvent(true);
            }
            
            eventTimer?.Destroy();
            reminderTimer?.Destroy();
        }

        void OnPlayerDisconnected(BasePlayer player, string reason)
        {
            if (player != null && playerEventItems.ContainsKey(player.userID))
            {
                RemovePlayerEventItems(player.userID);
            }
        }
        #endregion

        #region Chat Commands
        void ChatCommand(BasePlayer player, string command, string[] args)
        {
            if (args.Length == 0)
            {
                ShowHelp(player);
                return;
            }

            switch (args[0].ToLower())
            {
                case "start":
                    if (!HasPermission(player, config.AdminPermission))
                    {
                        SendReply(player, $"{config.ChatPrefix} You don't have permission to start events.");
                        return;
                    }
                    StartEvent(player);
                    break;

                case "stop":
                    if (!HasPermission(player, config.AdminPermission))
                    {
                        SendReply(player, $"{config.ChatPrefix} You don't have permission to stop events.");
                        return;
                    }
                    EndEvent();
                    break;

                case "status":
                    ShowEventStatus(player);
                    break;

                case "items":
                    ShowAvailableItems(player);
                    break;

                case "get":
                    if (!eventActive)
                    {
                        SendReply(player, $"{config.ChatPrefix} No raid event is currently active.");
                        return;
                    }
                    
                    if (!HasPermission(player, config.UsePermission))
                    {
                        SendReply(player, $"{config.ChatPrefix} You don't have permission to use raid event items.");
                        return;
                    }

                    if (args.Length < 2)
                    {
                        SendReply(player, $"{config.ChatPrefix} Usage: /{config.ChatCommand} get <item> [amount]");
                        return;
                    }
                    
                    GiveEventItem(player, args[1], args.Length > 2 ? args[2] : "1");
                    break;

                default:
                    ShowHelp(player);
                    break;
            }
        }
        #endregion

        #region Event Management
        void StartEvent(BasePlayer admin = null)
        {
            if (eventActive)
            {
                if (admin != null)
                    SendReply(admin, $"{config.ChatPrefix} A raid event is already active.");
                return;
            }

            eventActive = true;
            eventStartTime = DateTime.Now;
            eventEndTime = eventStartTime.AddMinutes(config.EventDuration);
            
            Server.Broadcast($"{config.ChatPrefix} <color=#00ff00>RAID EVENT STARTED!</color> Duration: {config.EventDuration} minutes");
            Server.Broadcast($"{config.ChatPrefix} Use <color=#ffff00>/{config.ChatCommand} items</color> to see available raid items!");
            Server.Broadcast($"{config.ChatPrefix} Use <color=#ffff00>/{config.ChatCommand} get <item></color> to spawn raid items!");

            // Set up event end timer
            eventTimer = timer.Once(config.EventDuration * 60f, () => EndEvent());

            // Set up reminder timers
            SetupReminderTimers();
        }

        void EndEvent(bool forced = false)
        {
            if (!eventActive && !forced)
                return;

            eventActive = false;
            
            // Remove all event items from all players
            RemoveAllEventItems();
            
            if (!forced)
            {
                Server.Broadcast($"{config.ChatPrefix} <color=#ff0000>RAID EVENT ENDED!</color> All raid event items have been removed.");
            }

            eventTimer?.Destroy();
            reminderTimer?.Destroy();
            
            eventItems.Clear();
            playerEventItems.Clear();
        }

        void SetupReminderTimers()
        {
            foreach (int interval in config.ReminderIntervals)
            {
                if (interval < config.EventDuration)
                {
                    float delay = (config.EventDuration - interval) * 60f;
                    timer.Once(delay, () => {
                        if (eventActive)
                        {
                            Server.Broadcast($"{config.ChatPrefix} <color=#ffaa00>Raid event ends in {interval} minute(s)!</color>");
                        }
                    });
                }
            }
        }

        void SetupAutoEvents()
        {
            // This would need more complex scheduling logic for production use
            // For now, just a simple example
            PrintWarning("Auto events configured but not implemented in this version");
        }
        #endregion

        #region Item Management
        void GiveEventItem(BasePlayer player, string itemKey, string amountStr)
        {
            if (!config.RaidItems.ContainsKey(itemKey.ToLower()))
            {
                SendReply(player, $"{config.ChatPrefix} Unknown item: {itemKey}");
                ShowAvailableItems(player);
                return;
            }

            if (!int.TryParse(amountStr, out int amount) || amount <= 0)
            {
                SendReply(player, $"{config.ChatPrefix} Invalid amount: {amountStr}");
                return;
            }

            var raidItem = config.RaidItems[itemKey.ToLower()];
            
            if (amount > raidItem.MaxAmount)
            {
                SendReply(player, $"{config.ChatPrefix} Maximum amount for {raidItem.DisplayName} is {raidItem.MaxAmount}");
                return;
            }

            var item = ItemManager.CreateByName(raidItem.ShortName, amount);
            if (item == null)
            {
                SendReply(player, $"{config.ChatPrefix} Failed to create item: {raidItem.ShortName}");
                return;
            }

            // Track this item for removal later
            eventItems.Add(item.uid.Value);
            
            if (!playerEventItems.ContainsKey(player.userID))
            {
                playerEventItems[player.userID] = new List<uint>();
            }
            playerEventItems[player.userID].Add(item.uid.Value);

            if (!player.inventory.GiveItem(item))
            {
                item.Drop(player.transform.position, player.transform.forward * 2f);
            }

            SendReply(player, $"{config.ChatPrefix} You received {amount}x {raidItem.DisplayName}");
        }

        void RemoveAllEventItems()
        {
            int removedCount = 0;
            
            foreach (var itemId in eventItems.ToList())
            {
                var item = BaseNetworkable.serverEntities.Find(new NetworkableId(itemId)) as Item;
                if (item != null)
                {
                    item.RemoveFromWorld();
                    removedCount++;
                }
            }

            PrintWarning($"Removed {removedCount} event items from the game");
        }

        void RemovePlayerEventItems(ulong playerId)
        {
            if (!playerEventItems.ContainsKey(playerId))
                return;

            foreach (var itemId in playerEventItems[playerId].ToList())
            {
                var item = BaseNetworkable.serverEntities.Find(new NetworkableId(itemId)) as Item;
                if (item != null)
                {
                    item.RemoveFromWorld();
                }
                eventItems.Remove(itemId);
            }

            playerEventItems.Remove(playerId);
        }
        #endregion

        #region UI and Information
        void ShowHelp(BasePlayer player)
        {
            SendReply(player, $"{config.ChatPrefix} <color=#ffff00>Raid Event Commands:</color>");
            SendReply(player, $"<color=#cccccc>/{config.ChatCommand} status</color> - Show event status");
            SendReply(player, $"<color=#cccccc>/{config.ChatCommand} items</color> - Show available items");
            if (eventActive)
            {
                SendReply(player, $"<color=#cccccc>/{config.ChatCommand} get <item> [amount]</color> - Get raid items");
            }
            if (HasPermission(player, config.AdminPermission))
            {
                SendReply(player, $"<color=#ff6b6b>/{config.ChatCommand} start</color> - Start raid event (Admin)");
                SendReply(player, $"<color=#ff6b6b>/{config.ChatCommand} stop</color> - Stop raid event (Admin)");
            }
        }

        void ShowEventStatus(BasePlayer player)
        {
            if (!eventActive)
            {
                SendReply(player, $"{config.ChatPrefix} No raid event is currently active.");
                return;
            }

            var timeRemaining = eventEndTime - DateTime.Now;
            SendReply(player, $"{config.ChatPrefix} <color=#00ff00>Event Active!</color>");
            SendReply(player, $"Time Remaining: <color=#ffff00>{timeRemaining.Minutes}m {timeRemaining.Seconds}s</color>");
            
            if (playerEventItems.ContainsKey(player.userID))
            {
                SendReply(player, $"You have <color=#ffaa00>{playerEventItems[player.userID].Count}</color> event items");
            }
        }

        void ShowAvailableItems(BasePlayer player)
        {
            SendReply(player, $"{config.ChatPrefix} <color=#ffff00>Available Raid Items:</color>");
            
            foreach (var kvp in config.RaidItems)
            {
                var item = kvp.Value;
                SendReply(player, $"<color=#cccccc>/{config.ChatCommand} get {kvp.Key}</color> - {item.DisplayName} (Max: {item.MaxAmount})");
            }
        }
        #endregion

        #region Helpers
        bool HasPermission(BasePlayer player, string perm)
        {
            return permission.UserHasPermission(player.UserIDString, perm);
        }
        #endregion
    }
}
