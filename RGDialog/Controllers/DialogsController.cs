using Microsoft.AspNetCore.Mvc;
using RGDialog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RGDialog.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DialogsController : ControllerBase
    {
        private readonly List<RGDialogsClients> dialogsClients = new RGDialogsClients().Init();

        public DialogsController()
        {
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Guid))]
        public async Task<IActionResult> GetDialog([FromQuery] List<Guid> clientIds)
        {
            var dialogId = dialogsClients
                .GroupBy(dc => dc.IDRGDialog)
                .Select(gp => new { DialogId = gp.Key, ClientIds = gp.Select(dc => dc.IDClient).ToHashSet() })
                .Where(gp => clientIds.All(clientId => gp.ClientIds.Contains(clientId)))
                .Select(gp => gp.DialogId)
                .FirstOrDefault();

            return Ok(dialogId);
        }
    }
}
