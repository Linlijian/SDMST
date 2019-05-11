using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentBootstrap.Icons
{
    public class IconSpan : Tag
    {
        internal IconSpan(BootstrapHelper helper, string cssClass, Icon icon)
            : base(helper, "span", cssClass, icon.GetDescription())
        {
        }

        internal IconSpan(BootstrapHelper helper, string cssClass, string icon)
            : base(helper, "span", cssClass, icon)
        {
        }
    }
}
