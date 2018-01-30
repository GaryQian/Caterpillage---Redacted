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
	local success = false
	local flyTime = 0
	local tableSize = table.getn(gameStats.land)
	for i,v in ipairs(gameStats.leap) do
		if (i + 1 <= tableSize) then
			flyTime = flyTime + gameStats.land[i + 1] - v
		end
	end
	if (flyTime >= 60) then
		success = true
	end
	return success
end

local description = "Be airborne for a total of 1 minute in one game"
local value = 100
local icon = "test.png"

return {
	check = check,
	description = description,
	value = value
}