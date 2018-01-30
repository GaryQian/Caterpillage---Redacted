generator = require("generator")
physics = require("physics")
collider = require("collider")
bullet = require("bullets")
wormSegment = require("wormSegment")
combo = require("combo")
text = require("text")
ai = require("ai")

scenery = {}
powerups = {}
texts = {}
score = 0
bossCheat = 0

paused = false
start = false
startTime = 2	-- animation time before level starts and after level ends

level = 0
levelText = true

keyU = false
keyD = false
keyL = false
keyR = false
keyTimer = 0

invincible = false
boosting = false

healthRatio = 0
instructNum = 0

outlineAlpha = 0

zoomed = false



animation = require("animation")
scenerySetup = require("scenerySetup")

local function load()
	titleScreenBg = nil
	BOSS = nil
	began = nil
	gameStats = nil

	collectgarbage()
	
	heroCollider = {}
	enemyCollider = {}
	heroBullets = {}
	enemyBullets = {}
	drawExplosion = {}
	scenery = {}
	planet = {}
	bullets = {} --enemies bullets
	shots = {} --heros bullets 
	blacks = {}
	texts = {}
	level = 1
	healthTime = 0
	starvationRate = config.starvationRate
	invincible = false
	keyU = false
	keyD = false
	keyL = false
	keyR = false
	keyTimer = 0
	boosting = false
	levelAlpha = 255
	instructionsAlpha = config.tutDelay
	instructNum = 0
	levelText = true
	start = false
	paused = false
	startTime = config.switchTime / 2
	outlineAlpha = 255;
	switchingTimer = config.switchTime / 2
	switchingSet = false
	clickTime = 50
	scoreTextOffset = 0
	planetGenerated = false
	endFilter = {}
	endFilter.r = 255
	endFilter.g = 255
	endFilter.b = 255
	endFilter.a = 0
	blackDelay = config.blackDelay
	blackDt = 0
	
	gameTime = 0
	gameStats = {}
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
	
	love.keyboard.setTextInput(false)
	
	if (tutorial) then
		tut1 = love.graphics.newImage("tut1.jpg")
		tut2 = love.graphics.newImage("tut2.jpg")
	end
	
	clickWarning = {}
	clickWarning = animation.create(clickWarning, 1, 1)
	clickWarning = animation.addAnimation(clickWarning, "clickWarning.png")
	clickWarning = physics.create(clickWarning, center.x, center.y, .7, 0, (clickWarning.sprites[1][1]:getWidth() / 2), (clickWarning.sprites[1][1]:getHeight() / 2))
	clickWarning:setActive(false)
	clickWarning:setFrozen(true)
	
	score = 0
	love.graphics.setColor(255, 255, 255)
	
	hero = {}
	
	hero.inputx = 0
	hero.inputy = 0
	hero.speed = config.hero.speed * 60
	hero.speedBoost = hero.speed * 1.5
	hero.speedNormal = hero.speed
	
	hero.undergroundMultiplier = config.hero.undergroundMultiplier
	
	hero = animation.create(hero, 2, .15)
	hero = animation.addAnimation(hero, "wormhead.png", "wormhead2.png")
	
	hero = physics.create(hero, center.x, center.y, .15, 0, 150, 150)
	
	hero.isUnderground = false;
	hero.hit = 10
	hero.angle = 0
	
	hero.bulletCost = config.hero.bulletCost
	
	hero = collider.create(hero, "hero", hero.scale * 70)
	
	hero.health = config.hero.health
	hero.maxHealth = config.hero.maxHealth
	
	hero.segmentCount = config.hero.segmentCount

	hero.segments = {}
	for i = 1, hero.segmentCount do
		table.insert(hero.segments, collider.create(wormSegment.create(hero.x + 10 * i, hero.y + 10 * i, i), "hero", hero.radius))
	end
	
	heroPositionCache = {}
	heroPositionCache.x = hero.x
	heroPositionCache.y = hero.y
	
	arrow = love.graphics.newImage("arrow.png")
	
	collider.compare()
	
	background = animation.create(background, 1, 1)
	background = animation.addAnimation(background, "largebg.png")
	background = physics.create(background, center.x, center.y, .5, 0, (background.sprites[1][1]:getWidth() / 2), (background.sprites[1][1]:getHeight() / 2))
	background:setActive(false)
	background:setFrozen(true)
	
	camera = require("camera")
	camera.scaleX = 1
	camera.scaleY = 1
	camera.shakeTime = 0
	camera:scale(config.camera.scale)
	camera.x = (hero.x + hero.offsetX) - (width*camera.scaleX)/2
	camera.y = (hero.y + hero.offsetY) - (height*camera.scaleY)/2
	camera.rotation = 0
	
	sounds = {}
	
	song:stop()
	local temp = math.floor(math.random(1, 3.999))
	if (temp == 1) then
		song = love.audio.newSource("Fist Full Of Adventure.mp3")
	elseif (temp == 2) then
		song = love.audio.newSource("Going To Prom.mp3")
	else
		song = love.audio.newSource("Silent Ghost.mp3")
	end
	song:setVolume(.6)
	song:setLooping(true)
	song:play()
	--table.insert(sounds, song)
	
	diggingSound = love.audio.newSource("digging.mp3")
	diggingSound:stop()
	diggingSound:setLooping(true)
	diggingSound:setVolume(0.3)
	table.insert(sounds, diggingSound)
	
	groundImpactSound = love.audio.newSource("GroundImpact2.mp3")
	groundImpactSound:setLooping(false)
	groundImpactSound:stop()
	table.insert(sounds, groundImpactSound)
	
	roar1Sound = love.audio.newSource("Roar.mp3")
	roar1Sound:stop()
	table.insert(sounds, roar1Sound)
	
	roar2Sound = love.audio.newSource("Roar2.mp3")
	roar2Sound:stop()
	table.insert(sounds, roar2Sound)
	
	roar3Sound = love.audio.newSource("Roar3.mp3")
	roar3Sound:stop()
	table.insert(sounds, roar3Sound)
	
	screamingSound = love.audio.newSource("screaming.mp3")
	screamingSound:setLooping(true)
	screamingSound:stop()
	table.insert(sounds, screamingSound)
	
	switchSound = love.audio.newSource("PlanetSwitch.mp3")
	switchSound:setLooping(false)
	switchSound:play()
	table.insert(sounds, switchSound)
	
	heartbeatSound = love.audio.newSource("heartbeat.mp3")
	heartbeatSound:setVolume(0)
	heartbeatSound:setLooping(true)
	heartbeatSound:play()
	table.insert(sounds, heartbeatSound)
	
	infoFont = love.graphics.newFont("Forque.ttf", 80)
	comboFont = love.graphics.newFont("Forque.ttf", height / 15)
	love.graphics.setFont(infoFont)
	
	pauseButton = {}
	button.create(pauseButton, "buttons/pause.png", "buttons/play.png", width - 75 * config.pauseButtonScale, 75 * config.pauseButtonScale, 0, config.pauseButtonScale)
	
	resumeButton = {}
	button.create(resumeButton, "buttons/resume.png", "buttons/resumeSelected.png", width / 2, height / 3, 0, config.pauseMenuScale)
	
	mainMenuButton = {}
	button.create(mainMenuButton, "buttons/mainMenu.png", "buttons/mainMenuSelected.png", width / 2, height / 2, 0, config.pauseMenuScale)
