using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAO_Calculator_Library
{
    public class Calculator
    {
        public Radionuclides Calc_List = new Radionuclides();
        public Radionuclides Lib_List = new Radionuclides();

        public Calculator()
        {

        }

        public string Code
        {
            get
            {
                string code = "";
                code += Number1();
                code += Number2();
                code += Number3();
                code += Number4();
                PeriodOpasn();
                code += Number5();
                code += Number6();
                code += Number7();
                code += Number8();
                code += Number9();
                code += Number10();
                return code;
            }
        }

        public bool IFRAO()
        {
            bool flag = true;

            return flag;
        }

        public string Number1()
        {
            if(Calc_List.TWG=="t")
            {
                return "2";
            }
            if (Calc_List.TWG == "w")
            {
                return "1";
            }
            if (Calc_List.TWG == "g")
            {
                return "3";
            }
            return "*";
        }

        public int num2 = -1;
        public string Number2()
        {
            if (num2 == -1)
            {
                bool flag_trit = false;
                bool flag_beta = false;
                bool flag_alph = false;
                bool flag_uran = false;
                double sum = 0;
                foreach (var item in Calc_List)
                {
                    sum += item.UDA;
                    if (item.Name == "H-3")
                    {
                        flag_trit = true;
                    }
                    if (item.RadiationType == "b" && item.Name != "H-3")
                    {
                        flag_beta = true;
                    }
                    if (item.RadiationType == "a" && !item.AfterUran)
                    {
                        flag_alph = true;
                    }
                    if (item.AfterUran)
                    {
                        flag_uran = true;
                    }
                }
                if (Calc_List.TWG == "t")
                {
                    if (flag_trit)
                    {
                        if (sum < 10000000)
                        {
                            return "0";
                        }
                        else
                        {
                            if (sum < 100000000)
                            {
                                return "1";
                            }
                            else
                            {
                                if (sum < 100000000000)
                                {
                                    return "2";
                                }
                                else
                                {
                                    return "3";
                                }
                            }
                        }
                    }
                    if (flag_beta)
                    {
                        if (sum < 1000)
                        {
                            return "0";
                        }
                        else
                        {
                            if (sum < 10000)
                            {
                                return "1";
                            }
                            else
                            {
                                if (sum < 10000000)
                                {
                                    return "2";
                                }
                                else
                                {
                                    return "3";
                                }
                            }
                        }
                    }
                    if (flag_alph)
                    {
                        if (sum < 100)
                        {
                            return "0";
                        }
                        else
                        {
                            if (sum < 1000)
                            {
                                return "1";
                            }
                            else
                            {
                                if (sum < 1000000)
                                {
                                    return "2";
                                }
                                else
                                {
                                    return "3";
                                }
                            }
                        }
                    }
                    if (flag_uran)
                    {
                        if (sum < 10)
                        {
                            return "0";
                        }
                        else
                        {
                            if (sum < 100)
                            {
                                return "1";
                            }
                            else
                            {
                                if (sum < 100000)
                                {
                                    return "2";
                                }
                                else
                                {
                                    return "3";
                                }
                            }
                        }
                    }
                }
                if (Calc_List.TWG == "w")
                {
                    if (flag_trit)
                    {
                        if (sum < 10000)
                        {
                            return "1";
                        }
                        else
                        {
                            if (sum < 100000000)
                            {
                                return "2";
                            }
                            else
                            {
                                return "3";
                            }
                        }
                    }
                    if (flag_beta)
                    {
                        if (sum < 1000)
                        {
                            return "1";
                        }
                        else
                        {
                            if (sum < 10000000)
                            {
                                return "2";
                            }
                            else
                            {
                                return "3";
                            }
                        }
                    }
                    if (flag_alph)
                    {
                        if (sum < 100)
                        {
                            return "1";
                        }
                        else
                        {
                            if (sum < 1000000)
                            {
                                return "2";
                            }
                            else
                            {
                                return "3";
                            }
                        }
                    }
                    if (flag_uran)
                    {
                        if (sum < 10)
                        {
                            return "1";
                        }
                        else
                        {
                            if (sum < 100000)
                            {
                                return "2";
                            }
                            else
                            {
                                return "3";
                            }
                        }
                    }
                }
                if (Calc_List.TWG == "g")
                {
                    if (flag_trit)
                    {
                        if (sum < 10000)
                        {
                            return "1";
                        }
                        else
                        {
                            if (sum < 100000000)
                            {
                                return "2";
                            }
                            else
                            {
                                return "3";
                            }
                        }
                    }
                    if (flag_beta)
                    {
                        if (sum < 1000)
                        {
                            return "1";
                        }
                        else
                        {
                            if (sum < 10000000)
                            {
                                return "2";
                            }
                            else
                            {
                                return "3";
                            }
                        }
                    }
                    if (flag_alph)
                    {
                        if (sum < 100)
                        {
                            return "1";
                        }
                        else
                        {
                            if (sum < 1000000)
                            {
                                return "2";
                            }
                            else
                            {
                                return "3";
                            }
                        }
                    }
                    if (flag_uran)
                    {
                        if (sum < 10)
                        {
                            return "1";
                        }
                        else
                        {
                            if (sum < 100000)
                            {
                                return "2";
                            }
                            else
                            {
                                return "3";
                            }
                        }
                    }
                }
            }
            else
            {
                if(num2==4)
                {
                    return "4";
                }
            }

            return "*";
        }

        public string Number3()
        {
            bool flag_beta = false;
            bool flag_alph = false;
            bool flag_uran = false;

            foreach (var item in Calc_List)
            {
                if (item.RadiationType == "b")
                {
                    flag_beta = true;
                }
                if (item.RadiationType == "a")
                {
                    flag_alph = true;
                }
                if (item.AfterUran)
                {
                    flag_uran = true;
                }
            }

            if (flag_beta)
            {
                if(flag_alph)
                {
                    if(flag_uran)
                    {
                        return "6";
                    }
                    else
                    {
                        return "5";
                    }
                }
                else
                {
                    return "4";
                }
            }
            else
            {
                if (flag_alph)
                {
                    if (flag_uran)
                    {
                        return "3";
                    }
                    else
                    {
                        return "2";
                    }
                }
                else
                {
                    if (flag_uran)
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }
            }
        }

        public int num4 = -1;
        public string Number4()
        {
            if(num4!=-1)
            {
                return num4.ToString();
            }
            return "*";
        }

        void PeriodOpasn()
        {
            if (Calc_List.Count > 1)
            {
                double a_cto = 0;
                double a_pat = 0;

                Radionuclide rad1_max = new Radionuclide();
                Radionuclide rad2_max = new Radionuclide();

                double tmp1_max = -1;
                double tmp2_max = -1;
                foreach (var item in Calc_List)
                {
                    double tmp_1 = item.UDA * Math.Pow(2, -100 / (item.HalfLifePeriod / (365 * 24 * 60 * 60)));
                    double tmp_2 = item.UDA * Math.Pow(2, -500 / (item.HalfLifePeriod / (365 * 24 * 60 * 60)));

                    if (tmp1_max < tmp_1)
                    {
                        tmp1_max = tmp_1;
                        rad1_max = item;
                    }
                    if (tmp2_max < tmp_2)
                    {
                        tmp2_max = tmp_2;
                        rad2_max = item;
                    }

                    double tm = 0;
                    if (Calc_List.TWG == "t")
                    {
                        tm = item.MOI;
                    }
                    if (Calc_List.TWG == "w")
                    {
                        tm = item.PZUA_W;
                    }

                    a_cto += tmp_1 / tm;
                    a_pat += tmp_2 / tm;
                }
                double t_val = -1;
                if (Calc_List.TWG == "t")
                {
                    t_val = 1;
                }
                if (Calc_List.TWG == "w")
                {
                    t_val = 0.1;
                }

                if (a_cto < t_val)
                {
                    num6 = 1;
                    if (rad1_max.HalfLifePeriod > 31 * 365 * 24 * 60 * 60)
                    {
                        num5 = 1;
                    }
                    else
                    {
                        num5 = 2;
                    }
                }
                else
                {
                    if (a_pat < t_val)
                    {
                        num6 = 2;
                        Radionuclide tmp = new Radionuclide();
                        if(tmp1_max>tmp2_max)
                        {
                            tmp = rad1_max;
                        }
                        else
                        {
                            tmp = rad2_max;
                        }
                        if (rad2_max.HalfLifePeriod > 31 * 365 * 24 * 60 * 60)
                        {
                            num5 = 1;
                        }
                        else
                        {
                            num5 = 2;
                        }
                    }
                    else
                    {
                        num6 = 3;
                        if (rad2_max.HalfLifePeriod > 31 * 365 * 24 * 60 * 60)
                        {
                            num5 = 1;
                        }
                        else
                        {
                            num5 = 2;
                        }
                    }
                }
            }
            else
            {
                if (Calc_List.Count != 0)
                {
                    var tmp = Calc_List[0];
                    double answ = -1;
                    if (tmp.TWG == "t")
                    {
                        answ = 1.44 * tmp.HalfLifePeriod * Math.Log(tmp.UDA / tmp.MOI);
                    }
                    else
                    {
                        if (tmp.TWG == "w")
                        {
                            answ = 1.44 * tmp.HalfLifePeriod * Math.Log((tmp.UDA * 0.1) / tmp.PZUA_W);
                        }
                    }
                    if (answ > 0)
                    {
                        if (answ < 100f * 365 * 24 * 60 * 60)
                        {
                            num6 = 1;
                        }
                        else
                        {
                            if (answ < 500f * 365 * 24 * 60 * 60)
                            {
                                num6 = 2;
                            }
                            else
                            {
                                num6 = 3;
                            }
                        }
                    }
                    if (tmp.HalfLifePeriod > 31 * 365 * 24 * 60 * 60)
                    {
                        num5 = 1;
                    }
                    else
                    {
                        num5 = 2;
                    }
                }
            }
        }
        int num5 = -1;
        public string Number5()
        {
            if (num5 != -1)
            {
                return num5.ToString();
            }
            return "*";
        }

        int num6 = -1;
        public string Number6()
        {
            if (num6 != -1)
            {
                return num6.ToString();
            }
            return "0";
        }

        public int num7 = -1;
        public string Number7()
        {
            if (num7 != -1)
            {
                return num7.ToString();
            }
            return "*";
        }

        public int num8 = -1;
        public string Number8()
        {
            if (num8 != -1)
            {
                return num8.ToString();
            }
            return "*";
        }

        public string num9 = "-1";
        public string Number9()
        {
            if (num9 != "-1")
            {
                return num9.ToString();
            }
            return "**";
        }

        public int num10 = -1;
        public string Number10()
        {
            if (num10 != -1)
            {
                return num10.ToString();
            }
            return "*";
        }

    }
}
