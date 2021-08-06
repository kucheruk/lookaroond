using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace lookaroond.Controllers
{
    public interface ISlackEventHandler
    {
        bool Match(JsonElement e);
        IActionResult Handle(JsonElement e);
    }
}