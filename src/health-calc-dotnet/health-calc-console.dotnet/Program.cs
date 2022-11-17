using health_calc_pack_dotnet;
using health_calc_pack_dotnet.Models;

Console.WriteLine("Entre com sua altura e peso para calcular seu IMC!");

Console.Write("Altura: ");
var Height = Console.ReadLine();

Console.Write("Peso: ");
var Weight = Console.ReadLine();

Console.Write("Sexo: ");
var Sexo = Console.ReadLine();

IMC objIMC = new IMC();
Macronutriente objMacronutriente = new Macronutriente();

double Result = objIMC.Calc(double.Parse(Height), double.Parse(Weight));
string Category = objIMC.GetIMCCategory(Result);

Console.WriteLine("O resultado de seu IMC foi: " + Category);

var SexoEnum = (Sexo == "F") ? health_calc_pack_dotnet.Enums.SexoEnum.Feminino : health_calc_pack_dotnet.Enums.SexoEnum.Masculino;

var ResultMacronutrientes = objMacronutriente.Calc(SexoEnum,
    double.Parse(Height),
    double.Parse(Weight),
    health_calc_pack_dotnet.Enums.NivelAtividadeFisicaEnum.BastanteAtivo,
    health_calc_pack_dotnet.Enums.ObjetivoFisicoEnum.Bulking);

Console.WriteLine($"O seu consumo de macronutrientes deve ser => " +
    $"Carboidratos: {ResultMacronutrientes.Carboidratos} gramas, " +
    $"Proteinas: {ResultMacronutrientes.Proteinas} gramas, " +
    $"Gorduras: {ResultMacronutrientes.Gorduras} gramas");

Console.ReadKey();