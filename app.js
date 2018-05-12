//Load dependencies
const discord = require("discord.js");
const fs = require("fs");
const path = require("path");
const bbb = require("./bbb.js");
const bot = require("./botfunctions.js");

//Load config
const config = require("./config.json");

//Initialise constants
const client = new discord.Client();
const kgLbConversionFactor = 0.453592;

//Ready event
client.on("ready", async () =>
{
	console.log("Bot Started");
	client.user.setActivity(bot.array.getRandomArray(config.activities));
});

//Guild join/leave events
client.on("guildCreate", guild => { console.log(`Joined: ${guild.name}`);});
client.on("guildDelete", guild => { console.log(`Left: ${guild.name}`);});

client.on("message", async message =>
{
	try
	{
		//Ignore bots or irrelevant messages
		const messageContent = message.content;	
		if(message.author.bot || messageContent.indexOf(config.prefix) !== 0) return;
	  
		//Get the message
		const args = messageContent.slice(config.prefix.length).trim().split(" ");
		const command = args.shift();
			  
		if(command === "use")
		{
			bbb.use(message, args);
		}
		
		if(command === "piggy")
		{
			message.channel.send(":pig2:");
			return;
		}
		
		if(command === "feed")
		{
			bbb.feed(message, args);
		}
		
		if(command === "help")
		{
			var callback = function(error, content)
			{
				if(error)
				{
					console.log(`Error: ${error}`);
				}
				else
				{
					message.channel.send(content);
				}
			}
			fs.readFile(path.join(__dirname, "./README.md"), "utf8", callback);
		}
		
		var isKgs = command === "kgs";
		var isLbs = command === "lbs";
		if(isKgs || isLbs)
		{
			var source = args[0];
			var kgs = isLbs ? source * kgLbConversionFactor : source;
			kgs = Math.round(kgs);
			var lbs = isKgs ? source / kgLbConversionFactor : source;
			lbs = Math.round(lbs);
			
			var response = `**${lbs}lbs** is **${kgs}kgs**!`;
			response += "\n\n"
			
			if(kgs < 60) response += "That's not very interesting is it?"
			else if(kgs >= 200) response += "That's a pork-a-saurus!"
			else if(kgs >= 100) response += "What a big cutie!";
			else if(kgs >= 70) response += "A little too much :cake:?"
			else if(kgs >= 60) response += "Pretty average."
			
			message.channel.send(response);
		}
		
		if(command === "admin" && message.author.tag === "FairyMaku#0920")
		{
			bbb.admin(message, args);
		}
	}
	catch(exception)
	{
		console.log(`Error: ${exception}`);
	}
});

client.login(config.token);
           