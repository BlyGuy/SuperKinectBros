# Super Kinect Bros

Based on the [RetroUnityFE](https://github.com/humbertodias/RetroUnityFE) project

## Documentation

See [wiki](https://github.com/Scorr/RetroUnity/wiki).

## External assets

The following assets were used in this project:

* [LibRetro for Linux](http://dimitry-i.blogspot.com/2013/01/mononet-how-to-dynamically-load-native.html)

Tested on Windows

Game ROMs need to be provided by oneself.
Put the cores and roms inside [Assets/StreamingAssets](Assets/StreamingAssets) folder

How to get cores on linux:

```bash
sudo apt install libretro-snes9x libretro-snes9x-next
retroarch --libretro /usr/lib/libretro/snes9x_libretro.so Classic\ Kong\ Complete\ \(U\) \V2-01.smc 
```
