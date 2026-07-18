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
		// Devices currently shown in the list, in display order.
		List<string> listedDevices = new List<string>();
		// Devices known to have wake disabled. Kept across a disable/enable action so a
		// device that just dropped out of the armed list stays visible until a refresh.
		HashSet<string> disabledDevices = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		// True when the list holds real device names (selectable), false for raw output
		// such as "Last Used Wake Device".
		bool listHoldsDevices;

		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			strikeoutFont = new Font(deviceList.Font, FontStyle.Strikeout);
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
				this.statusPanel.Text = "Error: " + e.Message;
				return("");
			}
		}

		/// <summary>
		/// Splits powercfg output into device names, dropping headers and blank lines.
		/// </summary>
		List<string> parseDevices(string result)
		{
			List<string> devices = new List<string>();

			string[] lines = result.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var element in lines) {
				string device = element.Trim();

				if (device != "" && device.Contains(@":\") == false && device.Contains("NONE") == false) {
					devices.Add(device);
				}
			}

			return devices;
		}

		/// <summary>
		/// Fills the list with the given lines. When <paramref name="areDevices"/> is false the
		/// lines are raw command output and cannot be enabled/disabled.
		/// </summary>
		void showList(List<string> lines, bool areDevices)
		{
			listedDevices = lines;
			listHoldsDevices = areDevices;

			deviceList.BeginUpdate();
			deviceList.Items.Clear();
			// Device names read better sorted; raw output must keep its original order.
			deviceList.Sorted = areDevices;
			foreach (string line in lines) {
				deviceList.Items.Add(line);
			}
			deviceList.EndUpdate();
		}

		/// <summary>
		/// Loads a device list and marks every device that is not armed as disabled, so the
		/// full list shows at a glance what is on and what is off.
		/// </summary>
		void loadDevices(string command, string title)
		{
			List<string> devices = parseDevices(runCommand(command));
			HashSet<string> armed = new HashSet<string>(
				parseDevices(runCommand("powercfg -devicequery wake_armed")), StringComparer.OrdinalIgnoreCase);

			disabledDevices.Clear();
			foreach (string device in devices) {
				if (armed.Contains(device) == false) {
					disabledDevices.Add(device);
				}
			}

			label1.Text = title;
			showList(devices, true);
		}

		/// <summary>
		/// The device selected in the list, or null when nothing usable is selected.
		/// </summary>
		string selectedDevice()
		{
			if (listHoldsDevices == false || deviceList.SelectedItem == null) {
				return null;
			}

			return deviceList.SelectedItem.ToString();
		}

		void DeviceListDrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0 || e.Index >= deviceList.Items.Count) {
				return;
			}

			string text = deviceList.Items[e.Index].ToString();
			bool disabled = listHoldsDevices && disabledDevices.Contains(text);
			bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

			e.DrawBackground();

			Font font = disabled ? strikeoutFont : e.Font;
			Color color = e.ForeColor;
			if (disabled) {
				color = selected ? Color.LightGray : Color.Gray;
			}

			using (Brush brush = new SolidBrush(color)) {
				e.Graphics.DrawString(text, font, brush, e.Bounds);
			}

			e.DrawFocusRectangle();
		}

		void DeviceListDoubleClick(object sender, EventArgs e)
		{
			string device = selectedDevice();
			if (device == null) {
				return;
			}

			if (disabledDevices.Contains(device)) {
				enableDevice(device);
			} else {
				disableDevice(device);
			}
		}

		void disableDevice(string device)
		{
			runCommand(@"powercfg -devicedisablewake """ + device + @"""");
			disabledDevices.Add(device);
			deviceList.Invalidate();
			this.statusPanel.Text = "Done: Disabled " + device;
		}

		void enableDevice(string device)
		{
			runCommand(@"powercfg -deviceenablewake """ + device + @"""");
			disabledDevices.Remove(device);
			deviceList.Invalidate();
			this.statusPanel.Text = "Done: Enabled " + device;
		}

		void DisableButtonClick(object sender, EventArgs e)
		{
			string device = selectedDevice();

			if (device != null) {
				disableDevice(device);
			} else {
				this.statusPanel.Text = "Error: No device selected. Select a device in the list";
			}
		}

		void DisableAllButtonClick(object sender, EventArgs e)
		{
			if (listHoldsDevices && listedDevices.Count > 0) {
				this.statusPanel.Text = "In progress: this process might take a minute or so";

				foreach (string device in listedDevices) {
					runCommand(@"powercfg -devicedisablewake """ + device + @"""");
					disabledDevices.Add(device);
				}

				deviceList.Invalidate();
				this.statusPanel.Text = "Done: Disabled all devices";
			} else {
				this.statusPanel.Text = "Error: No devices loaded yet. Select All Wake Devices to load";
			}
		}

		void BtnEnableWakeDeviceClick(object sender, EventArgs e)
		{
			string device = selectedDevice();

			if (device != null) {
				enableDevice(device);
			} else {
				this.statusPanel.Text = "Error: No device selected. Select a device in the list";
			}
		}

		void BtnAllWakeDevicesClick(object sender, EventArgs e)
		{
			loadDevices("powercfg -devicequery wake_from_any", "All Wake Devices");
			this.statusPanel.Text = "Loaded All Wake Devices";
		}

		void BtnArmedWakeDevicesClick(object sender, EventArgs e)
		{
			loadDevices("powercfg -devicequery wake_armed", "Armed Wake Devices");
			this.statusPanel.Text = "Loaded Armed Wake Devices";
		}

		void BtnLastUsedWakeDeviceClick(object sender, EventArgs e)
		{
			string result = runCommand("powercfg -lastwake");

			label1.Text = "Last Used Wake Devices";
			disabledDevices.Clear();
			showList(result.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList(), false);
			this.statusPanel.Text = "Loaded Last Used Wake Devices";
		}

	}
}
