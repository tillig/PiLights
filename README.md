# Pi Lights

Raspberry Pi hosted website for sending commands to [rpi-ws2812-server](https://github.com/tom-2015/rpi-ws2812-server) for LED control.

This was primarily created as a personal project to get some fun WS2812B lighting strips going in my house and enable control via a web/mobile app. I'm not planning on supporting it, though you can look through it to see how I did things.

# Publish

In Visual Studio there's a folder-based publish profile so you can publish from there and it'll end up in `artifacts/app`.

Post-publish:

- `chmod 755 PiLights` to enable the executable to run.

# Run

On the RPi you can run the app and provide the listener endpoints. You must listen on `*` or ASP.NET Core defaults to only localhost. For example, this allows you to access the site at port 80 on the RPi.

`PiLights --urls http://*:80`

# Debug

If you want to debug on the RPi you can select SSH debugging from VS and enter `username@yourpiname` like `pi@raspberrypi` as the destination. You can then select your app from the list of running processes.
