using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BIManagement.Modules.DataIntegration.MapperComponent
{
    public sealed partial class Mapper : IAsyncDisposable
    {
        [Inject]
        IJSRuntime JSRuntime { get; set; } = default!;
        MapperJsInterop? MapperJSInterop { get; set; }
        protected override async Task OnAfterRenderAsync(bool isFirst)
        {
            Console.WriteLine("OnAfterRenderAsync");
            Console.WriteLine(isFirst);
            //if (isFirst && MapperJSInterop == null && JSRuntime != null)
            {
                MapperJSInterop = new(JSRuntime); // TODO: Uncomment 

                //create a variable  in the component and assign it a value from the JSInterop
                var number = await MapperJSInterop.GetNumber();
                Console.WriteLine(number);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (MapperJSInterop != null)
            {
                await MapperJSInterop.DisposeAsync();
            }
        }

    }
}