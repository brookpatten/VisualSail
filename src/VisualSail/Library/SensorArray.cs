using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmphibianSoftware.VisualSail.Library
{
    public static class SensorArray
    {
        private static bool _started = false;
        private static List<ISensor> _sensors;
        private static int _count = 0;

        static SensorArray()
        {
            _sensors = new List<ISensor>();
        }

        public static void AddSensor(ISensor sensor)
        {
            //if (_sensors == null)
            //{
            //    _sensors = new List<ISensor>();
            //}

            sensor.OnUpdate += new Notify(SensorArray.Update);
            _sensors.Add(sensor);

            if (_started)
            {
                _sensors[_sensors.Count - 1].Start();
            }
        }

        public static void Start()
        {
            _started = true;
            foreach (ISensor sensor in _sensors)
            {
                sensor.Start();
            }
        }

        public static void Stop()
        {
            _started = false;
            foreach (ISensor sensor in _sensors)
            {
                sensor.Stop();
            }
        }

        public static List<ISensor> Sensors
        {
            get
            {
                return _sensors;
            }
        }

        private static void Update()
        {
            _count++;
        }

        public static int ReadingCount
        {
            get
            {
                return _count;
            }
        }
    }
}
