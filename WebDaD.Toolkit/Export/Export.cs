using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace WebDaD.Toolkit.Export
{
    public static class Export
    {
        private static readonly string TAB = "\t";
        /// <summary>
        /// Exports the Data given
        /// </summary>
        /// <param name="type">Export Format</param>
        /// <param name="data">A Data Object</param>
        /// <param name="template">A Template to print the Data into</param>
        /// <param name="basepath">The Basepath to create the file in</param>
        /// <returns>The Path of the created Export</returns>
        public static string DataExport(ExportType type, Exportable data, Template template, string basepath, ExportCount ec)
        {
            string path = basepath+"\\"+data.Filename(ec); //TODO: need to Add fileending

            switch (type)
            {
                case ExportType.PDF:
                    path = exportPDF(data.DataName(ec), data.ToContent(ec), template, path);
                    break;
                case ExportType.Word:
                    path = exportWord(data.DataName(ec), data.ToContent(ec), template, path);
                    break;
                case ExportType.Excel:
                    path = exportExcel(data.DataName(ec), data.ToContent(ec), template, path);
                    break;
                case ExportType.TXT:
                    path = exportTXT(data.DataName(ec), data.ToContent(ec), template, path);
                    break;
                case ExportType.CSV:
                    path = exportCSV(data.DataName(ec), data.ToContent(ec), template, path);
                    break;
                case ExportType.HTML:
                    path = exportHTML(data.DataName(ec), data.ToContent(ec), template, path);
                    break;
                case ExportType.XML:
                    path = exportXML(data.DataName(ec), data.ToContent(ec), template, path);
                    break;
                case ExportType.MD:
                    path = exportMD(data.DataName(ec), data.ToContent(ec), template, path);
                    break;
                default:
                    break;
            }

            return path;
        }

        private static string exportMD(string title, Content content, Template template, string path)
        {
            throw new NotImplementedException();
        }

        private static string exportXML(string title, Content content, Template template, string path)
        {
            throw new NotImplementedException();
        }

        private static string exportHTML(string title, Content content, Template template, string path)
        {
            throw new NotImplementedException();
        }

        private static string exportCSV(string title, Content content, Template template, string path)
        {
            throw new NotImplementedException();
        }

        private static string exportTXT(string title, Content content, Template template, string path)
        {
            path += ".txt";
            using (StreamWriter file = new StreamWriter(path))
            {
                if (!template.IsEmpty)
                {
                    file.WriteLine(template.Header_Left + TAB + TAB + template.Header_Center + TAB + TAB + template.Header_Right);
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
                            file.Write(col.Caption + TAB);
                        }
                        file.WriteLine("");
                        file.WriteLine(new String('-', c.TableWidth));
                        foreach (DataRow row in c.Table.Rows)
                        {
                            string r = String.Join(TAB, row.ItemArray);
                            r.Replace(Content.EMPTY, TAB);
                            file.WriteLine(r);
                        }
                        file.WriteLine(new String('-', c.TableWidth));
                        break;
                    case DataType.Paragraphs:
                        ContentParagraphs p = (ContentParagraphs)content as ContentParagraphs;
                        foreach (KeyValuePair<string,string> item in p.Paragraphs)
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
                    file.WriteLine(template.Footer_Left + "\t\t" + template.Footer_Center + "\t\t" + template.Footer_Right);
                }
            }
            return path;
        }

        private static string exportExcel(string title, Content content, Template template, string path)
        {
            throw new NotImplementedException();
        }

        private static string exportWord(string title, Content content, Template template, string path)
        {
            throw new NotImplementedException();
        }

        private static string exportPDF(string title, Content content, Template template, string path)
        {
            throw new NotImplementedException();
        }
    }
}
