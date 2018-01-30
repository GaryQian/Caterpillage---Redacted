
goal1 = nil
goal2 = nil
goal3 = nil

local function create()
	if (userdata.goal1 <= config.maxGoals) then
		goal1 = require("goal" .. userdata.goal1)
	else
		goal1 = nil
	end
	
	if (userdata.goal2 <= config.maxGoals) then
		goal2 = require("goal" .. userdata.goal2)
	else
		goal2 = nil
	end
	
	if (userdata.goal3 <= config.maxGoals) then
		goal3 = require("goal" .. userdata.goal3)
	else
		goal3 = nil
	end
end

local function nextGoal1()
	userdata.money = userdata.money + goal1.value
	userdata.goal1 = userdata.nextGoal
	if (userdata.nextGoal <= config.maxGoals) then
		goal1 = require("goal" .. userdata.goal1)
		userdata.nextGoal = userdata.nextGoal + 1
	else
		goal1 = nil
	end
	
	love.filesystem.write("userdata.json", JSON:encode_pretty(userdata))
end

local function nextGoal2()
	userdata.money = userdata.money + goal2.value
	userdata.goal2 = userdata.nextGoal
	if (userdata.nextGoal <= config.maxGoals) then
		goal2 = require("goal" .. userdata.goal2)
		userdata.nextGoal = userdata.nextGoal + 1
	else
		goal2 = nil
	end
	
	love.filesystem.write("userdata.json", JSON:encode_pretty(userdata))
end

local function nextGoal3()
	userdata.money = userdata.money + goal3.value
	userdata.goal3 = userdata.nextGoal
	if (userdata.nextGoal <= config.maxGoals) then
		goal3 = require("goal" .. userdata.goal3)
		userdata.nextGoal = userdata.nextGoal + 1
	else
		goal3 = nil
	end
	
	love.filesystem.write("userdata.json", JSON:encode_pretty(userdata))
end

return {
	create = create,
	nextGoal1 = nextGoal1,
	nextGoal2 = nextGoal2,
	nextGoal3 = nextGoal3

}