end

local function update(dt)
	textScale = (bgScale + 1) / 2
	if not (paused) and start then
		--check if planet is finished!
		if (table.getn(enemies) == 0) then
			level = level + 1
			starvationRate = starvationRate + config.starvationRate * config.starvationIncreasePercentage
			table.insert(gameStats.planetClear, gameTime)
			levelText = true
			levelAlpha = 255
			start = false
			startTime = config.switchTime
			switchingTimer = 0
			switchingSet = false
			heroPositionCache = {}
			heroPositionCache.x = hero.x
			heroPositionCache.y = hero.y
			hero.dx = 0
			hero.dy = 0
			hero.maxHealth = hero.maxHealth + config.hero.maxHealthIncrease
			if (hero.health < 250) then
				hero.health = 250
			end
			switchSound:play()
			screamingSound:stop()
		
		end
		gameTime = gameTime + dtime
		clickTime = clickTime - dtime
		instructionsAlpha = instructionsAlpha - dtime
		if instructionsAlpha <= 0 then
			if instructNum < 1 then
				instructionsAlpha = config.tutDelay
				instructNum = instructNum + 1
			else
				instructionsAlpha = 0
				tutorial = false
			end
		end

		if levelText then 
			levelAlpha = levelAlpha - (dtime * (255 / 3))
			if levelAlpha <= 0 then 
				levelAlpha = 0 
				levelText = false
			end
		end
		if (invincible == false) then
			if (hero.health > hero.maxHealth) then
				starvationNum = starvationRate + starvationRate * 2 * (hero.health - hero.maxHealth) / hero.maxHealth
			else
				starvationNum = starvationRate
			end
			
			if tutorial == false then
				hero.health = hero.health - starvationNum * dtime
			end
		end
		
		hero.angle = math.atan2(hero.y-center.y, hero.x - center.x)
		if hero.angle < 0 then
			hero.angle = hero.angle + 2*math.pi
		end
		
		jumpDelay = jumpDelay + dtime
		if frozen == false and tutorial == false then
			for i, v in ipairs(enemies) do
				v:updateAnimation(dt)
				v:updateAI()
				v:updatePhysics(dt)
				if (hero.isUnderground == false and v.aggressive and math.pow(v.x-hero.x, 2)+math.pow(v.y-hero.y,2) < 50000 and v.enemyType == "bird") then
					v.shootTimer = v.shootTimer + dtime
					if v.shootTimer > .7 then
						gunSound = love.audio.newSource("gunshot1.mp3")
						gunSound:setVolume(.2)
						gunSound:play()
						table.insert(shots, bullet.createBullets(v, hero.x, hero.y, "bullet.png", 200, "bird"))
						v.shootTimer = math.random(0, 0.2)
					end
				end
			end
		end
		keyTimer = keyTimer - dtime
		if (hero.health < 0) then
			screamingSound:stop()
			gameStateIndex = 4
			currentState = gameStates[gameStateIndex]
			currentState:load()
			
		end
		
		if (hero.isUnderground) then
			blackDt = blackDt + dtime
			if (blackDt >= blackDelay) then
				blackDt = 0
				table.insert(blacks, newBlack(.15));
				if (table.getn(blacks) > 800) then
					table.remove(blacks, 1)
				end
			end
			if not (keyU or keyD or keyL or keyR) and keyTimer < 0 then
				x, y = camera:mousePosition()
				
				if (math.abs(hero.x - x) > 10 or math.abs(hero.y - y) > 10) then
					mouseDist = math.sqrt(math.pow(x - (hero.x), 2) + math.pow(y - (hero.y), 2))
					hero.inputx = ((x - hero.x) / mouseDist) * hero.speed
					hero.inputy = ((y - hero.y) / mouseDist) * hero.speed
				else
					hero.inputx = 0
					hero.inputy = 0
				end 
			end
		end
		
		camera.targX = (hero.x) - (width*camera.scaleX)/2
		camera.targY = (hero.y) - (height*camera.scaleY)/2
		camera:setScale(config.camera.scale / bgScale);
		camera:updateCamera(dt)
		
		if (hero.isUnderground) then
			a, b = camera:mousePosition()
		end
		
		if (hero.inputx ~= 0 and hero.inputy ~= 0) then
			if (hero.isUnderground) then
				hero.rotation = math.atan2(hero.dy + hero.inputy, hero.dx + hero.inputx) + math.rad(90)
			else
				hero.rotation = math.atan2(hero.dy, hero.dx) + math.rad(90)
			end
		end
		if tutorial == false then
			hero:updateAnimation(dt)
			hero:updatePhysics(dt)
		end
		for i, v in ipairs(hero.segments) do
			v:updateWormSegment(dt)
		end
		
		if (score >= 1000000) then
			scoreTextOffset = 100
		end
		collider.compare()

		for _, ammo in ipairs(bullets) do 
			ammo:updateBullets()
		end
		
		for i, shot in ipairs(shots) do 
			shot.removeTimer = shot.removeTimer + dtime
			if (shot.removeTimer > 3) then
				table.remove(shots, i)
				shot.removeTimer = 0
			end
			shot:updateBullets()
		end
		
		for i, shot in ipairs(bullets) do 
			shot.removeTimer = shot.removeTimer + dtime
			if (shot.removeTimer > 3) then
				table.remove(shots, i)
				shot.removeTimer = 0
			end
			shot:updateBullets()
		end
		
		for i, v in ipairs(drawExplosion) do
			v:updateAnimation()
			if (v.timer <= 0) then
				table.remove(drawExplosion, i)
			end
		end

		--collider:compare()
		
		if (hero.health < 500) then
			heartbeatSound:play()
			song:setVolume(.6 * ((hero.health) / 500))
			diggingSound:setVolume((hero.health) / 500)
			heartbeatSound:setVolume(3 * (500 - hero.health) / 500)
		else
			heartbeatSound:setVolume(0)
			song:setVolume(.6)
			diggingSound:setVolume(1)
		end
		
		for i, v in pairs(scenery) do
			v:updatePhysics()
		end
		if (endFilter.a > 0) then
			endFilter.a = endFilter.a - 1
		end
		
		--TEXT OVERLAY
		for i, t in ipairs(texts) do
			t:updateText()
			if (t.time < 0) then
				table.remove(texts, i)
			end
		end
		--END TEXT OVERLAY
		
		--collect garbage
		if (time % 2 < .2) then
			collectgarbage()
		end
	
	elseif (paused) then
		--pause stuff goes here
	elseif not start then
		--switch stuff
		switchingTimer = switchingTimer + dtime
		if (switchingTimer < config.switchTime / 2) then
			hero.dx = 200
			camera:setScale(quadratic(switchingTimer, config.camera.scale, config.camera.zoomOutScale, config.switchTime / 2)  / bgScale)
			hero.x = quadratic(switchingTimer, heroPositionCache.x, heroPositionCache.x + planetRadius + (center.x - heroPositionCache.x) + config.sideDist, config.switchTime / 2)
		else
			if not (switchingSet) then
				switchingSet = true
				generator.generatePlanet()
				hero.x = center.x - config.sideDist
				hero.y = center.y
				hero.dx = 200
				hero.dy = 0
			end
			--inside planet
			if (math.abs(center.x - hero.x) < planetRadius) then
				hero:updatePhysics()
				hero.dx = hero.dx - hero.dx * dtime * 50
				table.insert(blacks, newBlack(.15));
				if (table.getn(blacks) > 800) then
					table.remove(blacks, 1)
				end
			end
			camera:setScale(quadratic(switchingTimer - config.switchTime / 2, config.camera.zoomOutScale, config.camera.scale - config.camera.zoomOutScale, config.switchTime / 2)  / bgScale)
			hero.x = center.x - config.sideDist - planetRadius * 5 + (((switchingTimer - (config.switchTime / 2)) / (config.switchTime / 2)) * (center.x + config.sideDist))
			for i, v in ipairs(hero.segments) do
				v.y = center.y
				v.x = hero.x - i * hero.radius * 2
			end
		end
		
		camera.targX = (hero.x) - (width*camera.scaleX)/2
		camera.targY = (hero.y) - (height*camera.scaleY)/2
		camera.x = camera.targX
		camera.y = camera.targY
		camera:updateCamera(dt)
		hero:updateAnimation(dt)
		hero.rotation = math.atan2(hero.dy, hero.dx) + math.rad(90)
		for i, v in ipairs(hero.segments) do
			v:updateWormSegment(dt)
		end
		
		for i, v in pairs(scenery) do
			v:updatePhysics()
		end
	end
