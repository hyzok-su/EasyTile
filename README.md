# FastTile3d
FastTile3d is not a Library but a thought. Its core is a fast method to pre-process the 3d tileset before the input of the WFC(wave function collapse) solver.

FastTile3d is aiming at simplifying the repetitive work of creating and arranging large amount of WFC asset for the game developers.
## How FastTile3d works
The logic of FastTile3d is simple. We know that tiles are cube-like and each tile has 6 faces. Each face contains a group of tiles identifying its valid pairs. However, when we apply transform to the tile(e.g. rotation or mirror), the information contained by faces also changes. For easier, we can call the faces with information as "socket". FastTile3d is just a set of rules associating transform with the tiles and sockets.

Because of the law of WFC, transform to a cube is restricted in 90 degrees at X,Y or Z axis. So after any transform the result is still a cube with 6 sockets. For this, we should have a tile system to store the sockets in fix order, and break the the "transform to the tiles" question down to the "transform to the sockets" question.

