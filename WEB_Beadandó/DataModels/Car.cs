    using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels;

public class Car
{
    public int carId {  get; set; }

    public required string branchName { get; set; }

    public required string brandName { get; set; }
    public required string typeName { get; set; }
    public int year { get; set; }
    public required int price { get; set; }

}
