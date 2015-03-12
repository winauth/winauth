#Windows Authenticator

*WinAuth is a portable, open-source Authenticator for Windows that provides a time-based RFC 6238 authenticator and common implementations, such as the Google Authenticator. WinAuth can be used with many Bitcoin trading websites as well as supporting Battle.net (World of Warcraft, Hearthstone, Diablo III), Guild Wars 2, Glyph (Rift and ArcheAge), WildStar and RuneScape.*

----

####Notice about  Google Code

This project was formally https://code.google.com/p/winauth but has been migrated to GitHub following Google's
accouncement that [Google Code is being shutdown](http://google-opensource.blogspot.com/2015/03/farewell-to-google-code.html).

----

Bitcoin donations can be sent to `1C4bMkMATViiWYsmJSDUx2MruWM785C36Y`

----

##WinAuth 3.1

WinAuth provides an alternative solution to combine various two-factor authenticator services in one convenient place.

[Download Latest Version (WinAuth-3.1.8)](https://winauth.com/downloads/3.x/WinAuth-3.1.8.zip)

<img src="https://winauth.com/images/winauth3-preview.png" alt="WinAuth3 Preview" />

Features include:

  * Support for time-based RFC 6238 authenticators, and common implementations such as Google Authenticator.
  * Supports Battle.net (World of Warcraft, Hearthstone, Diablo III), GuildWars 2, Trion / Glyph (Rift, ArcheAge), RuneScape and WildStar
  * Supports many Bitcoin trading websites such as Bitstamp, BTC-e, Coinbase, Cryptsy
  * Displays multiple authenticators simultaneously
  * Codes displayed and refreshed automatically or on demand
  * Data is encrypted with your own personal password and can be locked to Windows machine or account
  * Additional password protection per authenticator
  * Restore features for supported authenticators, e.g. Battle.net and Rift
  * Selection of standard or custom icons
  * Hot-key binding with standard or custom actions, such as code notification, keyboard input, and copy to clipboard
  * Portable mode preventing changes to other files or registry settings
  * Export in UriKeyFormat
  * Importing authenticators in UriKeyFormat and from Authenticator Plus for Android 

Visit [WinAuth.com](https://winauth.com) for more information.

###Download Latest Version

Use the following link to download the latest version of WinAuth, or go to the [downloads](https://winauth.com/download) page on winauth.com.

Download [WinAuth-3.1.8.zip](https://winauth.com/downloads/3.x/WinAuth-3.1.8.zip)

Requires [Microsoft .Net 4.0](http://www.microsoft.com/en-us/download/details.aspx?id=17851)

To use:
  * Click the Add button to add or import an authenticator
  * Right-click any authenticator to bring up context menu
  * Click the icon on the right to show the current code, if auto-refresh is not enabled
  * Click cog/options icon for program options

Changes:

3.1.8 RELEASE (2014-08-29)
  * Update Glyph/Trion branding and authenticator to use 6 digits
  * Fix issue 170 with error on copying to clipboard
  * Added Glyph and ArcheAge Icons

3.1.6 RELEASE (2014-07-14)
  * Remove default shadow from frame. Add option in config to restore if desired.
  * Added Icons
  * Add remember Position in portable mode
  * Fix crash on showing menu before password
  * Fix time-sync retry if network error

To compile and build from source:
  * Download source code file or clone project
  * Requires Microsoft Visual Studio 2012 (previous versions will work, but you will need to create your own solution/project files)
  * nuget is used for dependencies
  * any other dependencies are included in the source tree in the 3rd Party folder
  * Use [ILMerge](http://research.microsoft.com/en-us/people/mbarnett/ilmerge.aspx ) to combine assemblies into one single exe file

----

###Upgrading from WinAuth 2.x

When you first run WinAuth, it will check for an existing authenticator from any previous version 2.x, and will prompt you to import. You can either do this immediately or choose to do it later.

When importing, a copy of your authenticator is made and stored, ensuring that your previous file is not changed. In this way, WinAuth 3.0 can be run alongside the previous WinAuth 2.x.

If you have multiple authenticators from version 2.x, you can use the Add button to import each one.

----

##COMMON QUESTIONS

####Is it secure? Is it safe?

All authenticators just provide another layer of security. None are 100% effective.

A physical/keychain device is by far the best protection. Although still subject to any man-in-the-middle attack, there is no way to get at the secret key stored within it. If you are at all concerned, get one of these.

An iPhone app or app on a non-rooted Android device is also secure. There is no way to get at the secret key stored on the device, however, some apps provides way to export the key that could compromise your authenticator if you do not physically protect your phone. Also if those apps backup their data elsewhere, that data could be vulnerable.

A rooted-Android phone can have your secret key read off it by an app with access. Some apps also do not encrypt the keys and so this should be considered risky.

WinAuth stores you secret key in an encrypted file on your computer. Whilst it cannot therefore provide the same security as a separate physical device, as much as possible has been done to protect the key on your machine. As above, physical access to your machine would be the only way to compromise any authenticator.

####I'm concerned this might be a virus / malware / keylogger

WinAuth has been around and used since mid-2010 and has been downloaded by thousands of users.

It has always been open-source allowing everyone to inspect and review the code. A binary is provided, but the source code is always released simultaneously so that you can review the code and build it yourself.

No personal information is sent out to any other 3rd party servers. It never even sees your account information, only your authenticator details.

There are no other executables installed on your machine. There is no installer doing things you are unable to monitor. WinAuth is portable so you can just run it from anywhere.

####I found WinAuth on another website, is it the same thing?

WinAuth source code is uploaded to GitHub at http://github.com/cdmackie/winauth and pre-built binaries and installers are on [winauth.com](https://winauth.com). It had been hosted using Google Code at https://code.google.com/p/winauth, but has been moved to GiHub since Google Code is being closed down. It is not published anywhere else, so please do not download any other programs claiming to be WinAuth.

###Battle.net

####Is this against the TOS (Terms of Service)? Could I get banned?

No. Whlist Blizzard does not support or endorse WinAuth, they are not against its use. 

`"As you may have already seen from the small related section on the creator's website, you can use the program if you wish, but I should make clear that we obviously won't endorse it nor support or encourage its use."` [(source)](http://eu.battle.net/wow/en/forum/topic/2569217651)

####Will it work with Mists of Pandaria or Diablo III?

WinAuth provides security for your Battle.net account and so can secure any games that make use of authenticator. This includes all version of World of Warcraft, Starcraft 2 and Diablo III.

####When I add an authenticator it asking for a 10 digit serial number, but WinAuth is showing a 12 or 14 digit number?

There are two different types of authenticator: the physical keychain device and the Mobile authenticator. WinAuth works like the Mobile Authenticator, so you must make sure that is the one you are adding to your Battle.Net account.

####Do I need to have the existing Mobile Authenticator app or keychain to use WinAuth?

No. WinAuth is completely independent and can register a new authenticator code with the Battle.Net servers. You don't need to have or have used the official app beforehand.

####Where does WinAuth save my authenticator information?

Unlike some other authenticator applications, WinAuth does not store/send your information to any 3rd party servers. Your authenticator is saved in a file in a location that you specify. Optionally a backup can be made and emailed to you, however, this is really now redundant given the addition of the new Restore feature.

####I created an authenticator with WinAuth. Can I switch to using the iPhone/Android app?

With the Restore feature added in the official app, you can copy your authenticator between devices. In WinAuth, use the menu to show the "Restore Code" and then that can be added into the official app.

Of course, this means you can also copy your official app authenticator to WinAuth, by getting the Restore code and then using the "Restore..." feature in WinAuth. 

----

##More Information

  * [Battle.Net](http://www.battle.net)
  * [Battle.Net Authenticator Specification](http://www.wowwiki.com/Battle.net_Mobile_Authenticator_Specification)
  * [Google Authenticator](http://code.google.com/p/google-authenticator)

All trademarks are recognised, including but not limited to:

  * Blizzard, Battle.net, World of Warcraft, Starcraft, Diablo
  * ArenaNet, Guild Wars 2
  * Trion, Rift
  * Google
  * Microsoft

----

##Author

WinAuth was written by Colin Mackie. Copyright (C) 2010-2015.

----

##License

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License  along with this program.  If not, see http://www.gnu.org/licenses/.
