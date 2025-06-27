using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace StudyCards.Server.Controllers;

public class ApiControllerBase(ISender sender) : ControllerBase
{
    protected readonly ISender Sender = sender;
}
