using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.DbRelay
{
    [Table("SysMerchantUser")]
    public class SysMerchantUser : SysUser
    {
        public string MerchantId { get; set; }
        public string TppId { get; set; }
    }
}
