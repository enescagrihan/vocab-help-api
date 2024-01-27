using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;
using InterviewAssistantAPI.Data;
using InterviewAssistantAPI.DTOs;
using InterviewAssistantAPI.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace InterviewAssistantAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TagController : ControllerBase
    {
        private readonly DataContext _appDbContext;

        public TagController(DataContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public IActionResult GetAllTags()
        {
            try
            {
                var tags = _appDbContext.Tags.ToList();
                if (tags.Count == 0)
                {
                    return NotFound("No tags available.");
                }
                return Ok(tags);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetTagById(int id)
        {
            try
            {
                var tag = _appDbContext.Tags.Find(id);
                if (tag == null)
                {
                    return BadRequest($"Tag with id {id} not found.");
                }
                return Ok(tag);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private Tag MapTagObject(TagAddDto payload)
        {
            var result = new Tag();
            result.TagName = payload.TagName;
            result.Cards = null;
            return result;
        }

        [HttpPost]
        public IActionResult AddTag(TagAddDto tag)
        {
            try
            {
                var newTag = MapTagObject(tag);
                _appDbContext.Add(newTag);
                _appDbContext.SaveChanges();
                return Ok("Language Tag Added Succesfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult EditTag(TagEditDto tag)
        {
            if (tag == null)
            {
                return BadRequest("Data is invalid");
            }
            try
            {
                var ltag = _appDbContext.Tags.Find(tag.id);
                if (ltag == null) return NotFound($"Language Tag not found with id {ltag.Id}");
                ltag.TagName = tag.TagName;
                _appDbContext.SaveChanges();
                return Ok("Language tag's name updated.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("editTagName/{id}")]
        public IActionResult AddMoreCardExample(TagEditDto tag)
        {
            try
            {
                var ltag = _appDbContext.Tags.Find(tag.id);
                if (ltag == null) return NotFound($"Language Card not found with id {tag.id}");
                ltag.TagName = tag.TagName;
                _appDbContext.SaveChanges();
                return Ok("Language card updated.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        public IActionResult DeleteTag(int id)
        {
            try
            {
                var tag = _appDbContext.Tags.Find(id);
                if (tag == null) return BadRequest($"Tag not found with id {id}");
                _appDbContext.Tags.Remove(tag);
                _appDbContext.SaveChanges();
                return Ok("Language Tag Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}