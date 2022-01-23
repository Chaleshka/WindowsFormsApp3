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
    public partial class Form2 : Form
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/DaysTask";
        // "/Weeks.ini", "/Dates.ini"
        public Form2()
        {
            InitializeComponent();
        }
        DateTime dateForm;
        int day = 0;
        List<object> list;
        int old;
        public Form2(List<object> list, string date1, int day, DateTime date)
        {
            this.list = list;
            this.day = day;
            dateForm = date;
            InitializeComponent();
            textBox1.Text = date1;
            checkedListBox1.Items.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                checkedListBox1.Items.Add(list[i], Convert.ToBoolean(list[i+1]));
                i++;
            }
            checkedListBox1.Items.Add(1, false);
            checkedListBox1.Items.RemoveAt(checkedListBox1.Items.Count - 1);
            old = (this.list.Count + 1) / 2;
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        string textToFile = "";
        private async void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (this.Created)
            {

                await Task.Delay(100);

                textToFile = "";
                string text = "", text1;
                int ind = 0; bool b = false; int t = 0;
                DateTime dateTime = new DateTime();


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
                    if (text1[i] == Convert.ToChar("➟"))
                    {
                        text = "";
                        textToFile += text1[i];
                        ind = 0;
                        b = false;
                        if (textToFile == "➟")
                            textToFile = "";
                    }
                    else if (text1[i] == Convert.ToChar("⇔"))
                    {
                        ind++;
                        if (ind == 1)
                        {
                            dateTime = new DateTime(long.Parse(text));

                            if (dateForm.Month == dateTime.Month && dateForm.Year == dateTime.Year && dateTime.Day == day)
                            {
                                b = true;
                                try
                                {
                                    textToFile = textToFile.Remove(textToFile.IndexOf(text), 18);
                                }
                                catch { }
                            }
                            else
                            {
                                textToFile += text;
                                textToFile += text1[i];
                                b = false;
                            }
                            text = "";
                        }
                        else
                            textToFile += text1[i];
                    }
                    else if (!b && ind != 0)
                    {
                        textToFile += text1[i];
                        text += text1[i];
                    }
                    else if (!b)
                    {
                        text += text1[i];
                    }
                    else if (b)
                        text += text1[i];
                }
                if (checkedListBox1.Items.Count != 0)
                {
                    textToFile += new DateTime(dateForm.Year, dateForm.Month, day, 1, 0, 0).Ticks + "⇔";
                    //checkedListBox1.GetItemChecked;
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        textToFile += Convert.ToString(checkedListBox1.Items[i]);
                        textToFile += "⥏";
                        textToFile += Convert.ToString(checkedListBox1.GetItemChecked(i));
                        if (i < checkedListBox1.Items.Count - 1)
                            textToFile += "⇔";
                    }
                    textToFile += "➟";

                    string s = "";
                    for (int i = 1; i < old; i++)
                    {
                        s += "⇔";
                    }
                    s += "➟";
                    string s1 = "➟";
                    for (int i = 1; i < old; i++)
                    {
                        s1 += "⇔";
                    }
                    s1 += "➟";
                    if (textToFile.Contains(s) && s.Contains("⇔"))
                        textToFile = textToFile.Replace(s, "");
                    else if (textToFile.Contains(s1))
                        textToFile = textToFile.Replace(s1, "➟");
                    else if (textToFile[0] == Convert.ToChar("➟"))
                        textToFile.Remove(0);
                    if ((list.Count + 1) / 2 != old)
                        old = (list.Count + 1) / 2;
                    textToFile += "➟";
                }
                if (textToFile == "➟")
                    textToFile = "";
                if (textToFile.Contains("➟➟"))
                    textToFile = textToFile.Replace("➟➟", "➟");
                try
                {
                    TextFromFile[stroka] = textToFile;
                }
                catch
                {
                    for (int i = TextFromFile.Count; i < stroka; i++)
                        TextFromFile.Add("");
                    TextFromFile.Add(textToFile);
                }
                using (StreamWriter sr = File.CreateText(path + "/Dates.ini"))
                {
                    for (int i = 0; i < TextFromFile.Count - 1; i++)
                        sr.WriteLine(TextFromFile[i]);
                    sr.Write(TextFromFile[TextFromFile.Count - 1]);
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            InputBox inputBox = new InputBox();
            string text = inputBox.getString();

            if (text != null && text != "")
            {
                checkedListBox1.Items.Add(text);
                list.Add(text);
                list.Add(false);
                checkedListBox1.SelectedIndex = (checkedListBox1.Items.Count - 1);
                checkedListBox1_ItemCheck(checkedListBox1.SelectedItem, new ItemCheckEventArgs(checkedListBox1.SelectedIndex, CheckState.Unchecked, CheckState.Unchecked));
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            ChooseBox chooseBox = new ChooseBox(list);
            int index = chooseBox.getIndex();
            if(index != -1)
            {
                list.RemoveAt(index);
                list.RemoveAt(index);
                checkedListBox1.Items.RemoveAt(index);
                if (checkedListBox1.Items.Count > 0)
                {
                    checkedListBox1.SelectedIndex = 0;
                    checkedListBox1_ItemCheck(checkedListBox1.SelectedItem, new ItemCheckEventArgs(checkedListBox1.SelectedIndex, CheckState.Unchecked, CheckState.Unchecked));
                }
                else
                    checkedListBox1_ItemCheck(new object(), new ItemCheckEventArgs(checkedListBox1.SelectedIndex, CheckState.Unchecked, CheckState.Unchecked));
            }
        }
    }
}
