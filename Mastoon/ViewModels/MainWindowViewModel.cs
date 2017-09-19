using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace Mastoon.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        public DelegateCommand AlertCommand { get; }
        public InteractionRequest<INotification> AlertRequest { get; } = new InteractionRequest<INotification>();

        private string input;
        public string Input
        {
            get { return this.input; }
            set { this.SetProperty(ref this.input, value); }
        }

        public MainWindowViewModel()
        {
            this.AlertCommand = new DelegateCommand(() =>
            {
                this.AlertRequest.Raise(new Notification
                {
                    Title = "Alert",
                    Content = this.Input
                });
            });
        }
    }
}
