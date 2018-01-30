-- Chris Meseha cmeseha1@jhu.edu
-- Matthew Tan mtan13@jhu.edu
-- Hamima Halim hhalim@jhu.edu
-- Gary Qian gqian1@jhu.edu
play = "New game"
highscore = "Highscores"
bright = "Visual Brightness"
volume = "Volume"
quit = "quit"
instruct = "Tutorial"
mousePriority = false
sounds = {}

button = require("button")

playPressed = 0

local function load()
	--clear out all the game data if it exists.
	scenery = {}
	enemies = {}
	planet = {}
	blacks = {}
	shots = {}
	texts = {}
	heroBullets = {}
	bullets = {}
	powerups = {}
	enemyBullets = {}
	enemyCollider = {}
	sceneryCollider = {}
	powerupCollider = {}
	selectedPlants = {}
	selectedBuildings = {}
	selectedMisc = {}
	selectedEnemies = {}
	spawnRates = {}
	sounds = {}
	boss = nil
	loadingStarted = nil

	
	for i, v in ipairs(sounds) do
		v:stop()
	end
	
	love.keyboard.setTextInput(false)
	
	collectgarbage()
	
	titleScreenBg = love.graphics.newImage("titleimgplain.png")
	width = love.window.getWidth()
	height = love.window.getHeight()
	gameTitle = { img = love.graphics.newImage("title.png")}
	gameTitle.x = width / 6
	gameTitle.y = height / 9
	playPressed = 0
	volumeRatio = .5
	brightRatio = .5
	volumeChange = false
	brightChange = false
	
	if (userdata.games <= 2) then
		tutorial = true
	end
	
	time = 0
	BOSS = nil
	if (song ~= nil) then
		song:stop()
	end
	local temp = math.floor(math.random(1, 3.999))
	if (temp == 1) then
		song = love.audio.newSource("Fist Full Of Adventure.mp3")
	elseif (temp == 2) then
		song = love.audio.newSource("Going To Prom.mp3")
	else
		song = love.audio.newSource("Silent Ghost.mp3")
	end
	song:setVolume(.6)
	song:setLooping(true)
	song:play()
	
	playButton = {}
	highscoresButton = {}
	optionsButton = {}
	
	button.create(playButton, "buttons/BarracksButton.png", "buttons/Check.png", width / 2, 2 * height / 6, 0, .4)
	button.create(highscoresButton, "buttons/BarracksButton.png", "buttons/Check.png", width / 2, 3 * height / 6, 0, .4)
	button.create(optionsButton, "buttons/BarracksButton.png", "buttons/Check.png", width / 2, 4 * height / 6, 0, .4)
	
	loadingChomper1 = {}
	loadingChomper2 = {}
	
	loadingChomper1 = animation.create(loadingChomper1, 1, 1)
	loadingChomper1 = animation.addAnimation(loadingChomper1, "chomper.png")
	loadingChomper1 = physics.create(loadingChomper1, width / 2, height, bgScale, 0, 960, 0, 0, (height / 2) / config.loading.swipeTime)
	loadingChomper1:setActive(false)
	
	loadingChomper2 = animation.create(loadingChomper2, 1, 1)
	loadingChomper2 = animation.addAnimation(loadingChomper2, "chomper.png")
	loadingChomper2 = physics.create(loadingChomper2, width / 2, 0, bgScale, math.pi, 960, 0, 0, -height / 2 / config.loading.swipeTime)
	loadingChomper2:setActive(false)
	
end

