using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.DTO.Product
{
    public class SectionDTO : INamedEntity
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Order { get; set; }
    }
}
