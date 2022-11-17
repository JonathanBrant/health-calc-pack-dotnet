using health_calc_pack_dotnet.Enums;
using health_calc_pack_dotnet.Interfaces;
using health_calc_pack_dotnet.Models;
using health_calc_pack_dotnet.Strategy;

namespace health_calc_pack_dotnet
{
    public class Macronutriente : IMacronutriente
    {

        const int MIN_WEIGHT = 35;


        public MacronutrienteModel Calc(
            SexoEnum Sexo,
            double Height,
            double Weight,
            NivelAtividadeFisicaEnum NivelAtividadeFisica,
            ObjetivoFisicoEnum ObjetivoFisico)
        {

            if (!IsValidData(Weight))
                throw new Exception("Invalid Parameters!");


            IMacronutrienteStrategy macronutrienteStrategy = new CuttingStrategy();

            if (ObjetivoFisico == ObjetivoFisicoEnum.Cutting)
                macronutrienteStrategy = new CuttingStrategy();
            else if (ObjetivoFisico == ObjetivoFisicoEnum.Bulking)
            {
                if (NivelAtividadeFisica == NivelAtividadeFisicaEnum.BastanteAtivo ||
                    NivelAtividadeFisica == NivelAtividadeFisicaEnum.ExtremamenteAtivo)
                    macronutrienteStrategy = new BulkingNivelAtividadeAtivoStrategy(Sexo);
                else
                    macronutrienteStrategy = new BulkingStrategy();
            }
            else if (ObjetivoFisico == ObjetivoFisicoEnum.Maintenance)
                macronutrienteStrategy = new MaintenanceStrategy();


            var context = new MacronutrienteContext(macronutrienteStrategy);

            var Result = context.Execute(Weight);

            return Result;

        }
        public bool IsValidData(double Weight)
        {
            if (Weight <= MIN_WEIGHT)
                return false;


            return true;
        }
    }
}
