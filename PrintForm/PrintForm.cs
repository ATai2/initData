using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using PrintForm.dal;

namespace PrintForm
{
    public partial class PrintForm : Form
    {
        public readonly DbService Helper = new DbService();
        private PrintPreviewDialog ppdPicture;
        private PrintDocument pd;

        public PrintForm()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchBill();
        }

        /// <summary>
        ///     通过读卡器，输入身份信息
        /// </summary>
        private void FillFormField()
        {
//            var sql = "select * from ";
        }

        /// <summary>
        ///     检索：当前医院，所有表的检索？
        /// </summary>
        private void SearchBill()
        {
            var sqlBuilder = new StringBuilder();

            var list = Helper.GetListFromRemoteControllTable();
            for (var i = 0; i < list.Count; i++)
            {
                sqlBuilder.Append("select '" + list[i] + "' as tableName" +
                                  ",SUNITNAME_1 as '医院名称'" +
                                  ",'是否已打印'=case isprint  when 1 then '是' when 0 then '否' end " +
                                  ",PURCHASENO_1 as '业务流水号'" +
                                  ",CLASSITEMNAME_1 as '医疗机构类型'" +
                                  ",CLASSITEMNAME_1 as '医疗机构类型'" +
                                  ",GOODSNO_1 as 'NO编号'" +
                                  ",SUSERNAME_1 as '患者姓名'" +
                                  ",SMEMO_1 as '性别'" +
                                  ",SMEMO1_1 as '医保类型'" +
                                  ",SMEMO2_1 as '社会保障号码'" +
                                  ",SUBJECTNAME_1 as '项目名称1'" +
                                  ",LIMITNUM_1 as '项目1金额'" +
                                  ",SUBJECTNAME1_1 as '项目名称2'" +
                                  ",LIMITNUM1_1 as '项目2金额'" +
                                  ",SUBJECTNAME2_1 as '项目名称3'" +
                                  ",LIMITNUM2_1 as '项目3金额'" +
                                  ",SUBJECTNAME3_1 as '项目名称4'" +
                                  ",LIMITNUM3_1 as '项目4金额'" +
                                  ",SUBJECTNAME4_1 as '项目名称5'" +
                                  ",LIMITNUM4_1 as '项目5金额'" +
                                  ",SUBJECTNAME5_1 as '项目名称6'" +
                                  ",LIMITNUM5_1 as '项目6金额'" +
                                  ",SUBJECTNAME6_1 as '项目名称7'" +
                                  ",LIMITNUM6_1 as '项目7金额'" +
                                  ",SMONEYB_1 as '合计金额大写'" +
                                  ",MEDICALCARE_1 as '现金支付'" +
                                  ",MEDICALCARE1_1 as '个人账户支付'" +
                                  ",MEDICALCARE2_1 as '医保统筹支付'" +
                                  ",MEDICALCARE3_1 as '附加支付'" +
                                  ",MEDICALCARE4_1 as '分类自负'" +
                                  ",MEDICALCARE5_1 as '自负'" +
                                  ",MEDICALCARE6_1 as '自费'" +
                                  ",MEDICALCARE7_1 as '当年账户余额'" +
                                  ",MEDICALCARE8_1 as '历年账户余额'" +
                                  ",SPURPOSE_1 as '项目编码，名称1'" +
                                  ",SPURPOSE9_1 as '规格1'" +
                                  ",NUMBER_1 as '数量1'" +
                                  ",PRICE_1 as '单价1'" +
                                  ",SMONEY_1 as '金额1'" +
                                  ",SPURPOSE1_1 as '项目编码，名称2'" +
                                  ",SPURPOSE10_1 as '规格2'" +
                                  ",NUMBER1_1 as '数量2'" +
                                  ",PRICE1_1 as '单价2'" +
                                  ",SMONEY1_1 as '金额2'" +
                                  ",SPURPOSE2_1 as '项目编码，名称3'" +
                                  ",SPURPOSE11_1 as '规格1'" +
                                  ",NUMBER2_1 as '数量1'" +
                                  ",PRICE2_1 as '单价1'" +
                                  ",SMONEY2_1 as '金额1'" +
                                  ",SPURPOSE3_1 as '项目编码，名称4'" +
                                  ",SPURPOSE12_1 as '规格1'" +
                                  ",NUMBER3_1 as '数量1'" +
                                  ",PRICE3_1 as '单价1'" +
                                  ",SMONEY3_1 as '金额1'" +
                                  ",SPURPOSE4_1 as '项目编码，名称5'" +
                                  ",SPURPOSE13_1 as '规格5'" +
                                  ",NUMBER4_1 as '数量5'" +
                                  ",PRICE4_1 as '单价5'" +
                                  ",SMONEY4_1 as '金额5'" +
                                  ",SPURPOSE5_1 as '项目编码，名称6'" +
                                  ",SPURPOSE14_1 as '规格6'" +
                                  ",NUMBER5_1 as '数量6'" +
                                  ",PRICE5_1 as '单价6'" +
                                  ",SMONEY5_1 as '金额6'" +
                                  ",SPURPOSE6_1 as '项目编码，名称7'" +
                                  ",SPURPOSE15_1 as '规格7'" +
                                  ",NUMBER6_1 as '数量7'" +
                                  ",PRICE6_1 as '单价7'" +
                                  ",SMONEY6_1 as '金额7'" +
                                  ",SPURPOSE7_1 as '项目编码，名称8'" +
                                  ",SPURPOSE16_1 as '规格8'" +
                                  ",NUMBER7_1 as '数8'" +
                                  ",PRICE7_1 as '单价8'" +
                                  ",SMONEY7_1 as '金额8'" +
                                  ",SPURPOSE8_1 as '项目编码，名称9'" +
                                  ",SPURPOSE17_1 as '规格9'" +
                                  ",NUMBER8_1 as '数9'" +
                                  ",PRICE8_1 as '单价9'" +
                                  ",SMONEY8_1 as '金额9'" +
                                  ",SPURPOSE18_1 as 'aaa'" +
                                  ",SMONEYRNAME_1 as '收款员'" +
                                  ",SBIRTHDAY_1 as '日期'" + " from ").Append(list[i])
                    .Append(" where GOODSNO_1=").Append(tbNumber.Text.Trim())
                    .Append(" and SUSERNAME_1='").Append(tbName.Text.Trim()).Append("'");
                if (i != list.Count - 1)
                    sqlBuilder.Append(" union all ");
            }

            var table = Helper.GetTable(sqlBuilder.ToString());
            dgvList.DataSource = table;
        }

