const sqlite = require("sqlite3");
const path = require("path");
const bot = require("./botfunctions.js");
const fs = require("fs");
const db = new sqlite.Database(path.join(__dirname,"./bbb.db"));
const config = require("./config.json");

function getData(getDataCallback)
{
	var getRowCallback = function(error, row)
	{
		try
		{
			if(error)
			{
				console.log(`Error: ${error}`);
			}
			else
			{
				getDataCallback(row);
			}
		}
		catch(exception)
		{
			console.log(`Error: ${exception}`);
		}
	}
	
	db.each("SELECT * FROM BBB", getRowCallback);
}

function addWeight(kgs)
{
	db.run("UPDATE BBB SET Weight = Weight + ?;",kgs);
}

function tick()
{
	addWeight(-0.001);
	addHunger(0.01);
}

exports.admin = function (message, args)
{
	if(args[0] === "getbbb")
	{
		var callback = function(bbb)
		{
			try
			{
				message.channel.send(bbb.Weight);
			}
			catch(exception)
			{
				console.log(`Error: ${exception}`);
			}
		}
		
		getData(callback);
	}	
}

exports.feed = function (message, args)
{
	if(args.length > 1)
	{
		message.channel.send(":warning: You can't feed more than one person!");
	}
	else if(args.length < 1)
	{
		message.channel.send(":warning: Please specify your feedee!");
	}
	else
	{
		//Send the post
		var callback = function(error, dir)
		{
			var files = dir.filter(x => { return x.includes("."); });
			var file = bot.array.getRandomArray(files);
			var fileOptions = { files: [path.join(config.progFolder, file)] };
			
			var flavortext = `${message.author} fed ${args[0]}!`	
			message.channel.send(flavortext, fileOptions);
		}
		
		//Read the files
		fs.readdir(config.progFolder, callback)
	}
}

exports.use = function (message, args)
{
	var itemCode = args[0].charCodeAt(0);
	if(itemCode === 9878)//:scales:
	{
		var callback = function(bbb)
		{
			try
			{
				var displayNumber = Math.round(bbb.Weight);
				message.channel.send(`I currently weigh... ***${displayNumber}kgs***!`);
			}
			catch(exception)
			{
				console.log(`Error: ${exception}`);
			}
		}
		
		getData(callback);
	}
	else if(itemCode === 55356)//:cake:
	{
		addWeight(0.2);
		message.channel.send(`Yum! Thanks!`);
	}
	else
	{
		console.log(`Unknown ItemCode: ${itemCode}`)
	}
}

exports.status = function (message, args)
{
	
}