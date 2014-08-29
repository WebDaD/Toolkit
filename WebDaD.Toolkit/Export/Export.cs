using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Diagnostics;

namespace WebDaD.Toolkit.Export
{
    public static class Export
    {
        public static List<ExportType> NotYetSupported
        {
            get
            {
                List<ExportType> r = new List<ExportType>();

                r.Add(ExportType.MarkDown);
                r.Add(ExportType.XML);
                r.Add(ExportType.CSV);
                r.Add(ExportType.Excel);
                r.Add(ExportType.Word);

                return r;
            }
        }

        /// <summary>
        /// Exports the Data given
        /// </summary>
        /// <param name="type">Export Format</param>
        /// <param name="data">A Data Object</param>
        /// <param name="template">A Template to print the Data into</param>
        /// <param name="basepath">The Basepath to create the file in</param>
        /// <returns>The Path of the created Export</returns>
        public static string DataExport(ExportType type, Exportable data, Template template, string basepath, ExportCount ec, string pathtohtmltopdf)
        {
            string path = basepath + "\\" + data.Filename(ec); //need to Add fileending

            switch (type)
            {
                case ExportType.PDF:
                    path = exportPDF(data.DataName(ec), data.ToContent(ec), data.ObjectID, data.Adress, data.WorkerName, data.DateCreated, data.DateSecond, template, path, basepath, pathtohtmltopdf);
                    break;
                case ExportType.Word:
                    path = exportWord(data.DataName(ec), data.ToContent(ec), data.ObjectID, data.Adress, data.WorkerName, data.DateCreated, data.DateSecond, template, path);
                    break;
                case ExportType.Excel:
                    path = exportExcel(data.DataName(ec), data.ToContent(ec), data.ObjectID, data.Adress, data.WorkerName, data.DateCreated, data.DateSecond, template, path);
                    break;
                case ExportType.TXT:
                    path = exportTXT(data.DataName(ec), data.ToContent(ec), data.ObjectID, data.Adress, data.WorkerName, data.DateCreated, data.DateSecond, template, path);
                    break;
                case ExportType.CSV:
                    path = exportCSV(data.DataName(ec), data.ToContent(ec), data.ObjectID, data.Adress, data.WorkerName, data.DateCreated, data.DateSecond, template, path);
                    break;
                case ExportType.HTML:
                    path = exportHTML(data.DataName(ec), data.ToContent(ec), data.ObjectID, data.Adress, data.WorkerName, data.DateCreated, data.DateSecond, template, path, basepath);
                    break;
                case ExportType.XML:
                    path = exportXML(data.DataName(ec), data.ToContent(ec), data.ObjectID, data.Adress, data.WorkerName, data.DateCreated, data.DateSecond, template, path);
                    break;
                case ExportType.MarkDown:
                    path = exportMD(data.DataName(ec), data.ToContent(ec), data.ObjectID, data.Adress, data.WorkerName, data.DateCreated, data.DateSecond, template, path);
                    break;
                default:
                    break;
            }

            return path;
        }

        private static string exportMD(string title, Content content, string id, string adress, string worker, string datecreate, string datesecond, Template template, string path)
        {
            throw new NotImplementedException();
        }

        private static string exportXML(string title, Content content, string id, string adress, string worker, string datecreate, string datesecond, Template template, string path)
        {
            throw new NotImplementedException();
        }

