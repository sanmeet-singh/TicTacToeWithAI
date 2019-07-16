# TicTacToeWithAI
Tic-Tac-Toe game with simple Bot AI.

This project is compiled with Unity 2018.3.10f1

Steps to Play Game 
* Download Unity, if you don't have.
* Download/Clone the prjoect.
* Open SampleScene in Scenes folder.
* Play in Editor or build for any platform. (Tested on Windows, exe file).
 Enjoy!
 
 Game Features - 
 * User and Opponent (Bot) value (X,O) is shown on top.
 * Score is displayed above the grid.
 * Never ending game.
 * Intention 1 sec delay after user has played.
 
 Bot AI test cases covered - 
 * If total turns are less than 3 then Bot will choose random empty cell to play.
 * Bot tries to defend first, if opponent is not winning then Bot will attack.
 
 Bot AI cases not covered - 
 * even if its Bot's turn and Bot can win with current move , it will defend if row/col/diagnol has more than 1 Opponent's value (opponent will win in next move).
 
 
