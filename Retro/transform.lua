-- Chris Meseha cmeseha1@jhu.edu
-- Matthew Tan mtan13@jhu.edu
-- Hamima Halim hhalim@jhu.edu
-- Gary Qian gqian1@jhu.edu

function create(obj, x, y, sc, rot, offsetX, offsetY, dy, dx)
	
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
	
	if (sc == nil) then
		obj.scale = 1
	else
		obj.scale = sc
	end
	
	if (rot == nil) then
		obj.rotation = 0
	else
		obj.rotation = rot
	end
	
	if (offsetX == nil) then
		obj.offsetX = 0
	else
		obj.offsetX = offsetX
	end
	
	if (offsetY == nil) then
		obj.offsetY = 0
	else
		obj.offsetY = offsetY
	end
	
	function obj:updateTransform(dt)
		self.x = self.x + self.dx * dtime
		self.y = self.y + self.dy * dtime
	end
	
	function obj:setPosition(x, y)
		self.x = x
		self.y = y
	end
	
	function obj:setVelocity(dx, dy)
		self.dx = dx
		self.dy = dy
	end
	
	function obj:setDx(dx)
		self.dx = dx
	end
	
	function obj:setDy(dy)
		self.dy = dy
	end
	
	return obj
end

return {
	create = create
}