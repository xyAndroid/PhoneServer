using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SCMS.Web
{
    public partial class ValidPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string str_ValudateCode = GetRandomNumberString(4);
                Session["ValidateCode"] = str_ValudateCode;
                CreateImage(str_ValudateCode);
            }
        }

        public string GetRandomNumberString(int int_numberLength)
        {
            string str_number = string.Empty;
            Random aRandom = new Random();
            for (int int_index = 0; int_index < int_numberLength; int_index++)
            {
                str_number += aRandom.Next(10).ToString();
            }
            return str_number;
        }

        public System.Drawing.Color GetRandomColor()
        {
            Random aRandom = new Random((int)DateTime.Now.Ticks);
            System.Threading.Thread.Sleep(aRandom.Next(50));
            Random bRandom = new Random((int)DateTime.Now.Ticks);
            int int_Red = aRandom.Next(255);
            int int_Green = bRandom.Next(255);
            int int_Blue = ((int_Red + int_Green) > 400) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            return System.Drawing.Color.FromArgb(int_Red, int_Green, int_Blue);
        }

        public void CreateImage(string str_ValidateCode)
        {
            int int_ImageWidth = str_ValidateCode.Length * 14;
            Random newRandom = new Random();
            System.Drawing.Bitmap theBitmap = new System.Drawing.Bitmap(int_ImageWidth, 20);
            System.Drawing.Graphics theGraphics = System.Drawing.Graphics.FromImage(theBitmap);
            theGraphics.Clear(System.Drawing.Color.White);
            theGraphics.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.LightGray, 1), 0, 0, int_ImageWidth - 1, 19);
            System.Drawing.Font theFont = new System.Drawing.Font("Arial", 10);
            for (int int_index = 0; int_index < str_ValidateCode.Length; int_index++)
            {
                string str_char = str_ValidateCode.Substring(int_index, 1);
                System.Drawing.Brush newBrush = new System.Drawing.SolidBrush(GetRandomColor());
                System.Drawing.Point thePos = new System.Drawing.Point(int_index * 13 + 1 + newRandom.Next(3), 1 + newRandom.Next(3));
                theGraphics.DrawString(str_char, theFont, newBrush, thePos);
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            theBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            Response.ClearContent();
            Response.ContentType = "image/Png";
            Response.BinaryWrite(ms.ToArray());
            theGraphics.Dispose();
            theBitmap.Dispose();
            Response.End();
        }
    }
}