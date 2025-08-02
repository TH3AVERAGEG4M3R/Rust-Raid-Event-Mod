# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2025-08-02

### Added
- Initial release of Rust Oxide Raid Event Mod
- Timed raid events with configurable duration (default 60 minutes)
- 13 different raid items with individual player limits:
  - Basic Rockets (max 20)
  - HV Rockets (max 10)
  - Incendiary Rockets (max 10)
  - Timed Explosive Charges/C4 (max 15)
  - Satchel Charges (max 20)
  - Rocket Launcher (max 2)
  - MLRS Rockets (max 5)
  - Explosive 5.56 Rifle Ammo (max 100)
  - 40mm HE Grenades (max 30)
  - 40mm Smoke Grenades (max 20)
  - Beancan Grenades (max 40)
  - F1 Grenades (max 30)
  - Multiple Grenade Launcher (max 1)
- Automatic item tracking and removal system
- Permission-based access control (`raidevent.admin` and `raidevent.use`)
- Chat commands for event management and item spawning
- Configurable reminder system (30, 15, 5, 1 minute warnings)
- JSON configuration file support
- Language file support for localization
- Comprehensive error handling and validation
- Player disconnect cleanup (removes items when players leave)

### Features
- `/raidevent start` - Start a new raid event (admin only)
- `/raidevent stop` - Stop current event and remove all items (admin only)
- `/raidevent status` - Show current event status and remaining time
- `/raidevent items` - List all available raid items and limits
- `/raidevent get <item> [amount]` - Spawn raid items during active events

### Technical
- Unity NetworkableId system for reliable item tracking
- Dictionary-based player-item association
- Timer-based event management with automatic cleanup
- Memory-efficient removal system
- Configuration hot-reload support

## [Unreleased]

### Planned
- Auto-scheduling feature for recurring events
- GUI interface for easier item selection
- Team-based item limits
- Event statistics and logging
- Integration with other popular Rust plugins
- Multi-language support expansion