local function update(dt)
	love.mousepressed()
	if (love.system.getOS() == "Android" or love.system.getOS() == "iOS") then
		if (love.touch.getTouchCount() == 0) then
			playButton:unselect()
			highscoresButton:unselect()
			optionsButton:unselect()
		end
	end
	
	if (loadingStarted ~= nil) then
		loadingStarted = loadingStarted - dtime
		loadingChomper1.y = height - (height / 2) * (config.loading.swipeTime - loadingStarted) / config.loading.swipeTime
		loadingChomper2.y = (height / 2) * (config.loading.swipeTime - loadingStarted) / config.loading.swipeTime
		if (loadingStarted < 0) then
			--load loading screen
			gameStateIndex = 5
		    currentState = gameStates[gameStateIndex]
		    currentState:load()
		end
	end
	
	love.graphics.setColor(255, 255, 255)
	--[[love.graphics.print(play, width/2, height * 3/8.5)
	love.graphics.print(highscore, width/2, height*4/8.5)
	love.graphics.print(bright, width/2, height*5/8.5)
	love.graphics.print(volume, width/2, height*6/8.5)
	love.graphics.print(quit, width/2, height*7/8.5)]]
	
	menuTime = menuTime + dtime
	gameTitle.x = width / 6
	gameTitle.y = -height / 4 + math.sin(menuTime) * 20
	--gameTitle.y = gameTitle.y + math.sin(menuTime) / 4
	
	if (time > 300 and BOSS == nil) then
		BOSS = {}
		BOSS = animation.create(BOSS, 8, .1)
		BOSS = animation.addAnimation(BOSS, "left-tilt.png", "left-wiggle.png", "front.png", "right-wiggle.png", "right-tilt.png", "right-wiggle.png", "front.png", "left-wiggle.png")
		BOSS = physics.create(BOSS, center.x / 2, center.y, 9, 0, (BOSS.sprites[1][1]:getWidth() / 2), (BOSS.sprites[1][1]:getHeight() / 2))
		BOSS:setActive(false)
		BOSS:setFrozen(true)
	end
	
	if (BOSS ~= nil) then
		BOSS:updateAnimation()
		BOSS:updatePhysics()
	end
end

local function draw()
	love.graphics.draw(titleScreenBg, 0, 0, 0, bgScale, bgScale)
	love.graphics.draw(gameTitle.img, gameTitle.x, gameTitle.y, 0, bgScale, bgScale)
	if (playPressed == 0) then 
		love.graphics.setColor(200, 200, 0)
		love.graphics.setFont(chosenFont)
		love.graphics.print(play, width/2, height * 3/8.3)
	else
		love.graphics.setColor(255, 255, 255)
		love.graphics.setFont(menuFont)
		love.graphics.print(play, width/2, height* 3/8.3)
	end

	--this is the toggle stuff
	if (playPressed == 1) then
		love.graphics.setColor(200, 200, 0)
		love.graphics.setFont(chosenFont)
		love.graphics.print(instruct, width/2 + 50, height * 4/8.5)
		if tutorial then 
			love.graphics.rectangle("line", width/2, height * 4/8.5 + 25, 25, 25)
			love.graphics.setColor(200, 200, 0)
			love.graphics.rectangle("fill", width/2, height * 4/8.5 + 25, 25, 25)
			love.graphics.print("On", width/1.4, height * 4/8.5)
		else 
			love.graphics.rectangle("line", width/2, height * 4/8.5 + 25, 25, 25)
			love.graphics.setColor(255,255,255)
			love.graphics.print("Off", width/1.4, height * 4/8.5)
		end
	else
		love.graphics.setColor(255, 255, 255)
		love.graphics.setFont(menuFont)
		love.graphics.print(instruct, width/2 + 50, height* 4/8.5)
		if tutorial then 
			love.graphics.rectangle("line", width/2, height * 4/8.5 + 25, 25, 25)
			love.graphics.setColor(200, 200, 0)
			love.graphics.rectangle("fill", width/2, height * 4/8.5 + 25 , 25, 25)
			love.graphics.print("On", width/1.4, height * 4/8.5)
		else 
			love.graphics.rectangle("line", width/2, height * 4/8.5 + 25, 25, 25)
			love.graphics.setColor(255,255,255)
			love.graphics.print("Off", width/1.4, height * 4/8.5)
		end
	end

	if (playPressed == 2) then 
		love.graphics.setColor(200, 200, 0)
		love.graphics.setFont(chosenFont)
		love.graphics.print(highscore, width/2, 5*height/8.7)
	else
		love.graphics.setColor(255, 255, 255)
		love.graphics.setFont(menuFont)
		love.graphics.print(highscore, width/2, 5*height/8.7)
	end
		
	if (playPressed == 3) then 
		if brightChange then 
			love.graphics.rectangle("line", width/2, 6*height/8.7 + 20, 400, 50)
			love.graphics.rectangle("fill", width/2, 6*height/8.7 + 20, 400 * brightRatio, 50)
			love.audio.setVolume(brightRatio)
		else 
			love.graphics.setColor(200, 200, 0)
			love.graphics.setFont(chosenFont)
			love.graphics.print(bright, width/2, 6*height/8.8)
		end
	else
		love.graphics.setColor(255, 255, 255)
		love.graphics.setFont(menuFont)
		love.graphics.print(bright, width/2, 6*height/8.8)
	end
	
	if (playPressed == 4) then 
		if volumeChange then 
			love.graphics.rectangle("line", width/2, 7*height/8.7 - 10, 400, 50)
			love.graphics.rectangle("fill", width/2, 7*height/8.7 - 10, 400 * volumeRatio, 50)
			love.audio.setVolume(volumeRatio)
		else 
			love.graphics.setColor(200, 200, 0)
			love.graphics.setFont(chosenFont)
			love.graphics.print(volume, width/2, 7*height/9)
		end
	else
		love.graphics.setColor(255, 255, 255)
		love.graphics.setFont(menuFont)
		love.graphics.print(volume, width/2, 7*height/9)
	end

	if (playPressed == 5) then 
		love.graphics.setColor(200, 200, 0)
		love.graphics.setFont(chosenFont)
		love.graphics.print(quit, width/2, 8*height/9.2)
	else
		love.graphics.setColor(255, 255, 255)
		love.graphics.setFont(menuFont)
		love.graphics.print(quit, width/2, 8*height/9.2)
	end

	if (BOSS ~= nil) then
		love.graphics.setColor(255, 255, 255, 230)
		BOSS:drawAnimation()
		love.graphics.setColor(255, 255, 255)
	end
	
	
	playButton:drawButton()
	highscoresButton:drawButton()
	optionsButton:drawButton()
	
	if (loadingStarted ~= nil) then
		loadingChomper1:drawAnimation()
		loadingChomper2:drawAnimation()
	end
