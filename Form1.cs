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

namespace WindowsFormsApp3
{
    // 1 день = 864 000 000 000                 864000000000
    // следующая дата "➟"
    // следующая строка (оплата) "⇔"
    // состояние оплаты "⥏"
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        #region peremens 
        DateTime date;
        DateTime dateForm = DateTime.Now;
        DateTime now = DateTime.Now;
        bool vis = false;
        Button[] list;
        TextBox[] texts;
        string[] daysOfWeeks = { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс"};
        int[] visYear = {31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
        int[] Year = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
        #endregion
        string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/DaysTask"; 
        // "/Weeks.ini", "/Dates.ini"
        private void Form1_Load(object sender, EventArgs e)
        {
            #region init
            list = new Button[]{button01, button02, button03, button04, button05, button06, button07, button08, button09, button10, button11
            , button12, button13, button14, button15, button16, button17, button18, button19, button20, button21, button22, button23
            , button24, button25, button26, button27, button28, button29, button30, button31};
            texts = new TextBox[] {textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7};
            #endregion


            if (now.Day != 1)
                date = new DateTime(now.Year, now.Month, 1, 1, 0, 0);
            init();
        }

        private void init()
        {

            switch (dateForm.Month)
            {
                case 1:
                    DateAndYear.Text = "Январь ";
                    break;
                case 2:
                    DateAndYear.Text = "Февраль ";
                    break;
                case 3:
                    DateAndYear.Text = "Март ";
                    break;
                case 4:
                    DateAndYear.Text = "Апрель ";
                    break;
                case 5:
                    DateAndYear.Text = "Май ";
                    break;
                case 6:
                    DateAndYear.Text = "Июнь ";
                    break;
                case 7:
                    DateAndYear.Text = "Июль ";
                    break;
                case 8:
                    DateAndYear.Text = "Август ";
                    break;
                case 9:
                    DateAndYear.Text = "Сентябрь ";
                    break;
                case 10:
                    DateAndYear.Text = "Октябрь ";
                    break;
                case 11:
                    DateAndYear.Text = "Ноябрь ";
                    break;
                case 12:
                    DateAndYear.Text = "Декабрь ";
                    break;
            }
            DateAndYear.Text += dateForm.Year;

            #region vis
            if (dateForm.Year % 4 == 0)
                vis = true;
            else
                vis = false;
            #endregion



            foreach (Button b in list)
                b.Visible = false;
            if (vis)
                for (int i = 0; i < visYear[dateForm.Month - 1]; i++)
                {
                    list[i].Visible = true;
                    list[i].ForeColor = Color.Black;
                }
            else
                for (int i = 0; i < Year[dateForm.Month - 1]; i++)
                {
                    list[i].Visible = true;
                    list[i].ForeColor = Color.Black;
                }
            list[dateForm.Day - 1].ForeColor = Color.Blue;
            switch (Convert.ToString(date.DayOfWeek))
            {
                case "Monday": setDate(0, date.Month); break;
                case "Tuesday": setDate(1, date.Month); break;
                case "Wednesday": setDate(2, date.Month); break;
                case "Thursday": setDate(3, date.Month); break;
                case "Friday": setDate(4, date.Month); break;
                case "Saturday": setDate(5, date.Month); break;
                case "Sunday": setDate(6, date.Month); break;
            }
            SetColors();
        }

        private async void SetColors()
        {
            foreach(Button b in list)
            {
                b.BackColor = Color.White;
            }
            string text = "", text1 = File.ReadAllText(path + "/Dates.ini");
            int ind = 0;
            DateTime dateTime = new DateTime();
            List<string> TextFromFile = new List<string>();
            int stroka = 0;
            await TextFromFile = File.ReadAllLines(path + "/Dates.ini").ToList();
            stroka = (dateForm.Year - 2022) * 12 + dateForm.Month - 1;
            try
            {
                text1 = TextFromFile.ElementAt(stroka);
            }
            catch
            {
                text1 = "";
            }

            for (int i = 0; i < text1.Length; i++)
            {
                if (text1[i] == Convert.ToChar("➟"))
                {
                    if (dateTime.Month == dateForm.Month && dateTime.Year == dateForm.Year)
                    {
                        if (text.Contains("⥏false") || text.Contains("⥏False"))
                            list[dateTime.Day - 1].BackColor = Color.PaleVioletRed;
                        else if ((text.Contains("⥏true") && !text.Contains("⥏false")) || (text.Contains("⥏True") && !text.Contains("⥏False")))
                            list[dateTime.Day - 1].BackColor = Color.PaleGreen;
                        else if ((!text.Contains("⥏true") && !text.Contains("⥏false")) || (!text.Contains("⥏True") && !text.Contains("⥏False")))
                            list[dateTime.Day - 1].BackColor = Color.White;
                        else
                            list[dateTime.Day - 1].BackColor = Color.White;
                    }
                    text = "";
                    ind = 0;
                }
                else if (text1[i] == Convert.ToChar("⇔"))
                {
                    ind++;
                    if (ind == 1)
                    {
                        dateTime = new DateTime(long.Parse(text));
                        text = "";
                    }
                    else
                        text += text1[i];
                }
                else
                    text += text1[i];
            }
        }

        private void setDate(int day, int month)
        {
            for(int i = 0; i <= 6; i++)
            {
                texts[i].Text = daysOfWeeks[day];
                day++;
                if (day > 6)
                    day = 0;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Day != dateForm.Day)
            {
                list[dateForm.Day-1].ForeColor = Color.White;
                if (vis)
                    if (dateForm.Day == visYear[dateForm.Month - 1])
                        dateForm = new DateTime(dateForm.Year, dateForm.Month, 1, dateForm.Hour,
                            dateForm.Minute, dateForm.Second, dateForm.Millisecond, dateForm.Kind);
                    else
                        dateForm = dateForm.AddDays(1);
                else
                    if (dateForm.Day == visYear[dateForm.Month - 1])
                        dateForm = new DateTime(dateForm.Year, dateForm.Month, 1, dateForm.Hour,
                        dateForm.Minute, dateForm.Second, dateForm.Millisecond, dateForm.Kind);
                    else
                        dateForm = dateForm.AddDays(1);
                list[dateForm.Day - 1].ForeColor = Color.Blue;
            }
        }

        #region buttons
        private void next_Click(object sender, EventArgs e)
        {
            if (dateForm.Month == 12)
            {
                dateForm = new DateTime(dateForm.Year + 1, 1, dateForm.Day, dateForm.Hour,
                    dateForm.Minute, dateForm.Second, dateForm.Millisecond, dateForm.Kind);
                date = new DateTime(date.Year + 1, 1, 1, 1, 0, 0);
            }
            else 
            { 
                dateForm = dateForm.AddMonths(1); 
                date = date.AddMonths(1); 
            }
            init();
        }

        private void past_Click(object sender, EventArgs e)
        {
            if (dateForm.Month == 1)
            {
                dateForm = new DateTime(dateForm.Year - 1, 12, dateForm.Day, dateForm.Hour,
                    dateForm.Minute, dateForm.Second, dateForm.Millisecond, dateForm.Kind);
                date = new DateTime(date.Year, 12, 1, 1, 0, 0);
            }
            else
            {
                dateForm = new DateTime(dateForm.Year, dateForm.Month - 1, dateForm.Day, dateForm.Hour,
                    dateForm.Minute, dateForm.Second, dateForm.Millisecond, dateForm.Kind);
                date = new DateTime(date.Year, date.Month - 1, 1, 0, 0, 0);
            }
            init();
        }

        Form f = new Form2();
        private void button_Click(object sender, EventArgs e)
        {
            try
            {
                f.Close();
            }
            catch { }
            
            string ButtonDate = Convert.ToInt32((sender as Button).Text) + "." + dateForm.Month + "." + dateForm.Year;
            string text = "", znach = "", text1;
            int ind = 0; bool b = false, bolean = false;
            List<object> mas = new List<object>();
            List<string> TextFromFile = new List<string>();
            int stroka = 0;
            using (StreamReader sr = File.OpenText(path + "/Dates.ini"))
            {
                while ((text = sr.ReadLine()) != null)
                {
                    TextFromFile.Add(text);
                }
            }
            stroka = (dateForm.Year - 2022) * 12 + dateForm.Month - 1;
            try
            {
                text1 = TextFromFile.ElementAt(stroka);
            }
            catch
            {
                text1 = "";
            }
            for (int i = 0; i < text1.Length; i++)
            {
                if (text1[i] == Convert.ToChar("➟") && b)
                {
                    break;
                }
                else if (text1[i] == Convert.ToChar("➟") && !b)
                {
                    text = "";
                    mas = new List<object>();
                    ind = 0;
                }
                else if (text1[i] == Convert.ToChar("⇔"))
                {
                    ind++;
                    if(ind == 1)
                    {
                        if (dateForm.Year == new DateTime(long.Parse(text)).Year &&
                           dateForm.Month == new DateTime(long.Parse(text)).Month &&
                           Convert.ToInt32((sender as Button).Text) == new DateTime(long.Parse(text)).Day)
                        {
                            text = "";
                            bolean = true;
                        }
                        else
                            bolean = false;
                    }
                }
                else if (text1[i] == Convert.ToChar("⥏"))
                {
                    i++;
                    for(int integ = 0; integ < 4; i++)
                    {
                        if (text1[i] == Convert.ToChar("⇔") || text1[i] == Convert.ToChar("➟"))
                        {
                            mas.Add(text);
                            mas.Add(Convert.ToBoolean(znach));
                            znach = "";
                            text = "";
                            i--;
                            if (bolean)
                                b = true;
                            else
                                b = false;
                            break;
                        }
                        znach += text1[i];
                    }
                }
                else
                    text += text1[i];
            }
            if (text == null)
                text = "";
            f = new Form2(mas, ButtonDate, Convert.ToInt32((sender as Button).Text), dateForm);
            timer2.Enabled = true;
            f.Show(this);
        }

        private void Dublicate_Click(object sender, EventArgs e)
        {
            string text = "", znach = "", text1;
            int ind = 0; bool b = false, bolean = false;
            List<object> mas = new List<object>();
            List<string> TextFromFile = new List<string>();
            int stroka = 0; DayOfWeek dayOfWeek = DayOfWeek.Monday; bool IsNull = false;
            using (StreamReader sr = File.OpenText(path + "/Dates.ini"))
            {
                while ((text = sr.ReadLine()) != null)
                {
                    TextFromFile.Add(text);
                }
            }
            stroka = (dateForm.Year - 2022) * 12 + dateForm.Month - 1;
            try
            {
                text1 = TextFromFile.ElementAt(stroka);
            }
            catch
            {
                text1 = "";
            }

            ChooseDayBox cdb = new ChooseDayBox();
            switch (cdb.getDay())
            {
                case "Понедельник":
                    dayOfWeek = DayOfWeek.Monday;
                    IsNull = false;
                    break;
                case "Вторник":
                    dayOfWeek = DayOfWeek.Tuesday;
                    IsNull = false;
                    break;
                case "Среда":
                    dayOfWeek = DayOfWeek.Wednesday;
                    IsNull = false;
                    break;
                case "Четверг":
                    dayOfWeek = DayOfWeek.Thursday;
                    IsNull = false;
                    break;
                case "Пятница":
                    dayOfWeek = DayOfWeek.Friday;
                    IsNull = false;
                    break;
                case "Суббота":
                    dayOfWeek = DayOfWeek.Saturday;
                    IsNull = false;
                    break;
                case "Воскресенье":
                    dayOfWeek = DayOfWeek.Sunday;
                    IsNull = false;
                    break;
                default:
                    IsNull = true;
                    break;
            }
            DateTime dateTime = new DateTime(); int day = 0;
            if (!IsNull)
            {
                for (int i = 0; i < text1.Length; i++)
                {
                    if (text1[i] == Convert.ToChar("➟") && b)
                    {
                        text += text1[i].ToString();
                        break;
                    }
                    else if (text1[i] == Convert.ToChar("➟") && !b)
                    {
                        text = "";
                        ind = 0;
                    }
                    else if (text1[i] == Convert.ToChar("⇔"))
                    {
                        ind++;
                        if (ind == 1)
                        {
                            dateTime = new DateTime(long.Parse(text));
                            if (dateForm.Year == new DateTime(long.Parse(text)).Year &&
                               dateForm.Month == new DateTime(long.Parse(text)).Month &&
                               new DateTime(long.Parse(text)).Day <= 7 && new DateTime(long.Parse(text)).DayOfWeek == dayOfWeek)
                            {
                                text = "";
                                b = true;
                            }
                            else
                                b = false;
                        }
                        else
                        {
                            text += text1[i].ToString();
                        }
                    }
                    else
                        text += text1[i];
                }


                dateTime.AddDays(14);
                day = dateTime.Day;
                while (day <= (vis ? visYear[dateTime.Month - 1] : Year[dateTime.Month - 1]))
                {
                    TextFromFile[stroka] += new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0).Ticks +
                        "⇔" + text;
                    dateTime = dateTime.AddDays(7);
                    day += 7;
                }

                using (StreamWriter sr = File.CreateText(path + "/Dates.ini"))
                {
                    for (int i = 0; i < TextFromFile.Count - 1; i++)
                        sr.WriteLine(TextFromFile[i]);
                    sr.Write(TextFromFile[TextFromFile.Count - 1]);
                }
                init();
            }
        }
        #endregion

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (f.IsDisposed)
            {
                SetColors();
                timer2.Enabled = false;
            }
        }
    }
}
