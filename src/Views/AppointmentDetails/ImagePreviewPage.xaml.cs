using MopsterTeams.Views.Base;
using WinofficePrimeMobile.ViewModels;

namespace MopsterTeams.Views
{
    public partial class ImagePreviewPage : BaseModalPage
    {
        public ImagePreviewPage(ImagePreviewViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
