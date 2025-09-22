You will be working on a game called “Alone in the Dark”. There are feasibly many stages of
building this program, however if you want to try and write it all at once, feel free.
Concept:
This is a game where you will give your player very little information about the world around
them, and it is their job to find their way around, “in the dark”, to the end of the map. In the
following examples, I will be showing you what a display might look like. Understand that in
your final product there will be no display, only text based information and commands.
So the player navigates an invisible game board, using North, East, South, West commands to
fumble about looking for a way out. If they find it, they win.
There is also a monster in the dark with them. If the player ever run into the monster, or the
monster ever runs into the player, they lose.

With respect to the monster, whenever it is:
outside of 3 boxes away from the player, it will randomly wander (in bounds…)

exactly 3 boxes away from the player, the player will get a text notification on their turn
(eg. you can smell something awful nearby)

2 boxes away or less, moves in the direction of the player.

Requirements:
a. You will use a 2d array for your gameboard.
b. Allow the player to determine how large the gameboard will be
c. Randomly assign a location for the exit, the player, and the monster
d. Player movement options will be limited to only North South East or West of 1 unit.
i. The player will never be given an option that is out of bounds.
ii. Take care to prevent possibilities of out of bounds exceptions when running
e. Create some sort of AI for the monster, so as above (this will require some sort of
distance check algorithm
i. >=3 units away from a player moves in a random valid direction
ii. 3 units away from a player informs the player there is a monster nearby
iii. 2 units or less away from a player chases the player
iv. if the monster reaches the player or player moves into the monster, notify the
user they have lost
f. If the player reaches the end, notify them they have won
g. You will have a method for displaying the game board (with some way of determining the
difference between the player, the monster, and the exit.
i. this will be called during your player turn, but commented out
ii. the purpose of this is for debugging / testing purposes
Suggestions on where to start (You can ignore these if you like):
a. Start with creating a small game board based on a variable : board_size
b. Create some concept of determining which space is the player, the exit, and in the
future, the monster (I would not worry about implementing that in the beginning)
c. Create a method that displays the gameboard as is
d. Start your player in a corner, and work on movement commands, and methods.
i. make sure that you are only giving / following choices that are intended or given
e. Implement your exit, and determine how to end the game if your player reaches the exit.
f. Create your monster, work on implementation of the monster elements
i. distance from player - how does that change it’s behavior / notifications
ii. make sure game ends if the player touches it, or it touches the player
g. randomize player location and end location
Extensions (If you finish early and are bored, in no particular order):
a. create obstacles that the player / monster may not move across on the map. This will
make it harder for a player to determine when they are in a corner or on a wall for
example.
b. create an object where if the player found it, the first time the player and monster
interacted, it would “kill” the monster, and spawn another monster in a random location
on the map
c. find a creative way to give more information to the user that may help guide them
through the game. Perhaps a darker / brighter indication. Perhaps… ?
