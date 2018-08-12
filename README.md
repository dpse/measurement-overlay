# Overview

*MeasurementOverlay* is a utility that allows you to add guidelines and grids on top of other applications in order to e.g. assist in aligment or for making pixel measurements.

![Example usage](https://raw.githubusercontent.com/dpse/measurement-overlay/master/example.gif)


The code is based on [GameOverlay.Net](https://github.com/michel-pi/GameOverlay.Net).


# Hotkeys

The hotkeys below can be used while inserting guidelines or grids. Global hotkeys for entering insert mode or clearing/hiding the overlay can be set using the tray menu.

- Arrow keys/mouse: Move guidelines/grid.
- Escape/backspace/right click: Cancel.
- Return/left click: Place guidelines/grid.
- Number keys/middle click: Change color.
- Space: Change color opacity.
- Tab: Change mode between guidelines or grid.
- Add/subtract/scrollwheel: Change line pitch/width (hold Shift in grid mode to adjust width).

Holding down Shift, Alt or Control keys when moving using arrow keys or when adjusting line width will result in larger steps (e.g. 20, 10 and 5 pixels when moving).

# Dependendencies

- [SharpDX](http://sharpdx.org/) (SharpDX, SharpDX.DXGI, SharpDX.Direct2D1)
- [NHotkey](https://github.com/thomaslevesque/NHotkey)

# Licenses

- [GameOverlay.Net license](https://github.com/michel-pi/GameOverlay.Net/blob/master/LICENSE.md)
- [SharpDX license](https://github.com/sharpdx/SharpDX/blob/master/License.txt)
- [NHotkey license](https://github.com/thomaslevesque/NHotkey/blob/master/LICENSE.md)