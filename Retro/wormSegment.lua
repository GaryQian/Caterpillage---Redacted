physics = require("physics")
animation = require("animation")

targ = nil
segSep = 50
followSpeed = 1
local function create(xx, yy, id)
	local obj = {}
	obj = physics.create(obj, xx, yy, hero.scale, hero.rotation, hero.offsetX, hero.offsetY)
	obj:setActive(false)
	
	obj = animation.create(obj, 2, math.random(20, 30) / 100)
	obj = animation.addAnimation(obj, "wormbody1.png", "wormbody2.png")
	obj.followAngle = 0
	obj.id = id
	function obj:updateWormSegment(dt)
		
		targ = nil
		if (self.id == 1) then
			targ = hero
		else
			targ = hero.segments[self.id-1]
		end
		
		self.followAngle = math.atan((targ.y - self.y) / (targ.x - self.x))
		if (targ.x - self.x > 0) then
			self.followAngle = self.followAngle + math.rad(180)
		end
		
		if (math.pow(targ.x - self.x,2) + math.pow(targ.y - self.y, 2) > math.pow(self.radius + targ.radius, 2)) then
			self.x = self.x + ((targ.x - self.x) - 2 * (targ.radius * math.cos(self.followAngle))) * followSpeed * dtime
			self.y = self.y + ((targ.y - self.y) - 2 * (targ.radius * math.sin(self.followAngle))) * followSpeed * dtime

		end
		dist = math.sqrt(math.pow(self.x - targ.x, 2) + math.pow(self.y - targ.y, 2))
		self.x = ((self.x - targ.x) / (dist)) * (2 * self.radius) + targ.x
		self.y = ((self.y - targ.y) / (dist)) * (2 * self.radius) + targ.y
		
		self:updateAnimation(dt)
		
	end
	
	return obj
end

return {
	create = create
}