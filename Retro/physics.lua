-- Chris Meseha cmeseha1@jhu.edu
-- Matthew Tan mtan13@jhu.edu
-- Hamima Halim hhalim@jhu.edu
-- Gary Qian gqian1@jhu.edu

gravity = 250

function create(obj, x, y, sc, rot, offsetX, offsetY, dy, dx, isBullet, mass)
	
	if (x == nil) then
		obj.x = 0
	else
		obj.x = x
	end
	
	if (y == nil) then
		obj.y = 0
	else
		obj.y = y
	end
	
	if (dx == nil) then
		obj.dx = 0
	else
		obj.dx = dx
	end
	
	if (dy == nil) then
		obj.dy = 0
	else
		obj.dy = dy
	end
	
	obj.scale = sc or 1
	
	obj.scaleX = obj.scale
	
	obj.scaleY = obj.scale
	
	if (rot == nil) then
		obj.rotation = 0
	else
		obj.rotation = rot
	end
	
	if (offsetX == nil) then
		if (obj.sprites ~= nil) then
			obj.offsetX = sc * obj.sprites[1][1]:getWidth() / 2
		else
			obj.offsetX = 0
		end
	else
		obj.offsetX = offsetX-- * sc
	end
	
	if (offsetY == nil) then
		if (obj.sprites ~= nil) then
			obj.offsetY = sc * obj.sprites[1][1]:getHeight() / 2
		else
			obj.offsetY = 0
		end
	else
		obj.offsetY = offsetY-- * sc
	end
	
	if (mass == nil) then
		obj.mass = 1
	else
		obj.mass = mass
	end
	
	if (isBullet == nil or isBullet == false) then
		obj.isBullet = 1
	else
		obj.isBullet = .33
	end
	
	obj.frozen = false
	obj.physicsActive = true
	obj.stopped = false
	obj.isWorm = false
	
	obj.fx = 0
	obj.fy = 0
	obj.dr = 0
	obj.centerDist = 0-- = math.sqrt(math.pow(center.x - obj.x, 2) + math.pow(center.y - obj.y, 2))
	
	function obj:updatePhysics(dt)
		if (self.inputx ~= nil) then
			if (self.isUnderground) then
				self.x = self.x + self.inputx * config.hero.undergroundSlow * dtime
				self.y = self.y + self.inputy * config.hero.undergroundSlow * dtime
			else
				--self.x = self.x + self.inputx * dtime
				--self.y = self.y + self.inputy * dtime
			end
		end
		
		if (self.frozen == false) then
			self.x = self.x + self.dx * dtime
			self.y = self.y + self.dy * dtime
			self.rotation = self.rotation + self.dr * dtime
			self.dr = self.dr * .97
		end
		
		--[[if (self.x < 0) then
			self.x = 0
		elseif (self.x > width) then
			self.x = width
		end
		
		if (self.y < 0) then
			self.y = 0
		elseif (self.y > height) then
			self.y = height
		end]]
		
		if (self.physicsActive) then
			self.centerDist = math.sqrt(math.pow(center.x - self.x, 2) + math.pow(center.y - self.y, 2))
			if (self.centerDist - self.radius < planetRadius) then
				if (self.inputx ~= nil) then
					self.dx = self.dx - self.dx * self.undergroundMultiplier * dtime
					self.dy = self.dy - self.dy * self.undergroundMultiplier * dtime
					if (self.isUnderground == false) then
						self.isUnderground = true
						setDirection()
						screamingSound:pause()
						groundImpactSound:play()
						table.insert(gameStats.land, gameTime)
					end
					
					self.isUnderground = true
					
					
					diggingSound:play()
					roar1Sound:stop()
					roar2Sound:stop()
					roar3Sound:stop()
				else
					self.x = (((self.x) - center.x) / self.centerDist) * (planetRadius + self.radius) + center.x
					self.y = (((self.y) - center.y) / self.centerDist) * (planetRadius + self.radius) + center.y
					
					local theta = math.atan((self.x-center.x)/(self.y-center.y))
					local rotate_x = math.cos(theta) * (self.x-self.dx-center.x) - math.sin(theta) * (self.y-self.dy-center.y)
					local rotate_y = math.sin(theta) * (self.x -self.dx - center.x) + math.cos(theta) * (self.y - self.dy - center.y)
					rotate_x = -1 * rotate_x
					local mirror_x = math.cos(-1*theta) * rotate_x - math.sin(-1*theta) * rotate_y + center.x
					local mirror_y = math.sin(-1*theta) * rotate_x + math.cos(-1*theta) * rotate_y + center.y
					self.dx = (mirror_x - self.x) * .5
					self.dy = (mirror_y - self.y) * .5
				end
			else
				self:applyGravity()
				if (self.isUnderground ~= nil) then
					if (self.isUnderground == true) then
						temp = math.sqrt(math.pow(self.dx, 2) + math.pow(self.dy, 2))
						if (temp > config.hero.speed * 40) then
							self.dx = self.dx + self.inputx / 2
							self.dy = self.dy + self.inputy / 2
						else
							self.dx = self.dx + self.inputx
							self.dy = self.dy + self.inputy
						end
						if (self.isUnderground == true) then
							self.isUnderground = false
							setDirection()
						end
						table.insert(gameStats.leap, gameTime)
						self.isUnderground = false
						setDirection()
						diggingSound:pause()
						screamingSound:play()
						groundImpactSound:stop()
						local temp = math.floor(math.random(1, 3.99))
						if (planetIndex == 1) then
							roar1Sound:play()
						elseif (planetIndex == 2) then
							roar2Sound:play()
						else
							roar3Sound:play()
						end
					end
					
				end
			end

			self.dx = self.dx * .9985 + self.fx * dtime
			self.dy = self.dy * .9985 + self.fy * dtime
		end
	end
	
	function obj:setPosition(x, y)
		self.x = x
		self.y = y
		self.stopped = false
	end
	
	function obj:setActive(bool)
		self.physicsActive = bool
	end
	
	function obj:setFrozen(bool)
		self.frozen = bool
	end
	
	function obj:setVelocity(dx, dy)
		self.dx = dx
		self.dy = dy
		self.stopped = false
	end
	
	function obj:setDx(dx)
		self.dx = dx
		self.stopped = false
	end
	
	function obj:setDy(dy)
		self.dy = dy
		self.stopped = false
	end
	
	function obj:applyForce(dx, dy)
		self.dx = self.dx + dx * self.mass
		self.dy = self.dy + dy * self.mass
		self.stopped = false
	end
	
	function obj:applyImpulse(dx, dy)
		self.dx = self.dx + (dx / self.mass) * dtime
		self.dy = self.dy + (dy / self.mass) * dtime
		self.stopped = false
	end
	
	function obj:applyGravity()
		self.dx = self.dx + ((center.x - (self.x)) / self.centerDist) * dtime * gravity * self.isBullet
		self.dy = self.dy + ((center.y - (self.y)) / self.centerDist) * dtime * gravity * self.isBullet
	end
	return obj
	
end

return {
	create = create
}