using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using AmphibianSoftware.VisualSail.Data;
using AmphibianSoftware.VisualSail.UI;
using AmphibianSoftware.VisualSail.Library;
using AmphibianSoftware.VisualSail.Library.Nmea;
using AmphibianSoftware.VisualSail.Library.IO;

namespace AmphibianSoftware.VisualSail.UI
{
    public delegate void UpdateTreeDelegate(List<TreeNode> nodes);
    public partial class Dashboard : Form
    {
        StreamWriter _writer;
        public Dashboard()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void sensorsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void ReceiveLine(string line)
        {
            _writer.WriteLine(line);
        }
        private void StartFile()
        {
            _writer = new StreamWriter("output");
        }
        private void StopFile()
        {
            _writer.Flush();
            _writer.Close();
        }
        private void Dashboard_Load(object sender, EventArgs e)
        {
            //SensorArray.AddSensor(new NmeaSensor("Wind 3100", "COM101"));
            //SensorArray.AddSensor(new NmeaSensor("GPS", "COM102"));
            //SensorArray.AddSensor(new NmeaSensor("Multi 3100", "COM103"));
            ISensor vs = new VirtualNmeaSensor();
            vs.OnUpdate += new Notify(this.RefreshStatistics);
            vs.OnReceive += new RawReceive(this.ReceiveLine);
            SensorArray.AddSensor(vs);
            StartFile();
            SensorArray.Start();
        }

        private void RefreshStatistics()
        {
            List<TreeNode> nodes = new List<TreeNode>();
            foreach (ISensor sensor in SensorArray.Sensors)
            {
                TreeNode sensorNode = new TreeNode(sensor.Name);
                foreach (string sensorDescription in sensor.Values.Keys)
                {
                    TreeNode sensorDescriptionNode = new TreeNode(sensorDescription);
                    foreach (string name in sensor.Values[sensorDescription].Keys)
                    {
                        TreeNode sensorReading = new TreeNode(name + " = " + sensor.Values[sensorDescription][name]);
                        sensorDescriptionNode.Nodes.Add(sensorReading);
                    }
                    sensorNode.Nodes.Add(sensorDescriptionNode);
                }
                sensorNode.ExpandAll();
                nodes.Add(sensorNode);
            }
            dashTV.Invoke(new UpdateTreeDelegate(this.UpdateTree), nodes);
        }

        private void UpdateTree(List<TreeNode> nodes)
        {
            dashTV.Nodes.Clear();
            foreach (TreeNode tn in nodes)
            {
                dashTV.Nodes.Add(tn);
            }
        }

        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            SensorArray.Stop();
            StopFile();
        }
    }
}
