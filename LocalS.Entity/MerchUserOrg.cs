using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocalS.Entity
{
    [Table("MerchUserOrg")]
    public class MerchUserOrg
    {
        public string Id { get; set; }
        [Key, Column(Order = 1)]
        public string OrgId { get; set; }
        [Key, Column(Order = 2)]
        public string UserId { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
