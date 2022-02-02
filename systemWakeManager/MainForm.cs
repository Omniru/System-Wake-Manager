/*
 * Created by SharpDevelop.
 * User: user
 * Date: 3/7/2016
 * Time: 1:58 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;


namespace systemWakeManager
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		string itemToAlter;
        LinkedList<string> itemsToAlter = new LinkedList<string>();

		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			

		}
		
		string runCommand(string command)
		{
			try {
				
			
			// create the ProcessStartInfo using "cmd" as the program to be run,
			// and "/c " as the parameters.
			// Incidentally, /c tells cmd that we want it to execute the command that follows,
			// and then exit.
				
			//string command = "powercfg -devicequery wake_from_any";
			         
			System.Diagnostics.ProcessStartInfo procStartInfo =
				new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);
			
			// The following commands are needed to redirect the standard output.
			// This means that it will be redirected to the Process.StandardOutput StreamReader.
			procStartInfo.RedirectStandardOutput = true;
			procStartInfo.UseShellExecute = false;
			// Do not create the black window.
			procStartInfo.CreateNoWindow = true;
			// Now we create a process, assign its ProcessStartInfo and start it
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			proc.StartInfo = procStartInfo;			
			proc.StartInfo.Verb = "runas";
			
			proc.Start();
			// Get the output into a string
			string result = proc.StandardOutput.ReadToEnd();
			    
			return(result);
			
			} catch (Exception e) {				
				MessageBox.Show(e.ToString());				
				return("");
			}
		}
		
		void getWakeDevices(string command, bool justClear)
		{
			/*// Start the child process.
			 Process p = new Process();
			 // Redirect the output stream of the child process.
			 p.StartInfo.UseShellExecute = false;
			 p.StartInfo.RedirectStandardOutput = true;
			 p.StartInfo.FileName = "getAnyWakeDevice.bat";
			 p.Start();
			 // Do not wait for the child process to exit before
			 // reading to the end of its redirected stream.
			 // p.WaitForExit();
			 // Read the output stream first and then wait.
			 string output = p.StandardOutput.ReadToEnd();
			 p.WaitForExit();	
			 cmdResults.Text = output;
			 
			 string[] lines = output.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
			 
			 disableWakeDeviceListCoB.Items.Add("");
			 
			 foreach (var element in lines) {
			 	
			 	if (element != "" && element.Contains(@":\") == false)
			 	{
			 		disableWakeDeviceListCoB.Items.Add(element);
			 	}
			 }*/
			 
			string result = runCommand(command);
			 	
			try {
				
			 	cmdResults.Text = result;
			    
				//result = result.Replace("\r","");
				string[] lines = result.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			 
				//lines.Sort;
				
				alterWakeDeviceListCoB.Items.Clear();

				
				if (justClear == false) {
                    itemsToAlter.Clear();

                    alterWakeDeviceListCoB.Items.Add("");
			 
					foreach (var element in lines) {
			 	
						if (element != "" && element.Contains(@":\") == false && element.Contains("NONE") == false) {
							alterWakeDeviceListCoB.Items.Add(element);
                            itemsToAlter.AddLast(element);
						}
			    
					}
				}
			} catch (Exception objException) {
			 	MessageBox.Show(objException.ToString());
			}
			 
		}
		void AlterWakeDeviceListCoBSelectedIndexChanged(object sender, EventArgs e)
		{
			itemToAlter = alterWakeDeviceListCoB.Text;
		}
		void DisableButtonClick(object sender, EventArgs e)
		{
			if (itemToAlter != "" && itemToAlter != null) {
			    //runCommand(@"powercfg -devicedisablewake “" + itemToAlter + "”");
					
			    getWakeDevices(@"powercfg -devicedisablewake """ + itemToAlter + @"""", false);
					
                this.statusPanel.Text = "Done: Disabled " + itemToAlter;
					
			    label1.Text = "Armed Wake Devices";
			    getWakeDevices("powercfg -devicequery wake_armed", false);
            } else
            {
                this.statusPanel.Text = "Error: No device selected. Select All Wake Devices to load";
            }
        }
        void DisableAllButtonClick(object sender, EventArgs e)
        {
            if (itemsToAlter.Count() > 0)
            {
                this.statusPanel.Text = "In progress: this process might take a minute or so";
                //runCommand(@"powercfg -devicedisablewake “" + itemToAlter + "”");
                foreach (string device in itemsToAlter)
                {
                    getWakeDevices(@"powercfg -devicedisablewake """ + device + @"""", true);
                }

                this.statusPanel.Text = "Done: Disabled all devices";

                label1.Text = "Armed Wake Devices";
                getWakeDevices("powercfg -devicequery wake_armed", false);
            } else
            {
                this.statusPanel.Text = "Error: No devices loaded yet. Select All Wake Devices to load";
            }
        }
        void BtnEnableWakeDeviceClick(object sender, EventArgs e)
		{
			if (itemToAlter != "" && itemToAlter != null) {
					getWakeDevices(@"powercfg -deviceenablewake  """ + itemToAlter + @"""", false);
                    //runCommand(@"powercfg -deviceenablewake  “" + itemToAlter + "”");

                    this.statusPanel.Text = "Done: Enabled " + itemToAlter;

                    label1.Text = "Armed Wake Devices";
					getWakeDevices("powercfg -devicequery wake_armed", false);
            } else
            {
                this.statusPanel.Text = "Error: No device selected. Select All Wake Devices to load";
            }
        }

		void BtnAllWakeDevicesClick(object sender, EventArgs e)
		{
			label1.Text = "All Wake Devices";
			getWakeDevices("powercfg -devicequery wake_from_any", false);
            this.statusPanel.Text = "Loaded All Wake Devices";
        }
		void BtnArmedWakeDevicesClick(object sender, EventArgs e)
		{
			label1.Text = "Armed Wake Devices";
			getWakeDevices("powercfg -devicequery wake_armed", false);
            this.statusPanel.Text = "Loaded Armed Wake Devices";
        }
		void BtnLastUsedWakeDeviceClick(object sender, EventArgs e)
		{
			label1.Text = "Last Used Wake Devices";
			getWakeDevices("powercfg -lastwake", true);
            this.statusPanel.Text = "Loaded Last Used Wake Devices";
        }

	}
}