end

local function textinput(t)
end

local function keypressed(key)
	mousePriority = false 	

      if key == 'up' then
     
      	 playPressed = (playPressed -1)%6
	
      end

      if key == 'down' then
      	 
      	 playPressed = (playPressed + 1)%6
	
      end

      if key == 'right' then
      	if playPressed == 2 then
      		if brightChange then
      			if brightRatio < .9 then
      				brightRatio = brightRatio + .1
      			end
      		end
      	end
      	if playPressed == 3 then 
      		if volumeChange then
      			if volumeRatio < .9 then
      				volumeRatio = volumeRatio + .1
      			end
      		end
      	end
      end

      if key == 'left' then
      	if playPressed == 2 then
      		if brightChange then
      			if brightRatio > .1 then
      				brightRatio = brightRatio - .1
      			end
      		end
      	end
      	if playPressed == 3 then 
      		if volumeChange then
      			if volumeRatio > .1 then
      				volumeRatio = volumeRatio - .1
      			end
      		end
      	end
      end
		
      if key == ' ' or key == 'return' then
      	 
      	 if playPressed == 0 then
	        gameStateIndex = 3
	        paused = false
		    currentState = gameStates[3]
		    currentState:load()
	     end

	     if playPressed == 1 then
	     	if tutorial then
	     		tutorial = false 
	     	else 
	     		tutorial = true
	     	end
	     end

	     if playPressed == 2 then
 		    gameStateIndex = 4
		    currentState = gameStates[4]
		    currentState:load()
	     end

	     if playPressed == 4 then
	     	if volumeChange then
	     		volumeChange = false
	     	else
	     		volumeChange = true
	     	end
	     end

	     if playPressed == 3 then
	     	if brightChange then
	     		brightChange = false
	     	else
	     		brightChange = true
	     	end
	     end

		 if playPressed == 5 then
			love.event.quit()
		 end
      end
	  
	  if key == 'escape' or key == "q" then
		love.event.quit()
	  end
end

local function keyreleased(key)

end

local function mousemoved(x, y, dx, dy)
	--[[mousePriority = true
	if x > width/2 and y < height * (3/8.3 + 4/8.5)/2 then 
		playPressed = 0
	end
	if x > width/2 and y > height * (3/8.3 + 4/8.5)/2 and y < height * (5/8.7 + 4/8.5)/2 then 
		playPressed = 1
	end
	if x > width/2 and y > height * (5/8.7 + 4/8.5)/2 and y < height * (6/8.7 + 5/8.7)/2 then 
		playPressed = 2
	end
	if x > width/2 and y > height * (6/8.7 + 5/8.7)/2 and y < height * (7/8.7 + 6/8.7)/2 then 
		playPressed = 3
	end
	if x > width/2 and y > height * (7/8.7 + 6/8.7)/2 and y < height * (8/9.2 + 7/8.7)/2 then 
		playPressed = 4
	end
	if x > width/2 and y > height * (8/9.2 + 7/8.7)/2 then 
		playPressed = 5
	end]]
	
	--playButton:checkButton(x, y)
	--highscoresButton:checkButton(x, y)
	--optionsButton:checkButton(x, y)
