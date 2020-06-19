﻿using Microsoft.AspNetCore.Mvc;

namespace ThinkingHome.Alice.Service
{
    public class AliceController : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
            return Ok();
        }
    }
}