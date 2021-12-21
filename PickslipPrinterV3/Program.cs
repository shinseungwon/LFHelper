using iTextSharp.text.pdf;
using System;
using HelperDotNet;
using System.IO;
using iTextSharp.text;
using System.Data;
using System.Diagnostics;

namespace PickslipPrinterV3
{
    class Program
    {
        static void Main(string[] args)
        {
            //1.Get Config
            ConfigHelper ch = new ConfigHelper("Config.txt");

            string id = ch.Configs["id"];
            string password = ch.Configs["password"];

            string ip = ch.Configs["ip"];
            string catalog = ch.Configs["catalog"];

            string brand = ch.Configs["brand"];
            string route = ch.Configs["route"];
            string stlkey = ch.Configs["stlkey"];
            string endkey = ch.Configs["endkey"];

            //2.Connect DB + Call SP
            string connectionString = "Data Source=" + ip + ",1433; Initial Catalog=" + catalog
                + "; User id=" + id + "; Password=" + password + ";";

            string query = "exec krlocal..isp_kr_hmpickslip02 '"
                + brand + "', '" + route + "', '" + stlkey + "', '" + endkey + "'";

            DbHelper dh = new DbHelper(connectionString);
            Logger l = new Logger(Directory.GetCurrentDirectory() + @"\Logger");
            dh.SetLogger(l);
            DataSet ds = new DataSet();
            dh.CallQuery(query, ref ds);

            //3. Create File
            if (!Directory.Exists("Reports"))
            {
                Directory.CreateDirectory("Reports");
            }

            string filename = @"Reports\Pickslip_" + brand + "_" + stlkey + "~" + endkey + ".pdf";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
            //Document doc = new Document(new Rectangle(196, 127), 5, 5, 5, 5);

            string[] pageinfo = ch.gets("pageinfo");
            Document doc = new Document(new Rectangle(int.Parse(pageinfo[0]), int.Parse(pageinfo[1]))
                , int.Parse(pageinfo[2]), int.Parse(pageinfo[3]), int.Parse(pageinfo[4]), int.Parse(pageinfo[5]));

            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();

            FontFactory.Register("malgun.ttf");
            FontFactory.Register("malgunsl.ttf");
            FontFactory.Register("malgunbd.ttf");

            int i, j;

            foreach (DataRow r in ds.Tables[0].Rows)
            {
                //rects
                for (i = 1; i < 100; i++)
                {
                    if (ch.get("rect" + i) == "")
                    {
                        continue;
                    }
                    else
                    {
                        string[] list = ch.gets("rect" + i);
                        if (list.Length != 5)
                        {
                            continue;
                        }
                        else
                        {
                            PdfContentByte cb = writer.DirectContent;
                            cb.Rectangle(int.Parse(list[0]), int.Parse(list[1]), int.Parse(list[2]), int.Parse(list[3]));
                            if (int.Parse(list[4]) == 0)
                            {
                                cb.Stroke();
                            }
                            else
                            {
                                cb.FillStroke();
                            }
                        }
                    }
                }

                //Barcode
                if (ch.get("barcode") != "")
                {                    
                    string[] list = ch.gets("barcode");
                    if (list.Length == 3)
                    {
                        Barcode128 barcodeImg = new Barcode128();
                        barcodeImg.Code = r[list[0]].ToString();
                        System.Drawing.Image img = barcodeImg.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White);
                        iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(img, System.Drawing.Imaging.ImageFormat.Jpeg);
                        pic.SetAbsolutePosition(int.Parse(list[1]), int.Parse(list[2]));
                        doc.Add(pic);
                    }
                }

                for (i = 1; i < 100; i++)
                {
                    if (ch.get("text" + i) == "")
                    {
                        continue;
                    }
                    else
                    {
                        string[] list = ch.gets("text" + i, '/');
                        if (list.Length != 6)
                        {
                            continue;
                        }
                        else
                        {
                            Font font = FontFactory.GetFont("맑은 고딕", BaseFont.IDENTITY_H, int.Parse(list[1])
                                , int.Parse(list[2]), list[3] == "0" ? BaseColor.WHITE : BaseColor.BLACK);

                            string str = list[0];
                            foreach (DataColumn c in ds.Tables[0].Columns)
                            {
                                string rep = "{" + c.ColumnName + "}";
                                if (str.Contains(rep))
                                {
                                    str = str.Replace(rep, r[rep.Substring(1, rep.Length - 2)].ToString());
                                }
                            }

                            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER
                                , new Phrase(str, font), int.Parse(list[4]), int.Parse(list[5]), 0);
                        }
                    }
                }

                doc.NewPage();
            }

            doc.Close();

            //4. Open File            
            Process.Start(filename);
        }
    }
}
