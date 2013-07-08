/**
 * A Professional HTML Renderer You Will Use
 * 
 * The BSD License (BSD)
 * Copyright (c) 2011 Jose Menendez Póo, http://www.codeproject.com/Articles/32376/A-Professional-HTML-Renderer-You-Will-Use
 * 
 * Redistribution and use in source and binary forms, with or without modification, are 
 * permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list of 
 * conditions and the following disclaimer.
 * 
 * Redistributions in binary form must reproduce the above copyright notice, this list of 
 * conditions and the following disclaimer in the documentation and/or other materials 
 * provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
 * SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
 * TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR 
 * BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF 
 * THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace MetroFramework.Drawing.Html
{
    internal class CssTable
    {
        #region Subclasses

        /// <summary>
        /// Used to make space on vertical cell combination
        /// </summary>
        public class SpacingBox
            : CssBox
        {
            public readonly CssBox ExtendedBox;

            public SpacingBox(CssBox tableBox, ref CssBox extendedBox, int startRow)
                : base(tableBox, new HtmlTag("<none colspan=" + extendedBox.GetAttribute("colspan", "1") + ">"))
            {
                ExtendedBox = extendedBox;
                Display = CssConstants.None;

                _startRow = startRow;
                _endRow = startRow + int.Parse(extendedBox.GetAttribute("rowspan", "1")) - 1;
            }

            #region Props

            private int _startRow;
            /// <summary>
            /// Gets the index of the row where box starts
            /// </summary>
            public int StartRow
            {
                get { return _startRow; }
            }

            private int _endRow;

            /// <summary>
            /// Gets the index of the row where box ends
            /// </summary>
            public int EndRow
            {
                get { return _endRow; }
            }


            #endregion
        }

        #endregion

        #region Fields

        private CssBox _tableBox;
        private int _rowCount;
        private int _columnCount;
        private List<CssBox> _bodyrows;
        private CssBox _caption;
        private List<CssBox> _columns;
        private CssBox _headerBox;
        private CssBox _footerBox;
        private List<CssBox> _allRows;
        private float[] _columnWidths;
        private bool _widthSpecified;
        private float[] _columnMinWidths;

        #endregion

        #region Ctor

        private CssTable()
        {
            _bodyrows = new List<CssBox>();
            _columns = new List<CssBox>();
            _allRows = new List<CssBox>();
        }

        public CssTable(CssBox tableBox, Graphics g)
            :this()
        {
            if (!(tableBox.Display == CssConstants.Table || tableBox.Display == CssConstants.InlineTable))
                throw new ArgumentException("Box is not a table", "tableBox");

            _tableBox = tableBox;

            MeasureWords(tableBox, g);

            Analyze(g);
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets if the user specified a width for the table
        /// </summary>
        public bool WidthSpecified
        {
            get { return _widthSpecified; }
        }

        /// <summary>
        /// Hosts a list of all rows in the table, including those on the TFOOT, THEAD and TBODY
        /// </summary>
        public List<CssBox> AllRows
        {
            get { return _allRows; }
        }

        /// <summary>
        /// Gets the box that represents the caption of this table, if any.
        /// WARNING: May be null
        /// </summary>
        public CssBox Caption
        {
            get { return _caption; }
        }

        /// <summary>
        /// Gets the column count of this table
        /// </summary>
        public int ColumnCount
        {
            get { return _columnCount; }
        }

        /// <summary>
        /// Gets the minimum width of each column
        /// </summary>
        public float[] ColumnMinWidths
        {
            get 
            {
                if (_columnMinWidths == null)
                {
                    _columnMinWidths = new float[ColumnWidths.Length];

                    foreach (CssBox row in AllRows)
                    {
                        foreach (CssBox cell in row.Boxes)
                        {
                            int colspan = GetColSpan(cell);
                            int col = GetCellRealColumnIndex(row, cell);
                            int affectcol = col + colspan - 1;
                            float spannedwidth = GetSpannedMinWidth(row, cell, col, colspan) + (colspan - 1) * HorizontalSpacing;

                            _columnMinWidths[affectcol] = Math.Max(_columnMinWidths[affectcol], cell.GetMinimumWidth() - spannedwidth);

                        }
                    }

                }

                return _columnMinWidths; 
            }
        }

        /// <summary>
        /// Gets the declared Columns on the TABLE tag
        /// </summary>
        public List<CssBox> Columns
        {
            get { return _columns; }
        }

        /// <summary>
        /// Gets an array indicating the withs of each column.
        /// This must have the same count than <see cref="Columns"/>
        /// </summary>
        public float[] ColumnWidths
        {
            get { return _columnWidths; }
        }

        /// <summary>
        /// Gets the boxes that represents the table-row Boxes of the table, 
        /// including those inside of the TBODY tags
        /// </summary>
        public List<CssBox> BodyRows
        {
            get { return _bodyrows; }
        }

        /// <summary>
        /// Gets the table-footer-group Box
        /// WARNING: May be null
        /// </summary>
        public CssBox FooterBox
        {
            get { return _footerBox; }
        }

        /// <summary>
        /// Gets the table-header-group Box
        /// WARNING: May be null
        /// </summary>
        public CssBox HeaderBox
        {
            get { return _headerBox; }
        }

        /// <summary>
        /// Gets the actual horizontal spacing of the table
        /// </summary>
        public float HorizontalSpacing
        {
            get 
            {
                if (TableBox.BorderCollapse == CssConstants.Collapse)
                {
                    return -1f;
                }

                return TableBox.ActualBorderSpacingHorizontal;
            }
        }

        /// <summary>
        /// Gets the actual vertical spacing of the table
        /// </summary>
        public float VerticalSpacing
        {
            get
            {
                if (TableBox.BorderCollapse == CssConstants.Collapse)
                {
                    return -1f;
                }

                return TableBox.ActualBorderSpacingVertical;
            }
        }

        /// <summary>
        /// Gets the row count of this table, including the rows inside the table-row-group,
        /// table-row-heaer and table-row-footer Boxes
        /// </summary>
        public int RowCount
        {
            get { return _rowCount; }
        }

        /// <summary>
        /// Gets the original table box
        /// </summary>
        public CssBox TableBox
        {
            get { return _tableBox; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Analyzes the Table and assigns values to this CssTable object.
        /// To be called from the constructor
        /// </summary>
        private void Analyze(Graphics g)
        {
            float availSpace = GetAvailableWidth();
            float availCellSpace = float.NaN; //Will be set later

            #region Assign box kinds
            foreach (CssBox b in TableBox.Boxes)
            {
                b.RemoveAnonymousSpaces();
                switch (b.Display)
                {
                    case CssConstants.TableCaption:
                        _caption = b;
                        break;
                    case CssConstants.TableColumn:
                        for (int i = 0; i < GetSpan(b); i++)
                        {
                            Columns.Add(CreateColumn(b));
                        }
                        break;
                    case CssConstants.TableColumnGroup:
                        if (b.Boxes.Count == 0)
                        {
                            int gspan = GetSpan(b);
                            for (int i = 0; i < gspan; i++)
                            {
                                Columns.Add(CreateColumn(b));
                            }
                        }
                        else
                        {
                            foreach (CssBox bb in b.Boxes)
                            {
                                int bbspan = GetSpan(bb);
                                for (int i = 0; i < bbspan; i++)
                                {
                                    Columns.Add(CreateColumn(bb));
                                }
                            }
                        }
                        break;
                    case CssConstants.TableFooterGroup:
                        if (FooterBox != null)
                            BodyRows.Add(b);
                        else
                            _footerBox = b;
                        break;
                    case CssConstants.TableHeaderGroup:
                        if (HeaderBox != null)
                            BodyRows.Add(b);
                        else
                            _headerBox = b;
                        break;
                    case CssConstants.TableRow:
                        BodyRows.Add(b);
                        break;
                    case CssConstants.TableRowGroup:
                        foreach (CssBox bb in b.Boxes)
                            if (b.Display == CssConstants.TableRow)
                                BodyRows.Add(b);
                        break;
                    default:
                        break;
                }
            } 
            #endregion

            #region Gather AllRows

            if (HeaderBox != null) _allRows.AddRange(HeaderBox.Boxes);
            _allRows.AddRange(BodyRows);
            if (FooterBox != null) _allRows.AddRange(FooterBox.Boxes);

            #endregion

            #region Insert EmptyBoxes for vertical cell spanning

            if (!TableBox.TableFixed)
            {
                int currow = 0;
                int curcol = 0;
                List<CssBox> rows = BodyRows;

                foreach (CssBox row in rows)
                {
                    row.RemoveAnonymousSpaces();
                    curcol = 0;
                    for(int k = 0; k < row.Boxes.Count ; k++)
                    {

                        CssBox cell = row.Boxes[k];
                        int rowspan = GetRowSpan(cell);
                        int realcol = GetCellRealColumnIndex(row, cell); //Real column of the cell

                        for (int i = currow + 1; i < currow + rowspan; i++)
                        {
                            int colcount = 0;
                            for (int j = 0; j <= rows[i].Boxes.Count; j++)
                            {
                                if (colcount == realcol)
                                {
                                    rows[i].Boxes.Insert(colcount, new SpacingBox(TableBox, ref cell, currow));
                                    break;
                                }
                                colcount++;
                                realcol -= GetColSpan(rows[i].Boxes[j]) - 1;
                            }

                        } // End for (int i = currow + 1; i < currow + rowspan; i++)
                        curcol++;
                    } /// End foreach (Box cell in row.Boxes)
                    currow++;
                } /// End foreach (Box row in rows)

                TableBox.TableFixed = true;

            } /// End if (!TableBox.TableFixed)

            #endregion

            #region Determine Row and Column Count, and ColumnWidths

            //Rows
            _rowCount = BodyRows.Count +
                (HeaderBox != null ? HeaderBox.Boxes.Count : 0) +
                (FooterBox != null ? FooterBox.Boxes.Count : 0);

            //Columns
            if (Columns.Count > 0)
                _columnCount = Columns.Count;
            else
                foreach (CssBox b in AllRows) //Check trhough rows
                    _columnCount = Math.Max(_columnCount, b.Boxes.Count);

            //Initialize column widths array
            _columnWidths = new float[_columnCount];

            //Fill them with NaNs
            for (int i = 0; i < _columnWidths.Length; i++)
                _columnWidths[i] = float.NaN;

            availCellSpace = GetAvailableCellWidth();

            if (Columns.Count > 0)
            {
                #region Fill ColumnWidths array by scanning column widths

                for (int i = 0; i < Columns.Count; i++)
                {
                    CssLength len = new CssLength(Columns[i].Width); //Get specified width

                    if (len.Number > 0) //If some width specified
                    {
                        if (len.IsPercentage)//Get width as a percentage
                        {
                            ColumnWidths[i] = CssValue.ParseNumber(Columns[i].Width, availCellSpace);
                        }
                        else if (len.Unit == CssLength.CssUnit.Pixels || len.Unit == CssLength.CssUnit.None)
                        {
                            ColumnWidths[i] = len.Number; //Get width as an absolute-pixel value
                        }
                    }
                }

                #endregion
            }
            else
            {
                #region Fill ColumnWidths array by scanning width in table-cell definitions
                foreach (CssBox row in AllRows)
                {
                    //Check for column width in table-cell definitions
                    for (int i = 0; i < _columnCount; i++)
                    {
                        if (float.IsNaN(ColumnWidths[i]) &&                 //Check if no width specified for column
                            i < row.Boxes.Count &&                          //And there's a box to check
                            row.Boxes[i].Display == CssConstants.TableCell)//And the box is a table-cell
                        {
                            CssLength len = new CssLength(row.Boxes[i].Width); //Get specified width
                            
                            if (len.Number > 0) //If some width specified
                            {
                                int colspan = GetColSpan(row.Boxes[i]);
                                float flen = 0f;
                                if (len.IsPercentage)//Get width as a percentage
                                {
                                    flen = CssValue.ParseNumber(row.Boxes[i].Width, availCellSpace);
                                }
                                else if (len.Unit == CssLength.CssUnit.Pixels || len.Unit == CssLength.CssUnit.None)
                                {
                                    flen = len.Number; //Get width as an absolute-pixel value
                                }
                                flen /= Convert.ToSingle(colspan);

                                for (int j = i; j < i + colspan; j++)
                                {
                                    ColumnWidths[j] = flen;
                                }
                            }
                        }
                    }
                }
                #endregion
            }

            #endregion

            #region Determine missing Column widths

            if (WidthSpecified) //If a width was specified,
            {
                //Assign NaNs equally with space left after gathering not-NaNs
                int numberOfNans = 0;
                float occupedSpace = 0f;

                //Calculate number of NaNs and occuped space
                for (int i = 0; i < ColumnWidths.Length; i++)
                    if (float.IsNaN(ColumnWidths[i]))
                        numberOfNans++;
                    else
                        occupedSpace += ColumnWidths[i];

                //Determine width that will be assigned to un asigned widths
                float nanWidth = (availCellSpace - occupedSpace) / Convert.ToSingle(numberOfNans);

                for (int i = 0; i < ColumnWidths.Length; i++)
                    if (float.IsNaN(ColumnWidths[i]))
                        ColumnWidths[i] = nanWidth;
            }
            else
            {
                //Assign NaNs using full width
                float[] _maxFullWidths = new float[ColumnWidths.Length];

                //Get the maximum full length of NaN boxes
                foreach (CssBox row in AllRows)
                {
                    for (int i = 0; i < row.Boxes.Count; i++)
                    {
                        int col = GetCellRealColumnIndex(row, row.Boxes[i]);

                        if (float.IsNaN(ColumnWidths[col]) &&
                            i < row.Boxes.Count &&
                            GetColSpan(row.Boxes[i]) == 1)
                        {
                            _maxFullWidths[col] = Math.Max(_maxFullWidths[col], row.Boxes[i].GetFullWidth(g));
                        }
                    }
                }

                for (int i = 0; i < ColumnWidths.Length; i++)
                    if (float.IsNaN(ColumnWidths[i]))
                        ColumnWidths[i] = _maxFullWidths[i];
            }

            #endregion

            #region Reduce widths if necessary

            int curCol = 0;
            float reduceAmount = 1f;

            //While table width is larger than it should, and width is reductable
            while (GetWidthSum() > GetAvailableWidth() && CanReduceWidth())
            {
                while (!CanReduceWidth(curCol)) curCol++;

                ColumnWidths[curCol] -= reduceAmount;

                curCol++;

                if (curCol >= ColumnWidths.Length) curCol = 0;
            }

            #endregion

            #region Check for minimum sizes (increment widths if necessary)

            foreach (CssBox row in AllRows)
            {
                foreach (CssBox cell in row.Boxes)
                {
                    int colspan = GetColSpan(cell);
                    int col = GetCellRealColumnIndex(row, cell);
                    int affectcol = col + colspan - 1;
                    
                    if (ColumnWidths[col] < ColumnMinWidths[col])
                    {
                        float diff = ColumnMinWidths[col] - ColumnWidths[col];
                        ColumnWidths[affectcol] = ColumnMinWidths[affectcol];

                        if (col < ColumnWidths.Length - 1)
                        {
                            ColumnWidths[col + 1] -= diff;
                        }
                    }
                }
            }

            #endregion

            #region Set table padding

            TableBox.Padding = "0"; //Ensure there's no padding

            #endregion

            #region Layout cells

            //Actually layout cells!
            float startx = TableBox.ClientLeft + HorizontalSpacing;
            float starty = TableBox.ClientTop + VerticalSpacing;
            float curx = startx;
            float cury = starty;
            float maxRight = startx;
            float maxBottom = 0f;
            int currentrow = 0;

            foreach (CssBox row in AllRows)
            {
                if (row is CssAnonymousSpaceBlockBox || row is CssAnonymousSpaceBox) continue;

                curx = startx;
                curCol = 0;

                foreach (CssBox cell in row.Boxes)
                {
                    if (curCol >= ColumnWidths.Length) break;

                    int rowspan = GetRowSpan(cell);
                    float width = GetCellWidth(GetCellRealColumnIndex(row, cell), cell);

                    cell.Location = new PointF(curx, cury);
                    cell.Size = new SizeF(width, 0f);
                    cell.MeasureBounds(g); //That will automatically set the bottom of the cell

                    //Alter max bottom only if row is cell's row + cell's rowspan - 1
                    SpacingBox sb = cell as SpacingBox;
                    if (sb != null)
                    {
                        if (sb.EndRow == currentrow)
                        {
                            maxBottom = Math.Max(maxBottom, sb.ExtendedBox.ActualBottom);
                        }
                    }
                    else if(rowspan == 1)
                    {
                        maxBottom = Math.Max(maxBottom, cell.ActualBottom);
                    }
                    maxRight = Math.Max(maxRight, cell.ActualRight);
                    curCol++;
                    curx = cell.ActualRight + HorizontalSpacing;
                }

                foreach (CssBox cell in row.Boxes)
                {
                    SpacingBox spacer = cell as SpacingBox;

                    if (spacer == null && GetRowSpan(cell) == 1)
                    {
                        cell.ActualBottom = maxBottom;
                        CssLayoutEngine.ApplyCellVerticalAlignment(g, cell);
                    }
                    else if(spacer != null && spacer.EndRow == currentrow)
                    {
                        spacer.ExtendedBox.ActualBottom = maxBottom;
                        CssLayoutEngine.ApplyCellVerticalAlignment(g, spacer.ExtendedBox);
                    }
                }

                cury = maxBottom + VerticalSpacing;
                currentrow++;
            }

            TableBox.ActualRight = maxRight + HorizontalSpacing + TableBox.ActualBorderRightWidth;
            TableBox.ActualBottom = maxBottom + VerticalSpacing + TableBox.ActualBorderBottomWidth;

            #endregion
        }

        /// <summary>
        /// Gets the spanned width of a cell
        /// (With of all columns it spans minus one)
        /// </summary>
        /// <param name="row"></param>
        /// <param name="cell"></param>
        /// <param name="realcolindex"></param>
        /// <param name="colspan"></param>
        /// <returns></returns>
        private float GetSpannedMinWidth(CssBox row, CssBox cell, int realcolindex, int colspan)
        {
            float w = 0f;

            for (int i = realcolindex; i < row.Boxes.Count || i < realcolindex + colspan - 1; i++)
            {
                w += ColumnMinWidths[i];
            }

            return w;
        }

        /// <summary>
        /// Gets the cell column index checking its position and other cells colspans
        /// </summary>
        /// <param name="row"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        private int GetCellRealColumnIndex(CssBox row, CssBox cell)
        {
            int i = 0;

            foreach (CssBox b in row.Boxes)
            {
                if (b.Equals(cell)) break;
                i += GetColSpan(b);
            }

            return i;
        }

        /// <summary>
        /// Gets the cells width, taking colspan and being in the specified column
        /// </summary>
        /// <param name="column"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private float GetCellWidth(int column, CssBox b)
        {
            float colspan = Convert.ToSingle(GetColSpan(b));
            float sum = 0f;

            for (int i = column; i < column + colspan; i++)
            {
                if (column >= ColumnWidths.Length) break;
                if (ColumnWidths.Length <= i) break;
                sum += ColumnWidths[i]; 
            }

            sum += (colspan - 1) * HorizontalSpacing;

            return sum; ;// -b.ActualBorderLeftWidth - b.ActualBorderRightWidth - b.ActualPaddingRight - b.ActualPaddingLeft;
        }

        /// <summary>
        /// Gets the colspan of the specified box
        /// </summary>
        /// <param name="b"></param>
        private int GetColSpan(CssBox b)
        {
            string att = b.GetAttribute("colspan", "1");
            int colspan;

            if (!int.TryParse(att, out colspan))
            {
                return 1;
            }

            return colspan;
        }

        /// <summary>
        /// Gets the rowspan of the specified box
        /// </summary>
        /// <param name="b"></param>
        private int GetRowSpan(CssBox b)
        {
            string att = b.GetAttribute("rowspan", "1");
            int rowspan;

            if (!int.TryParse(att, out rowspan))
            {
                return 1;
            }

            return rowspan;
        }

        /// <summary>
        /// Recursively measures the specified box
        /// </summary>
        /// <param name="b"></param>
        /// <param name="g"></param>
        private void Measure(CssBox b, Graphics g)
        {
            if (b == null) return;

            foreach (CssBox bb in b.Boxes)
            {
                bb.MeasureBounds(g);
                Measure(bb, g);
            }
        }

        /// <summary>
        /// Recursively measures words inside the box
        /// </summary>
        /// <param name="b"></param>
        /// <param name="g"></param>
        private void MeasureWords(CssBox b, Graphics g)
        {
            if (b == null) return;

            foreach (CssBox bb in b.Boxes)
            {
                bb.MeasureWordsSize(g);
                MeasureWords(bb, g);
            }
        }

        /// <summary>
        /// Gets the number of reductable columns
        /// </summary>
        /// <returns></returns>
        private int GetReductableColumns()
        {
            int response = 0;

            for (int i = 0; i < ColumnWidths.Length; i++)
                if (CanReduceWidth(i))
                    response++;

            return response;
        }

        /// <summary>
        /// Tells if the columns widths can be reduced,
        /// by checking the minimum widths of all cells
        /// </summary>
        /// <returns></returns>
        private bool CanReduceWidth()
        {
            for (int i = 0; i < ColumnWidths.Length; i++)
            {
                if (CanReduceWidth(i))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Tells if the specified column can be reduced,
        /// by checking its minimum width
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private bool CanReduceWidth(int columnIndex)
        {
            if (ColumnWidths.Length >= columnIndex || ColumnMinWidths.Length >= columnIndex) return false;
            return ColumnWidths[columnIndex] > ColumnMinWidths[columnIndex];
        }

        /// <summary>
        /// Gets the available width for the whole table.
        /// It also sets the value of <see cref="WidthSpecified"/>
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// The table's width can be larger than the result of this method, because of the minimum 
        /// size that individual boxes.
        /// </remarks>
        private float GetAvailableWidth()
        {
            CssLength tblen = new CssLength(TableBox.Width);

            if (tblen.Number > 0)
            {
                _widthSpecified = true;

                if (tblen.IsPercentage)
                {
                    return CssValue.ParseNumber(tblen.Length, TableBox.ParentBox.AvailableWidth);
                }
                else
                {
                    return tblen.Number;
                }
            }
            else
            {
                return TableBox.ParentBox.AvailableWidth;
            }
        }

        /// <summary>
        /// Gets the width available for cells
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// It takes away the cell-spacing from <see cref="GetAvailableWidth()"/>
        /// </remarks>
        private float GetAvailableCellWidth()
        {
            return GetAvailableWidth() - 
                HorizontalSpacing * (ColumnCount + 1) -
                TableBox.ActualBorderLeftWidth - TableBox.ActualBorderRightWidth;
        }

        /// <summary>
        /// Gets the current sum of column widths
        /// </summary>
        /// <returns></returns>
        private float GetWidthSum()
        {
            float f = 0f;

            for (int i = 0; i < ColumnWidths.Length; i++)
                if (float.IsNaN(ColumnWidths[i]))
                    throw new Exception("CssTable Algorithm error: There's a NaN in column widths");
                else
                    f += ColumnWidths[i];
            
            //Take cell-spacing
            f += HorizontalSpacing * (ColumnWidths.Length + 1);

            //Take table borders
            f += TableBox.ActualBorderLeftWidth + TableBox.ActualBorderRightWidth;

            return f;
        }

        /// <summary>
        /// Gets the span attribute of the tag of the specified box
        /// </summary>
        /// <param name="b"></param>
        private int GetSpan(CssBox b)
        {
            float f = CssValue.ParseNumber(b.GetAttribute("span"), 1);

            return Math.Max(1, Convert.ToInt32(f));
        }

        /// <summary>
        /// Creates the column with the specified width
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        private CssBox CreateColumn(CssBox modelBox)
        {
            return modelBox;
            //Box b = new Box(null, new HtmlTag(string.Format("<COL style=\"width:{0}\" >", width)));
            //b.Width = width;
            //return b;
        }

        #endregion
    }
}

