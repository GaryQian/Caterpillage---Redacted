-- Chris Meseha cmeseha1@jhu.edu
-- Matthew Tan mtan13@jhu.edu
-- Hamima Halim hhalim@jhu.edu
-- Gary Qian gqian1@jhu.edu

bird = {[1] = {[1] = love.graphics.newImage("bird1.png"), [2] = love.graphics.newImage("bird2.png")}, [2] = {[1] = love.graphics.newImage("birdfly1.png"), [2] = love.graphics.newImage("birdfly2.png")}}

armybird = {[1] = {[1] = love.graphics.newImage("armybird1.png"), [2] = love.graphics.newImage("armybird2.png")}, [2] = {[1] = love.graphics.newImage("armybirdfly1.png"), [2] = love.graphics.newImage("armybirdfly2.png")}}

mole = {[1] = {[1] = love.graphics.newImage("mole1.png"), [2] = love.graphics.newImage("mole2.png"), [3] = love.graphics.newImage("mole3.png"), [4] = love.graphics.newImage("mole1.png"), [5] = love.graphics.newImage("mole2.png"), [6] = love.graphics.newImage("mole3.png"), [7] = love.graphics.newImage("mole1.png"), [8] = love.graphics.newImage("mole2.png")}, [2] = {[1] = love.graphics.newImage("moles/moleground1.png"), [2] = love.graphics.newImage("moles/moleground2.png"), [3] = love.graphics.newImage("moles/moleground3.png"), [4] = love.graphics.newImage("moles/moleground4.png"), [5] = love.graphics.newImage("moles/moleground5.png"), [6] = love.graphics.newImage("moles/moleground7.png"), [7] = love.graphics.newImage("moles/moleground8.png"), [8] = love.graphics.newImage("moles/moleground9.png")}}

black = {[1] = {[1] = love.graphics.newImage("black.png")}}

explosion = {[1] = {[1] = love.graphics.newImage("GroundExplosion1.png"), [2] = love.graphics.newImage("GroundExplosion2.png"), [3] = love.graphics.newImage("GroundExplosion3.png"), [4] = love.graphics.newImage("GroundExplosion4.png"), [5] = love.graphics.newImage("GroundExplosion5.png")}}

blood = {[1] = {[1] = love.graphics.newImage("Bloodsplat/Bloodsplat0.png"), [2] = love.graphics.newImage("Bloodsplat/Bloodsplat1.png"), [3] = love.graphics.newImage("Bloodsplat/Bloodsplat2.png"), [4] = love.graphics.newImage("Bloodsplat/Bloodsplat3.png"), [5] = love.graphics.newImage("Bloodsplat/Bloodsplat4.png"), [6] = love.graphics.newImage("Bloodsplat/Bloodsplat5.png"), [7] = love.graphics.newImage("Bloodsplat/Bloodsplat6.png")}}

bear = {[1] = {[1] = love.graphics.newImage("bear/bear1.png"), [2] = love.graphics.newImage("bear/bear2.png"), [3] = love.graphics.newImage("bear/bear3.png"), [4] = love.graphics.newImage("bear/bear4.png")}, [2] = {[1] = love.graphics.newImage("bear/attackbear1.png"), [2] = love.graphics.newImage("bear/attackbear2.png"), [3] = love.graphics.newImage("bear/attackbear3.png"), [4] = love.graphics.newImage("bear/attackbear4.png")}}

turret = {[1] = {[1] = love.graphics.newImage("black.png")}}


local function create(obj, frames, delay, sprites)
	obj.frames = frames
	obj.animations = 0
	obj.delay = delay
	obj.currentFrame = 1
	obj.currentAnimation = 1
	obj.delta = 0
	obj.animating = true
	obj.sprites = sprites or {}
	
	function obj:updateAnimation(dt)
		if (self.animating) then
			self.delta = self.delta + dtime
			if (self.delta >= self.delay) then
				self.currentFrame = (self.currentFrame % self.frames) + 1
				self.delta = self.delta - self.delay
				if (self.timer ~= nil) then
					self.timer = self.timer - 1
				end
			end
		end
		
	end
	function obj:drawAnimation()
		love.graphics.draw(
			self.sprites[self.currentAnimation][self.currentFrame], -- what to draw
			self.x, self.y, -- where to draw it
			self.rotation, -- rotation
			self.scaleX, self.scaleY, -- scale
			self.offsetX, self.offsetY -- origin
		)
		
	end
	function obj:stopAnimation()
		self.animating = false
	end

	function obj:startAnimation()
		self.animating = true
	end
	function obj:setAnimation(ani)
		self.currentAnimation = ani
		self.animating = true
	end
	return obj
end

function addAnimation(obj, one, two, three, four, five, six, seven, eight, nine, ten)
	obj.animations = obj.animations + 1
	obj.sprites[obj.animations] = {}
	for j = 1, obj.frames do
		if (j == 1) then
			obj.sprites[obj.animations][j] = love.graphics.newImage(one)
		elseif (j == 2) then
			obj.sprites[obj.animations][j] = love.graphics.newImage(two)
		elseif (j == 3) then
			obj.sprites[obj.animations][j] = love.graphics.newImage(three)
		elseif (j == 4) then
			obj.sprites[obj.animations][j] = love.graphics.newImage(four)
		elseif (j == 5) then
			obj.sprites[obj.animations][j] = love.graphics.newImage(five)
		elseif (j == 6) then
			obj.sprites[obj.animations][j] = love.graphics.newImage(six)
		elseif (j == 7) then
			obj.sprites[obj.animations][j] = love.graphics.newImage(seven)
		elseif (j == 8) then
			obj.sprites[obj.animations][j] = love.graphics.newImage(eight)
		elseif (j == 9) then
			obj.sprites[obj.animations][j] = love.graphics.newImage(nine)
		elseif (j == 10) then
			obj.sprites[obj.animations][j] = love.graphics.newImage(ten)
		end
	end
	
	return obj
end

function quadratic(t, b, c, d) 
	t = t / (d/2)
	if (t < 1) then
		return c/2*t*t + b
	end
	t = t - 1
	return -c/2 * (t*(t-2) - 1) + b
end

function linear(t, b, c, d) 
	return c*t/d + b;
end

return {
	create = create,
	addAnimation = addAnimation,
	quadratic = quadratic
}