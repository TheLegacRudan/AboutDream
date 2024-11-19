# AboutDream

# TicTacToe .NET project

## Set up
Firstly, position yourself in the TicTacToe/TicTacToe folder.
Install dependencies and set up the database in SQL Server if needed.
Run the ```dotnet watch run --launch-profile "https"``` command.

Swagger will open up on port **7271**.
There are 8 routes:

## Auth routes - register/login
Firstly, using the register route, register at least two users by filling up the request body.
The response for each one will be a JWT token.
With that token, you can *Authorize* to use other protected routes (every route except for login and register is protected).
Just copy the token and paste it into the dialog that opens after clicking on the **Authorize** button.

## InitiateGame routes
For playing the game, you firstly need to host a game using the **create** route or **join** the first free game using the join route.
Upon clicking create game, you will get the response with initial information, but you will not be able to play until someone else joins the game.

If you selected join game and got the desired response (initial information about the game with two players listed), you may start immediately.

## Overview routes
The get **profile** route fetches information from the database about the user, such as their ID, username, games played, games won, and win percentage.

The get **games** route fetches information about all the games someone has played, such as player usernames, starting date, status (is it finished or not), result (did X or Y win, i.e., the player that played first or second), and the winner's username.
Additionally, you can filter games by time range, game status, game result, and winner username.

## Play routes
The post **move** route plays the game that has already been initiated.
In the body, you send two parameters: the row and column of your desired move.
The response will give you a game overview.
After playing the move, you must wait for your opponent to play their move.
The get **status** route will give you information about the game status so you can see if the opponent has played their move or not.

When the game is over you will either see it as a response where you will have variable gameOver set to true or, if you try to play new move, will you get the message saying game is over
