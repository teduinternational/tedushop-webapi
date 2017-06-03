using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeduShop.Common
{
    public class NumberHelper
    {
        public static string NumberToWords(double number)
        {
            if (number == 0)
                return "Không đồng";

            string numberToWord = "";
            string mode = "";
            string separatorRemain = "";
            double Num = Math.Round(number, 0);
            string gN = Convert.ToString(Num);
            int m = Convert.ToInt32(gN.Length / 3);
            int mod = gN.Length - m * 3;
            string mark = "[+]";

            // Dau [+ , - ]
            if (number < 0)
                mark = "[-]";
            mark = "";

            // Tach hang lon nhat
            if (mod.Equals(1))
                mode = "00" + Convert.ToString(Num.ToString().Trim().Substring(0, 1)).Trim();
            if (mod.Equals(2))
                mode = "0" + Convert.ToString(Num.ToString().Trim().Substring(0, 2)).Trim();
            if (mod.Equals(0))
                mode = "000";
            // Tach hang con lai sau mod :
            if (Num.ToString().Length > 2)
                separatorRemain = Convert.ToString(Num.ToString().Trim().Substring(mod, Num.ToString().Length - mod)).Trim();

            ///don vi hang mod
            int im = m + 1;
            if (mod > 0)
                numberToWord = Separate(mode).ToString().Trim() + " " + Unit(im.ToString().Trim());
            /// Tach 3 trong tach_conlai

            int i = m;
            int _m = m;
            int j = 1;
            string tach3 = "";
            string tach3_ = "";

            while (i > 0)
            {
                tach3 = separatorRemain.Trim().Substring(0, 3).Trim();
                tach3_ = tach3;
                numberToWord = numberToWord.Trim() + " " + Separate(tach3.Trim()).Trim();
                m = _m + 1 - j;
                if (!tach3_.Equals("000"))
                    numberToWord = numberToWord.Trim() + " " + Unit(m.ToString().Trim()).Trim();
                separatorRemain = separatorRemain.Trim().Substring(3, separatorRemain.Trim().Length - 3);

                i = i - 1;
                j = j + 1;
            }
            if (numberToWord.Trim().Substring(0, 1).Equals("k"))
                numberToWord = numberToWord.Trim().Substring(10, numberToWord.Trim().Length - 10).Trim();
            if (numberToWord.Trim().Substring(0, 1).Equals("l"))
                numberToWord = numberToWord.Trim().Substring(2, numberToWord.Trim().Length - 2).Trim();
            if (numberToWord.Trim().Length > 0)
                numberToWord = mark.Trim() + " " + numberToWord.Trim().Substring(0, 1).Trim().ToUpper() + numberToWord.Trim().Substring(1, numberToWord.Trim().Length - 1).Trim() + " đồng chẵn.";

            return numberToWord.ToString().Trim();

        }
        private static string Separate(string value)
        {
            string separator = "";
            if (value.Equals("000"))
                return "";
            if (value.Length == 3)
            {
                string hundred = value.Trim().Substring(0, 1).ToString().Trim();
                string tenth = value.Trim().Substring(1, 1).ToString().Trim();
                string unit = value.Trim().Substring(2, 1).ToString().Trim();
                if (hundred.Equals("0") && tenth.Equals("0"))
                    separator = " không trăm lẻ " + Text(unit.ToString().Trim()) + " ";
                if (!hundred.Equals("0") && tenth.Equals("0") && unit.Equals("0"))
                    separator = Text(hundred.ToString().Trim()).Trim() + " trăm ";
                if (!hundred.Equals("0") && tenth.Equals("0") && !unit.Equals("0"))
                    separator = Text(hundred.ToString().Trim()).Trim() + " trăm lẻ " + Text(unit.Trim()).Trim() + " ";
                if (hundred.Equals("0") && Convert.ToInt32(tenth) > 1 && Convert.ToInt32(unit) > 0 && !unit.Equals("5"))
                    separator = " không trăm " + Text(tenth.Trim()).Trim() + " mươi " + Text(unit.Trim()).Trim() + " ";
                if (hundred.Equals("0") && Convert.ToInt32(tenth) > 1 && unit.Equals("0"))
                    separator = " không trăm " + Text(tenth.Trim()).Trim() + " mươi ";
                if (hundred.Equals("0") && Convert.ToInt32(tenth) > 1 && unit.Equals("5"))
                    separator = " không trăm " + Text(tenth.Trim()).Trim() + " mươi lăm ";
                if (hundred.Equals("0") && tenth.Equals("1") && Convert.ToInt32(unit) > 0 && !unit.Equals("5"))
                    separator = " không trăm mười " + Text(unit.Trim()).Trim() + " ";
                if (hundred.Equals("0") && tenth.Equals("1") && unit.Equals("0"))
                    separator = " không trăm mười ";
                if (hundred.Equals("0") && tenth.Equals("1") && unit.Equals("5"))
                    separator = " không trăm mười lăm ";
                if (Convert.ToInt32(hundred) > 0 && Convert.ToInt32(tenth) > 1 && Convert.ToInt32(unit) > 0 && !unit.Equals("5"))
                    separator = Text(hundred.Trim()).Trim() + " trăm " + Text(tenth.Trim()).Trim() + " mươi " + Text(unit.Trim()).Trim() + " ";
                if (Convert.ToInt32(hundred) > 0 && Convert.ToInt32(tenth) > 1 && unit.Equals("0"))
                    separator = Text(hundred.Trim()).Trim() + " trăm " + Text(tenth.Trim()).Trim() + " mươi ";
                if (Convert.ToInt32(hundred) > 0 && Convert.ToInt32(tenth) > 1 && unit.Equals("5"))
                    separator = Text(hundred.Trim()).Trim() + " trăm " + Text(tenth.Trim()).Trim() + " mươi lăm ";
                if (Convert.ToInt32(hundred) > 0 && tenth.Equals("1") && Convert.ToInt32(unit) > 0 && !unit.Equals("5"))
                    separator = Text(hundred.Trim()).Trim() + " trăm mười " + Text(unit.Trim()).Trim() + " ";

                if (Convert.ToInt32(hundred) > 0 && tenth.Equals("1") && unit.Equals("0"))
                    separator = Text(hundred.Trim()).Trim() + " trăm mười ";
                if (Convert.ToInt32(hundred) > 0 && tenth.Equals("1") && unit.Equals("5"))
                    separator = Text(hundred.Trim()).Trim() + " trăm mười lăm ";

            }


            return separator;

        }
        private static string Unit(string number)
        {
            string unit = "";

            if (number.Equals("1"))
                unit = "";
            if (number.Equals("2"))
                unit = "nghìn";
            if (number.Equals("3"))
                unit = "triệu";
            if (number.Equals("4"))
                unit = "tỷ";
            if (number.Equals("5"))
                unit = "nghìn tỷ";
            if (number.Equals("6"))
                unit = "triệu tỷ";
            if (number.Equals("7"))
                unit = "tỷ tỷ";

            return unit;
        }
        private static string Text(string gNumber)
        {
            string result = "";
            switch (gNumber)
            {
                case "0":
                    result = "không";
                    break;
                case "1":
                    result = "một";
                    break;
                case "2":
                    result = "hai";
                    break;
                case "3":
                    result = "ba";
                    break;
                case "4":
                    result = "bốn";
                    break;
                case "5":
                    result = "năm";
                    break;
                case "6":
                    result = "sáu";
                    break;
                case "7":
                    result = "bảy";
                    break;
                case "8":
                    result = "tám";
                    break;
                case "9":
                    result = "chín";
                    break;
            }
            return result;
        }
    }
}
