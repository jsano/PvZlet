# PvZlet

## HOW TO WRITE LEVELS

Each wave contains rows of zombie data represented in the form:

`[Count] [ZombieID] [Lane]`

Count: How many of this spawns

ZombieID: Index of ZombieSpawner to spawn

Lane: Which lane to spawn in. 0 for (appropriately) random

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