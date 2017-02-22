using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PrintForm
{

    public partial class FormPrintResult : Form
    {
        private PrintPreviewControl ppc;
        private Image temp;
        private PrintDocument pd;

     
        public Action PreUpdateTable { get; set; }

        public FormPrintResult()
        {
            InitializeComponent();
        }

        public FormPrintResult(Image temp)
        {  this.temp = temp;
            this.Width=temp.Width;
            this.Height = temp.Height + 40;
            InitializeComponent();
          
        }

        public FormPrintResult(PrintPreviewControl ppc)
        {
            InitializeComponent();
            this.ppc = ppc;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            pd=new PrintDocument();
            var margins = new Margins(20, 20, 20, 20);
            pd.DefaultPageSettings.Margins = margins;
            pd.DefaultPageSettings.Landscape = true;
            pd.PrintPage += pd_PrintPage;

            try
            {
               
                pd.Print();
                MessageBox.Show("打印成功");
                PreUpdateTable();
            }
            catch (InvalidPrinterException)
            {
                MessageBox.Show("无效的打印机");
            }
            catch (Exception)
            {
                MessageBox.Show("打印失败");
            }
            finally
            {
                this.Dispose();
            }
        }
        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            int printWidth = pd.DefaultPageSettings.PaperSize.Width; //打印机纸张的宽度
            int printHeight = pd.DefaultPageSettings.PaperSize.Height; //打印机纸张的高度
            e.Graphics.DrawImage(temp, 0, 0, temp.Width, temp.Height);
        }
        private void FormPrintResult_Load(object sender, EventArgs e)
        {
            picBox.Image = temp;
            List<string> list = new List<string>();
            PrintDocument printDocument=new PrintDocument();
            var defaultPrinter=printDocument.PrinterSettings.PrinterName;
            list.Add(defaultPrinter);
            var printers = PrinterSettings.InstalledPrinters;
            foreach (string printer in printers)
            {
                if (!list.Contains(printer))
                {
                    list.Add(printer);
                }
            }
            list.ForEach((s)=> cbPrinter.Items.Add(s));
            cbPrinter.SelectedIndex = 0;
        }
    }
}
