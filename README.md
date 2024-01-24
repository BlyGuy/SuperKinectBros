# Super Kinect Bros

Super Kinect Bros is a Unity project which allows a user to play Super Mario Bros. with Kinect gestures.

It is based on the [RetroUnityFE](https://github.com/humbertodias/RetroUnityFE) project.

## Credits

* [Marc Hoek](https://github.com/BlyGuy)
* [Lisa Schouten](https://github.com/froggypompom)

## Documentation

See [wiki](https://github.com/Scorr/RetroUnity/wiki).
The keyboard controls (used for testing) have been changed however:

### Keyboard controls

* Left, Up, Down, Right = A, W, S, D
* A-button = H
* B-button = J
* Start-button = Enter

## External assets

The following assets were used and are included in this project:

* [RetroUnityFE](https://github.com/humbertodias/RetroUnityFE)
* [SK.Libretro](https://github.com/Skurdt/SK.Libretro), which is also used by RetroUnityFE
* The [mesen-emulator](https://mesen.ca/) libretro core built for windows, *mesen_libretro.dll*
* [Microsoft's Unity Pro packages](https://go.microsoft.com/fwlink/p/?LinkId=513177)

## Installation

This project is tested on Windows with a Kinect 2.0 device and requires a installed [Microsoft Kinect 2.0 SDK](https://www.microsoft.com/en-gb/download/details.aspx?id=44561) to work properly. It might also need [Microsoft's Unity Pro packages](https://go.microsoft.com/fwlink/p/?LinkId=513177) to be imported into the project. This includes the main package and the visualGestureBuilder package.

### Game Assets (roms & cores)

Game ROMs (including Super Mario Bros) need to be provided by oneself to avoid legal problems.
Put the roms inside [Assets/StreamingAssets/~libretro/roms](Assets/StreamingAssets/~libretro/roms) folder and cores inside the [Assets/StreamingAssets/~libretro/cores](Assets/StreamingAssets/~libretro/cores) folder.
Then make sure that the Libretro gameObject's script-component has the right name of the rom and core set in the Unity Editor.

#### Acquiring different cores

How to get cores on debian/ubuntu-based linux distro's:

```bash
sudo apt install libretro-snes9x libretro-snes9x-next
```

Alternatively, cores can be retrieved from [Libretro's buildbot](https://buildbot.libretro.com/nightly/windows/x86_64/latest/)


