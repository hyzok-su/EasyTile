# FastTile3d
FastTile3d is not a Library but a thought. Its core is a fast method to pre-process the 3d tileset before the input of the WFC(wave function collapse) solver.

FastTile3d is aiming at simplifying the repetitive work of creating and arranging large amount of WFC asset for the game developers.
## How FastTile3d works
The logic of FastTile3d is simple. We know that tiles are cube-like and each tile has 6 faces. Each face contains a group of tiles identifying its valid pairs. However, when we apply transform to the tile(e.g. rotation or mirror), the information contained by faces also changes. We can call the faces with information as "socket". FastTile3d is just a set of rules associating transform with sockets.
