namespace BillUpConsole.controllers
{
    public interface IOperator
    {
        void InitTable();
        bool CheckNetWork();
      
        void DataTransfer();
      
        void Close();
        void CopyTableList();
    }
}