using System.Web.UI.WebControls;
using System.Web.UI;

namespace CustomerOrderWebForm
{
    public delegate void RenderHandler(HtmlTextWriter sw);
    public class Literal : WebControl
    {
        public event RenderHandler OnRender;

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            //do nothing
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            //do nothing 
        }

        protected override void Render(HtmlTextWriter sw)
        {
            if (OnRender != null)
            {
                OnRender(sw);
            }
        }
    }
}
