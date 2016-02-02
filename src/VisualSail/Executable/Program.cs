using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;
using System.IO;
using System.Reflection;

using AmphibianSoftware.VisualSail.Data;
using AmphibianSoftware.VisualSail.Data.Import;
using AmphibianSoftware.VisualSail.UI;
using AmphibianSoftware.VisualSail.Library;
using AmphibianSoftware.VisualSail.Library.Nmea;
using AmphibianSoftware.VisualSail.Library.IO;
using AmphibianSoftware.VisualSail.Data.Statistics;

#if !NOLICENSE
using License;
#endif

namespace AmphibianSoftware.VisualSail.Executable
{
    static class Program
    {
        private static string key;
        private static string version;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            key = "Amphibian Software VisualSail";
            #if !NOLICENSE
            string aboutLicense;
            if (CheckLicense(version,out aboutLicense))
            #endif
            {
                #if !NOLICENSE
                    try
                    {
                        //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                        Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                        Start(args, version, aboutLicense);
                    }
                    catch (Exception e)
                    {
                        //MessageBox.Show(e.Message);
                        ReportCrash(e.Message, e.StackTrace);
                    }
                #else
                    Start(args, version,"NOLICENSE");
                #endif
            }
        }

        #if!NOLICENSE
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception)
            {
                ReportCrash(((Exception)e.ExceptionObject).Message, ((Exception)e.ExceptionObject).StackTrace);
            }
            else
            {
                ReportCrash(e.ExceptionObject.ToString(), "");
            }
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            ReportCrash(e.Exception.Message, e.Exception.StackTrace);
        }
        private static void ReportCrash(string message, string stacktrace)
        {
            try
            {
                //WebServices.CrashReportSoapClient reporter = new AmphibianSoftware.VisualSail.WebServices.CrashReportSoapClient();
                //reporter.ReportCrash(key, version, message, stacktrace);
            }
            catch(Exception e)
            { 
            }
        }
        #endif
        private static void Start(string[] args,string version,string aboutLicense)
        {
            if (args.Length > 0)
            {
                Application.Run(new SkipperMDI(args[0],version,aboutLicense));
            }
            else
            {
                Application.Run(new SkipperMDI(version, aboutLicense));
            }
        }
        #if !NOLICENSE
        private static bool CheckLicense(string version,out string aboutLicense)
        {
            string trialLicensePath = AppDomain.CurrentDomain.BaseDirectory + "trial.license";
            //string activatedLicensePath = AppDomain.CurrentDomain.BaseDirectory + "activated.license";
            
            string oldActivatedLicensePath = AppDomain.CurrentDomain.BaseDirectory + "activated.license";

            string newActivatedLicenseFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Amphibian Software\VisualSail";
            string newActivatedLicensePath = newActivatedLicenseFolder + @"\activated.license";

            if (!Directory.Exists(newActivatedLicenseFolder))
            {
                Directory.CreateDirectory(newActivatedLicenseFolder);
            }

            if (!File.Exists(newActivatedLicensePath) && File.Exists(oldActivatedLicensePath))
            {
                File.Copy(oldActivatedLicensePath, newActivatedLicensePath);
                File.Delete(oldActivatedLicensePath);
            }


            if (File.Exists(newActivatedLicensePath))
            {
                Status.LoadLicense(newActivatedLicensePath);
                if (Status.Licensed)
                {
                    aboutLicense = "Activated";
                    return true;
                }
                else
                {
                    File.Delete(newActivatedLicensePath);
                }
            }

            if (File.Exists(trialLicensePath))
            {
                Status.LoadLicense(trialLicensePath);
                if (!Status.Licensed || (Status.Licensed && Status.Evaluation_Lock_Enabled))
                {
                    LicenseForm lf = new LicenseForm(newActivatedLicensePath);
                    if (lf.ShowDialog() == DialogResult.OK)
                    {
                        if (File.Exists(newActivatedLicensePath))
                        {
                            Status.LoadLicense(newActivatedLicensePath);
                            if (Status.Licensed)
                            {
                                MessageBox.Show("Activation Complete, Thank you");
                                aboutLicense = "Activated";
                                return true;
                            }
                            else
                            {
                                MessageBox.Show("Activation Failed");
                                aboutLicense = "Invalid License Loaded";
                                return false;
                            }
                        }
                        else if (Status.Licensed && Status.Evaluation_Lock_Enabled && (Status.Evaluation_Time_Current <= Status.Evaluation_Time))
                        {
                            aboutLicense = "Evaluation Day " + Status.Evaluation_Time_Current + " of " + Status.Evaluation_Time;
                            return true;
                        }
                        else
                        {
                            aboutLicense = "License Required";
                            return false;
                        }
                    }
                    else
                    {
                        aboutLicense = "Cancelled";
                        return false;
                    }
                }
            }
            else
            {
                aboutLicense = "Trial license was removed";
                return false;
            }

            aboutLicense = "Unreachable code reached";
            return false;
        }
        #endif
    }
}