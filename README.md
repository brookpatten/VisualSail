# VisualSail
VisualSail

![visual sail screenshot](https://raw.githubusercontent.com/brookpatten/VisualSail/master/visualsail.jpg)

Sailing GPS Replay Analysis

This is a (crusty) windows application that I wrote and sold circa 2005.  
It was written before I knew what I was doing in terms of SOLID, unit testing, UI design.... I could go on.
Suffice to say, the code is not pretty.

It also used to have a web application, a licensing system, the ability to record videos, and the ability to append pictures and have them play
in a slideshow have all been removed.  It also used to have an installer etc that handled the dependencies, again, this has been removed (I'm 99% sure it wouldn't work on modern OSs anyway)

The satelite imagery no longer works.  It relied upon a nasa webservice that no longer exists.

To compile, you will need [XNA 3.0](https://www.microsoft.com/en-us/download/details.aspx?id=15300) installed.
(Sorry, I told you it was crusty)

To run, you will need the [XNA runtime](https://www.microsoft.com/en-us/download/details.aspx?id=22588) installed.
If you just want to download and run it, Install the XNA runtime (above) and then download [the zip file](https://raw.githubusercontent.com/brookpatten/VisualSail/master/1.0.1.20.zip)

Hopefully I'll find the time to at least convert this to monogame to get rid of the xna assembly, and it would be nice to fix the satelite imagery too, but honestly I haven't touched it in years so no promises.
