
animation = require("animation")
physics = require("physics")
combo = require("combo")

heroCollider = {}
enemyCollider = {}
heroBullets = {}
enemyBullets = {}
drawExplosion = {}
sceneryCollider = {}
powerupCollider = {}

dist = 0
dirx = 0
diry = 0
heroSpeed = 1
ndx = 0
ndy = 0
angleRatio = 0
tempNum = 0
frozen = false

local function create(obj, class, radius)
	obj.class = class
	obj.radius = radius
	obj.collided = .5
	if class == "hero" then
		table.insert(heroCollider, obj)	
	elseif class == "enemy" then
		table.insert(enemyCollider, obj)
	elseif class == "herobullets" then
		table.insert(heroBullets, obj)
	elseif class == "enemyBullets" or class == "turretBullets" then
		table.insert(enemyBullets, obj)
	elseif class == "scenery" then
		table.insert(sceneryCollider, obj)
	else
		table.insert(powerupCollider, obj)
	end
	
	function obj:updateCollider(dt)
		--self.collided = self.collided - dtime
	end
	return obj
end

local function compare()
	for i, v in pairs(heroCollider) do
		for j, k in pairs(enemyCollider) do 
			if collided(v,k) then
				if (k.enemyType ~= "mole" or k.isEnemyUnderground ~= true) then
					
					
					if (k.hittable < 0) then
						table.insert(drawExplosion, createBloodsplat(k.x, k.y, 5))
						
						hero.health = hero.health + config.healAmount
						table.insert(gameStats.healthGained, config.healAmount)
						table.insert(gameStats.healed, gameTime)
						
						t = {}
						t = text.create(t, "+" .. config.healAmount, .5, 470 + math.random(-10, 10), 120 + math.random(-10, 10), .7, .3, -1, 0, 255, 0)
						table.insert(texts, t)
						
						k.health = k.health - 1
						k.hittable = config.hittableDelay
						if (k.health <= 0) then
							score = score + 10 * ((comboCount + 10) / 10)
							t = {}
							t = text.create(t, "+" .. math.floor(10 * ((comboCount + 10) / 10)), .5, 440 + math.random(-10, 10) + scoreTextOffset, 30 + math.random(-10, 10), .7, .3, -1)
							table.insert(texts, t)
							if (k.enemyType == "boss" or k.enemyType == "boss2") then
								enemies = {}
								hero.health = hero.health + 300
								t = {}
								t = text.create(t, "+300", .5, 470 + math.random(-10, 10), 120 + math.random(-10, 10), .7, .3, -1, 0, 255, 0)
								table.insert(texts, t)
								score = score + 1000
								t = {}
								t = text.create(t, "+" .. 1000, .5, 440 + math.random(-10, 10) + scoreTextOffset, 30 + math.random(-10, 10), .7, .3, -1)
								table.insert(texts, t)
								enemyCollider = {}
								bossLevel = false
							else
								if (k.enemyType == "bird") then
									table.insert(gameStats.birdEat, gameTime)
								elseif (k.enemyType == "mole") then
									table.insert(gameStats.moleEat, gameTime)
								elseif (k.enemyType == "turret") then
									table.insert(gameStats.turretEat, gameTime)
								elseif (k.enemyType == "bear") then
									table.insert(gameStats.bearEat, gameTime)
								end
								table.remove(enemyCollider, j)
								table.remove(enemies, j)
								combo:increment()
							end
						end
					end
					
				else
					if (invincible == false) then
						hero.health = hero.health - config.mole.damage * (.8 + .2 * level)
						table.insert(gameStats.damaged, gameTime)
						table.insert(gameStats.damageTaken, config.mole.damage * (.8 + .2 * level))
						t = {}
						t = text.create(t, "-" .. math.floor(config.mole.damage * (.8 + .2 * level)), .5, 470 + math.random(-10, 10), 120 + math.random(-10, 10), .7, .3, -1, 255, 0, 0)
						table.insert(texts, t)
						camera:shake(.1, 5)
						endFilter.a = 20
						endFilter.r = 255
						endFilter.g = 0
						endFilter.b = 0
					end
					table.remove(enemyCollider, j)
					table.remove(enemies, j)
					hero.hit = 0
					table.insert(drawExplosion, createBloodsplat(k.x, k.y, 5))
				end
			end
		end
	end
	
	for i, v in pairs(heroBullets) do
		for j, k in pairs(enemyCollider) do
			if collided(v,k) then
				
				

				table.insert(drawExplosion, createExplosion(k.x, k.y, 5))
				
				k.health = k.health - 1
				k.hittable = config.hittableDelay
				if (k.health <= 0) then
					if (k.enemyType == "boss" or k.enemyType == "boss2") then
						bossLevel = false
						enemies = {}
						hero.health = hero.health + 300
						table.insert(gameStats.healthGained, 300)
						table.insert(gameStats.healed, gameTime)
						table.insert(gameStats.bossKills, gameTime)
						t = {}
						t = text.create(t, "+300", .5, 470 + math.random(-10, 10), 120 + math.random(-10, 10), .7, .3, -1, 0, 255, 0)
						table.insert(texts, t)
						score = score + 1000
						t = {}
						t = text.create(t, "+" .. 1000, .5, 440 + math.random(-10, 10) + scoreTextOffset, 30 + math.random(-10, 10), .7, .3, -1)
						table.insert(texts, t)
						enemyCollider = {}
					else
						if (k.enemyType == "bird") then
							table.insert(gameStats.birdShoot, gameTime)
						elseif (k.enemyType == "mole") then
							table.insert(gameStats.moleShoot, gameTime)
						elseif (k.enemyType == "turret") then
							table.insert(gameStats.turretShoot, gameTime)
						elseif (k.enemyType == "bear") then
							table.insert(gameStats.bearShoot, gameTime)
						end
						score = score + 15 * ((comboCount + 10) / 10)
						t = {}
						t = text.create(t, "+" .. math.floor(15 * ((comboCount + 10) / 10)), .5, 440 + math.random(-10, 10) + scoreTextOffset, 30 + math.random(-10, 10), .7, .3, -1)
						table.insert(texts, t)
						table.remove(enemyCollider, j)
						table.remove(enemies, j)
						combo:increment()
					end
				end
				
				local explosionSound = love.audio.newSource("Explosion.mp3")
				explosionSound:play()
				
				table.remove(heroBullets, i)
				table.remove(bullets, i)

				
			end
		end
	end
	
	for i, v in pairs(enemyBullets) do
		for j, k in pairs(heroCollider) do
			if collided(v,k) then
				if (invincible == false) then
					hero.health = hero.health - config.bird1.bulletDamageBase * (.9 + .1 * level)
					table.insert(gameStats.damaged, gameTime)
					table.insert(gameStats.damageTaken, config.bird1.bulletDamageBase * (.9 + .1 * level))
					t = {}
					t = text.create(t, "-" .. (config.bird1.bulletDamageBase * (.9 + .1 * level)), .5, 470 + math.random(-10, 10), 120 + math.random(-10, 10), .7, .3, -1, 255, 0, 0)
					table.insert(texts, t)
				end
				
				hero.hit = 0
				--table.insert(drawExplosion, createExplosion(v.x, v.y, 5))
				
				--local explosionSound = love.audio.newSource("Explosion.mp3")
				--explosionSound:play()
				
				table.remove(enemyBullets, i)
				table.remove(shots, i)

			end
		end
	end
	
	for i, v in pairs(scenery) do
		if collided(v,hero) then
			dist = v.radius + hero.radius
			dirx = (v.x - hero.x) / dist
			diry = (v.y - hero.y) / dist
			if (hero.isUnderground) then
				heroSpeed = math.sqrt(math.pow(hero.dx + hero.inputx, 2) + math.pow(hero.dy + hero.inputy, 2))
			else
				heroSpeed = math.sqrt(math.pow(hero.dx, 2) + math.pow(hero.dy, 2))
			end
			
			v.dx = dirx * heroSpeed * 1.1
			v.dy = diry * heroSpeed * 1.1
			
			ndx = hero.dx / hero.speed
			ndy = hero.dy / hero.speed
			
			angleRatio = math.atan((dirx - ndx) / (diry - ndy)) / (math.pi / 2)
			
			v.dr = -3 * angleRatio
			
			tempNum = math.sqrt(lastCollidedDistSq)
			v.x = (((v.x) - hero.x) / tempNum) * (dist) + hero.x
			v.y = (((v.y) - hero.y) / tempNum) * (dist) + hero.y
			
			v.health = v.health - heroSpeed / (50)
			
			if (v.health < 0) then
				table.remove(scenery, i)
				hero.health = hero.health + 5
				table.insert(gameStats.healthGained, 5)
				table.insert(gameStats.healed, gameTime)
				table.insert(gameStats.sceneryEaten, gameTime)
				t = {}
				t = text.create(t, "+5", .5, 470 + math.random(-10, 10), 120 + math.random(-10, 10), .7, .3, -1, 0, 255, 0)
				table.insert(texts, t)
			end
			
		end
	end
	
	for i, v in pairs(powerups) do
		v.delay = v.delay - dtime
		if collided(v,hero) then
			if (v.delay < 0) then
				v.delay = config.powerups.hitDelay
				v.health = v.health - 1
				camera:shake(.1, 5)
				if (v.health <= 0) then
					table.remove(powerups, i)
					if (v.type == "health") then
						table.insert(gameStats.healthGained, config.powerups.health.health)
						table.insert(gameStats.healed, gameTime)
						table.insert(gameStats.healthPowerups, gameTime)
						hero.health = hero.health + config.powerups.health.health
						endFilter.a = 50
						endFilter.r = 0
						endFilter.g = 255
						endFilter.b = 0
						t = {}
						t = text.create(t, "+" .. config.powerups.health.health .. "health", .9, hero.x, hero.y, 1, 0, -1)
						table.insert(texts, t)
						score = score + 50 * ((comboCount + 10) / 10)
						t = text.create(t, "+" .. math.floor(50 * ((comboCount + 10) / 10)), .5, 440 + math.random(-10, 10) + scoreTextOffset, 30 + math.random(-10, 10), .7, .3, -1)
						t = {}
						t = text.create(t, "+" .. config.powerups.health.health, .5, 470 + math.random(-10, 10), 120 + math.random(-10, 10), .7, .3, -1, 0, 255, 0)
						table.insert(texts, t)
					elseif (v.type == "freeze") then
						table.insert(gameStats.freezePowerups, gameTime)
						frozen = true
						frozenTime = config.powerups.freeze.freeze
						score = score + 50 * ((comboCount + 10) / 10)
						t = {}
						t = text.create(t, "+" .. math.floor(50 * ((comboCount + 10) / 10)), .5, 440 + math.random(-10, 10) + scoreTextOffset, 30 + math.random(-10, 10), .7, .3, -1)
						table.insert(texts, t)
						t = {}
						t = text.create(t, "Freeze!", .5, hero.x, hero.y, 1, 0, -1)
					elseif (v.type == "shield") then
						table.insert(gameStats.shieldPowerups, gameTime)
						invincible = true
						print("------------")
						rampageTime = config.powerups.rampage
						print(rampageTime)
						score = score + 50 * ((comboCount + 10) / 10)
						t = {}
						t = text.create(t, "+" .. math.floor(50 * ((comboCount + 10) / 10)), .5, 440 + math.random(-10, 10) + scoreTextOffset, 30 + math.random(-10, 10), .7, .3, -1)
						table.insert(texts, t)
						t = {}
						t = text.create(t, "INVINCIBLE", .5, hero.x, hero.y, 1, 0, -1)
					end
				end
			end
		end
	end
	if frozen then
		endFilter.a = 50
		endFilter.r = 100
		endFilter.g = 100
		endFilter.b = 255
		table.insert(texts, t)
		frozenTime = frozenTime - dtime
		if frozenTime < 0 then
			frozen = false
		end
	end
	if invincible then
		endFilter.a = 50
		endFilter.r = 100
		endFilter.g = 100
		endFilter.b = 100
		table.insert(texts, t)
		rampageTime = rampageTime - dtime
		if rampageTime < 0 then
			invincible = false
		end
	end
