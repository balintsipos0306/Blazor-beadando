using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels;

public class CarDto
{
    public int carId {  get; set; }
    public int branchId { get; set; } = 0;
    public int typeId { get; set; } = 0;
    public int year { get; set; } = DateTime.Now.Year;
    public int price { get; set; } = 0;
}
