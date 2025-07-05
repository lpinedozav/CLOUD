using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLOUD.Helper
{
    public class RUTHelper
    {
        public bool IsValid { get; }
        public long Numero { get; }
        public string Verificador { get; }

        public string ConPuntosConGuion()
        {
            CultureInfo spanishFormatProvider = new CultureInfo("es-ES");
            return Numero.ToString("#,###", spanishFormatProvider) + "-" + Verificador;
        }

        public string SinPuntosConGuion => Numero + "-" + Verificador;

        public RUTHelper(string rut)
        {
            IsValid = Validar(rut.ToLower(), out int numero, out string verificador);
            if (!IsValid)
                return;
            Numero = numero;
            Verificador = verificador;
        }

        public RUTHelper(long rut)
        {
            Numero = rut;
            Verificador = CalcularDigitoVerificador(rut);
        }

        public static string CalcularDigitoVerificador(long rut)
        {
            long suma = 0;
            int multiplicador = 1;
            while (rut != 0)
            {
                multiplicador++;
                if (multiplicador == 8) multiplicador = 2;
                suma += (rut % 10) * multiplicador;
                rut = rut / 10;
            }

            suma = 11 - (suma % 11);
            if (suma == 11)
            {
                return "0";
            }

            if (suma == 10)
            {
                return "K";
            }

            return suma.ToString();
        }

        public bool Validar(string rut, out int numero, out string verificador)
        {
            bool flag = false;
            try
            {
                rut = rut.ToUpper();
                rut = rut.Replace(".", "");
                rut = rut.Replace("-", "");
                int num1 = int.Parse(rut.Substring(0, rut.Length - 1));
                numero = num1;
                char ch = char.Parse(rut.Substring(rut.Length - 1, 1));
                verificador = ch.ToString();
                int num2 = 0;
                int num3 = 1;
                for (; (uint)num1 > 0U; num1 /= 10)
                    num3 = (num3 + num1 % 10 * (9 - num2++ % 6)) % 11;
                if (ch == (num3 != 0 ? num3 + 47 : 75))
                    flag = true;
            }
            catch
            {
                numero = 0;
                verificador = "0";
            }

            return flag;
        }
    }
}