using DataStructFinalProj.Logic;

var game = new GameRunner();

game.inventory.DisplayInventory();
game.PressAnyKeyToContinue();
game.DisplayPlayerStats(game.player);
game.PressAnyKeyToContinue();
game.inventory.ProcessNextItem(game.player);
game.PressAnyKeyToContinue();
game.inventory.DisplayInventory();
game.PressAnyKeyToContinue();

game.DisplayPlayerStats(game.player);