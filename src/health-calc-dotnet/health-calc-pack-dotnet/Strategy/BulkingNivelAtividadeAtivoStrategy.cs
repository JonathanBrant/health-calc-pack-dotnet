using health_calc_pack_dotnet.Enums;
using health_calc_pack_dotnet.Interfaces;
using health_calc_pack_dotnet.Models;
using health_calc_pack_dotnet.Strategy.Base;

namespace health_calc_pack_dotnet.Strategy
{
    public class BulkingNivelAtividadeAtivoStrategy : MacronutrienteBase, IMacronutrienteStrategy
    {

        const int PROTEINA = 2;
        const int GORDURA = 2;
        const int CARBOIDRADO = 7;

        public BulkingNivelAtividadeAtivoStrategy(SexoEnum Sexo) : base(Sexo)
        {
        }

        public MacronutrienteModel Calc(double Weight)
        {

            var Result = new MacronutrienteModel()
            {
                Proteinas = PROTEINA * (int)Weight * GENDER_MULTIPLIER,
                Carboidratos = CARBOIDRADO * (int)Weight * GENDER_MULTIPLIER,
                Gorduras = GORDURA * (int)Weight * GENDER_MULTIPLIER,
            };

            return Result;
        }
    }
}
