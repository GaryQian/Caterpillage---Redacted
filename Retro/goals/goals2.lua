--[[
gameStats.birdEat = {}
gameStats.birdShoot = {}
gameStats.moleEat = {}
gameStats.moleShoot = {}
gameStats.moleHits = {}
gameStats.bearEat = {}
gameStats.bearShoot = {}
gameStats.turretEat = {}
gameStats.turretShoot = {}
gameStats.turretSpawn = {}
gameStats.shots = {}
gameStats.healthPowerups = {}
gameStats.freezePowerups = {}
gameStats.shieldPowerups = {}
gameStats.planetClear = {}
gameStats.damaged = {}
gameStats.damageTaken = {}
gameStats.healed = {}
gameStats.healthGained = {}
gameStats.bossKills = {}
gameStats.sceneryEaten = {}
gameStats.leap = {}
gameStats.land = {}
--]]
local function check()
	if (gameStats.planetClear[1] ~= nil and gameStats.healthPowerups[3] ~= nil and gameStats.freezePowerups[1] ~= nil and gameStats.shieldPowerups[1] ~= nil and gameStats.healthPowerups[1] < gameStats.planetClear[1] and gameStats.healthPowerups[2] < gameStats.planetClear[1] and gameStats.healthPowerups[3] < gameStats.planetClear[1] and gameStats.freezePowerups[1] < gameStats.planetClear[1] and gameStats.shieldPowerups[1] < gameStats.planetClear[1]) then
		return true
	else
		return false
	end
end

local description = "Collect all powerups on the first planet"
local value = 100
local icon = "test.png"

return {
	check = check,
	description = description,
	value = value
}