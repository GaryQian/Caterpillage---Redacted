juiceLevel = config.camera.juiceLevel
function set(self)
  love.graphics.push()
  love.graphics.rotate(-self.rotation)
  love.graphics.scale(1 / self.scaleX, 1 / self.scaleY)
  love.graphics.translate(-self.x, -self.y)
end

function unset()
  love.graphics.pop()
end

function move(self, dx, dy)
  self.x = self.x + (dx or 0)
  self.y = self.y + (dy or 0)
end

function rotate(self, dr)
  self.rotation = self.rotation + dr
end

function scale(self, sx, sy)
  sx = sx or 1
  self.scaleX = self.scaleX * sx
  self.scaleY = self.scaleY * (sy or sx)
end
function setScale(self, sx)
  self.scaleX = sx
  self.scaleY = sx
end

function setPosition(self, x, y)
  self.x = x or self.x
  self.y = y or self.y
end

function shake(self, t, amount)
	self.shakeTime = t
	self.shakeAmount = amount
end

--[[function setScale(self, sx, sy)
  self.scaleX = sx or self.scaleX
  self.scaleY = sy or self.scaleY
end]]

function mousePosition(self)
  return love.mouse.getX() * self.scaleX + self.x, love.mouse.getY() * self.scaleY + self.y
end

function updateCamera(self, dt)
	self.x = self.x + (self.targX - self.x) * dtime * juiceLevel
	self.y = self.y + (self.targY - self.y) * dtime * juiceLevel
	if (self.shakeTime > 0) then
		self.shakeTime = self.shakeTime - dtime
		self.x = self.x + math.random() * self.shakeAmount
		self.y = self.y + math.random() * self.shakeAmount
	end
end

return {
	set = set,
	unset = unset,
	move = move,
	rotate = rotate,
	scale = scale,
	shake = shake,
	setPosition = setPosition,
	setScale = setScale,
	updateCamera = updateCamera,
	mousePosition = mousePosition
}