end

local function draw()
	if (planetGenerated) then
		love.graphics.setColor(255, 255, 255)
		if not (paused) then
			camera:set()
		end
		
		if not start then
			if (switchingTimer < config.switchTime / 2) then
				love.graphics.setColor(255, 255, 255, ((config.switchTime / 2) - switchingTimer) / (config.switchTime / 2) * 255)
			else
				love.graphics.setColor(255, 255, 255, (switchingTimer) / (config.switchTime / 2) * 255)
			end
		end
		background:drawAnimation()
		--love.graphics.circle("fill", center.x, center.y, planetRadius)
		love.graphics.setColor(planet.color.r, planet.color.g, planet.color.b)
		planet:drawAnimation()
		
		love.graphics.setColor(255, 255, 255)
		for _, bl in ipairs(blacks) do
			bl:drawAnimation()
		end
		love.graphics.setColor(255, 255, 255, 20)
		love.graphics.circle("fill", center.x, center.y, planetRadius)
		love.graphics.setColor(255, 255, 255)
		for _, scene in ipairs(scenery) do 
			scene:drawAnimation()
		end
		for _, pup in ipairs(powerups) do 
			pup:drawAnimation()
			love.graphics.setColor(planet.color.r, planet.color.g, planet.color.b, 255 * (pup.health + 10) / (config.powerups.hits + 10))
			if (pup.delay > config.powerups.hitDelay - .2) then
				love.graphics.setColor(255, 0, 0)
			end
			love.graphics.draw(
				powerupCover[config.powerups.hits - pup.health + 1], -- what to draw
				pup.x, pup.y, -- where to draw it
				pup.rotation, -- rotation
				.3, .3, -- scale
				75, 75 -- origin
			)
			love.graphics.setColor(255, 255, 255)
		end
		--love.graphics.draw(hero.Img, hero.x, hero.y, 0, 0.5, 0.5)
		for j,k in ipairs(enemyCollider) do
			outlineAlpha = math.sin(os.clock()*20)*130
			if (outlineAlpha < 0) then
				outlineAlpha = -(outlineAlpha)
			end
			if ((k.enemyType == "mole" and k.isEnemyUnderground)) then
				love.graphics.setColor(255, 0, 0, outlineAlpha)
			elseif ((k.enemyType == "bird" and k.aggressive)) then
				love.graphics.setColor(255, 255, 0, outlineAlpha)
			elseif (k.enemyType == "bear") then
				love.graphics.setColor(100, 100, 0, outlineAlpha)
			elseif (k.enemyType == "turret") then
				love.graphics.setColor(100, 100, 0, outlineAlpha)
			else
				love.graphics.setColor(0, 255, 0, outlineAlpha)
			end
			love.graphics.circle("fill", k.x, k.y, k.radius*1.4, 100)
		end

		for _, e in ipairs(enemies) do
			love.graphics.setColor(e.color.r, e.color.g, e.color.b)
			e:drawAnimation()
			--love.graphics.circle("fill", e.x + e.offsetX, e.y + e.offsetY, 70)
		end
		
		if (table.getn(enemies) <= 5) then
			love.graphics.setColor(255, 150, 150, 170)
			for _, e in ipairs(enemies) do
				arrowPos = {}
				arrowDist = math.sqrt(math.pow(hero.x - e.x, 2) + math.pow(hero.y - e.y, 2))
				arrowPos.x = (e.x - hero.x) / arrowDist * 30
				arrowPos.y = (e.y - hero.y) / arrowDist * 30
				arrowPos.rotation = math.atan(arrowPos.y / arrowPos.x)
				if (arrowPos.x < 0) then
					arrowPos.rotation = arrowPos.rotation - math.rad(90)
				else
					arrowPos.rotation = arrowPos.rotation + math.rad(90)
				end
				love.graphics.draw(
					arrow, -- what to draw
					hero.x + arrowPos.x, hero.y + arrowPos.y, -- where to draw it
					arrowPos.rotation, -- rotation
					.2, .2, -- scale
					25, 25-- origin
				)
			end
		end
		
		if (bossLevel) then
			love.graphics.setColor(255, 150, 150, 170)
			arrowPos = {}
			arrowDist = math.sqrt(math.pow(hero.x - boss.x, 2) + math.pow(hero.y - boss.y, 2))
			arrowPos.x = (boss.x - hero.x) / arrowDist * 30
			arrowPos.y = (boss.y - hero.y) / arrowDist * 30
			arrowPos.rotation = math.atan(arrowPos.y / arrowPos.x)
			if (arrowPos.x < 0) then
				arrowPos.rotation = arrowPos.rotation - math.rad(90)
			else
				arrowPos.rotation = arrowPos.rotation + math.rad(90)
			end
			love.graphics.draw(
				arrow, -- what to draw
				hero.x + arrowPos.x, hero.y + arrowPos.y, -- where to draw it
				arrowPos.rotation, -- rotation
				.2 * 2, .2 * 2, -- scale
				25 * 2, 25 * 2-- origin
			)
		end
		
		love.graphics.setColor(255, 255, 255)
		for _, ammo in ipairs(bullets) do
			ammo:drawBullets()
		end
		for _, shots in ipairs(shots) do 
			shots:drawBullets()
		end
		combo:update()
		
		if invincible then 
			love.graphics.setColor(255 * math.abs(math.sin(math.random(0, 10))), 255 * math.abs(math.sin(math.random(0, 10))), 255 * math.abs(math.sin(math.random(0, 10))))
		else
			if (hero.hit < 4) then
				hero.hit = hero.hit + 1
				love.graphics.setColor(255, 80, 80)
			elseif (boosting) then
				love.graphics.setColor(255, 255, 255, 190)
			end
		end 
		hero:drawAnimation()
		for i, v in ipairs(hero.segments) do
			v:drawAnimation()
		end
		

		love.graphics.setColor(255, 255, 255)
		
		for i, v in ipairs(drawExplosion) do
			v:drawAnimation()
		end
		

		if not (paused) then
			camera:unset()
		else
			love.graphics.setColor(255, 255, 255, 255)
			resumeButton:drawButton()
			mainMenuButton:drawButton()
		end
		
		--HEALTH OVERLAY
		if (hero.health < 300) then
			love.graphics.setColor(255, 0, 0, ((300 - hero.health) / 300) * 180)
			love.graphics.rectangle("fill", -width, -height, width * 3, height * 3)
			love.graphics.setColor(255, 255, 255)
		end
		--END HEALTH OVERLAY
		
		
		--UI ELEMENTS
		combo:draw()
		love.graphics.print("Score: " .. math.floor(score), 20, 10, 0)
		
		healthRatio = math.floor(hero.health) / hero.maxHealth
		healthTime = healthTime + dtime
		love.graphics.print("Health: ", 20, 90, 0)
		if invincible then
			love.graphics.setColor(255, 255, 255)
			love.graphics.rectangle("line", 220, 115, 200, 50)
			love.graphics.setColor(255 * math.abs(math.sin(math.random(0, 10))), 255 * math.abs(math.sin(math.random(0, 10))), 255 * math.abs(math.sin(math.random(0, 10))))
			love.graphics.rectangle("fill", 220, 115, 200, 50)
		else
			if healthRatio < .5 then
			love.graphics.rectangle("line", 220, 115, 200 + 5*math.sin(healthTime * 10), 50 + 5*math.sin(healthTime * 10))
			else 	
				love.graphics.rectangle("line", 220, 115, 200, 50)
			end
			if healthRatio > 1 then
				healthRatio = 1
			end
			love.graphics.setColor(255 * (1 - healthRatio), 255 * healthRatio, 0)
			if (hero.hit < 4) then
				love.graphics.setColor(255, 0, 0)
			end
			if healthRatio < .5 then
				love.graphics.rectangle("fill", 220, 115, (200 * healthRatio) + 5*math.sin(healthTime * 10), 50 + 5*math.sin(healthTime * 10))
			else
				love.graphics.rectangle("fill", 220, 115, 200 * healthRatio, 50)
			end
			love.graphics.setColor(255, 255, 255)
			if (hero.hit < 4) then
				love.graphics.setColor(255, 150, 150)
			end
			love.graphics.print(math.floor(hero.health), 240, 115, 0, .5, .5)
		end
		love.graphics.setColor(255, 255, 255)
		
		--TEXT OVERLAY
		for i, t in ipairs(texts) do
			t:drawText()
		end
		--END TEXT OVERLAY
		
		--CLICK WARNING
		if (clickTime < 0) then
			love.graphics.setColor(255, 255, 255, 150)
			clickWarning:drawAnimation()
			love.graphics.setColor(255, 255, 255)
		end
		
		
		
		
		if not (start) then
			countdown()
		elseif (not paused) then
			if tutorial then
				if instructNum == 0 then
					love.graphics.setColor(255, 255, 255)
					love.graphics.draw(tut1, 0, 0, 0, bgScale * 2 / 3, bgScale * 2 / 3)
				end
				if instructNum == 1 then
					love.graphics.setColor(255, 255, 255)
					love.graphics.draw(tut2, 0, 0, 0, bgScale * 2 / 3, bgScale * 2 / 3)
				end
				love.graphics.print("Tap to skip", 80, 15, 0, .8, .8, 0, 0)
				love.graphics.print(math.floor(instructionsAlpha), 20, 15, 0, .8, .8, 0, 0)
			else
				if levelText == true and bossLevel == false then
					love.graphics.setColor(255, 255, 255, levelAlpha)
					--was 50
					love.graphics.print("Planet " .. level, width / 2, 50 , 0, 2, 2, width / 15)
					love.graphics.print("Eat All Life!", width / 2, height / 2, 0, 2, 2, width / 10)
				elseif levelText and bossLevel then
					love.graphics.setColor(255, 255, 255, levelAlpha)
					love.graphics.print("Level " .. level, width / 2, 50 , 0, 2, 2, width / 15)
					love.graphics.setColor(255, 0, 0, levelAlpha)
					love.graphics.print("Kill The BOSS!", width / 2, height / 2, 0, 2, 2, width / 10)
				end
			end
		end
		
		love.graphics.setColor(endFilter.r, endFilter.g, endFilter.b, endFilter.a)
		love.graphics.rectangle("fill", -width, -height, width * 3, height * 3)
		love.graphics.setColor(255, 255, 255)
		
		
		pauseButton:drawButton()
	end
