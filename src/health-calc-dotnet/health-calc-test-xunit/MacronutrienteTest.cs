using health_calc_pack_dotnet;
using health_calc_pack_dotnet.Enums;
using health_calc_pack_dotnet.Interfaces;
using health_calc_pack_dotnet.Models;

namespace Health_calc_test_xunit
{
    public class MacronutrienteTest
    {
        [Fact]
        public void When_RequestMacronutrientsCalcWithValidDataForCutting_ThenReturnResult()
        {
            //Arrange
            var MacronutrienteObj = new Macronutriente();
            var Sexo = SexoEnum.Masculino;
            var Height = 1.68;
            var Weight = 85;
            var NivelAtividadeFisica = NivelAtividadeFisicaEnum.Sedentario;
            var ObjetivoFisico = ObjetivoFisicoEnum.Cutting;
            var Expected = new MacronutrienteModel()
            {
                Carboidratos = 170,
                Proteinas = 170,
                Gorduras = 85
            };


            //Act
            var Result = MacronutrienteObj.Calc(
                Sexo,
                Height,
                Weight,
                NivelAtividadeFisica,
                ObjetivoFisico);

            //Assert
            Assert.Equal(Expected.Carboidratos, Result.Carboidratos);
            Assert.Equal(Expected.Proteinas, Result.Proteinas);
            Assert.Equal(Expected.Gorduras, Result.Gorduras);
        }


        [Theory]
        [InlineData(NivelAtividadeFisicaEnum.Sedentario, 340, 170, 170, SexoEnum.Masculino)]
        [InlineData(NivelAtividadeFisicaEnum.ModeradamenteAtivo, 340, 170, 170, SexoEnum.Masculino)]
        [InlineData(NivelAtividadeFisicaEnum.BastanteAtivo, 595, 170, 170, SexoEnum.Masculino)]
        [InlineData(NivelAtividadeFisicaEnum.ExtremamenteAtivo, 595, 170, 170, SexoEnum.Masculino)]
        [InlineData(NivelAtividadeFisicaEnum.BastanteAtivo, 476, 136, 136, SexoEnum.Feminino)]
        [InlineData(NivelAtividadeFisicaEnum.ExtremamenteAtivo, 476, 136, 136, SexoEnum.Feminino)]
        public void When_RequestMacronutrientsCalcWithValidDataForBuking_ThenReturnResult(
            NivelAtividadeFisicaEnum NivelAtividadeFisica,
            double Carboidratos,
            double Proteinas,
            double Gorduras,
            SexoEnum Sexo)
        {
            //Arrange
            var MacronutrienteObj = new Macronutriente();
            var Height = 1.68;
            var Weight = 85;
            var ObjetivoFisico = ObjetivoFisicoEnum.Bulking;
            var Expected = new MacronutrienteModel()
            {
                Carboidratos = (int)Carboidratos,
                Proteinas = (int)Proteinas,
                Gorduras = (int)Gorduras
            };


            //Act
            var Result = MacronutrienteObj.Calc(
                Sexo,
                Height,
                Weight,
                NivelAtividadeFisica,
                ObjetivoFisico);

            //Assert
            Assert.Equal(Expected.Carboidratos, Result.Carboidratos);
            Assert.Equal(Expected.Proteinas, Result.Proteinas);
            Assert.Equal(Expected.Gorduras, Result.Gorduras);
        }

        [Fact]
        public void When_RequestMacronutrientsCalcWithValidDataForMaintenance_ThenReturnResult()
        {
            //Arrange
            var MacronutrienteObj = new Macronutriente();
            var Sexo = SexoEnum.Masculino;
            var Height = 1.68;
            var Weight = 85;
            var NivelAtividadeFisica = NivelAtividadeFisicaEnum.Sedentario;
            var ObjetivoFisico = ObjetivoFisicoEnum.Maintenance;
            var Expected = new MacronutrienteModel()
            {
                Carboidratos = 425,
                Proteinas = 170,
                Gorduras = 85
            };


            //Act
            var Result = MacronutrienteObj.Calc(
                Sexo,
                Height,
                Weight,
                NivelAtividadeFisica,
                ObjetivoFisico);

            //Assert
            Assert.Equal(Expected.Carboidratos, Result.Carboidratos);
            Assert.Equal(Expected.Proteinas, Result.Proteinas);
            Assert.Equal(Expected.Gorduras, Result.Gorduras);
        }


        [Fact]
        public void When_RequestMacronutrientsCalcWithInValidData_ThenThrowException()
        {
            //Arrange
            var MacronutrienteObj = new Macronutriente();
            var Sexo = SexoEnum.Masculino;
            var Height = 1.68;
            var Weight = 34;
            var NivelAtividadeFisica = NivelAtividadeFisicaEnum.Sedentario;
            var ObjetivoFisico = ObjetivoFisicoEnum.Maintenance;

            //Act & Assert
            Assert.Throws<Exception>(() =>
            MacronutrienteObj.Calc(
                Sexo,
                Height,
                Weight,
                NivelAtividadeFisica,
                ObjetivoFisico));
        }


    }
}