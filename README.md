<img align="right" width="80" height="80" data-rmimg src="https://endev.at/content/projects/Q-Drive/QDriveLogo.svg">
  
# Q-Drive
![GitHub](https://img.shields.io/github/license/TobiHatti/Q-Drive)
[![GitHub Release Date](https://img.shields.io/github/release-date-pre/TobiHatti/Q-Drive)](https://github.com/TobiHatti/Q-Drive/releases)
[![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/TobiHatti/Q-Drive?include_prereleases)](https://github.com/TobiHatti/Q-Drive/releases)
[![GitHub last commit](https://img.shields.io/github/last-commit/TobiHatti/Q-Drive)](https://github.com/TobiHatti/Q-Drive/commits/master)
[![GitHub issues](https://img.shields.io/github/issues-raw/TobiHatti/Q-Drive)](https://github.com/TobiHatti/Q-Drive/issues)
[![GitHub language count](https://img.shields.io/github/languages/count/TobiHatti/Q-Drive)](https://github.com/TobiHatti/Q-Drive)

![image](https://endev.at/content/projects/Q-Drive/QDriveBanner300.svg)

[![Build status](https://ci.appveyor.com/api/projects/status/7pio7mrkhysxycnj?svg=true)](https://ci.appveyor.com/project/TobiHatti/q-drive)

Q-Drive allows you to quickly connect to network-drives, either linked to a PC or linked to a user account. It can automatically reconnect network-drives after a system restart,
or prompt the user for a password to connect to network drives in the first place.

## Features

- 2 Operation modes
  - Local Mode (for 1-user applications)
  - Online Mode (for multi-user applications or enterprices)
- Connecting quickly and reliable to network drives
- Manage Network-Shares for multiple users remotely
- Secure password encryption for drive authentication

![image](https://endev.at/content/projects/Q-Drive/projectImages/QDriveAllWindows.png)

## Usage

After installing Q-Drive, the first step is to configure it using the `Q-Drive Setup` (launched by default after installation). 
You can pick between 2 operation modes:

__Saving mapped drives locally__

Drive-data and authentication data gets saved on your device -> No MySQL-Database required.

This option is suitable for 1-user applications or private users.

When selecting QD-Local, your only way to connect network-drives is to add a new private network drive.


__Saving mapped drives online__

Drive-data and authentication data gets saved on your device (for device-linked drives), or in an online MySQL-Database (for user-linked drives).

This option is generally more usefull for small enterprices or network enthusiasts. 
(A MySQL-Database is required for this mode to work)

When selecting QD-Online, there are 3 ways to connect network drives:
- Adding public drives (provided by the QD-Admin of the network)
- Adding private drives (linked to the user-account)
- Adding private drives (linked to the device)

QD-Online is generally more usefull, since you can quickly switch between a set of network-drives by 
just logging off and logging into Q-Drive using another user-account.

![image](https://endev.at/content/projects/Q-Drive/projectImages/QDriveAddDrives.png)

After finishing the setup, you can add your network-drives in the `Q-Drive Manager`.

If you are using QD-Online, some options are disabled by default. These options can be enabled in the `Q-Drive Admin Console`. To log into the admin-console, 
the master-password, which was set at the Q-Drive setup, is required. 
From the admin-console, the QD-Admin (usually the network or system administrator) can register users, change settings and create public drives.
The advantage of using public drives is, that e.g. the drive-path can be changed in the admin-console, and every client which has added this drive 
in the `Q-Drive Manager` will automatically connect to the new path, without any extra work.

## Security

![image](https://endev.at/content/projects/Q-Drive/projectImages/QDriveLogin.png)

All passwords and authentication informations (MySQL connection data, user passwords, network drive authentication) get hashed using SHA256 encryption. 

Confidential data stored online, such as network-authentication, is stored in a way that only user A can decrypt data from user A and user B can only decrypt data from user B. 
As for data stored locally, such as authentication-information for device-linked network drives, the data gets encrypted using a unique device ID. 

If you need to migrate your Q-Drive settings and connections to another device, you can create a Q-Drive-Backup which can be imported on your new device.

__NOTE: The Q-Drive backup-files (`*.qdbackup`) are only weakly encrypted. Use them only when required and store them savely to avoid mallitious use!__


## Downloads

Get the current version [here](https://github.com/TobiHatti/Q-Drive/releases/latest)

Version: 1.1.0

MD5: A8012335DF8B4B1255666C88197D7405

