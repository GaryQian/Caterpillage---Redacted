-- Chris Meseha cmeseha1@jhu.edu
-- Matthew Tan mtan13@jhu.edu
-- Hamima Halim hhalim@jhu.edu
-- Gary Qian gqian1@jhu.edu
local function load()
	playButton = nil
	highscoresButton = nil
	optionsButton = nil
	
	collectgarbage()
	
	chompin = 50
	chompinTime = config.loading.swipeTime
	began = 0
	loadingChomper1.y = (height / 2)
	loadingChomper2.y = (height / 2)
	
end

local function update(dt)
	
	if (began > 1) then
		gameStateIndex = nextScreen
		currentState = gameStates[gameStateIndex]
		currentState:load()
	else
		began = began + 1
	end
end

local function draw()
	loadingChomper1:drawAnimation()
	loadingChomper2:drawAnimation()
end

local function keypressed(key)
	gameStateIndex = 3 
	currentState = gameStates[gameStateIndex]
	currentState:load()

end

local function keyreleased(key)
end

local function mousepressed(x, y, button)
	gameStateIndex = 2 
	currentState = gameStates[gameStateIndex]
	currentState:load()
end

local function textinput(t)

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