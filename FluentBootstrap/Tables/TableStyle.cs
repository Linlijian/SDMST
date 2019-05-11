using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentBootstrap
{
    [Flags]
    public enum TableStyle
    {
        [Description(Css.TableStriped)]
        Striped = 1,
        [Description(Css.TableBordered)]
        Bordered = 1 << 1,
        [Description(Css.TableHover)]
        Hover = 1 << 2,
        [Description(Css.TableCondensed)]
        Condensed = 1 << 3,
        [Description(Css.TableStripedBordered)]
        TableStripedBordered = 1 << 4,
        [Description(Css.TableStripedBorderedHover)]
        TableStripedBorderedHover = 1 << 5
    }
    [Flags]
    public enum TableType
    {
        [Description("multiselect")]
        Multiselect = 1,
        [Description("detail")]
        Detail = 1 << 1,
        [Description("editable")]
        Editable = 1 << 2
    }
}
