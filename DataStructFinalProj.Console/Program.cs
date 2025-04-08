using DataStructFinalProj.Logic;

var game = new GameRunner();

game.inventory.DisplayInventory();
game.PressAnyKeyToContinue();
game.inventory.ProcessNextItem();
game.PressAnyKeyToContinue();
game.inventory.DisplayInventory();
game.PressAnyKeyToContinue();