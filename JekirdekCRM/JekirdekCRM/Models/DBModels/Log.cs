using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace JekirdekCRM.Models.DBModels
{
    public class Log : EntityBase
    {
        [Required]
        public string Tag { get; set; }

        public string? Text { get; set; }

        [NotNull]
        [DefaultValue(0)]
        public int RelatedUserId { get; set; }
    }


    public enum LogTags
    {
        Error,
        Login,
        Exception,
        DatabaseAction,
        FilterAction,
    }
}
