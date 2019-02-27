using D3.Blog.Domain.Core.Commands;

namespace D3.Blog.Domain.Commands.PostGroup
{
    /// <summary>
    /// 添加Post分组 命令
    /// </summary>
    public class AddPostGroupCommands : PostGroupCommand
    {
        /// <summary>
        /// 构造函数初始化command
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userid"></param>
        public AddPostGroupCommands(string name,int userid)
        {
            GroupName = name;
            OwinUserId = userid;
        }

        /// <summary>
        /// 验证command是否有效
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return true;
        }
    }






    /// <summary>
    /// PostGroupCommand 基类
    /// </summary>
    public abstract class PostGroupCommand:Command
    {
        public int Id { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// group 所属人
        /// </summary>
        public int OwinUserId { get; set; }
    }
}