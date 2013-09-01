using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Anotar;
using Anotar.NLog;
using HidLibrary;
using NLog;
using PacketParser;
using Terminal_Interface;
using Terminal_Interface.Enums;
using Terminal_Interface.Events;

namespace Terminal
{
    public class HIDDataHandler : IDataDevice
    {
        private HidDevice _device;
        private CurrentPacketParser _parser;
        private HIDPreparser preparser;

        public HIDDataHandler(CurrentPacketParser parser)
        {
            _parser = parser;
            preparser = new HIDPreparser();
        }

        public IEnumerable<string> ListAvailableDevices()
        {
            return HidDevices.Enumerate().Select(GetFriendlyName);
        }

        private string GetFriendlyName(HidDevice device)
        {
            if (device == null)
                return "";
            return string.Format("{0}, {1}: {2}", device.Attributes.VendorHexId, device.Attributes.ProductHexId, device.Description);
        }

        public string[] Devices
        {
            get { return HidDevices.Enumerate().Select(x => x.Description).ToArray(); }
        }

        public string DeviceName
        {
            get
            {
                if (_device == null)
                    return string.Empty;

                return _device.DevicePath;
            }
            set
            {
                var usbid = __HACK__GetPIDVIDfromDeviceDescription(value);
                var devices = HidDevices.Enumerate(usbid[0], new[] {usbid[1]});
                _device = devices.FirstOrDefault(x => x.IsOpen == false);
                if (_device == null)
                    throw new Exception("Device in use"); // I lie.
            }
        }

        private int[] __HACK__GetPIDVIDfromDeviceDescription(string description)
        {
            // "0x04D8, 0xF745: HID-compliant device"
            var result = new int[2];
            result[0] = int.Parse(description.Substring(2, 4), NumberStyles.AllowHexSpecifier);
            result[1] = int.Parse(description.Substring(10, 4), NumberStyles.AllowHexSpecifier);
            return result;
        }

        public string FriendlyName
        {
            get { return string.Format("HID: {0}", GetFriendlyName(_device)); }
        }

        public string DeviceStatus
        {
            get { throw new NotImplementedException(); }
        }

        public deviceType DeviceType
        {
            get
            {
                return deviceType.HID;
            }
        }

        public PortState PortState
        {
            get
            {
                if (_device == null)
                    return PortState.Error;

                return _device.IsOpen ? PortState.Open : PortState.Closed;
            }
            set
            {
                //if (_device == null)
                //    return;
                if (_device == null)
                {
                    LogTo.Error("No device selected");
                    return;
                    throw new ArgumentException("_device should not be null");
                }

                if (value == PortState.Open)
                {
                    _device.OpenDevice();
                    _device.MonitorDeviceEvents = true;
                    _device.Removed += DeviceOnRemoved;
                    _device.Read(ReadCallback);
                }
                else
                {
                    // todo: Do this in a separate thread
                    _device.CloseDevice();
                    _device.MonitorDeviceEvents = false;
                    _device.Removed -= DeviceOnRemoved;
                }
            }
        }

        private void ReadCallback(HidDeviceData data)
        {
            LogTo.Debug("Got a ReadCallback of length {0}", data.Data.Length);

            _device.Read(ReadCallback);

            if (DataReceived == null)
                return;

            var bytes = data.Data;
            var preparsed = preparser.InterpretPacket(bytes);
            var dataString = _parser.CurrentParser.InterpretPacket(preparsed);
            var args = new DataReceivedEventArgs(dataString);
            DataReceived(this, args);
        }

        private void DeviceOnRemoved()
        {
            LogTo.Warn("Device removed unexpectedly");
        }

        // TODO Fix this to send properly formatted data
        public int Write(byte[] data)
        {
            LogTo.Trace("Writing {0} byte array", data.Length);
            byte[] continuation = null;
            if (data.Length > 60)
            {
                LogTo.Trace("Data longer than 60 bytes, setting up array for recursion");

                continuation = new byte[data.Length - 60];
                Array.Copy(data, 60, continuation, 0, data.Length - 60);

                var tempData = new byte[60];
                Array.Copy(data, tempData, 60);
                data = tempData;
            }

            var header = new byte[] { 0, 0x55, 0xB0, (byte)data.Length };
            var writeReport = _device.CreateReport();
            Array.Copy(header, writeReport.Data, header.Length);
            Array.Copy(data, 0, writeReport.Data, header.Length, data.Length);

            writeReport.Data[63] = calculateChecksum(writeReport.Data);

            if (!_device.WriteReport(writeReport))
                LogTo.Error("Failed to write data"); // TODO: User should be notified

            if (continuation != null)
                Write(continuation);

            return data.Length;
        }

        private byte calculateChecksum(byte[] message)
        {
            return message.Aggregate(message[0], (current, b) => (byte)(current ^ b));
        }

        public int Write(byte data)
        {
            return Write(new[] { data });
        }

        public int Write(string data)
        {
            return Write(Encoding.UTF8.GetBytes(data));
        }

        public int Write(char data)
        {
            return Write(new[] { (byte)data });
        }

        public event DataReceivedEventHandler DataReceived;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}