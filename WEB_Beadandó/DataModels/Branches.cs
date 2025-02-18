using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels;

public class Branches
{
    public int branchesId { get; set; }

    public required string branchName { get; set; }

    public required string address { get; set; }

}
