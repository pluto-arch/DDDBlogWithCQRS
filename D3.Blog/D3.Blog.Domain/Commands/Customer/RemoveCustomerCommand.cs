namespace D3.Blog.Domain.Commands.Customer
{
    /// <summary>
    /// 删除customer命令
    /// </summary>
    public class RemoveCustomerCommand:CustomerCommand
    {
        public RemoveCustomerCommand(int id)
        {
            Id = id;
        }

        public override bool IsValid()
        {
            if (this.Id>int.MinValue&&this.Id<int.MaxValue)
            {
                return true;
            }
            return false;
        }
    }
}
