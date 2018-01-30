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
	if (table.getn(gameStats.birdEat) >= 100) then
		return true
	else
		return false
	end
end

local description = "Eat 100 birds in one game"
local value = 100
local icon = "test.png"

return {
	check = check,
	description = description,
	value = value
}