﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Management;
using System.Windows;
using EasyVideoWin.CustomControls;
using log4net;
using Aliyun.OSS;
using EasyVideoWin.Model;

namespace EasyVideoWin.Helpers
{
	public class LogFileCollector
	{
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string _appdatafolder = "";
		private string _tempZipFileName = "";
        private string _onlyZipFileName = "";
		public static string _destFileName = "HexMeetLogs";
		public static string _sysInfoFileName = "SysInfo.txt";
		public static string _collectorFolderName = "LogCollect";
		public static string _logFilename = "HexMeet.log";
		public static string _logSubFolderName = "log";
        private static string ALI_OSS_KEY = "LTAIoTDUxeJqMI7x";
        private static string ALI_OSS_SECRET = "EjslX2fUAZ30zVcG1y3ruLUWn8Uwt2";
        private static string OSS_ENDPOINT = "oss-cn-beijing.aliyuncs.com";
        private static string OSS_BUCKET = "hexmeet-log";
        private static string OSS_UPLOAD_KEY = "hexmeethjt/win/";

        public string MailSubject { get; set; }

		public string MailContent { get; set; }

		public LogFileCollector()
		{

		}

		public LogFileCollector(string appdataFolder)
		{
			this._appdatafolder = appdataFolder;
		}
        
        private void CollectSystemInfoEx()
        {
            string fileInfoPath = Path.Combine(this._appdatafolder, _sysInfoFileName);
            // we only need collection one time.
            if (File.Exists(fileInfoPath))
                return;

            StringBuilder sb = new StringBuilder();
            CollectProcessorInfo(sb);
            CollectOSInfo(sb);
            CollectMachineInfo(sb);
            CollectLocalInfo(sb);
            CollectBIOSInfo(sb);
            sb.AppendFormat("log file path:{0} ", _collectorFolderName);
            
            using (StreamWriter sw = new StreamWriter(fileInfoPath, false))
            {
                sw.Write(sb.ToString());
            }
        }

        private void CollectProcessorInfo(StringBuilder sb)
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor"))
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        sb.AppendLine("-----------------------------------");
                        sb.AppendLine("Processor Information");
                        sb.AppendLine("-----------------------------------");
                        sb.AppendLine(string.Format("{0,-20}:{1}", "Name", queryObj["Name"]));
                        sb.AppendLine();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void CollectMachineInfo(StringBuilder sb)
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem"))
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        sb.AppendLine("-----------------------------------");
                        sb.AppendLine("Machine Infomation");
                        sb.AppendLine("-----------------------------------");
                        sb.AppendLine(string.Format("{0,-20}: {1}", "Manufacturer", queryObj["Manufacturer"]));
                        sb.AppendLine(string.Format("{0,-20}: {1}", "Model", queryObj["Model"]));
                        sb.AppendLine(string.Format("{0,-20}: {1}", "SystemType", queryObj["SystemType"]));
                        sb.AppendLine(string.Format("{0,-20}: {1}", "TotalPhysicalMemory", queryObj["TotalPhysicalMemory"]));
                        sb.AppendLine(string.Format("{0,-20}: {1}", "Domain", queryObj["Domain"]));
                        sb.AppendLine();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void CollectOSInfo(StringBuilder sb)
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        sb.AppendLine("-----------------------------------");
                        sb.AppendLine("OS Information");
                        sb.AppendLine("-----------------------------------");
                        sb.AppendLine(string.Format("{0,-20}: {1}", "Caption", queryObj["Caption"]));
                        sb.AppendLine(string.Format("{0,-20}: {1}", "Manufacturer", queryObj["Manufacturer"]));
                        sb.AppendLine(string.Format("{0,-20}: {1}", "Version", queryObj["Version"]));
                        sb.AppendLine(string.Format("{0,-20}: {1}", "BuildNumber", queryObj["BuildNumber"]));
                        sb.AppendLine(string.Format("{0,-20}: {1}", "CSDVersion", queryObj["CSDVersion"]));
                        sb.AppendLine(string.Format("{0,-20}: {1}", "OSArchitecture", queryObj["OSArchitecture"]));
                        sb.AppendLine(string.Format("{0,-20}: {1}", "WindowsDirectory", queryObj["WindowsDirectory"]));
                        sb.AppendLine(string.Format("{0,-20}: {1}", "SystemDirectory", queryObj["SystemDirectory"]));
                        sb.AppendLine(string.Format("{0,-20}: {1}", "SystemDevice", queryObj["SystemDevice"]));
                        sb.AppendLine(string.Format("{0,-20}: {1}", "BootDevice", queryObj["BootDevice"]));
                        sb.AppendLine();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void CollectLocalInfo(StringBuilder sb)
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_TimeZone"))
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        sb.AppendLine("-----------------------------------");
                        sb.AppendLine("TimeZone Information");
                        sb.AppendLine("-----------------------------------");
                        sb.AppendLine(string.Format("{0,-20}: {1}", "Caption", queryObj["Caption"]));
                        sb.AppendLine();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void CollectBIOSInfo(StringBuilder sb)
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS"))
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        sb.AppendLine("-----------------------------------");
                        sb.AppendLine("BIOS Information");
                        sb.AppendLine("-----------------------------------");
                        sb.AppendLine(String.Format("{0,-20}: {1}", "Manufacturer", queryObj["Manufacturer"]));
                        sb.AppendLine(String.Format("{0,-20}: {1}", "Name", queryObj["Name"]));
                        sb.AppendLine(String.Format("{0,-20}: {1}", "Version", queryObj["Version"]));
                        sb.AppendLine();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }


