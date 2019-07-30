'use strict';

// Example from https://github.com/plasticrake/tplink-smarthome-simulator/blob/master/examples/single-device.js
const Device = require('tplink-smarthome-simulator').Device;
const UdpServer = require('tplink-smarthome-simulator').UdpServer;

// Will generate random mac (used by Kasa app to uniqutely identify a device) and deviceId, etc
// add 100ms responseDelay to simulate network latency
// I have HS100 attached to PiLights, also have some HS105 devices.
let device = new Device({ model: 'hs100', port: 9999, responseDelay: 100 });
device.start();

// This uses a hardcoded mac and deviceId.
// devices.push(new Device({ model: 'hs100', data: { mac: '50:c7:bf:8f:58:18', deviceId: '12345' } }));
UdpServer.start();