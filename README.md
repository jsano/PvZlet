# PvZlet

## HOW TO WRITE LEVELS

Each wave contains rows of zombie data represented in the form:

`[Count] [ZombieID] [Row]`

Count: How many of this spawns

ZombieID: Index of ZombieSpawner to spawn

Row: Which row to spawn in. 0 for (appropriately) random

The ONE exception is Bungee Zombie (ID: 22) which needs:

`[Count] [ZombieID] [Row] [Column]`

Each wave can also contain rows of grave data represented in the form:

`grave [Row] [Column]`

Both Row and Column are 1-indexed

Separate waves with `-`

### EXAMPLE:
```
1 0 0		<- 1 basic on any lane
-		<- End of wave
2 0 0
-
3 0 0
-
4 0 0		<- 4 basic on any lane
grave 1 9	<- 1 grave on [1,9]
-		<- End of level
```