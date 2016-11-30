﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Api.Data;
using Api.Models;
using System;
using System.Globalization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class TodosController : ControllerBase
    {
        public ITodosRepository Repo { get; set; }

        public TodosController([FromServices] ITodosRepository repo)
        {
            Repo = repo;
        }

        // GET api/todos
        [HttpGet]
        public IEnumerable<TodoItem> Get()
        {
            return Repo.GetAllTodoItems();
        }

        // GET api/todos/2
        [HttpGet("{id}")]
        [Route("{id}", Name = "GetTodoItemByIdRoute")]        
        public TodoItem Get(int id)
        {
            return Repo.GetTodoItemById(id);
        }

        [HttpPost]
        public IActionResult Post([FromBody] TodoItem todoItem)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                var todoItemId = Repo.AddTodoItem(todoItem);
                var url = Url.RouteUrl("GetTodoItemByIdRoute", new { id = todoItemId }, Request.Scheme,
                    Request.Host.ToUriComponent());
                return Created(url, todoItem);

            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var idTodoItem = int.Parse(id, CultureInfo.InvariantCulture);
                if (Repo.GetTodoItemById(idTodoItem) == null) return NotFound();
                Repo.DeleteTodoItem(idTodoItem);
                return new StatusCodeResult(200);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Put(string id)
        {
            try
            {
                var idTodoItem = int.Parse(id, CultureInfo.InvariantCulture);
                if (Repo.GetTodoItemById(idTodoItem) == null) return NotFound();
                Repo.MarkTodoItemAsDone(idTodoItem);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}