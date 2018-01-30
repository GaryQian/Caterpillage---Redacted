-- Chris Meseha cmeseha1@jhu.edu
-- Matthew Tan mtan13@jhu.edu
-- Hamima Halim hhalim@jhu.edu
-- Gary Qian gqian1@jhu.edu
credit = "By Matthew Tan, Chris Meseha,\n\t\t Hamima Halim, and Gary Qian"
cutSceneCount = 0
local function load()
	titleScreenBg = love.graphics.newImage("titleimgplain.png")
	menuTime = 0
	gameTitle = { img = love.graphics.newImage("title.png")}
	gameTitle.x = width / 6
	gameTitle.y = height / 9
	
	scenerySetup.setup()

	menuFont = love.graphics.newFont("Forque.ttf", 60)
	chosenFont = love.graphics.newFont("Forque.ttf", 64)
	love.graphics.setFont(menuFont)

end

local function update(dt)
	menuTime = menuTime + dtime
	gameTitle.x = width / 6
	gameTitle.y = -height / 4 + math.sin(menuTime) * 20
	if (menuTime > 2) then
		if (cutSceneCount == 3) then
			gameStateIndex = 2 
			currentState = gameStates[gameStateIndex]
			currentState:load()
		elseif (cutSceneCount == 0) then
			cutSceneCount = cutSceneCount + 1
			menuTime = 0
			titleScreenBg = love.graphics.newImage("cutscenewords.png")
		elseif (cutSceneCount == 1 and menuTime > 5) then
			cutSceneCount = cutSceneCount + 1
			titleScreenBg = love.graphics.newImage("cutscene1.png")
			menuTime = 0
		elseif (cutSceneCount == 2) then
			cutSceneCount = cutSceneCount + 1
			titleScreenBg = love.graphics.newImage("cutscene2.png")
			menuTime = 0
		end
	end
end

local function draw()
	bgScale = math.max(width / 1280, height / 720)
	love.graphics.draw(titleScreenBg, 0, 0, 0, bgScale, bgScale)
	love.graphics.print("Press any key to skip Intro.", 50, 30)
	if (cutSceneCount == 0) then
		love.graphics.draw(gameTitle.img, gameTitle.x, gameTitle.y, 0, bgScale, bgScale)
		love.graphics.print(credit, center.x / 1.5, center.y)
	end
	
end

function average(large, small)
	return large / 2 - small / 2
end

local function keypressed(key)
	gameStateIndex = 2 
	currentState = gameStates[gameStateIndex]
	currentState:load()
    if key == ' ' or key == 'return' then
		--if (cutSceneCount == 3) then
			--gameStateIndex = 2 
			--currentState = gameStates[gameStateIndex]
			--currentState:load()
		--[[elseif (cutSceneCount == 0) then
			cutSceneCount = cutSceneCount + 1
			menuTime = 0
			titleScreenBg = love.graphics.newImage("cutscenewords.png")
		elseif (cutSceneCount == 1) then
			cutSceneCount = cutSceneCount + 1
			titleScreenBg = love.graphics.newImage("cutscene1.png")
			menuTime = 0
		elseif (cutSceneCount == 2) then
			cutSceneCount = cutSceneCount + 1
			titleScreenBg = love.graphics.newImage("cutscene2.png")
			menuTime = 0
		end]]
    end

	if key == 'escape' then
	   love.event.quit()
	end

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
