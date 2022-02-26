using System;
using System.Windows.Forms;

namespace VMDetector
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            using (var searcher = new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
            {
                using (var items = searcher.Get())
                {
                    foreach (var item in items)
                    {
                        string manufacturer = item["Manufacturer"].ToString().ToLower();
                        if ((manufacturer == "microsoft corporation" && item["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL"))
                            || manufacturer.Contains("vmware")
                            || item["Model"].ToString() == "VirtualBox")
                        {
                            //VM
                            MessageBox.Show("This program is a test program\n" +
                                "This program is running on VM\n" +
                                "Instagram: @cursed_dev.tiktok\n" +
                                "GitHub: megsystem", "SimpleVMDetector", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            //Real Hardware
                            MessageBox.Show("This program is a test program\n" +
                                "This program is running on Real Hardware\n" +
                                "Be careful on the internet, there are many vm detectors\n" +
                                "Instagram: @cursed_dev.tiktok\n" +
                                "GitHub: megsystem", "SimpleVMDetector", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Environment.Exit(-1);
                        }
                    }
                }
            }
        }
    }
}
