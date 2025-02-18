using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels;

public class TypeDto
{
    public int typeId { get; set; }
    public int brandId { get; set; }

    public string typeName { get; set; } = String.Empty;

}
