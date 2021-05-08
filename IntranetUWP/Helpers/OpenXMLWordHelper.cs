using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace IntranetUWP.Helpers
{
    public class OpenXMLWordHelper
    {
        public Table createWordTable()
        {
            Table tbl = new Table();
            // Set the style and width for the table.
            TableProperties tableProp = new TableProperties();
            TableStyle tableStyle = new TableStyle() { Val = "TableGrid" };
            //Table Width
            TableWidth tableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };
            // Apply Style
            tableProp.Append(tableStyle, tableWidth);
            tbl.AppendChild(tableProp);
            //// Create the table properties
            TableProperties tblProperties = new TableProperties();
            //// Create Table Borders
            TableBorders tblBorders = new TableBorders();
            TopBorder topBorder = new TopBorder();
            topBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            topBorder.Color = "Black";
            tblBorders.AppendChild(topBorder);
            BottomBorder bottomBorder = new BottomBorder();
            bottomBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            bottomBorder.Color = "Black";
            tblBorders.AppendChild(bottomBorder);
            RightBorder rightBorder = new RightBorder();
            rightBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            rightBorder.Color = "Black";
            tblBorders.AppendChild(rightBorder);
            LeftBorder leftBorder = new LeftBorder();
            leftBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            leftBorder.Color = "Black";
            tblBorders.AppendChild(leftBorder);
            InsideHorizontalBorder insideHBorder = new InsideHorizontalBorder();
            insideHBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            insideHBorder.Color = "Black";
            tblBorders.AppendChild(insideHBorder);
            InsideVerticalBorder insideVBorder = new InsideVerticalBorder();
            insideVBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            insideVBorder.Color = "Black";
            tblBorders.AppendChild(insideVBorder);
            //// Add the table borders to the properties
            tblProperties.AppendChild(tblBorders);
            //// Add the table properties to the table
            tbl.AppendChild(tblProperties);
            return tbl;
        }
        public TableRow createTableWordRow(string[] rowcontents, bool isHeader, bool isFooter)
        {
            int i = 0;
            TableRow tr = new TableRow();
            foreach (string content in rowcontents)
            {
                if(isFooter is false)
                {
                    var paragraph = isHeader == false && i == 1 ? new Paragraph(new Run(new Text(content))) 
                                            { 
                                                ParagraphProperties = new ParagraphProperties() 
                                                { 
                                                     Justification = new Justification() 
                                                     { Val = JustificationValues.Left } 
                                                } 
                                            } 
                                            : new Paragraph(new Run(new Text(content)))
                                            {
                                                ParagraphProperties = new ParagraphProperties()
                                                {
                                                    Justification = new Justification()
                                                    { Val = JustificationValues.Center }
                                                }
                                        };
                    TableCell tableCell = new TableCell(paragraph);
                    tr.Append(tableCell); i++;
                }
                else
                {
                    TableCellProperties tcp = new TableCellProperties() { GridSpan = new GridSpan() { Val = 2 } };
                    var footerParagraph = isFooter == true && i == 0 ? new Paragraph(new Run(new Text(content)))
                    {
                        ParagraphProperties = new ParagraphProperties()
                        {
                            Justification = new Justification()
                            { Val = JustificationValues.Right }
                        }
                    }
                    :
                    new Paragraph(new Run(new Text(content)))
                    {
                        ParagraphProperties = new ParagraphProperties()
                        {
                            Justification = new Justification()
                            { Val = JustificationValues.Center }
                        }
                    };
                    TableCell tableCell = new TableCell(footerParagraph);
                    tableCell.Append(tcp);
                    tr.Append(tableCell); i++;
                }
            }
            return tr;
        }
    }
}
