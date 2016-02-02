using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//using Google.GData.Extensions.MediaRss;
//using Google.GData.YouTube;
//using Google.YouTube;

namespace AmphibianSoftware.VisualSail.UI
{
    public partial class VideoUploadDialog : Form
    {
        private const string PRODUCTNAME = "VisualSail";
        private const string CLIENTID = "<redacted>";
        private const string DEVELOPERKEY = "<redacted>";

        private string _fileName;

        //YouTubeService _service;

        public VideoUploadDialog(string fileName,string raceName,string lakeName,DateTime start,DateTime end)
        {
            InitializeComponent();
            _fileName = fileName;

            locationTB.Text = lakeName;
            titleTB.Text = raceName;
            descriptionTB.Text = raceName + Environment.NewLine + start.ToShortTimeString() + " to " + end.ToShortTimeString();
            keywordTB.Text = "Sailing, Racing";
            //_service = new YouTubeService(PRODUCTNAME, CLIENTID, DEVELOPERKEY);
        }

        private void uploadBTN_Click(object sender, EventArgs e)
        {
            try
            {
                statusLBL.Text = "Signing in";
                signinGB.Enabled = false;
                detailsGB.Enabled = false;
                uploadBTN.Enabled = false;

                //_service.setUserCredentials(userTB.Text, passwordTB.Text);
                //string token = _service.QueryAuthenticationToken();

                //YouTubeEntry newEntry = new YouTubeEntry();
                //newEntry.Media = new Google.GData.YouTube.MediaGroup();
                //newEntry.Media.Title = new MediaTitle(titleTB.Text);
                //newEntry.Media.Categories.Add(new MediaCategory("Sports", YouTubeNameTable.CategorySchema));
                //newEntry.Media.Keywords = new MediaKeywords(keywordTB.Text);
                //newEntry.Media.Description = new MediaDescription(descriptionTB.Text);
                //newEntry.Private = true;
                //newEntry.Media.Categories.Add(new MediaCategory("test", YouTubeNameTable.DeveloperTagSchema));
                //newEntry.setYouTubeExtension("location", locationTB.Text);
                //newEntry.MediaSource = new Google.GData.Client.MediaFileSource(_fileName, "video/mpeg");

                //_service.AsyncOperationProgress += new Google.GData.Client.AsyncOperationProgressEventHandler(ms_AsyncOperationProgress);
                //_service.AsyncOperationCompleted += new Google.GData.Client.AsyncOperationCompletedEventHandler(ms_AsyncOperationCompleted);
                //statusLBL.Text = "Uploading";
                //_service.InsertAsync(new Uri("http://uploads.gdata.youtube.com/feeds/api/users/visualsail/uploads"), newEntry, _fileName);
            }
            catch (Exception ex)
            {
                statusLBL.Text = "Ready";
                MessageBox.Show("Failed to start upload" + Environment.NewLine + ex.Message);
                signinGB.Enabled = true;
                detailsGB.Enabled = true;
                uploadBTN.Enabled = true;
            }
        }

        void ms_AsyncOperationCompleted(object sender/*, Google.GData.Client.AsyncOperationCompletedEventArgs e*/)
        {
            //if (e.Error != null)
            //{
            //    statusLBL.Text = "Error";
            //    MessageBox.Show("There was a problem uploading your video to YouTube."+Environment.NewLine+e.Error.Message);
            //}
            //else
            //{
            //    statusLBL.Text = "Finished";
            //    cancelBTN.Text = "Close";
            //    uploadPB.Value = 100;
            //}
            signinGB.Enabled = true;
            detailsGB.Enabled = true;
            uploadBTN.Enabled = true;
        }

        void ms_AsyncOperationProgress(object sender/*, Google.GData.Client.AsyncOperationProgressEventArgs e*/)
        {
            //uploadPB.Value = e.ProgressPercentage;
        }

        private void cancelBTN_Click(object sender, EventArgs e)
        {
            //_service.CancelAsync(_fileName);
            this.Close();
        }
    }
}