end

function createExplosion(x, y, timer)
	local explode = {}
	explode = animation.create(explode, 5, .1, explosion)
	--explode = animation.addAnimation(explode, "GroundExplosion1.png", "GroundExplosion2.png", "GroundExplosion3.png", "GroundExplosion4.png", "GroundExplosion5.png")
	explode = physics.create(explode, x, y, .2, math.random(0, math.pi * 2), explode.sprites[1][1]:getWidth() / 2, 2 * explode.sprites[1][1]:getWidth() / 3)
	explode.timer = timer
	explode.rotation = math.atan((explode.y - center.y) / (explode.x - center.x))
	if (explode.x - center.x > 0) then
		explode.rotation = explode.rotation + math.rad(90)
	else
		explode.rotation = explode.rotation - math.rad(90)
	end
	return explode
end

function createBloodsplat(x, y, timer)
	local splat = {}
	splat = animation.create(splat, 7, .07, blood)
	--splat = animation.addAnimation(splat, "Bloodsplat/Bloodsplat0.png", "Bloodsplat/Bloodsplat1.png", "Bloodsplat/Bloodsplat2.png", "Bloodsplat/Bloodsplat3.png", "Bloodsplat/Bloodsplat4.png", "Bloodsplat/Bloodsplat5.png", "Bloodsplat/Bloodsplat6.png")
	splat = physics.create(splat, x, y, .4, 2 * math.pi * math.random(), splat.sprites[1][1]:getWidth() / 2, splat.sprites[1][1]:getWidth() / 2)
	splat.timer = timer
	
	return splat
end

function collided(v, k)
	lastCollidedDistSq = math.pow(v.x - k.x, 2) + math.pow(v.y - k.y, 2)
	return lastCollidedDistSq < math.pow(v.radius + k.radius, 2)
				
end

return {
	create = create,
	compare = compare,
}
