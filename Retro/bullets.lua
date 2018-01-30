physics = require("physics")
collider = require("collider")


--obj, x, y, sc, rot, offsetX, offsetY, dy, dx, isBullet, mass
function createBullets(obj, destx, desty, img, v, object)
	local bullet = {}
	if ( object == "hero") then
		bullet = collider.create(bullet, "herobullets", 10)
	elseif object == "bird" or object == "boss" then
		bullet = collider.create(bullet, "enemyBullets", 5)
	elseif object == "turret" then
		bullet = collider.create(bullet, "enemyBullets", 5)
		bullet.homing = true
	end
	bullet = animation.create(bullet, 1, 1)
	bullet = animation.addAnimation(bullet, img)
	bullet.removeTimer = 0
	bullet = physics.create(bullet, obj.x, obj.y, .3, 0, bullet.sprites[1][1]:getWidth() / 2, bullet.sprites[1][1]:getWidth() / 2, 0, 0, .1)
	bullet.v = v
	bullet.dx = ((destx-obj.x)/math.sqrt(math.pow(destx-obj.x, 2)+math.pow(desty-obj.y, 2))) * bullet.v
	bullet.dy = ((desty-obj.y)/math.sqrt(math.pow(destx-obj.x, 2)+math.pow(desty-obj.y, 2))) * bullet.v
	function bullet:updateBullets()
		if self.homing then
			bulletdist = math.sqrt(math.pow(hero.x - self.x, 2) + math.pow(hero.y - self.y, 2))
			self.dx = (hero.x - self.x) * (v) / bulletdist
			self.dy = (hero.y - self.y) * (v) / bulletdist
			self:updatePhysics()
		else
			self:updatePhysics()
		end
		self.rotation = math.atan2(self.dy, self.dx)
	end
	
	function bullet:drawBullets()
		self:drawAnimation()
	end
	
	return bullet
end

return {
	createBullets = createBullets
}
