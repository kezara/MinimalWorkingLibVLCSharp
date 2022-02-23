using MinimalWorkingLibVLCSharp.ViewModel;
using Stylet;
using StyletIoC;

namespace MinimalWorkingLibVLCSharp
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            base.ConfigureIoC(builder);
            builder.Bind<WindowViewManager>().ToSelf().InSingletonScope();
            builder.Bind<LibVLCSingleton>().ToSelf().InSingletonScope();
        }
    }
}
