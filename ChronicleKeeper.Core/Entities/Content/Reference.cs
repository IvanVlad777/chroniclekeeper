using ChronicleKeeper.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronicleKeeper.Core.Entities.Content
{
    public class Reference
    {
        [Key]
        public int Id { get; set; }
        //public int LoreEntityId { get; set; }
        //public LoreEntity? LoreEntity { get; set; }
    }
}
