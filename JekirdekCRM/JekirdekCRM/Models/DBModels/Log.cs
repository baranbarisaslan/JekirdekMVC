using System.ComponentModel.DataAnnotations;

namespace JekirdekCRM.Models.DBModels
{
    public class Log
    {
        public LogTags Tag { get; set; }

        public string Text { get; set; }
    }


    public enum LogTags
    {
        Error,
        Login,
        DatabaseAction,
        FilterAction,
    }
}
