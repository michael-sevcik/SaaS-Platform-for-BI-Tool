using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BIManagement.Modules.DataIntegration.MapperComponent
{
    public partial class Mapper : IAsyncDisposable
    {
        private string number = "";

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
            Console.WriteLine("DisposeAsync" + ((MapperJSInterop is null) ? "null" : "not null"));
            if (MapperJSInterop != null)
            {

                //await MapperJSInterop.DisposeAsync();
            }
        }

    }
}