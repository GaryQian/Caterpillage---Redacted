 
selectedPlants = {}
selectedBuildings = {}
selectedMisc = {}
bossLevel = false
boss = {}
bossNumber = 0

local function generatePlanet()
	scenery = {}
	enemies = {}
	planet = {}
	blacks = {}
	shots = {}
	texts = {}
	heroBullets = {}
	bullets = {}
	powerups = {}
	enemyBullets = {}
	enemyCollider = {}
	sceneryCollider = {}
	powerupCollider = {}
	--pick out the props for the planet
	selectedPlants = {}
	selectedBuildings = {}
	selectedMisc = {}
	selectedEnemies = {}
	spawnRates = {} -- spawn rate for mole and bird
	boss = nil
	hero.isUnderground = false
	
	collectgarbage()
	
	levelRange = 10
	
	planetGenerated = true
	
	tempLevel = level
	if (level >= levelRange) then
		tempLevel = levelRange
	end
	stdDev = config.generator.stdDev --this adjusts the variety of types of objects on each planet
	while (selectedPlants[1] == nil) do
		for i, v in pairs(plants) do
			if (math.random() < 0.00 + (1/(stdDev* math.sqrt(math.pi * 2))) * math.exp(-math.pow(i - ((tempLevel/(levelRange/2)) * (table.getn(plants)/2)), 2)/(2 * math.pow(stdDev, 2)))) then
				table.insert(selectedPlants, v)
			end
		end
	end
	while selectedBuildings[1] == nil or selectedBuildings[2] == nil do
		--table.insert(selectedBuildings, {"shanghai.png", 150})
		for i, v in pairs(buildings) do
			if (math.random() < 0.005 + (1/(stdDev* math.sqrt(math.pi * 2))) * math.exp(-math.pow(i - ((tempLevel/(levelRange/2)) * (table.getn(buildings)/2)), 2)/(2 * math.pow(stdDev, 2)))) then
				table.insert(selectedBuildings, v)
				
			end
		end
	end
	while selectedMisc[1] == nil or selectedMisc[2] == nil do
		for i, v in pairs(misc) do
			if (math.random() < 0.40 + (1/(stdDev* math.sqrt(math.pi * 2))) * math.exp(-math.pow(i - ((tempLevel/(levelRange/2)) * (table.getn(misc)/2)), 2)/(2 * math.pow(stdDev, 2)))) then
				table.insert(selectedMisc, v)
			end
		end
	end
	
	
	--(1/(stdDev* math.sqrt(math.pi * 2))) * math.exp(-math.pow(i - ((tempLevel/3) * (table.getn(buldings)/2)), 2)/(2 * math.pow(stdDev, 2)))
	planetRadius = math.random(160, 220 + 6 * level)
	cityCount = math.floor(math.random(1 + .3 * level, 4 + .5 * level))
	
	
	
	planet = animation.create(planet, 1, 1)
	local planetIndex = math.floor(math.random(1, 3.999))
	if (planetIndex == 1) then
		planet = animation.addAnimation(planet, "planet.png")
	elseif (planetIndex == 2) then
		planet = animation.addAnimation(planet, "planet2.png")
	else
		planet = animation.addAnimation(planet, "planet3.png")
	end
	planet = physics.create(planet, center.x, center.y, (planetRadius / 500) * 1.07, 0, (planet.sprites[1][1]:getWidth() / 2), (planet.sprites[1][1]:getHeight() / 2))
	planet:setActive(false)
	planet:setFrozen(true)
	planet.color = {r = math.random(50, 255), g = math.random(50, 255), b = math.random(50, 255)}
	
	spawnRates.bird1 = math.max(0, config.bird1.initspawn+level*config.bird1.spawninc)
	spawnRates.bird2 = math.max(0, config.bird2.initspawn+level*config.bird2.spawninc)
	spawnRates.mole = math.max(0, config.mole.initspawn+level*config.mole.spawninc)
	spawnRates.bear = math.max(0, config.bear.initspawn+level*config.bear.spawninc)
	spawnRates.total = math.max(0, spawnRates.bird1+spawnRates.bird2+spawnRates.mole+spawnRates.bear)

	if (level == 1) then
		bosspawn = config.boss1.bosspawn
	else
		bosspawn = bosspawn + config.boss1.spawninc
	end

	if (((math.random() < math.max(0, bosspawn)) or bossCheat) and level > 1) then
		bossCheat = false
		bossLevel = true
		bossNumber = bossNumber + 1
		
		bosspawn = config.boss1.bosspawn
		table.insert(enemies, generateBoss(bossNumber))
		newPowerup(powerHealth)
	else
		bossCheat = false
		bossLevel = false
		for i = 0, 25 + 3.8 * level do
			table.insert(enemies, newEnemy(spawnRates))
		end
	end

	--plants and nature
	for i = 0, 180 / math.sqrt(level) - 10 * math.sqrt(level) do
		newScenery(pickObject(0))
	end
	for i = 0, cityCount do

		generateCity()
	end
	
	newPowerup(powerHealth)
	newPowerup(powerHealth)
	newPowerup(powerHealth)
	newPowerup(powerFreeze)
	newPowerup(powerShield)
	
