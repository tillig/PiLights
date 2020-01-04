# Pi Lights

Container-hosted website for sending commands to [rpi-ws2812-server](https://github.com/tom-2015/rpi-ws2812-server) for LED control.

This was primarily created as a personal project to get some fun WS2812B lighting strips going in my house and enable control via a web/mobile app. I'm not planning on supporting it, though you can look through it to see how I did things.

At some point I may switch this so it directly controls the LEDs instead of using the [rpi-ws2812-server](https://github.com/tom-2015/rpi-ws2812-server). This would allow the RPi to host just the ASP.NET Core app and would mean no need for a separate container that calls the server. The RPi I have isn't strong enough to support both the [rpi-ws2812-server](https://github.com/tom-2015/rpi-ws2812-server) and this app at the same time. Something hangs in the networking. Haven't figured out what.

# Publish

In Visual Studio there's a folder-based publish profile so you can publish from there and it'll end up in `artifacts/app`.

# Run

If you do try hosting this on the RPi, you must listen on `*` or ASP.NET Core defaults to only localhost. For example, this allows you to access the site at port 80 on the RPi.

`PiLights --urls http://*:80`

# Debug

If you want to debug on the RPi you can select SSH debugging from VS and enter `username@yourpiname` like `pi@raspberrypi` as the destination. You can then select your app from the list of running processes.

# Raspberry Pi Setup

I have [a gist](https://gist.github.com/tillig/b3dff3f37c93db73e2e721cd82c5650e) with a mostly-automated setup script for pulling down the server bits and getting things compiled. You can use that to see what prerequisites you need for build/setup.
