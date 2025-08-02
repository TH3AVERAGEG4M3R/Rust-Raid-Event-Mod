# 🚀 Rust Oxide Raid Event Mod

[![Rust](https://img.shields.io/badge/Rust-Server-orange.svg)](https://rust.facepunch.com/)
[![Oxide](https://img.shields.io/badge/Oxide-Plugin-blue.svg)](https://umod.org/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Version](https://img.shields.io/badge/Version-1.0.0-brightgreen.svg)](#)

A comprehensive Oxide plugin for Rust servers that creates timed raid events where players can spawn raiding items that are automatically removed when the event ends. Perfect for "raid wipe day" events and special server occasions.

## Features

- **Timed Raid Events**: Start events with configurable duration
- **Comprehensive Raid Items**: Support for all major raiding tools including rockets, C4, satchels, grenades, and launchers
- **Automatic Item Removal**: All event items are tracked and removed when the event ends
- **Permission System**: Admin and user permissions for event management
- **Reminder System**: Configurable time warnings before event ends
- **Item Limits**: Per-player limits on each type of raid item
- **Chat Commands**: Easy-to-use command interface
- **Configurable**: Extensive configuration options

## Supported Raid Items

- **Rockets**: Basic, HV, Incendiary, MLRS
- **Explosives**: C4 (Timed Explosive Charge), Satchel Charges
- **Launchers**: Rocket Launcher, Multiple Grenade Launcher
- **Ammunition**: Explosive 5.56 rounds, 40mm HE/Smoke grenades
- **Grenades**: Beancan, F1 grenades

## Installation

1. Copy `RaidEventMod.cs` to your server's `oxide/plugins/` directory
2. Copy the `config/` and `lang/` folders to your server's `oxide/` directory
3. Restart your server or use `oxide.reload RaidEventMod`

## Permissions

- `raidevent.admin` - Allows starting and stopping events
- `raidevent.use` - Allows players to spawn raid items during events

Grant permissions using: `oxide.grant user <username> raidevent.use`
Or for groups: `oxide.grant group <groupname> raidevent.use`

## Commands

### Player Commands
- `/raidevent status` - Show current event status and time remaining
- `/raidevent items` - List all available raid items and their limits
- `/raidevent get <item> [amount]` - Spawn raid items (during active events)

### Admin Commands
- `/raidevent start` - Start a new raid event
- `/raidevent stop` - Stop the current event (removes all items immediately)

### Item Keys for `/raidevent get` command:
- `rocket` - Basic Rocket (max 20)
- `hvrocket` - HV Rocket (max 10) 
- `incendiary` - Incendiary Rocket (max 10)
- `c4` - Timed Explosive Charge (max 15)
- `satchel` - Satchel Charge (max 20)
- `rpg` - Rocket Launcher (max 2)
- `mlrs` - MLRS Rocket (max 5)
- `expammo` - Explosive 5.56 Rifle Ammo (max 100)
- `40mmhe` - 40mm HE Grenade (max 30)
- `40mmsmoke` - 40mm Smoke Grenade (max 20)
- `beancan` - Beancan Grenade (max 40)
- `f1` - F1 Grenade (max 30)
- `gl` - Multiple Grenade Launcher (max 1)

## Configuration

The plugin creates a configuration file at `oxide/config/RaidEventConfig.json` with the following options:

```json
{
  "Event Duration (minutes)": 60,
  "Reminder Intervals (minutes)": [30, 15, 5, 1],
  "Available Raid Items": { ... },
  "Admin Permission": "raidevent.admin",
  "Use Permission": "raidevent.use",
  "Chat Command": "raidevent",
  "Chat Prefix": "<color=#ff6b6b>[Raid Event]</color>",
  "Auto Start Events": false,
  "Auto Event Times (24h format, e.g., 18:00)": ["18:00", "20:00"]
}
```

### Configuration Options

- **Event Duration**: How long events last in minutes (default: 60)
- **Reminder Intervals**: When to send time warnings before event ends
- **Available Raid Items**: Customize which items are available and their limits
- **Permissions**: Customize permission names
- **Chat Command**: Change the base command (default: raidevent)
- **Chat Prefix**: Customize chat message appearance
- **Auto Start Events**: Enable automatic event scheduling (future feature)

## How It Works

1. **Event Start**: Admin runs `/raidevent start`
2. **Item Tracking**: Every item spawned through the plugin is tracked with a unique ID
3. **Player Tracking**: Items are associated with specific players
4. **Time Management**: Timer runs for the configured duration with reminders
5. **Event End**: All tracked items are automatically removed from the game
6. **Cleanup**: Player disconnections also trigger item cleanup

## Example Usage

```
Admin: /raidevent start
Server: [Raid Event] RAID EVENT STARTED! Duration: 60 minutes

Player: /raidevent items
Server: [Raid Event] Available Raid Items:
        /raidevent get rocket - Rocket (Max: 20)
        /raidevent get c4 - Timed Explosive Charge (Max: 15)
        ...

Player: /raidevent get rocket 10
Server: [Raid Event] You received 10x Rocket

Player: /raidevent get c4 5
Server: [Raid Event] You received 5x Timed Explosive Charge

[30 minutes later]
Server: [Raid Event] Raid event ends in 30 minute(s)!

[After 60 minutes]
Server: [Raid Event] RAID EVENT ENDED! All raid event items have been removed.
```

## Technical Details

- **Item Tracking**: Uses Unity's NetworkableId system to track items
- **Memory Management**: Automatically cleans up tracking data
- **Performance**: Efficient removal system that doesn't lag the server
- **Safety**: Items are removed even if players disconnect
- **Compatibility**: Works with standard Rust item system

## Support

This plugin is designed for Rust servers running the Oxide framework. It has been tested with the latest version of Rust and Oxide.

For issues or suggestions, please refer to the server administrator.

## 📊 Installation Statistics

![Downloads](https://img.shields.io/badge/Downloads-0-blue.svg)
![Contributors](https://img.shields.io/badge/Contributors-1-green.svg)
![Last Commit](https://img.shields.io/badge/Last%20Commit-2025--08--02-green.svg)

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

- **Issues**: GitHub Issues (update URL when repository is created)
- **Discussions**: GitHub Discussions (update URL when repository is created)  
- **Discord**: Join our Discord Server (add invite link if available)

## ⭐ Show Your Support

Give a ⭐️ if this project helped you!

## 🔄 Version History

- **v1.0.0**: Initial release with full raid event functionality
  - Complete item tracking system
  - 13 different raid items supported
  - Admin permission system
  - Configurable event duration and reminders

## 🧪 Development

### Prerequisites
- Rust server with Oxide framework
- Basic understanding of C# and Oxide plugin development

### Testing
1. Copy plugin to test server
2. Grant yourself admin permissions
3. Test event lifecycle with `/raidevent start`
4. Verify item removal after event ends

### Building
This plugin is distributed as source code. Simply place the `.cs` file in your Oxide plugins directory.

---

**Made with ❤️ for the Rust community**