using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for PageIndex
/// </summary>
public class PageIndex
{
    //ASPX C# Class
    public string URI = "?";
    public int TotalRows = 0;
    public int CurrentPage = 1;
    public int PageSize = 50;
    public int cols = 9;
    public int StartPageButton = 1;
    public int PageButtonCount = 9;
    public PageIndex()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public int GetStartRow()
    {
        return (CurrentPage - 1) * PageSize;
    }

    public string Print()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Page: ");
        double pages = TotalRows / PageSize; // + 1;
        double tempMode = (TotalRows % PageSize);

        if (tempMode > 0)
            pages = pages + 1;

        //int pages = TotalRows / PageSize + 1;
        int i = StartPageButton;
        if (StartPageButton > 10)
        {
            sb.Append("<a href=" + URI + "&p=");
            sb.Append((i - 10).ToString());
            sb.Append("&spb=");
            sb.Append((i - 10).ToString());
            sb.Append(">...</a> ");
        }
        for (; i <= StartPageButton + PageButtonCount; i++)
        {
            if (i > pages)
                break;
            if (i != CurrentPage)
            {
                sb.Append("<a href=" + URI + "&p=");
                sb.Append(i.ToString());
                sb.Append("&spb=" + StartPageButton.ToString() + ">");
                sb.Append(i.ToString());
                sb.Append("</a> ");
            }
            else
            {
                sb.Append("<b><font color=#495C77>" + i.ToString() + "</font></b>");
                sb.Append(" ");
            }
        }
        if (i < pages)
        {
            sb.Append("<a href=" + URI + "&p=");
            sb.Append(i.ToString());
            sb.Append("&spb=");
            sb.Append(i.ToString());
            sb.Append(">...</a> ");
        }
        return sb.ToString();
    }

    ~PageIndex()
    {
    }
}