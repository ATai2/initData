using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PrintForm.table
{
    /// <summary>
    /// ����ί��
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public delegate int EventPagingHandler(EventPagingArg e);
    /// <summary>
    /// ��ҳ�ؼ�����
    /// </summary>
    public partial class Pager : UserControl
    {
        public Pager()
        {
            InitializeComponent();
        }
        public event EventPagingHandler EventPaging;
        /// <summary>
        /// ÿҳ��ʾ��¼��
        /// </summary>
        private int _pageSize = 20;
        /// <summary>
        /// ÿҳ��ʾ��¼��
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                GetPageCount();
            }
        }

        private int _nMax = 0;
        /// <summary>
        /// �ܼ�¼��
        /// </summary>
        public int NMax
        {
            get { return _nMax; }
            set
            {
                _nMax = value;
                GetPageCount();
            }
        }

        private int _pageCount = 0;
        /// <summary>
        /// ҳ��=�ܼ�¼��/ÿҳ��ʾ��¼��
        /// </summary>
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = value; }
        }

        private int _pageCurrent = 0;
        /// <summary>
        /// ��ǰҳ��
        /// </summary>
        public int PageCurrent
        {
            get { return _pageCurrent; }
            set { _pageCurrent = value; }
        }

        public BindingNavigator ToolBar
        {
            get { return this.bindingNavigator; }
        }

        private void GetPageCount()
        {
            if (this.NMax > 0)
            {
                this.PageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(this.NMax) / Convert.ToDouble(this.PageSize)));
            }
            else
            {
                this.PageCount = 0;
            }
        }

        /// <summary>
        /// ��ҳ�ؼ����ݰ󶨵ķ���
        /// </summary>
        public void Bind()
        {
            if (this.EventPaging != null)
            {
                this.NMax = this.EventPaging(new EventPagingArg(this.PageCurrent));
            }

            if (this.PageCurrent > this.PageCount)
            {
                this.PageCurrent = this.PageCount;
            }
            if (this.PageCount == 1)
            {
                this.PageCurrent = 1;
            }
            lblPageCount.Text = this.PageCount.ToString();
            this.lblMaxPage.Text = "��"+this.NMax.ToString()+"����¼";
            this.txtCurrentPage.Text = this.PageCurrent.ToString();

            if (this.PageCurrent == 1)
            {
                this.btnPrev.Enabled = false;
                this.btnFirst.Enabled = false;
            }
            else
            {
                btnPrev.Enabled = true;
                btnFirst.Enabled = true;
            }

            if (this.PageCurrent == this.PageCount)
            {
                this.btnLast.Enabled = false;
                this.btnNext.Enabled = false;
            }
            else
            {
                btnLast.Enabled = true;
                btnNext.Enabled = true;
            }

            if (this.NMax == 0)
            {
                btnNext.Enabled = false;
                btnLast.Enabled = false;
                btnFirst.Enabled = false;
                btnPrev.Enabled = false;
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            PageCurrent = 1;
            this.Bind();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            PageCurrent -= 1;
            if (PageCurrent <= 0)
            {
                PageCurrent = 1;
            }
            this.Bind();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.PageCurrent += 1;
            if (PageCurrent > PageCount)
            {
                PageCurrent = PageCount;
            }
            this.Bind();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            PageCurrent = PageCount;
            this.Bind();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (this.txtCurrentPage.Text != null && txtCurrentPage.Text != "")
            {
                if (Int32.TryParse(txtCurrentPage.Text, out _pageCurrent))
                {
                    this.Bind();
                }
                else
                {
//                    Common.MessageProcess.ShowError("�������ָ�ʽ����");
                }
            }
        }

        private void txtCurrentPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Bind();
            }
        }

    }
    /// <summary>
    /// �Զ����¼����ݻ���
    /// </summary>
    public class EventPagingArg : EventArgs
    {
        private int _intPageIndex;
        public EventPagingArg(int PageIndex)
        {
            _intPageIndex = PageIndex;
        }
    }
    /// <summary>
    /// ����Դ�ṩ
    /// </summary>
    public class PageData
    {
        private int _PageSize = 10;
        private bool _isQueryTotalCounts = true;//�Ƿ��ѯ�ܵļ�¼����
        /// <summary>
        /// �Ƿ��ѯ�ܵļ�¼����
        /// </summary>
        public bool IsQueryTotalCounts
        {
            get { return _isQueryTotalCounts; }
            set { _isQueryTotalCounts = value; }
        }
        /// <summary>
        /// ��ʾҳ��
        /// </summary>
        public int PageSize
        {
            get
            {
                return _PageSize;

            }
            set
            {
                _PageSize = value;
            }
        }
        /// <summary>
        /// ��ǰҳ
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// ��ҳ��
        /// </summary>
        public int PageCount { get; private set; } = 0;

        /// <summary>
        /// �ܼ�¼��
        /// </summary>
        public int TotalCount { get; private set; } = 0;

        /// <summary>
        /// ������������ͼ
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// ���ֶ�FieldStr
        /// </summary>
        public string QueryFieldName { get; set; } = "*";

        /// <summary>
        /// �����ֶ�
        /// </summary>
        public string OrderStr { get; set; } = string.Empty;

        /// <summary>
        /// ��ѯ����
        /// </summary>
        public string QueryCondition { get; set; } = string.Empty;

        /// <summary>
        /// ����
        /// </summary>
        public string PrimaryKey { get; set; } = string.Empty;

        public DataSet QueryDataTable()
        {
            SqlParameter[] parameters = {
					new SqlParameter("@Tables", SqlDbType.VarChar, 255),
				    new SqlParameter("@PrimaryKey" , SqlDbType.VarChar , 255),	
                    new SqlParameter("@Sort", SqlDbType.VarChar , 255 ),
                    new SqlParameter("@CurrentPage", SqlDbType.Int),
					new SqlParameter("@PageSize", SqlDbType.Int),									
                    new SqlParameter("@Fields", SqlDbType.VarChar, 255),
					new SqlParameter("@Filter", SqlDbType.VarChar,1000),
                    new SqlParameter("@Group" ,SqlDbType.VarChar , 1000 )
					};
            parameters[0].Value = TableName;
            parameters[1].Value = PrimaryKey;
            parameters[2].Value = OrderStr;
            parameters[3].Value = PageIndex;
            parameters[4].Value = PageSize;
            parameters[5].Value =QueryFieldName;
            parameters[6].Value = QueryCondition;
            parameters[7].Value = string.Empty;
//            DataSet ds = DbHelperSQL.RunProcedure("SP_Pagination", parameters, "dd");
            if (_isQueryTotalCounts)
            {
                TotalCount = GetTotalCount();
            }
            if (TotalCount == 0)
            {
                PageIndex = 0;
                PageCount = 0;
            }
            else
            {
                PageCount = TotalCount % _PageSize == 0 ? TotalCount / _PageSize : TotalCount / _PageSize + 1;
                if (PageIndex > PageCount)
                {
                    PageIndex = PageCount;

                    parameters[4].Value = _PageSize;

//                    ds = QueryDataTable();
                }
            }
            return null;
        }

        public int GetTotalCount()
        {
            string strSql = " select count(1) from "+TableName;
            if (QueryCondition != string.Empty)
            {
                strSql +=" where " + QueryCondition;
            }
//            return int.Parse(DbHelperSQL.GetSingle(strSql).ToString());
            return 0;
        }
    }
}
