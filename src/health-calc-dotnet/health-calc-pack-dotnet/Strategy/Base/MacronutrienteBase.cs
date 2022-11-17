using health_calc_pack_dotnet.Enums;

namespace health_calc_pack_dotnet.Strategy.Base
{
    public abstract class MacronutrienteBase
    {
        public double GENDER_MULTIPLIER;
        public MacronutrienteBase(SexoEnum Sexo)
        {
            GENDER_MULTIPLIER = (Sexo == SexoEnum.Feminino) ? 0.8 : 1;
        }

    }
}
