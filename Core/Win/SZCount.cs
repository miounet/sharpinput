using Core.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace Core.Win
{

    public partial class SZCount : Form
    {
        public SZCount()
        {
            InitializeComponent();
        }

        private void MapCount_Load(object sender, EventArgs e)
        {
            List<szobjclass> szlist = new List<szobjclass>();
            szobjclass lxz = new szobjclass();
            lxz.sz = "左小指";
            lxz.down = 0;
            szlist.Add(lxz);
            szobjclass lwmz = new szobjclass();
            lwmz.sz = "左无名指";
            lwmz.down = 0;
            szlist.Add(lwmz);
            szobjclass lzz = new szobjclass();
            lzz.sz = "左中指";
            lzz.down = 0;
            szlist.Add(lzz);
            szobjclass lsz = new szobjclass();
            lsz.sz = "左食指";
            lsz.down = 0;
            szlist.Add(lsz);
            szobjclass lmz = new szobjclass();
            lmz.sz = "左拇指";
            lmz.down = 0;
            szlist.Add(lmz);

            szobjclass rmz = new szobjclass();
            rmz.sz = "右拇指";
            rmz.down = 0;
            szlist.Add(rmz);
            szobjclass rsz = new szobjclass();
            rsz.sz = "右食指";
            rsz.down = 0;
            szlist.Add(rsz);
            szobjclass rzz = new szobjclass();
            rzz.sz = "右中指";
            rzz.down = 0;
            szlist.Add(rzz);
            szobjclass rwmz = new szobjclass();
            rwmz.sz = "右无名指";
            rwmz.down = 0;
            szlist.Add(rwmz);
            szobjclass rxz = new szobjclass();
            rxz.sz = "右小指";
            rxz.down = 0;
            szlist.Add(rxz);
            List<string> XList = new List<string>();
            List<long> YList = new List<long>();

            foreach (var item in Core.Win.WinInput.Input.mapkeys)
            {
                if (item.keydown > 0)
                {
                    long keydown = item.keydown;
                    string sz = item.ZM;
                    if ("aseclp" == sz)
                    {
                        szlist.Find(f => f.sz == "左拇指").down += keydown;
                    }
                    else if ("asecrp" == sz)
                    {
                        szlist.Find(f => f.sz == "右拇指").down += keydown;
                    }
                    else
                    {
                        for (int i = 0; i < sz.Length; i++)
                        {
                            string dz = sz.Substring(i, 1);
                            if ("1qaz".IndexOf(dz.ToLower()) >= 0)
                            {
                                szlist.Find(f => f.sz == "左小指").down += keydown;
                            }
                            else if ("2wsx".IndexOf(dz.ToLower()) >= 0)
                            {
                                szlist.Find(f => f.sz == "左无名指").down += keydown;
                            }
                            else if ("3edc".IndexOf(dz.ToLower()) >= 0)
                            {
                                szlist.Find(f => f.sz == "左中指").down += keydown;
                            }
                            else if ("4rfv5tgb".IndexOf(dz.ToLower()) >= 0)
                            {
                                szlist.Find(f => f.sz == "左食指").down += keydown;
                            }
                            else if ("0p;/'[]-=".IndexOf(dz.ToLower()) >= 0)
                            {
                                szlist.Find(f => f.sz == "右小指").down += keydown;
                            }
                            else if ("9ol.".IndexOf(dz.ToLower()) >= 0)
                            {
                                szlist.Find(f => f.sz == "右无名指").down += keydown;
                            }
                            else if ("8ik,".IndexOf(dz.ToLower()) >= 0)
                            {
                                szlist.Find(f => f.sz == "右中指").down += keydown;
                            }
                            else if ("7ujm6yhn".IndexOf(dz.ToLower()) >= 0)
                            {
                                szlist.Find(f => f.sz == "右食指").down += keydown;
                            }

                        }
                    }
                }
            }

            foreach (var item in szlist)
            {
                if (item.down > 0)
                {
                    YList.Add(item.down);
                    XList.Add(item.sz);
                }
            }
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "#次";
            chart1.Series["手指"].Points.DataBindXY(XList, YList);


            List<szobjclass> dzlist = new List<szobjclass>();

            szobjclass dzsl = new szobjclass();
            dzsl.sz = "单字数";
            dzsl.down = 0;
            dzlist.Add(dzsl);
            szobjclass cznum = new szobjclass();
            cznum.sz = "词组";
            cznum.down = 0;
            dzlist.Add(cznum);
            szobjclass czzs = new szobjclass();
            czzs.sz = "词组字数";
            czzs.down = 0;
            dzlist.Add(czzs);
            szobjclass bjs = new szobjclass();
            bjs.sz = "并击数";
            bjs.down = 0;
            dzlist.Add(bjs);
            szobjclass zzs = new szobjclass();
            zzs.sz = "总字数";
            zzs.down = 0;
            dzlist.Add(zzs);

            var testdata = File.ReadAllLines(System.IO.Path.Combine(Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
               , "testdata.txt"), Encoding.UTF8);//读 

            if (testdata.Length > 0)
            {

                if (testdata[0].Split('_').Length > 8)
                {

                    dzlist.Find(f => f.sz == "单字数").down = long.Parse(testdata[0].Split('_')[4]);

                    dzlist.Find(f => f.sz == "词组").down = long.Parse(testdata[0].Split('_')[6]);

                    dzlist.Find(f => f.sz == "词组字数").down = long.Parse(testdata[0].Split('_')[8]);
                    dzlist.Find(f => f.sz == "并击数").down = long.Parse(testdata[0].Split('_')[9]);
                }
            }
            dzlist.Find(f => f.sz == "总字数").down = dzlist.Find(f => f.sz == "单字数").down + dzlist.Find(f => f.sz == "词组字数").down;
            dzlist = dzlist.OrderByDescending(o => o.down).ToList();
            chart1.ChartAreas[1].AxisY.LabelStyle.Format = "#量";
            chart1.Series["练习"].Points.DataBindXY(dzlist.Select(s => s.sz).ToList(), dzlist.Select(s => s.down).ToList());

        }

        /// <summary>

        /// 获取某个对象中的属性值

        /// </summary>

        /// <param name="info"></param>

        /// <param name="field"></param>

        /// <returns></returns>

        private object GetPropertyValue(object info, string field)

        {

            if (info == null) return null;

            Type t = info.GetType();

            IEnumerable<System.Reflection.PropertyInfo> property = from pi in t.GetProperties() where pi.Name.ToLower() == field.ToLower() select pi;

            return property.First().GetValue(info, null);

        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            try

            {

                HitTestResult Result = new HitTestResult();

                Result = chart1.HitTest(e.X, e.Y);

                if (Result.Series != null && Result.Object != null)

                {

                    // 获取当前焦点x轴的值

                    string xValue = GetPropertyValue(Result.Object, "AxisLabel").ToString();

                    // 获取当前焦点所属区域名称

                    string areaName = GetPropertyValue(Result.Object, "LegendText").ToString();

                    // 获取当前焦点y轴的值

                    double yValue = Result.Series.Points[Result.PointIndex].YValues[0];

                    // 鼠标经过时label显示

                    skinLabel4.Visible = true;

                    skinLabel4.Text = xValue + "\n" + areaName + "量: " + yValue;

                    skinLabel4.Location = new Point(e.X, e.Y - 20);

                }

                else

                {

                    // 鼠标离开时label隐藏

                    skinLabel4.Visible = false;

                }

            }

            catch (Exception se)

            {

                // 鼠标离开时label隐藏

                skinLabel4.Visible = false;

            }
        }
    }
}