end

function generateCity()
	local dir = math.random() * math.pi * 2
	local cityWidth = math.random(70 + 5 * level, 150 + 5 * level)
	cityPopulation = math.floor(math.random(7 + level, 20 + level))
	pos = {}
	pos.x = math.cos(dir) * planetRadius + center.x
	pos.y = math.sin(dir) * planetRadius + center.y
	for i = 0, cityPopulation do
		newScenery(pickObject(math.floor(math.random(1, 3))), pos.x + math.random(0, cityWidth), pos.y + math.random(0, cityWidth))
	end
end

function pickObject(cat)
	if (cat == nil) then
		cat = math.random(0, 2.999)
	end
	cat = math.floor(cat)
	
	if (cat == 0) then
		return plants[math.floor(math.random(1, table.getn(plants)))]
	elseif (cat == 1) then
		return selectedBuildings[math.floor(math.random(1, table.getn(selectedBuildings)))]
	else
		return selectedMisc[math.floor(math.random(1, table.getn(selectedMisc)))]
	end
end

function newScenery(sceneType, posX, posY, sca)
	
	local e = {}
	e = animation.create(e, 1, 1, sceneType)
	if (posX == nil) then
		posX = math.random(0, width)
		posY = math.random(0, height)
	end
	
	if (sca == nil) then
		sca = .2 + math.random() / 10
	end

	e = collider.create(e, "scenery", sceneType[2] * sca)
	--e = animation.addAnimation(e, sceneType[1])
	
	e = physics.create(e, posX, posY, sca, 0, sceneType[1][1]:getWidth() / 2, sceneType[1][1]:getHeight() / 2)
	e.health = math.sqrt(sceneType[2])
	e.color = {r = math.random(190, 255), g = math.random(190, 255), b = math.random(190, 255)}
 	
	e.centerDist = math.sqrt(math.pow(center.x - e.x, 2) + math.pow(center.y - e.y, 2))
	e.x = (((e.x) - center.x) / e.centerDist) * (planetRadius + e.radius) + center.x
	e.y = (((e.y) - center.y) / e.centerDist) * (planetRadius + e.radius) + center.y
	
	e.rotation = math.atan((e.y - center.y) / (e.x - center.x))
	if (e.x - center.x > 0) then
		e.rotation = e.rotation + math.rad(90)
	else
		e.rotation = e.rotation - math.rad(90)
	end
	
	table.insert(scenery, e)
	return e
end

function newEnemy(spawnRates)
	local e = {}
	local temp = math.random()
	local sc = .1
	e.health = 1
	e.hittable = config.hittableDelay
	if (temp - spawnRates.mole/spawnRates.total < 0 and e.aggressive == nil) then
		e = animation.create(e, 8, .15, mole)
		--e = animation.addAnimation(e, "mole1.png", "mole2.png", "mole3.png", "mole1.png", "mole2.png", "mole3.png", "mole1.png", "mole2.png")
		--e = animation.addAnimation(e, "moles/moleground1.png", "moles/moleground2.png", "moles/moleground3.png", "moles/moleground4.png", "moles/moleground5.png", "moles/moleground7.png", "moles/moleground8.png", "moles/moleground9.png")
		e.aggressive = false
		e.enemyType = "mole"
	end
	temp = temp - spawnRates.mole/spawnRates.total;
	if (temp - spawnRates.bear / spawnRates.total < 0 and e.aggressive == nil) then
		e = animation.create(e, 4, .05, bear)
		--e = animation.addAnimation(e, "bear/bear1.png", "bear/bear2.png", "bear/bear3.png", "bear/bear4.png")
		--e = animation.addAnimation(e, "bear/attackbear1.png", "bear/attackbear2.png", "bear/attackbear3.png", "bear/attackbear4.png")
		e.aggressive = false
		e.enemyType = "bear"
		e.hoverSinOffset = math.random() * 2 * 3.141
		sc = .2
	end 

	temp = temp - spawnRates.bear/spawnRates.total;
	if (temp - spawnRates.bird1/spawnRates.total < 0 and e.aggressive == nil) then
		e = animation.create(e, 2, .2, armybird)
		--e = animation.addAnimation(e, "armybird1.png", "armybird2.png")
		--e = animation.addAnimation(e, "armybirdfly1.png", "armybirdfly2.png")
		e.aggressive = true
		e.enemyType = "bird"
	end
	if (e.aggressive == nil) then
		e = animation.create(e, 2, .2, bird)
		--e = animation.addAnimation(e, "bird1.png", "bird2.png")
		--e = animation.addAnimation(e, "birdfly1.png", "birdfly2.png")
		e.aggressive = false
		e.enemyType = "bird"
	end
	e = ai.createAI(e, hero, math.random(40, 90), e.enemyType)
	e = physics.create(e, math.random(0, width), math.random(0, height), sc, 0, e.sprites[1][1]:getWidth() / 2, e.sprites[1][1]:getHeight() / 2)
	e.color = {r = math.random(190, 255), g = math.random(190, 255), b = math.random(190, 255)}
	e.shootTimer = 0
	
	e = collider.create(e, "enemy", 75*e.scale)
	
	e.centerDist = math.sqrt(math.pow(center.x - e.x, 2) + math.pow(center.y - e.y, 2))
	e.x = (((e.x) - center.x) / e.centerDist) * (planetRadius + e.radius) + center.x
	e.y = (((e.y) - center.y) / e.centerDist) * (planetRadius + e.radius) + center.y
	
	--setmetatable(e, alien)
 	
	return e
