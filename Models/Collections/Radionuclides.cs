using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;
using System.Data.OleDb;

namespace RAO_Calculator_Library
{
    [Serializable]
    public class Radionuclides:ObservableCollection<Radionuclide>
    {
        public string TWG { get; set; }
        public int AddFromDB(string connectionString)
        {
            try
            {
                string sql = "SELECT Name_RN_Lat,UdA_TRO,UdA_GRO,Kod_gruppy,Period_p_r,Edinica_izmer_p_r,Num_TM,ObA_GaRO,MOI,D_val FROM RN "
                + "WHERE Kod_gruppy<>NULL AND Period_p_r<>NULL AND Edinica_izmer_p_r<>NULL AND Num_TM<>NULL AND D_val<>NULL "
                + "AND (ObA_GaRO<>0 OR UdA_GRO<>0 OR UdA_TRO<>0) AND Period_p_r<>0 AND Num_TM<>0 AND D_val<>0";
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    OleDbCommand command = new OleDbCommand(sql, connection);
                    OleDbDataAdapter odp = new OleDbDataAdapter();
                    odp.SelectCommand = command;
                    DataTable dt = new DataTable();
                    odp.FillSchema(dt, SchemaType.Source);
                    odp.Fill(dt);

                    foreach (DataRow item in dt.Rows)
                    {
                        if (!(item.ItemArray[1] == DBNull.Value && item.ItemArray[2] == DBNull.Value && item.ItemArray[7] == DBNull.Value))
                        {
                            Radionuclide rd = new Radionuclide();
                            rd.Name = (string)item.ItemArray[0];
                            if ((string)item.ItemArray[3] == "б")
                            {
                                rd.RadiationType = "b";
                            }
                            if ((string)item.ItemArray[3] == "а")
                            {
                                rd.RadiationType = "a";
                            }
                            if ((string)item.ItemArray[5] == "лет")
                            {
                                rd.HalfLifePeriod = Convert.ToDouble(item.ItemArray[4]) * (365 * 24 * 60 * 60);
                            }
                            if ((string)item.ItemArray[5] == "сут")
                            {
                                rd.HalfLifePeriod = Convert.ToDouble(item.ItemArray[4]) * (24 * 60 * 60);
                            }
                            if ((string)item.ItemArray[5] == "час")
                            {
                                rd.HalfLifePeriod = Convert.ToDouble(item.ItemArray[4]) * (60 * 60);
                            }
                            if ((string)item.ItemArray[5] == "мин")
                            {
                                rd.HalfLifePeriod = Convert.ToDouble(item.ItemArray[4]) * (60);
                            }
                            if (Convert.ToDouble(item.ItemArray[6]) > 92)
                            {
                                rd.AfterUran = true;
                            }
                            else
                            {
                                rd.AfterUran = false;
                            }
                            if (item.ItemArray[1] != DBNull.Value)
                            {
                                rd.PZUA_T = Convert.ToDouble(item.ItemArray[1]);
                            }
                            if (item.ItemArray[2] != DBNull.Value)
                            {
                                rd.PZUA_W = Convert.ToDouble(item.ItemArray[2]);
                            }
                            if (item.ItemArray[7] != DBNull.Value)
                            {
                                rd.PZUA_G = Convert.ToDouble(item.ItemArray[7]);
                            }
                            if (item.ItemArray[8] != DBNull.Value)
                            {
                                rd.MOI = Convert.ToDouble(item.ItemArray[8]);
                            }
                            if (item.ItemArray[9] != DBNull.Value)
                            {
                                rd.D_val = Convert.ToDouble(item.ItemArray[9]);
                            }
                            Add(rd);
                        }
                    }
                }
                return 0;
            }
            catch
            {
                return -1;
            }
        }
        public IEnumerable<Radionuclide> Filter()
        {
            if (TWG == "t")
            {
                return from item in this where item.PZUA_T != 0 select item;
            }
            if (TWG == "w")
            {
                return from item in this where item.PZUA_W != 0 select item;
            }
            if (TWG == "g")
            {
                return from item in this where item.PZUA_G != 0 select item;
            }
            return this;
        }
        public int FindIndex(Radionuclide obj)
        {
            for(int i=0;i<this.Count;i++)
            {
                if(this[i]==obj)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
