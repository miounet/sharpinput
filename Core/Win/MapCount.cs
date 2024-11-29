using Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Core.Win
{
    public class szobjclass
    {
        public string sz { get; set; }
        public long down { get; set; }
    }
    public partial class MapCount : Form
    {
        public MapCount()
        {
            InitializeComponent();
        }

        private void MapCount_Load(object sender, EventArgs e)
        {
            List<string> XList = new List<string>();
            List<long> YList = new List<long>();
            List<szobjclass> maplist = new List<szobjclass>();

            foreach (var item in Core.Win.WinInput.Input.mapkeys)
            {
                if (item.keydown > 0 && maplist.Find(f => f.sz == item.ZM) == null)
                {

                    szobjclass szo = new szobjclass();
                    szo.sz = item.ZM;

                    szo.down = item.keydown;
                    maplist.Add(szo);

                }
            }

            maplist = maplist.OrderByDescending(o => o.down).ToList();
            foreach (var item in maplist)
            {
                YList.Add(item.down);
                XList.Add(item.sz);
            }
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "#次";
            chart1.Series["映射指法"].Points.DataBindXY(XList, YList);


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

                    skinLabel4.Text = "指法: " + xValue + "\n" + areaName + "按键次数: " + yValue;

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
