# totpa - A CLI Tool for Managing TOTP Codes in Testing Environments

## Overview
`totpa` is a simple and efficient CLI tool designed to **persist and retrieve TOTP (Time-Based One-Time Passwords) during pipeline execution** in testing environments. This tool allows automated test pipelines to handle 2FA authentication seamlessly, eliminating the need for manual input.

## Features
✅ **Add TOTP accounts** using a secret key or TOTP URL  
✅ **List all stored accounts** for quick reference  
✅ **Generate a TOTP code on demand** with expiration countdown  
✅ **Configure storage type** (JSON, SQLite, SQL, Redis, or WMI)  
✅ **Designed for CI/CD pipelines** to persist TOTP secrets during test execution  
✅ **Cross-platform support** (Windows, Linux, macOS)  

## Installation
You can install `totpa` globally as a .NET CLI tool:
```sh
dotnet tool install --global totpa
```
To verify the installation, run:
```sh
totpa --help
```

## Usage

### **Add a TOTP Account**
```sh
totpa add -n myapp -u "otpauth://totp/App:User?secret=JBSWY3DPEHPK3PXP&issuer=App"
```

### **List Stored Accounts**
```sh
totpa list
```

### **Generate a TOTP Code**
```sh
totpa get -n myapp
```
**Example output:**
```
Generated TOTP Code for myapp: 123456 (expires in 20s)
```

### **Change Storage Type**
`totpa` supports multiple storage options:
```sh
totpa settings -t json   # Use JSON storage
```
```sh
totpa settings -t sqlite # Use SQLite database
```

## Use in CI/CD Pipelines
`totpa` can be integrated into your automated test pipelines to manage 2FA authentication during execution.

Example usage in **GitHub Actions**:
```yaml
jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - name: Install totpa
        run: dotnet tool install --global totpa --version 0.1.0
      - name: Add TOTP Account
        run: totpa add -n testapp -u "otpauth://totp/App:User?secret=JBSWY3DPEHPK3PXP&issuer=App"
      - name: Generate TOTP Code
        run: totpa get -n testapp
```

## License
This project is licensed under the MIT License. See the `LICENSE.txt` file for details.

## Contributing
Feel free to open issues or submit pull requests to improve `totpa`. Feedback is always welcome!

## Author
Developed by **Your Name**

## Support
For questions, reach out via GitHub issues or email.