end

function generateBoss(bossNum)
	local e = {}
	if (bossNum%2 == 1) then
		e = animation.create(e, 8, .1)
		e = animation.addAnimation(e, "moleboss/moleboss1.png", "moleboss/moleboss2.png", "moleboss/moleboss3.png", "moleboss/moleboss4.png", "moleboss/moleboss5.png", "moleboss/moleboss6.png", "moleboss/moleboss7.png", "moleboss/moleboss8.png")
		e.aggressive = false
		e.enemyType = "boss"
		e.health = config.boss1.health
		e.hittable = config.hittableDelay
		sc = .2
		e = ai.createAI(e, hero, math.random(70, 100), e.enemyType)
		e = physics.create(e, math.random(0, width), math.random(0, height), sc, math.rad(-90), e.sprites[1][1]:getWidth() / 2, e.sprites[1][1]:getHeight() / 2)
		e.color = {r = math.random(190, 255), g = math.random(190, 255), b = math.random(190, 255)}
		e.cooldown = 0
	
		e = collider.create(e, "enemy", 35)
	
		e.centerDist = math.sqrt(math.pow(center.x - e.x, 2) + math.pow(center.y - e.y, 2))
		e.x = (((e.x) - center.x) / e.centerDist) * (planetRadius + e.radius) + center.x
		e.y = (((e.y) - center.y) / e.centerDist) * (planetRadius + e.radius) + center.y
		boss = e
	else
		e = animation.create(e, 8, .1)
		e = animation.addAnimation(e, "birdboss/birdboss1.png", "birdboss/birdboss2.png", "birdboss/birdboss3.png", "birdboss/birdboss4.png", "birdboss/birdboss5.png", "birdboss/birdboss6.png", "birdboss/birdboss7.png", "birdboss/birdboss8.png")
		e.aggressive = false
		e.enemyType = "boss2"
		e.health = config.boss2.health
		e.hittable = config.hittableDelay
		sc = .2
		e = ai.createAI(e, hero, math.random(70, 100), e.enemyType)
		e = physics.create(e, math.random(0, width), math.random(0, height), sc, math.rad(-90), e.sprites[1][1]:getWidth() / 2, e.sprites[1][1]:getHeight() / 2)
		e.color = {r = math.random(190, 255), g = math.random(190, 255), b = math.random(190, 255)}
		e.cooldown = 0
	
		e = collider.create(e, "enemy", 35)
	
		e.centerDist = math.sqrt(math.pow(center.x - e.x, 2) + math.pow(center.y - e.y, 2))
		e.x = (((e.x) - center.x) / e.centerDist) * (planetRadius + e.radius) + center.x
		e.y = (((e.y) - center.y) / e.centerDist) * (planetRadius + e.radius) + center.y
		boss = e
	end
	return e
end

function newPowerup(powerupType, posX, posY, sca)
	
	local e = {}
	e = animation.create(e, 1, 1)
	
	local dir = math.random() * math.pi * 2
	local rand = math.random() * .6 + .2
	pos = {}
	posX = math.cos(dir) * planetRadius * rand + center.x
	posY = math.sin(dir) * planetRadius * rand + center.y
	
	if (sca == nil) then
		sca = .2 + math.random() / 10
	end
	e.type = powerupType[3]
	e.health = config.powerups.hits
	e = collider.create(e, "powerup", powerupType[2] * sca)
	e = animation.addAnimation(e, powerupType[1])
	
	e = physics.create(e, posX, posY, sca, 0, e.sprites[1][1]:getWidth() / 2, e.sprites[1][1]:getHeight() / 2)
	e:setActive(false)
	e:setFrozen(true)
	
	--e.color = {r = math.random(190, 255), g = math.random(190, 255), b = math.random(190, 255)}
	
	e.rotation = math.random() * math.pi * 2
	e.delay = 0
	table.insert(powerups, e)
	return e
end




return {
	generatePlanet = generatePlanet
}