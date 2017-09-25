using System;
using Reactive.Bindings;

namespace Mastoon.ViewModels
{
    public class BindableTextViewModel
    {
        public ReactiveProperty<string> Text { get; set; }
    }

    public class HyperlinkViewModel : BindableTextViewModel
    {
        public ReactiveProperty<Uri> Uri { get; set; }
    }
}