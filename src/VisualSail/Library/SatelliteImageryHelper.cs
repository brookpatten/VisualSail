using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Net;

//using AmphibianSoftware.Wms;
using AmphibianSoftware.VisualSail.Data;

namespace AmphibianSoftware.VisualSail.Library
{
    public static class SatelliteImageryHelper
    {
        //private static WmsServer _server;
        //private static Dictionary<WmsBoundingBox, string> _queue;
        static SatelliteImageryHelper()
        {
            //_queue = new Dictionary<WmsBoundingBox, string>();
            //_server = new WmsServer();
            //_server.Open(new Uri("http://onearth.jpl.nasa.gov/wms.cgi"));
            //_server.OnImageReceived += new WmsServer.WmsImageDelegate(server_OnImageReceived);
            //_server.OnCapabilitiesReceived += new WmsServer.WmsCapabilitiesDelegate(server_OnCapabilitiesReceived);
            //_server.WmsProcessRequest(new WmsGetCapabilitiesRequest());
        }
        public static void Shutdown()
        {
            //_server.Close();
        }
        public static string GetSatelliteImage(double north, double south, double east, double west, int width, int height)
        {
            throw new NotImplementedException();
            if (width > 4096 || height > 4096)
            {
                if (width > height)
                {
                    double f = 4096.0 / (double)width;
                    height = (int)((double)height * f);
                    width = 4096;
                }
                else
                {
                    double f = 4096.0 / (double)height;
                    width = (int)((double)width * f);
                    height = 4096;
                }
            }
            if (width < 512)
            {
                width = 512;
            }
            if (height < 512)
            {
                height = 512;
            }

            //http://wms.jpl.nasa.gov/wms.cgi
            //http://onearth.jpl.nasa.gov/wms.cgi
            string baseUrl = "http://wms.jpl.nasa.gov/wms.cgi";
            string pRequest = "request=GetMap";
            string pLayers = "layers=modis,global_mosaic";
            string pSrs = "srs=EPSG:4326";
            string pFormat = "format=image/jpeg";
            string pStyles = "styles=";//visual
            string pWidth = string.Format("width={0}",width.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-us")));
            string pHeight = string.Format("height={0}", height.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-us")));
            string pBBox = string.Format("bbox={0},{1},{2},{3}", west.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-us")), south.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-us")), east.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-us")), north.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-us")));

            StringBuilder url = new StringBuilder();
            url.Append(baseUrl);
            url.Append("?");
            url.Append(pRequest);
            url.Append("&");
            url.Append(pLayers);
            url.Append("&");
            url.Append(pSrs);
            url.Append("&");
            url.Append(pFormat);
            url.Append("&");
            url.Append(pStyles);
            url.Append("&");
            url.Append(pWidth);
            url.Append("&");
            url.Append(pHeight);
            url.Append("&");
            url.Append(pBBox);

            WebRequest request = HttpWebRequest.Create(url.ToString());
            WebResponse response = request.GetResponse();

