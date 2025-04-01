# FastTile3d
FastTile3d is not a Library but a thought. Its core is a fast method to pre-process the 3d tileset before the input of the WFC(wave function collapse) solver.

FastTile3d is aiming at simplifying the repetitive work of creating and arranging large amount of WFC asset for the game developers.
## How FastTile3d works
The logic of FastTile3d is simple. We know that tiles are cube-like and each tile has 6 faces. Each face contains a group of tiles identifying its valid pairs. However, when we apply transform to the tile(e.g. rotation or mirror), the information contained by faces also changes. For easier, we can call the faces with information as "socket". FastTile3d is just a set of rules associating transform with the tiles and sockets.

Because of the law of WFC, rotation to a cube is restricted in 90 degrees at X, Y or Z axis. The mirror axis is also restricted in X, Y or Z plane. So after any transform, the result is still a cube with 6 sockets. For this, we should have a tile system to store the sockets in fix order, and break the the "transform to the tiles" question down to the "transform to the sockets" question. When we rotate or mirror a tile, the sockets shift or swap in order. At the same time, some shapes of sockets would be transformed. So we also need to record the "transform message" at the socket for differentiating its parent shape.

How many results of transform can one parent socket has in total? We know one socket has 4 rotations and 2 flips as its a square. Notice here I call it "flip" rather than "mirror", because we care about the differnence in shape but not the mirror plane. So it's easy to get the number of 8. 

But is that true? Imagine that if we have a circle in a socket. No matter how we rotate and flip, the shape remains the same. So we have to know the law behind it. I notice that shapes can be categorized by its symmetry type, and it's highly relavent to the number of sub-sockets(some out of 8 have the same shape).

And also, the number of sub-sockets is relavent to how many symmetric axis at the square's centor the shape has. By this information, we can easily define differnet type of sockets without worring introducing repeated sockets when we transform a tile.


