-- Chris Meseha cmeseha1@jhu.edu
-- Matthew Tan mtan13@jhu.edu
-- Hamima Halim hhalim@jhu.edu
-- Gary Qian gqian1@jhu.edu
titleScreen = require("titleScreen")
gameScreen = require("gameScreen")
highScoreScreen = require("highScoreScreen")
menuScreen = require("menuScreen")
loadingScreen = require("loadingScreen")

animation = require("animation")

JSON = require "JSON"

physics = require("physics")

gameStateIndex = 1
nextScreen = 1
chompin = 0
chompinTime = 0
gameStates = { titleScreen, menuScreen, gameScreen, highScoreScreen, loadingScreen }

currentState = {}
currentState.__index = currentState
--speed = 10 * 60

time = 0
dtime = 0
frames = 0
enteredText = ""
width = 1280
height = 720

function love.load()
	math.randomseed(os.time())
	world = love.physics.newWorld( 0, 0, false )
	
	width = love.window.getWidth()
	height = love.window.getHeight()
	
	center = {}
	center.x = width / 2
	center.y = height / 2
	
	background = {
		Img = love.graphics.newImage("space.jpg"),
	}
	background.width = background.Img:getWidth()
	background.height = background.Img:getHeight()

	currentState = titleScreen
	currentState:load()
	song = love.audio.newSource("external.xm")
	song:setLooping(true)
	song:play()
	
	rawConfig = love.filesystem.read("config.json")
    config = JSON:decode(rawConfig)
	
	if (love.filesystem.exists(config.highscores.fileName) == false) then
		love.filesystem.write(config.highscores.fileName, "{\"scores\": [{\"name\": \"CPU\", \"score\": 0}, {\"name\": \"CPU\", \"score\": 0}, {\"name\": \"CPU\", \"score\": 0}, {\"name\": \"CPU\", \"score\": 0}, {\"name\": \"CPU\", \"score\": 0}]}")
	end
	rawHighscores = love.filesystem.read(config.highscores.fileName)
	highscores = JSON:decode(rawHighscores)
	
	if (love.filesystem.exists("userdata.json") == false) then
		love.filesystem.write("userdata.json", "{\"games\": 0, \"goal1\": 1, \"goal2\": 2, \"goal3\": 3, \"nextGoal\": 4,  \"money\": 0}")
	end
	rawStats = love.filesystem.read("userdata.json")
	userdata = JSON:decode(rawStats)
	
end

function love.update(dt)
	if (time % 2 < .06) then
		width = love.window.getWidth()
		height = love.window.getHeight()
		bgScale = math.max(width / 1280, height / 720)
	end
	---------------------------
	currentState:update(dt)
	---------------------------
	time = time + dt
	dtime = dt
	frames = frames + 1
	
	--LOADING SCREEN STUFF
	if (chompin >= 1) then
		if (dtime < 1 / 4) then
			chompin = chompin - 5
			if (chompin < 1) then
				chompin = 1
			end
		end
		loadingChomper1.y = height - (height / 2) * (chompinTime) / config.loading.swipeTime
		loadingChomper2.y = (height / 2) * (chompinTime) / config.loading.swipeTime
		if (chompin == 1 and planet ~= nil) then
			chompinTime = chompinTime - dtime
		else
			chompin = chompin - 1
		end
		
		if (chompinTime < 0) then
			chompin = 0
			loadingChomper1 = nil
			loadingChomper2 = nil
		end
	end
	
	if (dtime > 0.1) then
		dtime = 0.1
	end
	--END LOADING SCREEN STUFF
end

function love.draw()
	currentState:draw()
	if (chompin >= 1) then
		loadingChomper1:drawAnimation()
		loadingChomper2:drawAnimation()
	end
	--love.graphics.print(rawHighscores, 10, 10)
end

function love.textinput(t)
	enteredText = t
	currentState.textinput(t)
end

function love.mousepressed(x, y, button) 
	currentState.mousepressed(x, y, button)
end

function love.mousemoved(x, y, dx, dy)
	if gameStateIndex == 2 then
		currentState.mousemoved(x, y, dx, dy)
	end
end

love.keyboard.setKeyRepeat(false)

function love.keypressed(key)
	currentState.keypressed(key)
	
end

function love.keyreleased(key)
	currentState.keyreleased(key)
end

function love.touchpressed(id, x, y, pressure)
	currentState.touchpressed(id, x, y, pressure)
end

function love.touchreleased(id, x, y, pressure)
	currentState.touchreleased(id, x, y, pressure)
end

function love.touchmoved(id, x, y, pressure)
	currentState.touchmoved(id, x, y, pressure)
end

function love.lowmemory()
	collectgarbage()
end






