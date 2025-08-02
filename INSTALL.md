# Installation Guide

## Quick Install

1. **Download the plugin**
   ```bash
   wget https://github.com/yourusername/rust-oxide-raid-event-mod/releases/latest/download/RaidEventMod.cs
   ```

2. **Install on your server**
   - Copy `RaidEventMod.cs` to `oxide/plugins/`
   - Copy `config/RaidEventConfig.json` to `oxide/config/`
   - Copy `lang/en.json` to `oxide/lang/`

3. **Reload or restart**
   ```
   oxide.reload RaidEventMod
   ```

## Detailed Installation

### Prerequisites
- Rust dedicated server
- Oxide mod framework installed
- Admin access to server files

### Step-by-Step

#### 1. Download Files
Clone or download this repository:
```bash
git clone https://github.com/yourusername/rust-oxide-raid-event-mod.git
cd rust-oxide-raid-event-mod
```

#### 2. File Placement
```
YourRustServer/
├── oxide/
│   ├── plugins/
│   │   └── RaidEventMod.cs          # Main plugin file
│   ├── config/
│   │   └── RaidEventConfig.json     # Configuration
│   └── lang/
│       └── en.json                  # Language file
```

#### 3. Set Permissions
Grant permissions to users or groups:
```bash
# For individual users
oxide.grant user <username> raidevent.use
oxide.grant user <adminname> raidevent.admin

# For groups
oxide.grant group default raidevent.use
oxide.grant group admin raidevent.admin
```

#### 4. Configuration
Edit `oxide/config/RaidEventConfig.json` to customize:
- Event duration
- Item limits
- Reminder intervals
- Permissions

#### 5. Test Installation
1. Join your server
2. Type `/raidevent` in chat
3. If you see the help menu, installation was successful

## Troubleshooting

### Plugin Won't Load
- Check server console for error messages
- Verify file is in correct directory
- Ensure Oxide is up to date

### Commands Don't Work
- Verify you have the correct permissions
- Check plugin loaded with `oxide.plugins`
- Make sure you're using the right command syntax

### Items Not Spawning
- Confirm an event is active with `/raidevent status`
- Check you have `raidevent.use` permission
- Verify item names are correct (use `/raidevent items`)

### Getting Help
- Check the [Issues](https://github.com/yourusername/rust-oxide-raid-event-mod/issues) page
- Join our [Discord](https://discord.gg/yourinvite)
- Read the main [README](README.md) for detailed usage

## Updating

1. Download the latest version
2. Replace `RaidEventMod.cs` in `oxide/plugins/`
3. Update config file if needed (check CHANGELOG.md)
4. Reload: `oxide.reload RaidEventMod`

## Uninstalling

1. Remove `RaidEventMod.cs` from `oxide/plugins/`
2. Delete `RaidEventConfig.json` from `oxide/config/`
3. Delete `en.json` from `oxide/lang/` (if not used by other plugins)
4. Remove permissions: `oxide.revoke group default raidevent.use`