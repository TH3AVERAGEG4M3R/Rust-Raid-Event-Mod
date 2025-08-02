# Contributing to Rust Oxide Raid Event Mod

Thank you for your interest in contributing to this project! We welcome contributions from the community.

## How to Contribute

### Reporting Bugs

1. Check if the bug has already been reported in [Issues](https://github.com/yourusername/rust-oxide-raid-event-mod/issues)
2. If not, create a new issue with:
   - Clear description of the bug
   - Steps to reproduce
   - Expected vs actual behavior
   - Rust server version and Oxide version
   - Plugin configuration (if relevant)

### Suggesting Features

1. Check existing issues for similar suggestions
2. Create a new issue with the "enhancement" label
3. Describe the feature and its benefits
4. Explain how it fits with the plugin's purpose

### Code Contributions

#### Getting Started

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature-name`
3. Make your changes
4. Test thoroughly on a development server
5. Commit with clear, descriptive messages
6. Push to your fork
7. Create a Pull Request

#### Coding Standards

- Follow C# naming conventions
- Use meaningful variable and method names
- Add comments for complex logic
- Maintain consistent indentation (4 spaces)
- Keep methods focused and concise

#### Testing

Before submitting:

1. Test on a local Rust server with Oxide
2. Verify all commands work correctly
3. Test event lifecycle (start → items → end → cleanup)
4. Confirm no errors in server console
5. Test with multiple players if possible

### Pull Request Process

1. Ensure your code follows the project's coding standards
2. Update documentation if you're adding new features
3. Add or update tests if applicable
4. Update the README if necessary
5. Reference any related issues in your PR description

### Code Review

All submissions require review. We may ask for changes before merging. Please be patient and responsive to feedback.

## Development Setup

1. Set up a Rust development server with Oxide
2. Place the plugin in `oxide/plugins/`
3. Grant yourself permissions: `oxide.grant user <username> raidevent.admin`
4. Test changes with `/raidevent start`

## Questions?

Feel free to open an issue for any questions about contributing.

Thank you for helping make this plugin better!