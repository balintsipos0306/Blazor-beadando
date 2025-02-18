using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels;

public class BranchDto
{
    public int branchesId { get; set; }

    public string branchName { get; set; } = string.Empty;

    public string address { get; set; } = string.Empty;
}
