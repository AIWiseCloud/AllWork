using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AllWork.Model
{
    public class BaseModel:IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
    }
}