end


local function mousepressed(x, y, button)
	playButton:unselect()
	highscoresButton:unselect()
	optionsButton:unselect()
	a, b = love.mouse.getPosition()
	local results = {}
	results = playButton:checkButton(a, b)
	if (results[1]) then
		if (loadingStarted == nil) then
			loadingStarted = config.loading.swipeTime
			nextScreen = 3
		end
	end
	
	results = highscoresButton:checkButton(a, b)
	if (results[1]) then
		gameStateIndex = 4
		currentState = gameStates[4]
		currentState:load()
	end
	
	results = optionsButton:checkButton(a, b)
	if (results[1]) then
		
	end
	
    --[[Checking if touch is within an object/area
	local results = playButton:checkButton(x, y)
	if (results[1]) then
		
		gameStateIndex = 3
		currentState = gameStates[3]
		currentState:load()
	end
	
	results = highscoresButton:checkButton(x, y)
	if (results[1]) then
		
		gameStateIndex = 4
		currentState = gameStates[4]
		currentState:load()
	end
	
	results = optionsButton:checkButton(x, y)
	if (results[1]) then
		
	end]]
	
	--[[if button == 'l' then
		mousemoved(x, y, 0, 0)
		if playPressed == 0 then
	       gameStateIndex = 3
		   currentState = gameStates[3]
		   currentState:load()
	    end

	    if playPressed == 1 then
	    	if tutorial then
	    		tutorial = false 
	    	else 
	    		tutorial = true
	    	end
	    end

	    if playPressed == 2 then
 		   gameStateIndex = 4
		   currentState = gameStates[4]
		   currentState:load()
	    end

	    if playPressed == 4 then
	    	if volumeChange then
	    		volumeChange = false
	    	else
	    		volumeChange = true
	    	end
	    end

	    if playPressed == 3 then
	    	if brightChange then
	    		brightChange = false
	    	else
	    		brightChange = true
	    	end
	    end

		if playPressed == 5 then
			love.event.quit()
		end
		 
		playButton:checkButton(x, y)
		highscoresButton:checkButton(x, y)
		optionsButton:checkButton(x, y)
	end]]
	--[[print(y)
	print(height)
	gameStateIndex = 3
	currentState = gameStates[gameStateIndex]
	currentState:load()
	local xl, yl = love.mouse.getPosition()
	if yl < height / 4 then
		gameStateIndex = 3
		currentState = gameStates[3]
		currentState:load()
	elseif yl < height * 5 / 12 then 
		gameStateIndex = 4
		currentState = gameStates[4]
		currentState:load()
	elseif yl < height * 3/4 then 
	else
		love.event.quit()
	end
	]]
end

local function touchpressed(id, x, y, pressure)
	-- Converting the touchscreen proximity coordinates
    -- to actual pixel coordinates
    local cx = x * width
    local cy = y * height

    -- Checking if touch is within an object/area
	playButton:checkButton(cx, cy)
	highscoresButton:checkButton(cx, cy)
	optionsButton:checkButton(cx, cy)
end

local function touchreleased(id, x, y, pressure)
	-- Converting the touchscreen proximity coordinates
    -- to actual pixel coordinates
    local cx = x * width
    local cy = y * height
	playButton:unselect()
	highscoresButton:unselect()
	optionsButton:unselect()
    -- Checking if touch is within an object/area
	local results = playButton:checkButton(cx, cy)
	if (results[1]) then
		
		gameStateIndex = 3
		currentState = gameStates[3]
		currentState:load()
	end
	
	results = highscoresButton:checkButton(cx, cy)
	if (results[1]) then
		
		gameStateIndex = 4
		currentState = gameStates[4]
		currentState:load()
	end
	
	results = optionsButton:checkButton(cx, cy)
	if (results[1]) then
		
	end
end

local function touchmoved(id, x, y, pressure)
	-- Converting the touchscreen proximity coordinates
    -- to actual pixel coordinates
    local cx = x * width
    local cy = y * height
	playButton:unselect()
	highscoresButton:unselect()
	optionsButton:unselect()
    -- Checking if touch is within an object/area
	playButton:checkButton(cx, cy)
	highscoresButton:checkButton(cx, cy)
	optionsButton:checkButton(cx, cy)
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
	mousemoved = mousemoved,
	textinput = textinput,
	touchpressed = touchpressed,
	touchreleased = touchreleased,
	touchmoved = touchmoved
}