        private static string exportHTML(string title, Content content, string id, string adress, string worker, string datecreate, string datesecond, Template template, string path, string basepath, string css = "html.css")
        {
            path += ".html";
            using (StreamWriter file = new StreamWriter(path))
            {
                //html header (containing CSS!)
                file.WriteLine("<html>");
                file.WriteLine("<head>");
                file.WriteLine("<title>" + title + "</title>");
                foreach (string line in getCSS(basepath + Path.DirectorySeparatorChar + "templates" + Path.DirectorySeparatorChar + css))
                {
                    file.WriteLine(line);
                }
                file.WriteLine("</head>");

                //header
                file.WriteLine("<body>");
                if (!template.IsEmpty)
                {
                    if (template.Header != null)
                    {
                        file.WriteLine("<div id=\"header_full\">" + template.Header.Replace(Template.IMAGE_TAG, "<img src=\"").Replace(Template.IMAGE_END, "\"/>").Replace(Template.LINEBREAK, "<br/>").Replace(Template.FULLSTARTER,"") + "</div>");
                    }
                    else
                    {
                        file.WriteLine("<div id=\"header\">");
                        file.WriteLine("<div id=\"header_left\">" + template.Header_Left.Replace(Template.IMAGE_TAG, "<img src=\"").Replace(Template.IMAGE_END, "\"/>") + "</div>");
                        file.WriteLine("<div id=\"header_center\">" + template.Header_Center.Replace(Template.IMAGE_TAG, "<img src=\"").Replace(Template.IMAGE_END, "\"/>") + "</div>");
                        file.WriteLine("<div id=\"header_right\">" + template.Header_Right.Replace(Template.IMAGE_TAG, "<img src=\"").Replace(Template.IMAGE_END, "\"/>") + "</div>");
                        file.WriteLine("</div>");
                    }

                    file.WriteLine("<div id=\"textBefore\">");
                    file.WriteLine("<div id=\"textBefore_left\">" + Template.ReplacePlaceholder(template.TextBefore_Left.Replace(Template.LINEBREAK, "<br/>"), id, adress, worker, datecreate, datesecond) + "</div>");
                    file.WriteLine("<div id=\"textBefore_right\">" + Template.ReplacePlaceholder(template.TextBefore_Right.Replace(Template.LINEBREAK, "<br/>"), id, adress, worker, datecreate, datesecond) + "</div>");
                    file.WriteLine("</div>");

                    file.WriteLine("<div id=\"beforeContent\">" + template.BeforeContent + "</div>");
                }

                file.WriteLine("<h1>" + title + "</h1>");

                //content
                switch (content.Type)
                {
                    case DataType.Table:
                        ContentTable c = (ContentTable)content as ContentTable;
                        file.WriteLine("<table>");
                        file.WriteLine("<tr>");
                        foreach (DataColumn col in c.Table.Columns)
                        {
                            file.Write("<th>" + col.Caption + "</th>");
                        }
                        file.WriteLine("</tr>");
                        foreach (DataRow row in c.Table.Rows)
                        {
                            file.WriteLine("<tr>");
                            foreach (string item in row.ItemArray)
                            {
                                file.WriteLine("<td>" + item.Replace(Template.EMPTY, Template.HTML_SPACE) + "</td>");
                            }
                            file.WriteLine("</tr>");
                        }
                        file.WriteLine("</table>");
                        break;
                    case DataType.Paragraphs:
                        ContentParagraphs p = (ContentParagraphs)content as ContentParagraphs;
                        file.WriteLine("<dl>");
                        foreach (KeyValuePair<string, string> item in p.Paragraphs)
                        {
                            file.WriteLine("<dt>" + item.Key + "</dt>");
                            file.WriteLine("<dd>" + item.Value + "</dd>");
                        }
                        file.WriteLine("</dl>");
                        break;
                    case DataType.Text:
                        ContentText t = (ContentText)content as ContentText;
                        file.WriteLine("<p>" + t.Text + "</p>");
                        break;
                    default:
                        break;
                }


                //footer

                if (!template.IsEmpty)
                {
                    file.WriteLine("<div id=\"afterContent\">" + template.AfterContent + "</div>");

                    if (template.Footer != null)
                    {
                        file.WriteLine("<div id=\"footer_full\">" + template.Footer.Replace(Template.IMAGE_TAG, "<img src=\"").Replace(Template.IMAGE_END, "\"/>").Replace(Template.LINEBREAK, "<br/>").Replace(Template.FULLSTARTER, "") + "</div>");
                    }
                    else
                    {
                        file.WriteLine("<div id=\"footer\">");
                        file.WriteLine("<div id=\"footer_left\">" + template.Footer_Left.Replace(Template.IMAGE_TAG, "<img src=\"").Replace(Template.IMAGE_END, "\"/>") + "</div>");
                        file.WriteLine("<div id=\"footer_center\">" + template.Footer_Center.Replace(Template.IMAGE_TAG, "<img src=\"").Replace(Template.IMAGE_END, "\"/>") + "</div>");
                        file.WriteLine("<div id=\"footer_right\">" + template.Footer_Right.Replace(Template.IMAGE_TAG, "<img src=\"").Replace(Template.IMAGE_END, "\"/>") + "</div>");
                        file.WriteLine("</div>");
                    }
                }
                file.WriteLine("</body>");
                file.WriteLine("</html>");
            }
            return path;
        }

        private static List<string> getCSS(string file)
        {
            List<string> c = new List<string>();

            try
            {
                c = File.ReadAllLines(file).ToList<string>();
            }
            catch (Exception)
            {

            }
            return c;
        }

        private static string exportCSV(string title, Content content, string id, string adress, string worker, string datecreate, string datesecond, Template template, string path)
        {
            throw new NotImplementedException();
        }

