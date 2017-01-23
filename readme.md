# PERI.SMS

A simple SMS library for sending & receiving text messages using dongle

## Getting Started

### Prerequisites

Here thing/s you need

- [Visual Studio](https://www.visualstudio.com/)
- Huawei mobile dongle
	- Tested on the ff models:
		- E303
		- E153u-2
- [Huawei mobile partner](http://huawei-mobile-partner.software.informer.com/download/)
	- This serves as the main driver for the dongle.
		1. Install the Huawei Mobile Partner then check if it has opened a COMPort in Device Manager
		2. You need to close the apllication once installed because it will result conflict. You only need the driver & not application.

## People to blame

The following personnel are responsible for managing this project.
- [actchua@periapsys.com](mailto:actchua@periapsys.com)

## Developer's Guide

The project uses the ff. technology:
- .Net framework 4.5

Solution structure:

- PERI.SMS.Core
	- The main project
		- COMPort
			- Contains the COMPort functionalies(read, response, etc.) 
        - SMS
        	- Send/Recieve
- PERI.SMS.Model
	- The entities or objects of the project
- PERI.SMS.TEST
	- The Unit Test project
	- Sample codes are available here
- PERI.SMS.XU
	- The XUnit project for extended testing		
