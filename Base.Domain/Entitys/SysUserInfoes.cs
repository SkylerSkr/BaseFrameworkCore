using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;

namespace Base.Domain.Entitys
{
    [Serializable]
    public class SysUserInfoes:EntityBase
    {
        [Key]
        public int UID { get; set; }

        public string ULoginName { get; set; }
        public string ULoginPWD { get; set; }
        public string URealName { get; set; }
    }
}
