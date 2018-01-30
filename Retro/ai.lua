

local function createAI(e, hero, speed, enemyType)
	e.dir = "none"
	e.dirSet = -1
	e.dxAI = 0
	e.dyAI = 0
	e.dxActual = 0
	e.dyActual = 0
	e.isEnemyUnderground = false
	e.blackDt = 0
	e.flying = false
	e.jumpTimeDelay = math.random(4, 8) + math.random()
	jumpDelay = 0
	e.speed = speed

	function e:updateAI()
			self.perpVector = {}
			self.perpVector.x = -(self.y - center.y) / self.centerDist
			self.perpVector.y = (self.x - center.x) / self.centerDist
			
			self.hittable = self.hittable - dtime
			
			self.angle = math.atan2(self.y - center.y, self.x - center.x)
            if self.y-center.y < 0 and self.x-center.x>0 then
                    self.angle = self.angle + math.pi
            elseif self.y-center.y < 0 and self.x-center.x < 0 then
                    self.angle = self.angle + math.pi
            end
			
			if (self.enemyType == "bird" or (self.enemyType == "mole" 
				and hero.isUnderground == false and self.isEnemyUnderground == false))then
				--Normal Surface Behavior
				self:moveSurface()
			elseif (self.enemyType == "mole") then

				if (self.isEnemyUnderground == false) then
					local rand = math.random()
					if (rand < config.mole.chance) then
						self:dig()
					else
						self:moveSurface()
					end
				else
					self:checkUnderground()
				end
			elseif (self.enemyType == "bear") then
				local rand = math.random()
				if (hero.isUnderground == false and rand < config.bear.chance) then
					self:newTurret()
					table.insert(gameStats.turretSpawn, gameTime)
				else
					self:moveFloat()
				end
			elseif self.enemyType == "turret" then
				self.shootTime = self.shootTime + dtime
				if (hero.isUnderground == false and self.shootTime > self.shootDelay) then
					table.insert(shots, bullet.createBullets(self, hero.x, hero.y, "bear/bee.png", 200, "turret"))
					self.shootTime = 0
				end
			elseif self.enemyType == "boss" then
				self:bossAI(1);
			elseif self.enemyType == "boss2" then
				self:bossAI(2);
			end

			self.dxActual = self.dxActual + (self.dxAI - self.dxActual) * dtime * 3
			self.dyActual = self.dyActual + (self.dyAI - self.dyActual) * dtime * 3
			self.x = self.x + self.dxActual * dtime
			self.y = self.y + self.dyActual * dtime

			jumpDelay = jumpDelay + dtime
			if jumpDelay % self.jumpTimeDelay < .1 and self.flying and self.enemyType == "bird" then
				self.flying = false
				self:setAnimation(1)
				--self.dx = self.dx - 120 * (self.x - center.x) / planetRadius
				--self.dy = self.dy - 120 * (self.y - center.y) / planetRadius
			elseif jumpDelay % self.jumpTimeDelay > 4 and self.flying == false and self.enemyType == "bird" then
				self.flying = true
				self:setAnimation(2)
				if math.abs((self.x - center.x)/planetRadius) < 1 then 
					self.dx = 100 * (self.x - center.x) / planetRadius
				end
				if math.abs((self.y - center.y)/planetRadius) < 1 then
					self.dy = 100 * (self.y - center.y) / planetRadius
				end
			end


	end

	function e:bossAI(type)
		self:moveSurface()

		if (type == 1) then
			if (self.cooldown < 0) then
				self.cooldown = 1
				if (table.getn(enemies) < 20) then
					local e = {}
					e = animation.create(e, 8, .15, mole)
					--e = animation.addAnimation(e, "mole1.png", "mole2.png", "mole3.png", "mole1.png", "mole2.png", "mole3.png", "mole1.png", "mole2.png")
					--e = animation.addAnimation(e, "moles/moleground1.png", "moles/moleground2.png", "moles/moleground3.png", "moles/moleground4.png", "moles/moleground5.png", "moles/moleground7.png", "moles/moleground8.png", "moles/moleground9.png")
					e.aggressive = false
					e.health = 1
					e.hittable = config.hittableDelay
					e.enemyType = "mole"
					e = ai.createAI(e, hero, math.random(40, 90), e.enemyType)
					e = physics.create(e, self.x, self.y, 0.1, 0, e.sprites[1][1]:getWidth() / 2, e.sprites[1][1]:getHeight() / 2)
					e.color = {r = math.random(190, 255), g = math.random(190, 255), b = math.random(190, 255)}
					e.shootTimer = 0
					e = collider.create(e, "enemy", 75*e.scale)
		
					e.centerDist = math.sqrt(math.pow(center.x - e.x, 2) + math.pow(center.y - e.y, 2))
					e.x = (((e.x) - center.x) / e.centerDist) * (planetRadius + e.radius) + center.x
					e.y = (((e.y) - center.y) / e.centerDist) * (planetRadius + e.radius) + center.y
					table.insert(enemies, e);
					e:dig()
				end
			else 
				self.cooldown = self.cooldown - dtime
			end
		else 
			if (self.cooldown < 0) then
				self.cooldown = 1
				if (table.getn(enemies) < 20) then
					local e = {}
					e = animation.create(e, 2, .2, armybird)
					--e = animation.addAnimation(e, "armybird1.png", "armybird2.png")
					--e = animation.addAnimation(e, "armybirdfly1.png", "armybirdfly2.png")
					e.aggressive = true
					e.enemyType = "bird"
					e.health = 1
					e.hittable = config.hittableDelay
					e = ai.createAI(e, hero, math.random(40, 90), e.enemyType)
					e = physics.create(e, self.x, self.y, 0.1, 0, e.sprites[1][1]:getWidth() / 2, e.sprites[1][1]:getHeight() / 2)
					e.color = {r = math.random(190, 255), g = math.random(190, 255), b = math.random(190, 255)}
					e.shootTimer = 0
					e = collider.create(e, "enemy", 75*e.scale)
		
					e.centerDist = math.sqrt(math.pow(center.x - e.x, 2) + math.pow(center.y - e.y, 2))
					e.x = (((e.x) - center.x) / e.centerDist) * (planetRadius + e.radius) + center.x
					e.y = (((e.y) - center.y) / e.centerDist) * (planetRadius + e.radius) + center.y
					table.insert(enemies, e);
				end
			else 
				self.cooldown = self.cooldown - dtime
			end
		end

	end

	function e:dig()
		self.isEnemyUnderground = true
		self:setActive(false)
		self:setAnimation(2)
		--animation
		dist = math.sqrt(math.pow(self.x - hero.x, 2) + math.pow(self.y - hero.y, 2))
		sp = math.random(config.hero.speed * 60 * .6, config.hero.speed * 60 * .7)
		self.dxAI = ((hero.x - self.x) / dist) * sp
		self.dyAI = ((hero.y - self.y) / dist) * sp
		
		--set the scaling right
		if (self.dir == "away") then
			if (self.angle - hero.angle < math.pi and self.angle - hero.angle > 0) or self.angle - hero.angle  < -math.pi then
				self.scaleX = -self.scale
				self.scaleY = -self.scale
			else
				self.scaleX = self.scale
				self.scaleY = self.scale
			end
		elseif (self.dir == "left") then
			self.scaleX = -self.scale
			self.scaleY = -self.scale
		elseif (self.dir == "right") then
			self.scaleX = self.scale
			self.scaleY = self.scale
		end
		
	end

	function e:checkUnderground()
		self.blackDt = self.blackDt + dtime
		if (self.blackDt >= blackDelay) then
			self.blackDt = 0
			table.insert(blacks, self:newBlackMole(.07));
			if (table.getn(blacks) > 800) then
				table.remove(blacks, 1)
			end
		end
		if (math.pow(self.x-center.x, 2)+math.pow(self.y-center.y,2) > math.pow(self.radius+planetRadius,2)) then
			self:setActive(true)
			self:setAnimation(1)
			self.dxAI = 0
			self.dyAI = 0
			self.isEnemyUnderground = false
			self:setDirection()
		end
	end
	
	function e:newBlackMole(scale)
		local b = {}
		b = animation.create(b, 1, 200, black)
		--b = animation.addAnimation(b, "black.png")
		b = physics.create(b, self.x, self.y, scale, 0, b.sprites[1][1]:getWidth() / 2, b.sprites[1][1]:getHeight() / 2)
		b:setActive(false)
		b:setFrozen(true)
		return b
	end
	
	function e:newTurret()
		local e = {}
		e.shootTimer = 0
		e.health = 1
		e.hittable = config.hittableDelay
		e = animation.create(e, 2, .2)
		e = animation.addAnimation(e, "bear/turret.png", 'bear/turret.png')
		e = animation.addAnimation(e, "bear/turret.png", 'bear/turret.png')
		e.aggressive = false
		e.enemyType = "turret"
		e.shootDelay = config.turret.shootDelay
		e.shootTime = 0
		e = ai.createAI(e, hero, 0, e.enemyType)
		e = physics.create(e, self.x, self.y, .4, 0, e.sprites[1][1]:getWidth() / 2, e.sprites[1][1]:getHeight() / 2, 0, 0)
		e.color = {r = math.random(190, 255), g = math.random(190, 255), b = math.random(190, 255)}
		
		e = collider.create(e, "enemy", 30*e.scale)
		
		e.centerDist = math.sqrt(math.pow(center.x - e.x, 2) + math.pow(center.y - e.y, 2))
		table.insert(enemies, e)
	end

	function e:setDirection()
		if (hero.isUnderground == false) then
			if (e.aggressive) then
				--check distance
				if (math.pow(self.x-hero.x, 2)+math.pow(self.y-hero.y, 2) < 50000) then
					self.dir = "stop"
				else
					--random direction
					--[[if (math.random(0, 1) < .5) then
						self.dir = "left"
					else
						self.dir = "right"
					end]]
				end
			else
				self.dir = "away"
			end
		else
			if (math.random(0, 1) < .5) then
				self.dir = "left"
			else
				self.dir = "right"
			end
		end
	end
	
	function e:moveSurface()
		--Normal Surface Behavior
		if (self.dir == "none") then
			self:setDirection()
		elseif (self.dir == "stop") then
			self.dxAI = 0
			self.dyAI = 0
		elseif (self.dir == "away") then
			if (self.angle - hero.angle < math.pi and self.angle - hero.angle > 0) or self.angle - hero.angle  < -math.pi then
				self.dxAI = self.perpVector.x * -self.speed
				self.dyAI = self.perpVector.y * -self.speed
			else
				self.dxAI = self.perpVector.x * self.speed
				self.dyAI = self.perpVector.y * self.speed
			end
		elseif (self.dir == "left") then
			self.scaleX = -self.scale
			self.scaleY = -self.scale
			self.dxAI = self.perpVector.x * -self.speed
			self.dyAI = self.perpVector.y * -self.speed
			self.rotation = math.atan2(self.dyAI, self.dxAI)
		elseif (self.dir == "right") then
			self.scaleX = -self.scale
			self.scaleY = self.scale
			self.dxAI = self.perpVector.x * self.speed
			self.dyAI = self.perpVector.y * self.speed
			self.rotation = math.atan2(self.dyAI, self.dxAI)
		elseif (self.dir == "towards") then
			if (self.angle - hero.angle < math.pi and self.angle - hero.angle > 0) or self.angle - hero.angle  < -math.pi then
				self.dxAI = self.perpVector.x * -self.speed
				self.dyAI = self.perpVector.y * -self.speed
			else
				self.dxAI = self.perpVector.x * self.speed
				self.dyAI = self.perpVector.y * self.speed
			end
		else
			self.dir = "none"
		end
		--End normal surface behavior
	end
	
	function e:moveFloat()
		self.floatRadius = planetRadius + math.sin(time * 2 + self.hoverSinOffset) * 20 + config.bear.floatheight
		if (self.dir ~= "left" and self.dir ~= "right") then
			if (math.random() > .5) then
				self.dir = "left"
			else
				self.dir = "right"
			end
		end
		self.dx = 0
		self.dy = 0
		--self.centerDist = math.sqrt(math.pow(center.x - self.x, 2) + math.pow(center.y - self.y, 2))
		self.x = (((self.x) - center.x) / self.centerDist) * (self.floatRadius + self.radius) + center.x
		self.y = (((self.y) - center.y) / self.centerDist) * (self.floatRadius + self.radius) + center.y
		
		if (self.dir == "left") then
			self.scaleX = -self.scale
			self.scaleY = -self.scale
			self.dxAI = self.perpVector.x * -self.speed * .5
			self.dyAI = self.perpVector.y * -self.speed * .5
			self.rotation = math.atan2(self.dyAI, self.dxAI)
		else
			self.scaleX = -self.scale
			self.scaleY = self.scale
			self.dxAI = self.perpVector.x * self.speed * .5
			self.dyAI = self.perpVector.y * self.speed * .5
			self.rotation = math.atan2(self.dyAI, self.dxAI)
		end
	end

return e
end

return {
	createAI = createAI
}