        private static string exportTXT(string title, Content content, string id, string adress, string worker, string datecreate, string datesecond, Template template, string path)
        {
            path += ".txt";
            using (StreamWriter file = new StreamWriter(path))
            {
                if (!template.IsEmpty)
                {
                    if (template.Header != null)
                    {
                        file.WriteLine(template.Header.Replace(Template.IMAGE_TAG, "<").Replace(Template.IMAGE_END, ">").Replace(Template.LINEBREAK, "\n").Replace(Template.FULLSTARTER, ""));
                    }
                    else
                    {
                        file.WriteLine(template.Header_Left + Template.TAB + Template.TAB + template.Header_Center + Template.TAB + Template.TAB + template.Header_Right);
                    }
                    file.WriteLine("");

                    int llines = 0;
                    if (template.TextBefore_Left.Contains(Template.LINEBREAK)) llines = template.TextBefore_Left.Split(Template.LINEBREAK.ToArray(), StringSplitOptions.RemoveEmptyEntries).Length;
                    else llines = 1;

                    int rlines = 0;
                    if (template.TextBefore_Right.Contains(Template.LINEBREAK)) rlines = template.TextBefore_Right.Split(Template.LINEBREAK.ToArray(), StringSplitOptions.RemoveEmptyEntries).Length;
                    else rlines = 1;

                    if (llines > 1 && rlines > 1)
                    {
                        string[] ltext = Template.ReplacePlaceholder(template.TextBefore_Left, id, adress, worker, datecreate, datesecond).Split(Template.LINEBREAK.ToArray(), StringSplitOptions.RemoveEmptyEntries);
                        string[] rtext = Template.ReplacePlaceholder(template.TextBefore_Right, id, adress, worker, datecreate, datesecond).Split(Template.LINEBREAK.ToArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (llines != rlines)
                        {
                            if (llines > rlines)
                            {
                                int j = 0;
                                for (int i = 0; i < rlines; i++)
                                {
                                    file.WriteLine(ltext[i] + Template.TAB + Template.TAB + Template.TAB + Template.TAB + rtext[i]);
                                    j++;
                                }
                                for (int i = j; i < rlines; i++)
                                {
                                    file.WriteLine(Template.TAB + Template.TAB + Template.TAB + Template.TAB + Template.TAB + rtext[i]);
                                }
                            }
                            else
                            {
                                int j = 0;
                                for (int i = 0; i < llines; i++)
                                {
                                    file.WriteLine(ltext[i] + Template.TAB + Template.TAB + Template.TAB + Template.TAB + rtext[i]);
                                    j++;
                                }
                                for (int i = j; i < rlines; i++)
                                {
                                    file.WriteLine(Template.TAB + Template.TAB + Template.TAB + Template.TAB + Template.TAB + rtext[i]);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < llines; i++)
                            {
                                file.WriteLine(ltext[i] + Template.TAB + Template.TAB + Template.TAB + Template.TAB + rtext[i]);
                            }
                        }
                    }
                    else
                    {
                        file.WriteLine(template.TextBefore_Left + Template.TAB + Template.TAB + Template.TAB + Template.TAB + template.TextBefore_Right);
                    }
                    file.WriteLine("");
                }

                file.WriteLine(title);
                file.WriteLine("");
                if (!template.IsEmpty)
                {
                    file.WriteLine(template.BeforeContent);
                    file.WriteLine("");
                }
                switch (content.Type)
                {
                    case DataType.Table:
                        ContentTable c = (ContentTable)content as ContentTable;
                        foreach (DataColumn col in c.Table.Columns)
                        {
                            file.Write(col.Caption + Template.TAB);
                        }
                        file.WriteLine("");
                        file.WriteLine(new String('-', c.TableWidth));
                        foreach (DataRow row in c.Table.Rows)
                        {
                            string r = String.Join(Template.TAB, row.ItemArray);
                            r.Replace(Template.EMPTY, Template.TAB);
                            file.WriteLine(r);
                        }
                        file.WriteLine(new String('-', c.TableWidth));
                        break;
                    case DataType.Paragraphs:
                        ContentParagraphs p = (ContentParagraphs)content as ContentParagraphs;
                        foreach (KeyValuePair<string, string> item in p.Paragraphs)
                        {
                            file.WriteLine(item.Key);
                            file.WriteLine(item.Value);
                            file.WriteLine("");
                        }
                        break;
                    case DataType.Text:
                        ContentText t = (ContentText)content as ContentText;
                        file.WriteLine(t.Text);
                        break;
                    default:
                        break;
                }
                if (!template.IsEmpty)
                {
                    file.WriteLine("");
                    file.WriteLine(template.AfterContent);
                    file.WriteLine("");
                    if (template.Footer != null)
                    {
                        file.WriteLine(template.Footer.Replace(Template.IMAGE_TAG, "<").Replace(Template.IMAGE_END, ">").Replace(Template.LINEBREAK, "\n").Replace(Template.FULLSTARTER, ""));
                    }
                    else
                    {
                        file.WriteLine(template.Footer_Left + Template.TAB + Template.TAB + template.Footer_Center + Template.TAB + Template.TAB + template.Footer_Right);
                    }
                }
            }
            return path;
        }

        private static string exportExcel(string title, Content content, string id, string adress, string worker, string datecreate, string datesecond, Template template, string path)
        {
            throw new NotImplementedException();
        }

        private static string exportWord(string title, Content content, string id, string adress, string worker, string datecreate, string datesecond, Template template, string path)
        {
            throw new NotImplementedException();
        }

        private static string exportPDF(string title, Content content, string id, string adress, string worker, string datecreate, string datesecond, Template template, string path, string basepath, string pathtohtmltopdf)
        {
            string temp = basepath + ".html";
            path += ".pdf";
            temp = exportHTML(title, content, id, adress, worker, datecreate, datesecond, template, path, basepath, "pdf.css");

            path = PdfGenerator.HtmlToPdf(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path), temp, null, pathtohtmltopdf);



            File.Delete(temp);
            return path;
        }
    }
}
