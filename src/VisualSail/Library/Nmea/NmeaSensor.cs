using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmphibianSoftware.VisualSail.Library;
using AmphibianSoftware.VisualSail.Library.IO;

namespace AmphibianSoftware.VisualSail.Library.Nmea
{
    public class NmeaSensor:ISensor
    {
        private string _name;
        private SerialDevice _sd;
        private Dictionary<string, Dictionary<string, string>> _currentValues;
        private bool _enableLog = false;
        private Notify _update;
        private RawReceive _receive;
        public NmeaSensor(string name, string port)
        {
            _currentValues = new Dictionary<string, Dictionary<string, string>>();
            _name = name;
            _sd = new SerialDevice(port, new Notify(this.UpdateValues));
        }
        public Notify OnUpdate
        {
            get
            {
                return _update;
            }
            set
            {
                _update = value;
            }
        }
        public RawReceive OnReceive
        {
            get
            {
                return _receive;
            }
            set
            {
                _receive = value;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public void Start()
        {
            _sd.Start();
        }
        public void Stop()
        {
            _sd.Stop();
        }
        private void UpdateValues()
        {
            while (_sd.BufferSize > 0)
            {
                string line = _sd.ReadLine();
                if (_receive != null)
                {
                    _receive(line);
                }
                try
                {
                    Dictionary<string, Dictionary<string, string>> reading = NmeaParser.Parse(line);
                    foreach (string sentenceName in reading.Keys)
                    {
                        _currentValues[sentenceName] = reading[sentenceName];
                    }
                    if (_enableLog)
                    {
                        //write to db
                    }
                }
                catch
                {
                    //TODO:bad line, what should we do?
                }
            }
            _update();
        }
        public Dictionary<string, Dictionary<string, string>> Values
        {
            get
            {
                return _currentValues;
            }
        }
    }
}