		void proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			log.Debug(e.Data);
		}

		void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			log.Error(e.Data);
		}

		private void Initialize()
		{

		}

		/// <summary>
		/// clear the appdata/Logcollect folder, and then 
		/// copy the configs, db files and logs to appdata/Logcollect folder
		/// </summary>
		private void CopyContentToCachedFolder()
		{
			string collectorFolderPath = Path.Combine(this._appdatafolder, _collectorFolderName);
			string logFolderPath = Path.Combine(this._appdatafolder, _logSubFolderName);
			if (Directory.Exists(this._appdatafolder))
			{
				if (Directory.Exists(collectorFolderPath))
				{
					Directory.Delete(collectorFolderPath, true);
				}
				Directory.CreateDirectory(collectorFolderPath);
				string[] files = Directory.GetFiles(_appdatafolder);
				foreach (string file in files)
				{
                    ////ignore the client certificate and password files
                    //if (file.EndsWith("client.p12", StringComparison.OrdinalIgnoreCase)
                    //	|| file.EndsWith("client.pwd", StringComparison.OrdinalIgnoreCase))
                    //{
                    //	continue;
                    //}
                    // easyvideo.log file may be very big, let's skip it.
                    if (file.EndsWith("easyvideo.log", StringComparison.OrdinalIgnoreCase))
                        continue;
					File.Copy(file, Path.Combine(collectorFolderPath, Path.GetFileName(file)));
				}

                //string[] dumpFiles = Directory.GetFiles(Path.Combine(this._appdatafolder, _logSubFolderName), "[HexMeet]*.???");
                //if (dumpFiles != null)
                //{
                //    foreach (string dump in dumpFiles)
                //    {
                //        File.Copy(dump, Path.Combine(collectorFolderPath, Path.GetFileName(dump)));
                //    }
                //}
                //copy config.ini to log.
                string conf_file = Utils.GetCurConfigFile(); 
                File.Copy(conf_file,Path.Combine(collectorFolderPath, Path.GetFileName(conf_file)));
            }
		}

		/// <summary>
		/// Package the appdata/logCollect foder into a zipfile
		/// </summary>
		private void PackageCachedFolderIntoZip()
		{
			this.CopyContentToCachedFolder();
			Ionic.Zip.ZipFile zipfile = new Ionic.Zip.ZipFile();
			zipfile.AddDirectory(Path.Combine(this._appdatafolder, _collectorFolderName));
            string user = Utils.GetUserName(LoginManager.Instance.ServiceType);
            if (String.IsNullOrEmpty(user))
            {
                user = "unlogined";
            }
            string savedFilename = string.Format("{0}_{1}_{2}.zip", _destFileName,
                        DateTime.Now.ToString("yyyyMMddHHmmss"), user);
			zipfile.Save(Path.Combine(this._appdatafolder, savedFilename));
			_tempZipFileName = Path.Combine(this._appdatafolder, savedFilename);
            _onlyZipFileName = savedFilename;

        }


		public bool Collect()
		{
			try
			{
                log.DebugFormat(DateTime.Now.ToString());
				ClearTheFolder();
                log.DebugFormat ("**************finished log clean: {0}", DateTime.Now);
				CollectSystemInfoEx();
                log.DebugFormat("**************finished get System info: {0}", DateTime.Now); 
                PackageCachedFolderIntoZip();
                log.DebugFormat("**************finished package log: {0}", DateTime.Now);
			}
			catch (Exception ex)
			{
                //MessageBox.Show(string.Format("Error:\n {0}", ex.Message), "Error");
                MessageBoxTip tip = new MessageBoxTip((MainWindow)Application.Current.MainWindow);
                tip.SetTitleAndMsg(LanguageUtil.Instance.GetValueByKey("PROMPT"), string.Format("Error:\n {0}", ex.Message), LanguageUtil.Instance.GetValueByKey("CONFIRM"));
                tip.ShowDialog();
                //if (!EventLog.SourceExists("HexMeetLogCollect"))
                //    EventLog.CreateEventSource("HexMeetLogCollect", "Application");

                //EventLog.WriteEntry("HexMeetLogCollect -- Collect", ex.ToString(), EventLogEntryType.Error); 

                return false;
			}
			return true;
		}

		private void ClearTheFolder()
		{
			string[] files = Directory.GetFiles(this._appdatafolder, "HexMeetLogs_*.zip", SearchOption.TopDirectoryOnly);
			if (files != null)
			{
				foreach (string f in files)
				{
					try
					{
						File.Delete(f);
					}
					catch (Exception ex)
					{
                        //MessageBox.Show(string.Format("Error:\n {0}", ex.Message), "Error");
                        MessageBoxTip tip = new MessageBoxTip((MainWindow)System.Windows.Application.Current.MainWindow);
                        tip.SetTitleAndMsg(LanguageUtil.Instance.GetValueByKey("PROMPT"), string.Format("Error:\n {0}", ex.Message), LanguageUtil.Instance.GetValueByKey("CONFIRM"));
                        tip.ShowDialog();
                        //if (!EventLog.SourceExists("HexMeetLogCollect"))
                        //    EventLog.CreateEventSource("HexMeetLogCollect", "Application");

                        //EventLog.WriteEntry("HexMeetLogCollect -- ClearFolder", ex.ToString(), EventLogEntryType.Error); 
                        //ignore the exception
                    }
				}
			}
		}

        public Dictionary<string, string> GetOSInfo()
        {
            Dictionary<string, string> map = new Dictionary<string, string>();
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        map.Add("platform", "WINDOWS");
                        map.Add("deviceSN", queryObj["SerialNumber"].ToString());
                        map.Add("deviceName", queryObj["Caption"].ToString());
                        map.Add("description", "");
                        map.Add("osVersion", queryObj["Version"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return map;
        }

		public void SaveAsTo(string destFolder)
		{
			if (!File.Exists(this._tempZipFileName) || this._tempZipFileName == string.Empty)
			{
				throw new Exception("No Logs package found!");
			}

			File.Copy(this._tempZipFileName, Path.Combine(destFolder, Path.GetFileName(this._tempZipFileName)));
		}

        public bool UploadToOss()
        {
            if (!File.Exists(this._tempZipFileName) || this._tempZipFileName == string.Empty)
            {
                log.Error("No Logs package found!");
                return false;
            }
            OssClient ossclient = new OssClient(OSS_ENDPOINT, ALI_OSS_KEY, ALI_OSS_SECRET);
            try
            {
                string key = OSS_UPLOAD_KEY + _onlyZipFileName;
                ossclient.PutObject(OSS_BUCKET, key, _tempZipFileName);
            }
            catch(Exception ex)
            {
                log.InfoFormat("Failed to upload to oss, exception:{0}", ex);
                return false;
            }

            return true;
        }
	}
}
