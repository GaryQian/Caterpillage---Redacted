-- Chris Meseha cmeseha1@jhu.edu
-- Matthew Tan mtan13@jhu.edu
-- Hamima Halim hhalim@jhu.edu
-- Gary Qian gqian1@jhu.edu
highscores = {}
highscoreNames = {}

local utf8 = require("utf8") 

local namex = 0
local namey = 0
local scorex = 0
local scorey = 0

highestScore = 0
highScoreIndex = 0
highestScoreName = "" 
set = false
setted = false

local function load()
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
	--pick out the props for the planet
	selectedPlants = {}
	selectedBuildings = {}
	selectedMisc = {}
	selectedEnemies = {}
	spawnRates = {} -- spawn rate for mole and bird
	boss = nil
	tut1 = nil
	--[[song = nil
	diggingSound = nil
	groundImpactSound = nil
	roar1Sound = nil
	roar2Sound = nil
	roar3Sound = nil
	screamingSound = nil
	switchSound = nil
	heartbeatSound = nil]]
	
	collectgarbage()
	
	bodyFont = love.graphics.newFont("Forque.ttf", height / 15)
	titleFont = love.graphics.newFont("Forque.ttf", height / 12)
	love.graphics.setFont(bodyFont)
	newName = ""
	keyboardUp = false
	newScore = false
	highscoreScreenTime = 0
	scoreIndex = 0
	scoreCount = 0
	
	--find minimum score
	minScore = 0
	--[[for i, v in ipairs(highscores.scores) do
		if (v.score < minScore) then
			minScore = v.score
		end
	end]]
	if (highscores.scores[config.highscores.maxScores] ~= nil) then
		minScore = highscores.scores[config.highscores.maxScores].score
	end
	
	score = math.floor(score)
	
	if (score > 900) then
		userdata.games = userdata.games + 1
		love.filesystem.write("userdata.json", JSON:encode_pretty(userdata))
	end
	
	
	scoreCount = table.getn(highscores.scores)
	--create a slot for the new score if it is good enough/is space for it
	if (score ~= 0 and (scoreCount < config.highscores.maxScores or minScore < score)) then
		newScore = true
		--show keyboard on mobile
		if (love.system.getOS() == "Android" or love.system.getOS() == "iOS") then
			love.keyboard.setTextInput(true)
			keyboardUp = true
		end
		--[[if (scoreCount >= config.highscores.maxScores) then
			--remove the lowest score
			table.remove(highscores.scores, scoreCount)
			scoreCount = scoreCount - 1
		end]]
		
		--make new slot
		for i=1,scoreCount do
			if (highscores.scores[i].score < score) then
				table.insert(highscores.scores, {})
				scoreCount = scoreCount + 1
				local storage = highscores.scores[j]
				local storage2
				for j=scoreCount,i,-1 do
					highscores.scores[j] = highscores.scores[j - 1]
				end
				highscores.scores[i] = {}
				highscores.scores[i].score = score
				highscores.scores[i].name = newName
				scoreIndex = i
				break
			end
		end
		
	end
	
	scoreUpper = scoreIndex - 1
	scoreLower = scoreIndex + 1
	if (scoreIndex < 3) then
		scoreUpper = 3
		scoreLower = 5
	end
	
	background = animation.create(background, 1, 1)
	background = animation.addAnimation(background, "largebg.png")
	background = physics.create(background, center.x, center.y, .5, 0, (background.sprites[1][1]:getWidth() / 2), (background.sprites[1][1]:getHeight() / 2))
	background:setActive(false)
	background:setFrozen(true)
end

local function update(dt)
	highscoreScreenTime = highscoreScreenTime + dtime
end

local function draw()
	love.graphics.setColor(255, 255, 255)
	background:drawAnimation()
	
	love.graphics.setFont(bodyFont)
	
	namex = width/4 - 80
	namey = height / 9
	scorex = width/1.5
	scorey = height / 9 
	
	--love.graphics.print(newName, 10, 10)
	
	for i=1,scoreCount do
		if (i == 1 or i == 2 or (i >= scoreUpper and i <= scoreLower)) and highscores.scores[i] ~= nil then
			if (i == scoreIndex and highscoreScreenTime % .5 < .25) then
				love.graphics.print(i .. ". " .. highscores.scores[i].name .. "|", namex, namey)
			else
				love.graphics.print(i .. ". " .. highscores.scores[i].name, namex, namey)
			end
			love.graphics.print(highscores.scores[i].score, scorex, scorey)
			namey = namey + height / 16
			scorey = scorey + height / 16
		end
	end
	
	love.graphics.setFont(titleFont)
	if (newScore) then
		love.graphics.print("New High Score! Type your name below", width / 5.5, 10)
	else
		love.graphics.print("Highscores", width/2.7 + 40, 10)
	end
	

end

local function keypressed(key)
	if key == 'backspace' and newScore then
		local byteoffset = utf8.offset(newName, -1)
 
        if byteoffset then
            -- remove the last UTF-8 character.
            -- string.sub operates on bytes rather than UTF-8 characters, so we couldn't do string.sub(text, 1, -2).
            newName = string.sub(newName, 1, byteoffset - 1)
        end
		--newName = string.sub(newName, -1 * string.len(newName) + 1)
		--end
		highscores.scores[scoreIndex].name = newName
		return
	end
	
	if key == 'escape' then
		exitHighscores()
		love.event.quit()
	end

	if key == ' '  or key == 'return' then
		--score = 0
		if (keyboardUp) then
			love.keyboard.setTextInput(false)
			keyboardUp = false
		else
			exitHighscores()
		end
		return
	end
	
	if (newScore and string.len(newName) <= config.highscores.maxNameLength and string.len(key) == 1) then
		newName = newName .. key
		highscores.scores[scoreIndex].name = newName
	end
end	

local function keyreleased(key)
end

local function mousepressed(x, y, button)
	if (highscoreScreenTime > .5) then
		exitHighscores()
	end
end

local function textinput(t)
	if enteredText ~= nil and newScore == true and string.len(newName) <= config.highscores.maxNameLength then
		newName = newName .. enteredText
		highscores.scores[scoreIndex].name = newName
	end
end

function exitHighscores()
	--save the scores
	if (newScore) then
		if (string.len(newName) == 0) then
			newName = config.highscores.defaultName
		end
		highscores.scores[scoreIndex].name = newName
		love.filesystem.write(config.highscores.fileName, JSON:encode_pretty(highscores))
		love.keyboard.setTextInput(false)
	end
	--return to menu
	score = 0
	gameStateIndex = 2
	currentState = gameStates[gameStateIndex]
	currentState:load()
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
	topHighScore = topHighScore,
	setHighScore = setHighScore,
	textinput = textinput,
	touchpressed = touchpressed,
	touchreleased = touchreleased,
	touchmoved = touchmoved
}
