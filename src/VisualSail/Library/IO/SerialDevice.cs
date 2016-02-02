using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Data;
using System.Configuration;

using AmphibianSoftware.VisualSail;
using AmphibianSoftware.VisualSail.Library.Nmea;

namespace AmphibianSoftware.VisualSail.Library.IO
{
    public class SerialDevice
    {
        string _port;
        private SerialPort _portDevice;
        private Queue<string> _buffer;
        private Notify _notifier;

        public SerialDevice(string port)
        {
            _port = port;
        }
        public SerialDevice(string port, Notify notifier)
        {
            _port = port;
            _notifier = notifier;
        }
        public void Start()
        {
            _buffer = new Queue<string>();
            _portDevice = new SerialPort();
            _portDevice.BaudRate = 4800;
            //_portDevice.DtrEnable = true;
            _portDevice.PortName = _port;
            _portDevice.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.port_DataReceived);
            if (_portDevice.IsOpen)
            {
                _portDevice.Close();
            }
            try
            {
                _portDevice.Close();
            }
            catch { }
            _portDevice.Open();
            
        }
        public void Stop()
        {
            _portDevice.Close();
        }
        public string Port
        {
            get
            {
                return _port;
            }
        }
        public int BufferSize
        {
            get
            {
                return _buffer.Count;
            }
        }
        public Queue<string> Buffer
        {
            get
            {
                return _buffer;
            }
        }
        private void port_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            while (_portDevice.BytesToRead > 0)
            {
                string line = _portDevice.ReadLine();
                _buffer.Enqueue(line);
                if (_notifier != null)
                {
                    _notifier();
                }
            }
        }
        public string ReadLine()
        {
            return _buffer.Dequeue();
        }
    }
}
