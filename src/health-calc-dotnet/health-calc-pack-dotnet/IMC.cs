using health_calc_pack_dotnet.Interfaces;

namespace health_calc_pack_dotnet
{
    public class IMC : IIMC
    {
        public double Calc(double Height, double Weight)
        {
            if (!IsValidData(Height, Weight))
                throw new Exception("Invalid Parameters!");

            var Result = Math.Round(Weight / (Math.Pow(Height, 2)), 2);
            return Result;

        }

        public string GetIMCCategory(double IMC)
        {
            var Result = string.Empty;

            if (IMC < 18.5)
                Result = "Abaixo do Peso";
            else if (IMC >= 18.5 && IMC < 25)
                Result = "Peso Normal";
            else if (IMC >= 25 && IMC < 30)
                Result = "Pré-Obesidade";
            else if (IMC >= 30 && IMC < 35)
                Result = "Obesidade Grau 1";
            else if (IMC >= 35 && IMC < 40)
                Result = "Obesidade Grau 2";
            else if (IMC >= 40)
                Result = "Obesidade Grau 3";

            return Result;

        }

        public bool IsValidData(double Height, double Weight)
        {
            if (Height <= 0 || Weight <= 0)
                return false;


            return true;
        }
    }
}