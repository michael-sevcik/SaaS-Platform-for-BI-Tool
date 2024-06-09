using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using BIManagement.Modules.Users.Api;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;

namespace BIManagement.Modules.DataIntegration.MapperComponent
{
    public sealed partial class Mapper : IAsyncDisposable
    {
        [Inject]
        IJSRuntime JSRuntime { get; set; } = default!;

        [Inject]
        ICostumerDbModelManager CostumerDbModelManager { get; set; } = default!;

        [Inject]
        IUserAccessor UserAccessor { get; set; } = default!;

        [Inject]
        IHttpContextAccessor HttpContextAccessor { get; set; } = default!;

        MapperJsInterop? mapperJSInterop { get; set; }

        DbModel? dbModel { get; set; }

        /// <summary>
        /// The Costumer id to use for the component.
        /// Initialized either by the parent component or by the user id of the current Costumer.
        /// </summary>
        [Parameter]
        public string? CostumerId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (CostumerId == null)
            {
                var userIdResult = await UserAccessor.GetCostumerId(
                    HttpContextAccessor.HttpContext ?? throw new InvalidOperationException("Http context is not accesible."));
                if (userIdResult.IsFailure)
                {
                    throw new InvalidOperationException(userIdResult.Error.Message);
                }

                CostumerId = userIdResult.Value;
            }
        }

        protected override async Task OnAfterRenderAsync(bool isFirst)
        {
            if (!isFirst)
            {
                return;
            }
            
            dbModel = await CostumerDbModelManager.GetAsync(CostumerId!);

            if (mapperJSInterop == null && JSRuntime != null)
            {
                mapperJSInterop = new(JSRuntime); // TODO: Uncomment 

                //create a variable  in the component and assign it a value from the JSInterop
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