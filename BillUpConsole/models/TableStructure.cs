namespace BillUpConsole.models
{
    public class TableStructure
    {
        public string Name { get; set; }

        public string SystemTypeName { get; set; }

        public int Maxlength { get; set; }

        public bool IsNullable { get; set; }

        public bool IsPk { get; set; }

        public override bool Equals(object obj)
        {
            if (obj==null)
            {
                return false;
            }
            TableStructure ts = (TableStructure) obj;
            return this.IsNullable==ts.IsNullable&&this.IsPk==ts.IsPk&&this.Maxlength==ts.Maxlength&&this.Name.Equals(ts.Name)&&this.SystemTypeName.Equals(ts.SystemTypeName);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}