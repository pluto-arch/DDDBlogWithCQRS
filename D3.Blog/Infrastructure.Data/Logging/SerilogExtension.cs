using Serilog;

namespace Infrastructure.Logging
{
    /// <summary>
    /// Serilog扩展类
    /// </summary>
    public static class SerilogExtension
    {
        /*User,
       Other,
       Enviornment,
       Host*/

        public static void CustomInformation(this ILogger logger, string user = "", string other = "", string enviornment = "", string host = "", string informationMessage = "")
        {
            logger.ForContext(CustomColumn.User.ToString(), user)
                  .ForContext(CustomColumn.Other.ToString(), other)
                  .ForContext(CustomColumn.Enviornment.ToString(), enviornment)
                  .ForContext(CustomColumn.Host.ToString(), host)
                  .Information(informationMessage);
        }

        public static void CustomWarning(this ILogger logger, string user = "", string other = "", string enviornment = "", string host = "", string warningMessage = "")
        {
            logger.ForContext(CustomColumn.User.ToString(), user)
                  .ForContext(CustomColumn.Other.ToString(), other)
                  .ForContext(CustomColumn.Enviornment.ToString(), enviornment)
                  .ForContext(CustomColumn.Host.ToString(), host)
                  .Warning(warningMessage);
        }

        public static void CustomError(this ILogger logger, string user = "", string other = "", string enviornment = "", string host = "", string errorMessage = "")
        {
            logger.ForContext(CustomColumn.User.ToString(), user)
                  .ForContext(CustomColumn.Other.ToString(), other)
                  .ForContext(CustomColumn.Enviornment.ToString(), enviornment)
                  .ForContext(CustomColumn.Host.ToString(), host)
                  .Error(errorMessage);
        }

        public static void CustomFatal(this ILogger logger, string user = "", string other = "", string enviornment = "", string host = "", string fatalMessage = "")
        {
            logger.ForContext(CustomColumn.User.ToString(), user)
                  .ForContext(CustomColumn.Other.ToString(), other)
                  .ForContext(CustomColumn.Enviornment.ToString(), enviornment)
                  .ForContext(CustomColumn.Host.ToString(), host)
                  .Fatal(fatalMessage);
        }
    }
}
