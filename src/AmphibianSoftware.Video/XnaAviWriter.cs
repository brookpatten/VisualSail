using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using Microsoft.Xna.Framework.Graphics;

namespace AmphibianSoftware.Video
{
    public class XnaAviWriter
    {
        private AviWriter _writer;
        private GraphicsDevice _device;
        private ResolveTexture2D _texture;
        private Bitmap _buffer;//textures go into the buffer first
        private Bitmap _videoFrame;//then get drawn to the videoframe
        private Bitmap _scaledFrame;
        private Graphics _videoG;
        private Graphics _scaledG;
        private BitmapData _bmpd;
        private uint _frameRate;
        private bool _closing = false;
        private bool _recording = false;
        public XnaAviWriter(GraphicsDevice device)
        {
            _writer = new AviWriter();
            _device = device;
            _texture = new ResolveTexture2D(_device, _device.PresentationParameters.BackBufferWidth, _device.PresentationParameters.BackBufferHeight, 1, SurfaceFormat.Color);
            _buffer = new Bitmap(_device.PresentationParameters.BackBufferWidth, _device.PresentationParameters.BackBufferHeight);
            _bmpd = new BitmapData();
        }
        public void VideoInitialize(string filename, uint frameRate, int videoWidth, int videoHeight)
        {
            _frameRate = frameRate;
            _videoFrame = _writer.Open(filename, frameRate, videoWidth, videoHeight);
            _scaledFrame = new Bitmap(videoWidth, videoHeight, _videoFrame.PixelFormat);
            _scaledG = Graphics.FromImage(_scaledFrame);
            _videoG = Graphics.FromImage(_videoFrame);
            _recording = true;
        }
        public bool Recording
        {
            get
            {
                return _recording && !_closing;
            }
        }
        public uint FrameRate
        {
            get
            {
                return _frameRate;
            }
        }
        public Size VideoSize
        {
            get
            {
                return new Size(_videoFrame.Width, _videoFrame.Height);
            }
        }
        public Bitmap Buffer
        {
            get
            {
                return _scaledFrame;
            }
        }
        public void CommitBufferChanges()
        {
            _scaledFrame.RotateFlip(RotateFlipType.RotateNoneFlipY);
        }
        public void RepeatBuffer()
        {
            _videoG.DrawImageUnscaled(_scaledFrame, 0, 0);
            _writer.AddFrame();
        }
        public unsafe void AddFrame()
        {
            lock (_bmpd)
            {
                lock (_texture)
                {
                    lock (_buffer)
                    {
                        lock (_writer)
                        {
                            if (!_closing)
                            {
                                _device.ResolveBackBuffer(_texture);

                                int[] d = new int[_texture.Width * _texture.Height];
                                _texture.GetData<int>(d);

                                _bmpd = _buffer.LockBits(
                                                new System.Drawing.Rectangle(0, 0, _buffer.Width, _buffer.Height),
                                                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                                                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                                Marshal.Copy(d, 0, _bmpd.Scan0, d.Length);

                                _buffer.UnlockBits(_bmpd);

                                _scaledG.DrawImageUnscaled(_buffer, 0, 0);
                                _scaledFrame.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                _videoG.DrawImageUnscaled(_scaledFrame, 0, 0);
                                _writer.AddFrame();
                            }
                        }
                    }
                }
            }
        }

        public unsafe void ScreenShot(string path,int width,int height)
        {
            lock (_bmpd)
            {
                lock (_texture)
                {
                    lock (_buffer)
                    {
                        if (!_closing)
                        {
                            _device.ResolveBackBuffer(_texture);

                            int[] d = new int[_texture.Width * _texture.Height];
                            _texture.GetData<int>(d);

                            _bmpd = _buffer.LockBits(
                                            new System.Drawing.Rectangle(0, 0, _buffer.Width, _buffer.Height),
                                            System.Drawing.Imaging.ImageLockMode.WriteOnly,
                                            System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                            Marshal.Copy(d, 0, _bmpd.Scan0, d.Length);

                            _buffer.UnlockBits(_bmpd);

                            Bitmap screenshot = new Bitmap(width, height);
                            Graphics screenshotG = Graphics.FromImage(screenshot);
                            screenshotG.DrawImageUnscaled(_buffer, 0, 0);
                            //screenshot.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            screenshot.Save(path, ImageFormat.Jpeg);
                            screenshotG.Dispose();
                            screenshot.Dispose();
                        }
                    }
                }
            }
        }
        public static Bitmap ConvertTexture2DToBitmap(Texture2D texture)
        {
            Bitmap buffer = new Bitmap(texture.Width, texture.Height);
            BitmapData data;

            int[] d = new int[texture.Width * texture.Height];
            texture.GetData<int>(d);

            data = buffer.LockBits(
                            new System.Drawing.Rectangle(0, 0, buffer.Width, buffer.Height),
                            System.Drawing.Imaging.ImageLockMode.WriteOnly,
                            System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Marshal.Copy(d, 0, data.Scan0, d.Length);

            buffer.UnlockBits(data);
            return buffer;
        }
        public void Close()
        {
            _closing = true;
            lock (_writer)
            {
                _writer.Close();
            }
            _recording = false;
        }
    }
}