            if (response.ContentType == "image/jpeg")
            {
                Image i = Image.FromStream(response.GetResponseStream());
                string path = ContentHelper.DynamicContentPath + GetFileName(north, south, east, west);
                i.Save(path);
                return path;
            }
            else
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string exception = reader.ReadToEnd();
                throw new Exception(exception);
            }
        }
        //public static string GetSatelliteImage(double north,double south,double east,double west,int width,int height)
        //{
        //    if (width > 4096 || height > 4096)
        //    {
        //        if (width > height)
        //        {
        //            double f = 4096.0 / (double)width;
        //            height = (int)((double)height * f);
        //            width = 4096;
        //        }
        //        else
        //        {
        //            double f = 4096.0 / (double)height;
        //            width = (int)((double)width * f);
        //            height = 4096;
        //        }
        //    }

        //    int timeout = 15000;
        //    int time = 0;
        //    int increment=100;

        //    while (_server.Capabilities == null && time<timeout)
        //    {
        //        Thread.Sleep(increment);
        //        time = time + increment;
        //    }
        //    if (_server.Capabilities == null)
        //    {
        //        throw new Exception("Failed to retrieve WMS Server Capabilities");
        //    }

        //    //GeoRect rect = new GeoRect(new GeoRef(40.493311, -83.882651), 0.08, 0.08);
        //    WmsGetMapRequest req;
        //    req = new WmsGetMapRequest();
        //    req.Version = _server.Capabilities.Version;

        //    WmsBoundingBox rect = new WmsBoundingBox();
        //    rect.Top = north;
        //    rect.Bottom = south;
        //    rect.Left = west;
        //    rect.Right = east;

        //    req.BoundingBox = rect;
        //    string[] layerNames = { "global_mosaic"/*"global_mosaic"*//*capabilities.Layers[0].Name*/};
        //    req.Layers = layerNames;
        //    string[] layerStyleNames = { "pseudo_bright"/*server.Capabilities.Layers[0].Style.Name*/ };
        //    req.Styles = layerStyleNames;
        //    req.ImageSize = new Size(width, height);
        //    req.BackgroundColor = Color.Black;
        //    req.ImageFormat = _server.Capabilities.ImageFormats[0];
        //    _server.WmsProcessRequest(req);

        //    bool found = false;
        //    WmsBoundingBox key=null;
        //    time = 0;
        //    while (!found && time<timeout)
        //    {
        //        Thread.Sleep(increment);
        //        lock (_queue)
        //        {
        //            foreach (WmsBoundingBox box in _queue.Keys)
        //            {
        //                if (box.Left == rect.Left && box.Right == rect.Right && box.Top == rect.Top && box.Bottom == rect.Bottom)
        //                {
        //                    key = box;
        //                    found = true;
        //                    break;
        //                }
        //            }
        //        }
        //        time = time + increment;
        //    }
        //    if (key == null)
        //    {
        //        throw new Exception("Imagery Retrieval Operation Timed out");
        //    }
        //    else
        //    {
        //        string path = _queue[key];
        //        string newFileName = GetFileName(north, south, east, west);
        //        FileInfo fi = new FileInfo(path);
        //        fi.MoveTo(ContentHelper.ContentPath + newFileName);
        //        return ContentHelper.ContentPath + newFileName;
        //    }
        //}

        public static string GetFileName(double north, double south, double east, double west)
        {
            return north.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-us")) + "_" + south.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-us")) + "_" + east.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-us")) + "_" + west.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-us")) + ".jpg";
        }

        public static Image GetImageForLake(Lake lake)
        {
            Image satelite;
            if (!File.Exists(ContentHelper.DynamicContentPath + AmphibianSoftware.VisualSail.Library.SatelliteImageryHelper.GetFileName(lake.North, lake.South, lake.East, lake.West)))
            {
                string lakeFile = SatelliteImageryHelper.GetSatelliteImage(lake.North, lake.South, lake.East, lake.West, (int)lake.WidthInMeters / 50, (int)lake.HeightInMeters / 50);
                FileInfo fi = new FileInfo(lakeFile);
                fi.MoveTo(ContentHelper.DynamicContentPath + SatelliteImageryHelper.GetFileName(lake.North, lake.South, lake.East, lake.West));
                satelite = Image.FromFile(ContentHelper.DynamicContentPath + SatelliteImageryHelper.GetFileName(lake.North, lake.South, lake.East, lake.West));
            }
            else
            {
                satelite = Image.FromFile(ContentHelper.DynamicContentPath + AmphibianSoftware.VisualSail.Library.SatelliteImageryHelper.GetFileName(lake.North, lake.South, lake.East, lake.West));
            }

            return satelite;
        }

        //static void server_OnCapabilitiesReceived(WmsServer server, WmsCapabilities capabilities)
        //{
            
        //}

        //private static void server_OnImageReceived(WmsServer server, string filename, WmsBoundingBox bbox)
        //{
        //    lock(_queue)
        //    {
        //        _queue.Add(bbox,filename);
        //    }
        //}
    }
}