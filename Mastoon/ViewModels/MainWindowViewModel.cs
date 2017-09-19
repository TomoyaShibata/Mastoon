using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace Mastoon.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        public DelegateCommand PostCommand { get; }
        public InteractionRequest<INotification> PostRequest { get; } = new InteractionRequest<INotification>();

        private string input;
        public string Input
        {
            get { return this.input; }
            set { this.SetProperty(ref this.input, value); }
        }

        public MainWindowViewModel()
        {
            this.PostCommand = new DelegateCommand(() =>
            {
                this.PostRequest.Raise(new Notification
                {
                    Title = "Post content",
                    Content = this.Input
                });
            });
        }
    }
}
