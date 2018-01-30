
sceneryIdCounter = 1

function addScenery(tab, file, radius)
	table.insert(tab, {[1] = {[1] = love.graphics.newImage(file)}, [2] = radius, [3] = sceneryIdCounter})
	sceneryIdCounter = sceneryIdCounter + 1
end


function setup()
	plants = {}
	buildings = {}
	misc = {}
	singles = {}
	
	addScenery(plants, "grass.png", 25)
	addScenery(plants, "grass2.png", 25)
	addScenery(plants, "rock1.png", 41)
	addScenery(plants, "flowers1.png", 50)
	addScenery(plants, "bush1.png", 30)
	addScenery(plants, "rock2.png", 31)
	addScenery(plants, "flowers2.png", 50)
	addScenery(plants, "tree1.png", 88)
	addScenery(plants, "tree2.png", 80)
	addScenery(plants, "grass3.png", 25)
	addScenery(plants, "cactus1.png", 35)
	addScenery(plants, "tree3.png", 110)
	
	addScenery(buildings, "house2.png", 67)
	addScenery(buildings, "house3.png", 65)
	addScenery(buildings, "house.png", 139)
	addScenery(buildings, "house4.png", 114)
	addScenery(buildings, "house5.png", 95)
	addScenery(buildings, "house6.png", 130)
	addScenery(buildings, "house7.png", 169)
	addScenery(buildings, "house8.png", 129)
	addScenery(buildings, "house9.png", 128)
	addScenery(buildings, "house10.png", 128)
	addScenery(buildings, "house11.png", 53)
	addScenery(buildings, "house12.png", 150)
	addScenery(buildings, "house13.png", 135)
	addScenery(buildings, "house14.png", 130)
	addScenery(buildings, "store1.png", 130)
	addScenery(buildings, "store2.png", 130)
	addScenery(buildings, "london.png", 150)
	addScenery(buildings, "paris.png", 150)
	addScenery(buildings, "shanghai.png", 150)
	addScenery(buildings, "newyork.png", 225)
	
	addScenery(misc, "puddle.png", 13)
	addScenery(misc, "fire.png", 15)
	addScenery(misc, "stop.png", 50)
	addScenery(misc, "swing.png", 50)
	addScenery(misc, "statue.png", 80)
	addScenery(misc, "lamp1.png", 79)
	addScenery(misc, "bench.png", 13)
	addScenery(misc, "mailbox.png", 45)
	addScenery(misc, "stoplight.png", 60)
	addScenery(misc, "car.png", 42)
	addScenery(misc, "car2.png", 42)
	addScenery(misc, "car3.png", 30)

	powerupCover = {[1] = love.graphics.newImage("box/box1.png"), [2] = love.graphics.newImage("box/box3.png"), [3] = love.graphics.newImage("box/box5.png")}
	
	powerHealth = {"powerHealth.png", 50, "health"}
	powerFreeze = {"powerFreeze.png", 50, "freeze"}
	powerShield = {"powerInv.png", 50, "shield"}
end


return {
	setup = setup
}