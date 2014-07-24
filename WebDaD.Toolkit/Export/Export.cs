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
        private static readonly int TABLENGTH = 3;
        /// <summary>
        /// Exports the Data given
        /// </summary>
        /// <param name="type">Export Format</param>
        /// <param name="data">A Data Object</param>
        /// <param name="template">A Template to print the Data into</param>
        /// <param name="basepath">The Basepath to create the file in</param>
        /// <returns>The Path of the created Export</returns>
        public static string DataExport(ExportType type, Exportable data, Template template, string basepath)
        {
            string path = basepath+"\\"+data.Filename(); //TODO: need to Add fileending

            switch (type)
            {
                case ExportType.PDF:
                    path = exportPDF(data.DataName(), data.ToContent(), template, path);
                    break;
                case ExportType.Word:
                    path = exportWord(data.DataName(), data.ToContent(), template, path);
                    break;
                case ExportType.Excel:
                    path = exportExcel(data.DataName(), data.ToContent(), template, path);
                    break;
                case ExportType.TXT:
                    path = exportTXT(data.DataName(), data.ToContent(), template, path);
                    break;
                case ExportType.CSV:
                    path = exportCSV(data.DataName(), data.ToContent(), template, path);
                    break;
                case ExportType.HTML:
                    path = exportHTML(data.DataName(), data.ToContent(), template, path);
                    break;
                case ExportType.XML:
                    path = exportXML(data.DataName(), data.ToContent(), template, path);
                    break;
                case ExportType.MD:
                    path = exportMD(data.DataName(), data.ToContent(), template, path);
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
                file.WriteLine(template.Header_Left + TAB + TAB + template.Header_Center + TAB + TAB + template.Header_Right);
                file.WriteLine("");
                file.WriteLine(title);
                file.WriteLine("");
                file.WriteLine(template.BeforeContent);
                file.WriteLine("");
                switch (content.Type)
                {
                    case DataType.Table:
                        int col_length = 0;
                        foreach (DataColumn col in content.Table.Columns)
                        {
                            file.Write(col.Caption + TAB);
                            col_length += col.Caption.Length + TABLENGTH; //TODO: correctly guess length of this
                        }
                        file.WriteLine("");
                        file.WriteLine(new String('-', col_length));
                        foreach (DataRow row in content.Table.Rows)
                        {
                            string r = String.Join(TAB, row.ItemArray);
                            file.WriteLine(r);
                        }
                        break;
                    case DataType.Paragraphs:
                        foreach (KeyValuePair<string,string> item in content.Paragraphs)
                        {
                            file.WriteLine(item.Key);
                            file.WriteLine(item.Value);
                            file.WriteLine("");
                        }
                        break;
                    case DataType.Text:
                        file.WriteLine(content.Text);
                        break;
                    default:
                        break;
                }

                file.WriteLine("");
                file.WriteLine(template.AfterContent);
                file.WriteLine("");
                file.WriteLine(template.Footer_Left + "\t\t" + template.Footer_Center + "\t\t" + template.Footer_Right);
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
