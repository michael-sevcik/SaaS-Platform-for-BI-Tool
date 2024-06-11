using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using BIManagement.Modules.Users.Api;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace BIManagement.Modules.DataIntegration.MapperComponent;

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

    [Inject]
    ILogger<Mapper> Logger { get; set; } = default!;


    [Inject]
    ILogger<MapperJsInterop> MapperLogger { get; set; } = default!;

    private MapperJsInterop? mapperJSInterop;
    private IJSObjectReference? mappingEditorObjectRefernce;
    private DbModel? dbModel;

    // TODO: delete after the object 
    private static string SerializedMapping = "{\r\n  \"name\": \"EmployeeHoursWorked2\",\r\n  \"sourceEntity\": {\r\n    \"$id\": \"0\",\r\n    \"type\": \"join\",\r\n    \"name\": \"join1\",\r\n    \"joinType\": \"inner\",\r\n    \"leftSourceEntity\": {\r\n      \"$id\": \"1\",\r\n      \"type\": \"sourceTable\",\r\n      \"name\": \"TabMzdList\",\r\n      \"selectedColumns\": [\r\n        {\r\n          \"$id\": \"5\",\r\n          \"name\": \"ZamestnanecId\",\r\n          \"type\": \"int\"\r\n        },\r\n        {\r\n          \"$id\": \"4\",\r\n          \"name\": \"OdpracHod\",\r\n          \"type\": \"decimal\"\r\n        },\r\n        {\r\n          \"$id\": \"3\",\r\n          \"name\": \"HodSaz\",\r\n          \"type\": \"decimal\"\r\n        },\r\n        {\r\n          \"$id\": \"2\",\r\n          \"name\": \"IdObdobi\",\r\n          \"type\": \"int\"\r\n        }\r\n      ]\r\n    },\r\n    \"rightSourceEntity\": {\r\n      \"$id\": \"6\",\r\n      \"type\": \"sourceTable\",\r\n      \"name\": \"TabMzdObd\",\r\n      \"selectedColumns\": [\r\n        {\r\n          \"$id\": \"9\",\r\n          \"name\": \"MzdObd_DatumOd\",\r\n          \"type\": \"date\"\r\n        },\r\n        {\r\n          \"$id\": \"8\",\r\n          \"name\": \"MzdObd_DatumDo\",\r\n          \"type\": \"date\"\r\n        },\r\n        {\r\n          \"$id\": \"7\",\r\n          \"name\": \"IdObdobi\",\r\n          \"type\": \"int\"\r\n        }\r\n      ]\r\n    },\r\n    \"joinCondition\": {\r\n      \"relation\": \"equals\",\r\n      \"leftColumn\": {\r\n        \"$ref\": \"2\"\r\n      },\r\n      \"rightColumn\": {\r\n        \"$ref\": \"7\"\r\n      },\r\n      \"linkedCondition\": null\r\n    },\r\n    \"selectedColumns\": [\r\n      {\r\n        \"$ref\": \"2\"\r\n      },\r\n      {\r\n        \"$ref\": \"3\"\r\n      },\r\n      {\r\n        \"$ref\": \"4\"\r\n      },\r\n      {\r\n        \"$ref\": \"5\"\r\n      },\r\n      {\r\n        \"$ref\": \"7\"\r\n      },\r\n      {\r\n        \"$ref\": \"8\"\r\n      },\r\n      {\r\n        \"$ref\": \"9\"\r\n      }\r\n    ]\r\n  },\r\n  \"sourceEntities\": [\r\n    {\r\n      \"$id\": \"1\",\r\n      \"type\": \"sourceTable\",\r\n      \"name\": \"TabMzdList2\",\r\n      \"selectedColumns\": [\r\n        {\r\n          \"$id\": \"5\",\r\n          \"name\": \"ZamestnanecId\",\r\n          \"type\": \"int\"\r\n        },\r\n        {\r\n          \"$id\": \"4\",\r\n          \"name\": \"OdpracHod\",\r\n          \"type\": \"decimal\"\r\n        },\r\n        {\r\n          \"$id\": \"3\",\r\n          \"name\": \"HodSaz\",\r\n          \"type\": \"decimal\"\r\n        },\r\n        {\r\n          \"$id\": \"2\",\r\n          \"name\": \"IdObdobi\",\r\n          \"type\": \"int\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"$id\": \"6\",\r\n      \"type\": \"sourceTable\",\r\n      \"name\": \"TabMzdObd\",\r\n      \"selectedColumns\": [\r\n        {\r\n          \"$id\": \"9\",\r\n          \"name\": \"MzdObd_DatumOd\",\r\n          \"type\": \"date\"\r\n        },\r\n        {\r\n          \"$id\": \"8\",\r\n          \"name\": \"MzdObd_DatumDo\",\r\n          \"type\": \"date\"\r\n        },\r\n        {\r\n          \"$id\": \"7\",\r\n          \"name\": \"IdObdobi\",\r\n          \"type\": \"int\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"$id\": \"0\",\r\n      \"type\": \"join\",\r\n      \"name\": \"join1\",\r\n      \"joinType\": \"inner\",\r\n      \"leftSourceEntity\": {\r\n        \"$id\": \"1\",\r\n        \"type\": \"sourceTable\",\r\n        \"name\": \"TabMzdList\",\r\n        \"selectedColumns\": [\r\n          {\r\n            \"$id\": \"5\",\r\n            \"name\": \"ZamestnanecId\",\r\n            \"type\": \"int\"\r\n          },\r\n          {\r\n            \"$id\": \"4\",\r\n            \"name\": \"OdpracHod\",\r\n            \"type\": \"decimal\"\r\n          },\r\n          {\r\n            \"$id\": \"3\",\r\n            \"name\": \"HodSaz\",\r\n            \"type\": \"decimal\"\r\n          },\r\n          {\r\n            \"$id\": \"2\",\r\n            \"name\": \"IdObdobi\",\r\n            \"type\": \"int\"\r\n          }\r\n        ]\r\n      },\r\n      \"rightSourceEntity\": {\r\n        \"$id\": \"6\",\r\n        \"type\": \"sourceTable\",\r\n        \"name\": \"TabMzdObd\",\r\n        \"selectedColumns\": [\r\n          {\r\n            \"$id\": \"9\",\r\n            \"name\": \"MzdObd_DatumOd\",\r\n            \"type\": \"date\"\r\n          },\r\n          {\r\n            \"$id\": \"8\",\r\n            \"name\": \"MzdObd_DatumDo\",\r\n            \"type\": \"date\"\r\n          },\r\n          {\r\n            \"$id\": \"7\",\r\n            \"name\": \"IdObdobi\",\r\n            \"type\": \"int\"\r\n          }\r\n        ]\r\n      },\r\n      \"joinCondition\": {\r\n        \"relation\": \"equals\",\r\n        \"leftColumn\": {\r\n          \"$ref\": \"2\"\r\n        },\r\n        \"rightColumn\": {\r\n          \"$ref\": \"7\"\r\n        },\r\n        \"linkedCondition\": null\r\n      },\r\n      \"selectedColumns\": [\r\n        {\r\n          \"$ref\": \"2\"\r\n        },\r\n        {\r\n          \"$ref\": \"3\"\r\n        },\r\n        {\r\n          \"$ref\": \"4\"\r\n        },\r\n        {\r\n          \"$ref\": \"5\"\r\n        },\r\n        {\r\n          \"$ref\": \"7\"\r\n        },\r\n        {\r\n          \"$ref\": \"8\"\r\n        },\r\n        {\r\n          \"$ref\": \"9\"\r\n        }\r\n      ]\r\n    }\r\n  ],\r\n  \"columnMappings\": {\r\n    \"PersonalId\": {\r\n      \"$ref\": \"5\"\r\n    },\r\n    \"HoursCount\": {\r\n      \"$ref\": \"4\"\r\n    },\r\n    \"DateFrom\": {\r\n      \"$ref\": \"9\"\r\n    },\r\n    \"DateTo\": null,\r\n    \"note\": null\r\n  }\r\n}";


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

        Logger.LogDebug("Mapper component initialized for Costumer with id: {CostumerId}.", CostumerId);
    }

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool isFirst)
    {
        Logger.LogDebug(
            "Mapper component rendered. Is rendered for the first time: {isFirstRender}, mapperJSInterop {mapperJSInterop}, JsRuntime {JsRuntime}",
            isFirst,
            mapperJSInterop,
            JSRuntime);

        if (isFirst)
        {
            dbModel = await CostumerDbModelManager.GetAsync(CostumerId!);
            mapperJSInterop = new(JSRuntime, MapperLogger); // TODO: Uncomment  
            mappingEditorObjectRefernce = await mapperJSInterop!.GetMappingEditor();
        }

        if (mappingEditorObjectRefernce != null)
        {
            await mappingEditorObjectRefernce.InvokeVoidAsync("loadSerializedEntityMapping", SerializedMapping);
        }

    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (mapperJSInterop is not null)
            {
                Logger.LogDebug("MapperJSInterop is being disposed.");
                await mapperJSInterop.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {

        }
    }

}