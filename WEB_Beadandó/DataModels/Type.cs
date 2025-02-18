using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels;

public class Type
{
    public int typeId {  get; set; }
    public int brandId { get; set; }

    public required string typeName { get; set; }

}