end


function newBlack(scale)
	local b = {}
	b = animation.create(b, 1, 200, black)
	--b = animation.addAnimation(b, "black.png")
	b = physics.create(b, hero.x, hero.y, scale, 0, b.sprites[1][1]:getWidth() / 2, b.sprites[1][1]:getHeight() / 2)
	b:setActive(false)
	b:setFrozen(true)
	return b
end

function newBlackMole(self, scale)
	local b = {}
	b = animation.create(b, 1, 200, black)
	--b = animation.addAnimation(b, "black.png")
	b = physics.create(b, self.x, self.y, scale, 0, b.sprites[1][1]:getWidth() / 2, b.sprites[1][1]:getHeight() / 2)
	b:setActive(false)
	b:setFrozen(true)
	return b
end

love.keyboard.setKeyRepeat(true)

local function textinput(t)
end

function keypressed(key)

	if (hero.isUnderground) then
		if key == 'left' or key == 'a' then
			keyL = true
			keyTimer = 1
			hero.inputx = -hero.speed
		end
		if key == 'right' or key == 'd' then
			keyR = true
			keyTimer = 1
			hero.inputx = hero.speed
		end
		if key == 'down' or key == 's' then
			keyD = true
			keyTimer = 1
			hero.inputy = hero.speed
		end
		if key == 'up' or key == 'w' then
			keyU = true
			keyTimer = 1
			hero.inputy = -hero.speed
		end
	end
	
	if key == 'i' then
		invincible = true
	end

	if key == 'q' then
		if paused then 
			currentState = gameStates[2]
			currentState:load()
		end
	end
	
	if key == 'k' then
		table.remove(enemies, 1)
		table.remove(enemies, 1)
		table.remove(enemies, 1)
		table.remove(enemyCollider, 1)
		table.remove(enemyCollider, 1)
		table.remove(enemyCollider, 1)
	end
	
	if key == 'e' then
		hero.health = 2
	end
	
	if key == 'o' then
		starvationRate = starvationRate * 10
	end
	if key == 'r' then
		planet = animation.create(planet, 1, 1)
		local planetIndex = math.floor(math.random(1, 3.999))
		if (planetIndex == 1) then
			planet = animation.addAnimation(planet, "planet.png")
		elseif (planetIndex == 2) then
			planet = animation.addAnimation(planet, "planet2.png")
		else
			planet = animation.addAnimation(planet, "planet3.png")
		end
		planet.color = {r = math.random(50, 255), g = math.random(50, 255), b = math.random(50, 255)}
	end
	
	if start then
		if instructNum >= 1 then
			instructionsAlpha = 0
			tutorial = false
		end
		instructionsAlpha = config.tutDelay
		instructNum = instructNum + 1
	end
