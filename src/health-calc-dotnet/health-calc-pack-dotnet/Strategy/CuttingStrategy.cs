using health_calc_pack_dotnet.Enums;
using health_calc_pack_dotnet.Interfaces;
using health_calc_pack_dotnet.Models;

namespace health_calc_pack_dotnet.Strategy
{
    public class CuttingStrategy : IMacronutrienteStrategy
    {

        const int PROTEINA = 2;
        const int GORDURA = 1;
        const int CARBOIDRADO = 2;


        public MacronutrienteModel Calc(double Weight)
        {

            var Result = new MacronutrienteModel()
            {
                Proteinas = PROTEINA * (int)Weight,
                Carboidratos = CARBOIDRADO * (int)Weight,
                Gorduras = GORDURA * (int)Weight,
            };

            return Result;
        }
    }
}
