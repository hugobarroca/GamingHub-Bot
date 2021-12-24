# GamingHub-Bot

### Summary

GamingHubBot is a discord bot created for usage within my own discord server. Currently it supports changing roles over text commands and reactions. It relies on Discord.NET for communication with the Discord API.

### How to run

To run this program, first you need to dotnet SDK installed, which is available for both [Windows, Linux and Mac](https://dotnet.microsoft.com/en-us/download). To confirm that the dotnet environment was installed properly, you can simply run the command "dotnet" in your terminal, and check for any output.  
Then, you can compile the sourcecode into a folder with the following command:

`dotnet build -o <OUTPUT_DIRECTORY>`

To run the application, navigate to the target directory, create a token.txt file place you bot token inside. Next, run the following command to actually run the bot:

`dotnet run`

### Supported Commands

**!help:** Displays this informative message.  
**!ping:** Respondes with a pong to confirm online status.  
**!permittedroles:** Lists all the roles that you can add or remove from yourself with this bot.  
**!addrole {role}:** Adds specified role to the user. Use quotations marks for roles with spaces.  
**!removerole {role}:** Removes specified role from the user. Use quotations marks for roles with spaces.  
**!weeb:** Gives you a random anime quote.  
