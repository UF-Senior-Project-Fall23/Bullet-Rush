[ Aquanoctis IsoSlice Basic Dungeon Assets ]

+ ASSETS

A collection of basic dungeon asset sprite sheets to be used in conjunction with the 'Sprite Stacking' technique.

+ IMPORT

If you are using Game Maker Studio 2, you can drop the assets directly into the IDE and the sprite sheets will automatically be cut up into frames, ready to use.

+ IMPLEMENT

By drawing each frame of the sprite offset from the frame before it by a set increment (i.e 1 pixel) a 'fake 3D' effect can be achieved. A basic, unoptimised implemtation may looks something like this:

	draw_stack(sprite, x, y, z, rotation)

	/// @func draw_stack(sprite, x, y, z, rotation)
	/// @param sprite
	/// @param x
	/// @param y
	/// @param z
	/// @param rotation

	var _sprite = argument0;
	var _x = argument1;
	var _y = argument2;
	var _z = argument3;
	var _rot = argument4;
	var _frame_num = sprite_get_number(_sprite);

	var _ang_dcos = dcos(YOUR_CAMERA_ANGLE);
	var _ang_dsin = -dsin(YOUR_CAMERA_ANGLE);
	
	for (var _frame = 0; _frame < _frame_num; _frame += 1){
  	var _dist = _frame + _z;
   	var _lx = _dist * _ang_dcos;
  	var _ly = _dist * _ang_dsin;
  	draw_sprite_ext(_sprite, _frame, _x - _lx, _y - _ly, 1, 1, _rot, c_white, 1);
  	}

+ USAGE

The assets contained within this pack are free to use in either non-commercial or commercial projects. No credit is required but very welcomed.

+ CONTACT

Feel free to ask any questions at:
	+ https://aquanoctis.itch.io/isoslicedungeonassets

	--- Thanks!