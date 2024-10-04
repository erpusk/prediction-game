using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core.Pipeline;
using BackEnd.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController(ApplicationUserRepo repo) : ControllerBase()
    {
        private readonly ApplicationUserRepo _repo = repo;

    }
}