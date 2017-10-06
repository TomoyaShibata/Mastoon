using System.Windows.Media;

namespace Mastoon.Controls
{
    public class BeautifulImage : System.Windows.Controls.Image
    {
        protected override void OnRender(DrawingContext dc)
        {
            this.VisualBitmapScalingMode = BitmapScalingMode.Fant;
            base.OnRender(dc);
        }
    }
}