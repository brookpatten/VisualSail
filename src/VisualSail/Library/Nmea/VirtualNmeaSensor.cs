using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AmphibianSoftware.VisualSail.Library;
using AmphibianSoftware.VisualSail.Library.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace AmphibianSoftware.VisualSail.Library.Nmea
{
    public class VirtualNmeaSensor : ISensor
    {
        private Dictionary<string, Dictionary<string, string>> _currentValues;
        private Notify _update;
        private DateTime _start;
        private bool _started = false;
        private Thread _runner;

        private Vector3 _windDirection;
        //private float _windSpeed;
        private float[,] _wayPoints;
        private int _currentWayPoint;

        private Vector3 _currentPosition;
        private float _currentDirection;

        private float _wayPointTolerance = 2.0f;
        private float _turnRate = 0.2f;

        private NmeaDictionary _dictionary;
        private RawReceive _receive;

        public VirtualNmeaSensor()
        {
            _currentValues = new Dictionary<string, Dictionary<string, string>>();
            _windDirection = new Vector3(0, 0, 1);
            _wayPoints = new float[,] {{64f,91f,64f,10f,64f,91f},
                        {64f,37f,10f,64f,118f,91f}};

            _currentPosition = new Vector3(64, 0, 64);
            _currentDirection = 0f;

            _currentWayPoint = 1;

            //_windSpeed = 10.0f;

            _dictionary = new NmeaDictionary("../../nmea.xml");
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
                return "Virtual Nmea Sensor";
            }
            set
            {
            }
        }
        public void Start()
        {
            _started = true;
            _start = DateTime.Now;
            _runner = new Thread(new ThreadStart(this.Run));
            _runner.Start();
        }
        public void Stop()
        {
            _started = false;
            Thread.Sleep(2000);
            try
            {
                _runner.Abort();
            }
            catch
            {
                //TODO: do something
            }
        }
        private void Run()
        {
            while (_started)
            {
                UpdateValues();
                Thread.Sleep(50);
            }
        }
        private void UpdateValues()
        {
            if (_receive != null)
            {
                _receive("Test Line");
            }
            Dictionary<string, Dictionary<string, string>> reading = new Dictionary<string, Dictionary<string, string>>();
            Move();
            Navigate();
            Dictionary<string, string> gga = new Dictionary<string, string>();
            gga["Time"] = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(); ;
            gga["Latitude"]=_currentPosition.Z.ToString();
            gga["Latitude Direction"] = "N";
            gga["Longitude"] = _currentPosition.X.ToString();
            gga["Longitude Direction"] = "W";
            //gga["Angle"] = _currentDirection.ToString();
            reading["GGA"] = gga;
            _currentValues = reading;
            _update();
        }
        private void Move()
        {
            //find angle to current waypoint.
            float wayX = _wayPoints[0,_currentWayPoint];
            float wayZ = _wayPoints[1,_currentWayPoint];

            float curX = _currentPosition.X;
            float curZ = _currentPosition.Z;

            float dirX = wayX - curX;
            float dirZ = wayZ - curZ;

            float wayAngle = (-(float)Math.Atan2((float)dirZ, (float)dirX)); //angle to the waypoint
            wayAngle = AngleHelper.NormalizeAngle(wayAngle);

            float diff = Math.Abs(wayAngle - _currentDirection);

            if (_currentDirection < wayAngle || (_currentDirection > wayAngle + MathHelper.Pi))
            {
                //add
                if (diff < _turnRate)
                {
                    _currentDirection = _currentDirection + diff;
                }
                else
                {
                    _currentDirection = _currentDirection + _turnRate;
                }
            }
            else if (_currentDirection > wayAngle || (_currentDirection < wayAngle + MathHelper.Pi))
            {
                //subtract
                if (diff < _turnRate)
                {
                    _currentDirection = _currentDirection - diff;
                }
                else
                {
                    _currentDirection = _currentDirection - _turnRate;
                }
            }

            _currentDirection = AngleHelper.NormalizeAngle(_currentDirection);



            //move to the waypoint
            //_currentPosition.X = wayX;
            //_currentPosition.Z = wayZ;
            
            //now that we have the new angle, move the boat;
            
            
            float dx = 1.0f;
            float dz = 0f;

            curX = curX + (float)Math.Cos(_currentDirection) * dx - (float)Math.Sin(_currentDirection) * dz;
            curZ = curZ - (float)Math.Sin(_currentDirection) * dx - (float)Math.Cos(_currentDirection) * dz;

            _currentPosition.X = curX;
            _currentPosition.Z = curZ;
            
            
        }
        private void Navigate()
        {
            if (
                    MathHelper.Distance(_currentPosition.X, _wayPoints[0,_currentWayPoint]) < _wayPointTolerance && 
                    MathHelper.Distance(_currentPosition.Z, _wayPoints[1,_currentWayPoint])<_wayPointTolerance
                )
            {
                _currentWayPoint++;
            }
            if (_currentWayPoint >= _wayPoints.Length/2)
            {
                _currentWayPoint = 0;
            }
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
