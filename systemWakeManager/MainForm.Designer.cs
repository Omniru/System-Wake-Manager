/*
 * Created by SharpDevelop.
 * User: user
 * Date: 3/7/2016
 * Time: 1:58 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace systemWakeManager
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TextBox cmdResults;
		private System.Windows.Forms.Button btnDisableButton;
        private System.Windows.Forms.Button btnDisableAllButton;
		private System.Windows.Forms.ComboBox alterWakeDeviceListCoB;
		private System.Windows.Forms.Button btnAllWakeDevices;
		private System.Windows.Forms.Button btnArmedWakeDevices;
		private System.Windows.Forms.Button btnLastUsedWakeDevice;
		private System.Windows.Forms.Button btnEnableWakeDevice;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusBarPanel statusPanel;
        private System.Windows.Forms.StatusBar mainStatusBar;

        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.cmdResults = new System.Windows.Forms.TextBox();
			this.btnDisableButton = new System.Windows.Forms.Button();
            this.btnDisableAllButton = new System.Windows.Forms.Button();
			this.alterWakeDeviceListCoB = new System.Windows.Forms.ComboBox();
			this.btnAllWakeDevices = new System.Windows.Forms.Button();
			this.btnArmedWakeDevices = new System.Windows.Forms.Button();
			this.btnLastUsedWakeDevice = new System.Windows.Forms.Button();
			this.btnEnableWakeDevice = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// cmdResults
			// 
			this.cmdResults.Location = new System.Drawing.Point(12, 38);
			this.cmdResults.Multiline = true;
			this.cmdResults.Name = "cmdResults";
			this.cmdResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.cmdResults.Size = new System.Drawing.Size(661, 227);
			this.cmdResults.TabIndex = 0;
			// 
			// btnDisableButton
			// 
			this.btnDisableButton.Location = new System.Drawing.Point(540, 326);
			this.btnDisableButton.Name = "btnDisableButton";
			this.btnDisableButton.Size = new System.Drawing.Size(133, 23);
			this.btnDisableButton.TabIndex = 1;
			this.btnDisableButton.Text = "Disable Wake Device";
			this.btnDisableButton.UseVisualStyleBackColor = true;
			this.btnDisableButton.Click += new System.EventHandler(this.DisableButtonClick);
            // 
            // btnDisableAllButton
            // 
            this.btnDisableAllButton.Location = new System.Drawing.Point(540, 300);
            this.btnDisableAllButton.Name = "btnDisableAllButton";
            this.btnDisableAllButton.Size = new System.Drawing.Size(133, 23);
            this.btnDisableAllButton.TabIndex = 1;
            this.btnDisableAllButton.Text = "Disable All Devices";
            this.btnDisableAllButton.UseVisualStyleBackColor = true;
            this.btnDisableAllButton.Click += new System.EventHandler(this.DisableAllButtonClick);
            //
            // statusPunel
            //
            this.mainStatusBar = new System.Windows.Forms.StatusBar();
            this.statusPanel = new System.Windows.Forms.StatusBarPanel();

            // Set first panel properties and add to StatusBar  
            this.statusPanel.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.Sunken;
            this.statusPanel.Text = "Application started. No action yet.";
            this.statusPanel.ToolTipText = "Last Activity";
            this.statusPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.mainStatusBar.Panels.Add(this.statusPanel);
            this.mainStatusBar.ShowPanels = true;

            // 
            // alterWakeDeviceListCoB
            // 
            this.alterWakeDeviceListCoB.FormattingEnabled = true;
			this.alterWakeDeviceListCoB.Location = new System.Drawing.Point(12, 326);
			this.alterWakeDeviceListCoB.Name = "alterWakeDeviceListCoB";
			this.alterWakeDeviceListCoB.Size = new System.Drawing.Size(383, 21);
			this.alterWakeDeviceListCoB.Sorted = true;
			this.alterWakeDeviceListCoB.TabIndex = 2;
			this.alterWakeDeviceListCoB.SelectedIndexChanged += new System.EventHandler(this.AlterWakeDeviceListCoBSelectedIndexChanged);
			// 
			// btnAllWakeDevices
			// 
			this.btnAllWakeDevices.Location = new System.Drawing.Point(12, 271);
			this.btnAllWakeDevices.Name = "btnAllWakeDevices";
			this.btnAllWakeDevices.Size = new System.Drawing.Size(108, 23);
			this.btnAllWakeDevices.TabIndex = 3;
			this.btnAllWakeDevices.Text = "All Wake Devices";
			this.btnAllWakeDevices.UseVisualStyleBackColor = true;
			this.btnAllWakeDevices.Click += new System.EventHandler(this.BtnAllWakeDevicesClick);
			// 
			// btnArmedWakeDevices
			// 
			this.btnArmedWakeDevices.Location = new System.Drawing.Point(126, 271);
			this.btnArmedWakeDevices.Name = "btnArmedWakeDevices";
			this.btnArmedWakeDevices.Size = new System.Drawing.Size(125, 23);
			this.btnArmedWakeDevices.TabIndex = 4;
			this.btnArmedWakeDevices.Text = "Armed Wake Devices";
			this.btnArmedWakeDevices.UseVisualStyleBackColor = true;
			this.btnArmedWakeDevices.Click += new System.EventHandler(this.BtnArmedWakeDevicesClick);
			// 
			// btnLastUsedWakeDevice
			// 
			this.btnLastUsedWakeDevice.Location = new System.Drawing.Point(257, 271);
			this.btnLastUsedWakeDevice.Name = "btnLastUsedWakeDevice";
			this.btnLastUsedWakeDevice.Size = new System.Drawing.Size(138, 23);
			this.btnLastUsedWakeDevice.TabIndex = 5;
			this.btnLastUsedWakeDevice.Text = "Last Used Wake Device";
			this.btnLastUsedWakeDevice.UseVisualStyleBackColor = true;
			this.btnLastUsedWakeDevice.Click += new System.EventHandler(this.BtnLastUsedWakeDeviceClick);
			// 
			// btnEnableWakeDevice
			// 
			this.btnEnableWakeDevice.Location = new System.Drawing.Point(401, 326);
			this.btnEnableWakeDevice.Name = "btnEnableWakeDevice";
			this.btnEnableWakeDevice.Size = new System.Drawing.Size(133, 23);
			this.btnEnableWakeDevice.TabIndex = 6;
			this.btnEnableWakeDevice.Text = "Enable Wake Device";
			this.btnEnableWakeDevice.UseVisualStyleBackColor = true;
			this.btnEnableWakeDevice.Click += new System.EventHandler(this.BtnEnableWakeDeviceClick);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(661, 23);
			this.label1.TabIndex = 8;
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(688, 375);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnEnableWakeDevice);
			this.Controls.Add(this.btnLastUsedWakeDevice);
			this.Controls.Add(this.btnArmedWakeDevices);
			this.Controls.Add(this.btnAllWakeDevices);
			this.Controls.Add(this.alterWakeDeviceListCoB);
			this.Controls.Add(this.btnDisableButton);
            this.Controls.Add(this.btnDisableAllButton);
			this.Controls.Add(this.cmdResults);
            this.Controls.Add(this.mainStatusBar);
			this.Name = "MainForm";
			this.Text = "System Wake Manager";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
