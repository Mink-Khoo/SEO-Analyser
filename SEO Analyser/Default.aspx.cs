using SEO_Analyser.Abstraction;
using SEO_Analyser.Core;
using SEO_Analyser.Constants;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using System.Linq;

namespace SEO_Analyser
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            if (!cbIsCalculateOccuranceInText.Checked)
            {
                gvOccuranceInText.DataSource = null;
                gvOccuranceInText.DataBind();
                lblWordResult.Text = string.Empty;
            }

            if (!cbIsCalculateOccuranceInMetaTag.Checked)
            {
                gvOccuranceInMetaTag.DataSource = null;
                gvOccuranceInMetaTag.DataBind();
                lblTagResult.Text = string.Empty;
            }

            if (!cbIsCalculateExternalLink.Checked)
            {
                gvExternalLink.DataSource = null;
                gvExternalLink.DataBind();
                lblExternalLinkCount.Text = string.Empty;
            }
        }

        protected void rbTextMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvOccuranceInText.DataSource = null;
            gvOccuranceInText.DataBind();
            lblWordResult.Text = string.Empty;

            gvOccuranceInMetaTag.DataSource = null;
            gvOccuranceInMetaTag.DataBind();
            lblTagResult.Text = string.Empty;

            gvExternalLink.DataSource = null;
            gvExternalLink.DataBind();
            lblExternalLinkCount.Text = string.Empty;

            if (rbTextMode.SelectedIndex == (int)TextMode.Text)
            {
                tbInput.TextMode = TextBoxMode.MultiLine;
                tbInput.Height = 200;
                tbInput.Width = new Unit("100%");
                tbInput.Text = string.Empty;
            }
            else
            {
                tbInput.TextMode = TextBoxMode.SingleLine;
                tbInput.Height = 20;
                tbInput.Text = string.Empty;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbInput.Text))
            {
                lblError.Text = Constant.EMPTY_INPUT;
                return;
            }

            try
            {
                BaseAnalyser analyser;
                switch (rbTextMode.SelectedIndex)
                {
                    case (int)TextMode.Text:
                        analyser = new TextAnalyser(tbInput.Text, tbStopWords.Text, cbIsFilterStopWords.Checked, cbIsCalculateOccuranceInText.Checked, cbIsCalculateOccuranceInMetaTag.Checked, cbIsCalculateExternalLink.Checked);
                        break;
                    case (int)TextMode.URL:
                        analyser = new URLAnalyser(tbInput.Text, tbStopWords.Text, cbIsFilterStopWords.Checked, cbIsCalculateOccuranceInText.Checked, cbIsCalculateOccuranceInMetaTag.Checked, cbIsCalculateExternalLink.Checked);
                        break;
                    default:
                        throw new KeyNotFoundException();
                }

                CoreProcessor cp = new CoreProcessor();
                cp.AnalyzeData(analyser, out Dictionary<string, int> occuranceInTextDictionary, out Dictionary<string, int> occuranceInMetaTagDictionary, out Dictionary<string, int> ExternalLinkDictionary);

                GridView gv = new GridView();

                if (cbIsCalculateOccuranceInText.Checked)
                {
                    gv = gvOccuranceInText;
                    CreateGridView(occuranceInTextDictionary, gv);
                    lblWordResult.Text = $"Each Word In Text";
                }

                if (cbIsCalculateOccuranceInMetaTag.Checked)
                {
                    gv = gvOccuranceInMetaTag;
                    CreateGridView(occuranceInMetaTagDictionary, gv);
                    lblTagResult.Text = $"Each Word Listed In Meta Tag";
                }

                if (cbIsCalculateExternalLink.Checked)
                {
                    gv = gvExternalLink;
                    CreateGridView(ExternalLinkDictionary, gv);
                    lblExternalLinkCount.Text = $"Unique/Total External Link In Text : {ExternalLinkDictionary?.Count}/{ExternalLinkDictionary?.Sum(x => x.Value)}";
                }
            }
            catch (UriFormatException ex)
            {
                lblError.Text = Constant.INVALID_URI_FORMAT;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void CreateGridView(Dictionary<string, int> dic, GridView gv)
        {
            if (dic == null)
                return;

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]
            {
                    new DataColumn(){
                        DataType = Type.GetType("System.String"),
                        ColumnName = "Key",
                        ReadOnly = true,
                        Unique = false
                    },
                    new DataColumn()
                    {
                        DataType = Type.GetType("System.Int32"),
                        ColumnName = "Occurance",
                        ReadOnly = true,
                        Unique = false
                    }
            });

            foreach (var kvp in dic)
            {
                DataRow dr = dt.NewRow();
                dr["Key"] = kvp.Key;
                dr["Occurance"] = kvp.Value;
                dt.Rows.Add(dr);
            }

            //create a session object to cache the table data for sorting
            Session[gv.ID] = dt;
            gv.DataSource = dt;
            gv.DataBind();
        }

        protected void gvOccuranceInText_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortGridView(gvOccuranceInText, e.SortExpression);
        }

        protected void gvOccuranceInMetaTag_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortGridView(gvOccuranceInMetaTag, e.SortExpression);

        }

        protected void gvExternalLink_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortGridView(gvExternalLink, e.SortExpression);

        }

        private void SortGridView(GridView gv, string sort)
        {
            //Retrieve the table from the session object.
            DataTable dt = Session[gv.ID] as DataTable;

            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = sort + " " + GetSortDirection(sort);
                gv.DataSource = Session[gv.ID];
                gv.DataBind();
            }
        }

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        protected void gvOccuranceInText_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Width = new Unit("100%");
            e.Row.Cells[1].Width = new Unit("100%");
        }

        protected void gvOccuranceInMetaTag_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Width = new Unit("100%");
            e.Row.Cells[1].Width = new Unit("100%");
        }

        protected void gvExternalLink_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Width = new Unit("100%");
            e.Row.Cells[1].Width = new Unit("100%");
        }
    }
}