        private void dgvList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var b = new SolidBrush(dgvList.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(CultureInfo.CurrentUICulture),
                dgvList.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        private void btnPrePrint_Click(object sender, EventArgs e)
        {

            var rows=dgvList.SelectedRows;
                
                  //打印预览
//             ppdPicture = new PrintPreviewDialog();
            var ppc = new PrintPreviewControl();
            pd =printDocument;

            var margins = new Margins(20, 20, 20, 20);
            pd.DefaultPageSettings.Margins = margins;
            pd.DefaultPageSettings.Landscape = true;
            pd.PrintPage += pd_PrintPage;

            ppc.Document = pd;
            //预览
            Form formPreview=new Form();
            formPreview.Controls.Add(ppc);
            formPreview.Controls[0].Dock=DockStyle.Fill;
            formPreview.ShowDialog();
            formPreview.Dispose();

//            ppdPicture.Document = pd;
//            ppdPicture.ShowDialog();

               

            //数据表更新
//            var update="update "
           
          
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            var path = @"D:\天庸公司\数字凭证(制作)\上海市财政局_非税收入_上海交通大学医学院附属新华医院_dfgdfg.tiff";
            var temp = Image.FromFile(path);

            int printWidth = pd.DefaultPageSettings.PaperSize.Width; //打印机纸张的宽度
            int printHeight = pd.DefaultPageSettings.PaperSize.Height; //打印机纸张的高度
            int x = temp.Width, y = temp.Height;

//            pd.DefaultPageSettings.PaperSize.Width=x; //打印机纸张的宽度
//            pd.DefaultPageSettings.PaperSize.Height=y; //打印机纸张的高度

            e.Graphics.DrawImage(temp, 0, 0, temp.Width, temp.Height);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            pd =printDocument;

            var margins = new Margins(20, 20, 20, 20);
            pd.DefaultPageSettings.Margins = margins;
            pd.DefaultPageSettings.Landscape = true;
            pd.PrintPage += pd_PrintPage;

            try
            {
                pd.Print();
            }
            finally
            {
                MessageBox.Show("打印失败");
            }
            
        }

        private void printDocument_EndPrint(object sender, PrintEventArgs e)
        {
            
        }
    }
}