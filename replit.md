# Rust Oxide Raid Event Mod Project

## Project Overview
A Rust Oxide plugin that creates timed raid events where players can spawn raiding items (rockets, C4, explosives, etc.) that are automatically tracked and removed when the event ends. Perfect for "raid wipe day" events on Rust servers.

## Project Architecture

### Core Components
- **RaidEventMod.cs**: Main plugin file with all functionality
- **config/RaidEventConfig.json**: Configuration file for customization
- **lang/en.json**: Language file for localization
- **README.md**: Complete documentation and usage guide

### Key Features Implemented
- Timed raid events with configurable duration
- 13 different raid items (rockets, C4, grenades, launchers, etc.)
- Automatic item tracking and removal system
- Permission-based access control
- Real-time status and reminder system
- Per-player item limits
- Admin controls for event management

### Technical Implementation
- Uses Unity's NetworkableId system for item tracking
- Dictionary-based player item association
- Timer-based event management with automatic cleanup
- Comprehensive error handling and validation

## User Preferences
- User requested a comprehensive raid event mod for Rust Oxide
- Focus on all types of raiding gear (rockets, C4, explosives, launchers)
- Automatic removal of items when event ends
- "Raid wipe day" event concept

## Recent Changes
- **2025-08-02**: Created complete Rust Oxide raid event mod
  - Implemented main plugin with event management system
  - Added 13 different raid items with configurable limits  
  - Created permission system for admin/user access
  - Built automatic item tracking and removal
  - Added configuration and language files
  - Created comprehensive documentation
- **2025-08-02**: Created professional Git repository structure
  - Added professional README with badges and detailed documentation
  - Created LICENSE (MIT), .gitignore, CONTRIBUTING.md, CHANGELOG.md
  - Added INSTALL.md with step-by-step setup instructions
  - Repository ready for GitHub publication

## Dependencies
- Rust server with Oxide framework
- Newtonsoft.Json (for configuration handling)
- Unity Engine (Rust game engine)

## Installation Notes
- Plugin goes in `oxide/plugins/` directory
- Config and language files go in respective `oxide/` subdirectories
- Requires server restart or `oxide.reload RaidEventMod`
- Permissions need to be granted to users/groups

## Command Structure
- Base command: `/raidevent`
- Admin commands: start, stop
- User commands: status, items, get
- Item spawning: `/raidevent get <item> [amount]`

## Configuration Highlights
- 60-minute default event duration
- Configurable reminder intervals (30, 15, 5, 1 minute warnings)
- Per-item player limits (e.g., 20 rockets, 15 C4, 2 RPGs)
- Customizable chat prefix and permissions
- Future auto-scheduling capability framework