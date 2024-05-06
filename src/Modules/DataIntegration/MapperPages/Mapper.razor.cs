using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MapperPages
{
    public partial class Mapper : IAsyncDisposable
    {
        private string number = "";

        [Inject]
        IJSRuntime JSRuntime { get; set; } = default!;
        ExampleJsInterop? mapperJSInterop { get; set; }
        protected override async Task OnAfterRenderAsync(bool isFirst)
        {
            if (isFirst && mapperJSInterop == null && JSRuntime != null)
            {
                mapperJSInterop = new(JSRuntime);

                // create a variable  in the component and assign it a value from the JSInterop
                var number = await mapperJSInterop.GetNumber();
                Console.WriteLine(number);
            }
        }      

        public async ValueTask DisposeAsync()
        {
            if (mapperJSInterop != null)
            {
                await mapperJSInterop.DisposeAsync();
            }
        }

    }
}