end

function keyreleased(key)
	if key == 'left' or key == 'a' then
		keyL = false
		keyTimer = 1
		hero.inputx = 0
	end
	if key == 'right' or key == 'd' then
		keyR = false
		keyTimer = 1
		hero.inputx = 0
	end
	if key == 'down' or key == 's' then
		keyD = false
		keyTimer = 1
		hero.inputy = 0
	end
	if key == 'up' or key == 'w' then
		keyU = false
		keyTimer = 1
		hero.inputy = 0
	end
	
	if key == 'i' then
		invincible = false
	end
	
	if key == 'n' then
		enemies = {}
		enemyCollider = {}
	end
	
	if key == 'h' then
		hero.health = hero.health + 200
	end

	if key == 'b' then
		enemies = {}
		enemyCollider = {}
		bossCheat = 1
	end

	if key == 'escape' or 'p' or 'home' or 'search' then
		if not (paused) then
			if diggingSound:isPlaying() then
				diggingSound:pause()
			end
		else
			if diggingSound:isPaused() and hero.isUnderground then
				diggingSound:play()
			end
		end
		paused = not paused
	end
	
	if key == 'o' then
		starvationRate = starvationRate / 10
	end
end

local function mousepressed(x, y, button)
	--a, b = love.mouse.getPosition()
	a, b = camera:mousePosition()
	if (not tutorial and start and pauseButton:checkButton(x, y)[1]) then
		if (paused) then
			paused = false
			pauseButton:unselect()
		else
			paused = true
		end
		return
	end
	if (paused == false) then
		if hero.isUnderground == false then
			spitSound = love.audio.newSource("spit.mp3")
			spitSound:setVolume(1.5)
			spitSound:play()
			table.insert(bullets, bullet.createBullets(hero, a, b, "bullett.png", 300, "hero"))
			table.insert(gameStats.shots, gameTime)
			--hero.health = hero.health - hero.bulletCost
			clickTime = 45
			
			if (table.getn(bullets) > 10) then
				table.remove(bullets, 1)
				collectgarbage()
			end
		end
	else
		if (resumeButton:checkButton(x, y)[1]) then
			paused = false
			pauseButton:unselect()
			return
		else
			pauseButton:select()
		end
		if (mainMenuButton:checkButton(x, y)[1]) then
			paused = false
			pauseButton:unselect()
			hero.health = 0
			return
		end
	end
	if start then
		if instructNum >= 1 then
			instructionsAlpha = 0
			tutorial = false
		end
		instructionsAlpha = config.tutDelay
		instructNum = instructNum + 1
	end
end

function setDirection()
	for i, v in ipairs(enemies) do
		v:setDirection()
	end
end

function countdown()
	startTime = startTime - dtime
	love.graphics.setColor(255, 255, 255, 255)
	if (startTime > 0) then
		love.graphics.print(math.ceil(startTime), width / 2, height / 3 - 40, 0)
	else
		start = true
	end
end

local function touchpressed(id, x, y, pressure)
end

local function touchmoved(id, x, y, pressure)
end

local function touchreleased(id, x, y, pressure)
end

return {
	load = load,
	update = update,
	draw = draw,
	keypressed = keypressed,
	keyreleased = keyreleased,
	mousepressed = mousepressed,
	textinput = textinput,
	touchpressed = touchpressed,
	touchreleased = touchreleased,
	touchmoved = touchmoved
}
