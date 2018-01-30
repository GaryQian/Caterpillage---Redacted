

local function create(obj, text, t, x, y, s, dx, dy, r, g, b)
	
	obj.text = text
	obj.time = t
	obj.maxTime = t
	obj.alpha = 255
	obj.x = x
	obj.y = y
	obj.scale = s or 1
	obj.dx = dx or 0
	obj.dy = dy or -1
	obj.outCamera = outCamera or false
	obj.r = r or 255
	obj.g = g or 255
	obj.b = b or 255
	
	function obj:updateText()
		self.time = self.time - dtime
		self.y = self.y + self.dy
		self.x = self.x + dx
		obj.alpha = obj.time / obj.maxTime * 255
	end
	
	function obj:drawText()
	love.graphics.setColor(self.r, self.g, self.b, self.alpha)
		love.graphics.print(self.text, self.x - 40, self.y - 10, 0, self.scale, self.scale)
		love.graphics.setColor(255, 255, 255)
	end
	
	return obj

end



return {
	create = create
}