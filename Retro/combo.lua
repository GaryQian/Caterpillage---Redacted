comboCount = 0
lastTime = -1
--nshfsdfsodifj
function update()
	if (lastTime ~= -1 and paused == false) then 
		lastTime = lastTime - dtime
		if (lastTime < 0) then
			comboCount = 0
			lastTime = -1
		end
	end
end

function increment()
	if (lastTime == -1) then
		comboCount = comboCount + 1
		lastTime = config.combo.delay
	elseif (lastTime > 0) then
		comboCount = comboCount + 1
		lastTime = config.combo.delay
	end

end

function draw()
	love.graphics.setFont(comboFont)
	if (lastTime > config.combo.delay / 2) then
		love.graphics.setColor(255, 255, 255 - 255 * (lastTime - config.combo.delay / 2) / (config.combo.delay / 2), 255)
		love.graphics.print("Combo " .. comboCount .. " x" .. ((comboCount + 10) / 10), width / 2, height - height / 12, 0, 1 + .5 * (lastTime - config.combo.delay / 2), 1 + .5 * (lastTime - config.combo.delay / 2), height / 7, height / 50)

	else
		love.graphics.print("Combo " .. comboCount .. " x" .. ((comboCount + 10) / 10), width / 2, height - height / 12, 0, 1, 1, height / 7, height / 50)
	end
	if (lastTime > 0) then
		love.graphics.rectangle("fill", width / 2 - width / 12 * (lastTime) / (config.combo.delay), height - height / 8.5, width / 6 * (lastTime) / (config.combo.delay), height / 80)
	end
	love.graphics.setColor(255, 255, 255, 255)
	love.graphics.setFont(infoFont)
end

return {
	increment = increment,
	draw = draw,
	update = update
}