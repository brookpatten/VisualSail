# VisualSail

![visual sail screenshot](https://raw.githubusercontent.com/brookpatten/VisualSail/master/visualsail.jpg)

Sailing GPS Replay Analysis

[Read more on my blog](http://blog.mrgibbs.io/visualsail-now-free/)

This is a windows application that I wrote and sold circa 2005.  
It was written before I knew what I was doing in terms of SOLID, unit testing, UI design.... I could go on.
Suffice to say, the code is not pretty.

It also used to have a web application, a licensing system, the ability to record videos, and the ability to append pictures and have them play
in a slideshow have all been removed.  It also used to have an installer etc that handled the dependencies, again, this has been removed (I'm 99% sure it wouldn't work on modern OSs anyway)

The satelite imagery no longer works.  It relied upon a nasa webservice that no longer exists.

To compile, you will need [XNA 3.0](https://www.microsoft.com/en-us/download/details.aspx?id=15300) installed.
(Sorry, I told you it was crusty)

To run, you will need the [XNA runtime](https://www.microsoft.com/en-us/download/details.aspx?id=22588) installed.
If you just want to download and run it, Install the XNA runtime (above) and then download [the zip file](https://github.com/brookpatten/VisualSail/releases/download/v1.0.1.20/1.0.1.20.zip)

For all of its flaws (and there are many), I think it is still a useful application for its purpose, and now the price is right :)

Hopefully I'll find the time to at least convert this to monogame to get rid of the xna assembly and allow it to run on linux and osx. It would be nice to fix the satelite imagery as well.  Honestly though I haven't touched it in years so no promises.  I will happily accept pull requests if you have something you would like to add.

PS: if you were one of the people that bought this back in the day, thanks for humoring me with my hobby and giving a college kid (at the time) some extra spending money.

PPS: Sorry this is just a file import rather than full source control history.  I do not have time to stand up SVN (!?) to recover the backup, and even if I could the source tree is quite bloated with all sorts of tools, resources etc that would make the git repo quite onerous.  If you have a question about something ask and I'll try to answer (but remember, I wrote this 11 years ago)
