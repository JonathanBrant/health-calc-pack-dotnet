using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace health_calc_pack_dotnet.Interfaces
{
    public interface IIMC
    {
        double Calc(double Height, double Weight);
        string GetIMCCategory(double IMC);
        bool IsValidData(double Height, double Weight);

    }
}
