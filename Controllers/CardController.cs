using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using InterviewAssistantAPI.Data;
using InterviewAssistantAPI.DTOs;
using InterviewAssistantAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace InterviewAssistantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardController : ControllerBase
    {
        private readonly DataContext _appDbContext;

        public CardController(DataContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public IActionResult GetAllCards()
        {
            try
            {
                var cards = _appDbContext.Cards.ToList();
                if (cards.Count == 0)
                {
                    return NotFound("No cards available.");
                }
                return Ok(cards);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetLast10Records")]
        public IActionResult GetLast10Cards()
        {
            try
            {
                var cards = _appDbContext.Cards.OrderByDescending(c => c.Id).Take(10).ToList();
                if (cards.Count == 0)
                {
                    return NotFound("No cards available.");
                }
                return Ok(cards);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetRandomCard")]
        public IActionResult GetRandomCard()
        {
            try
            {
                var card = _appDbContext.Cards.Where(c => c.Known == false).OrderBy(c => Guid.NewGuid()).FirstOrDefault();
                if (card == null)
                {
                    return NotFound("No unknown cards available.");
                }
                return Ok(card);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("getByTagId/{tagId}")]
        public IActionResult GetCardByTagId(int tagId)
        {
            try
            {
                var card = _appDbContext.Cards.Where(c => c.TagId == tagId).OrderByDescending(c => c.Id).ToList();
                if (card == null)
                {
                    return BadRequest($"Cards with tag id {tagId} not found.");
                }
                return Ok(card);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCardById(int id)
        {
            try
            {
                var card = _appDbContext.Cards.Find(id);
                if (card == null)
                {
                    return BadRequest($"Card with id {id} not found.");
                }
                return Ok(card);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private Card MapCardObject(CardAddDto card)
        {
            var result = new Card();
            result.Front = card.Front;
            result.Back = card.Back;
            result.TagId = card.TagId;
            result.Example = card.Example;
            return result;
        }

        [EnableCors]
        [HttpPost]
        public IActionResult AddCard(CardAddDto card)
        {
            try
            {
                var newCard = MapCardObject(card);
                _appDbContext.Add(newCard);
                _appDbContext.SaveChanges();
                return Ok("Card Added Succesfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult EditCard(Card card)
        {
            if (card == null)
            {
                return BadRequest("Data is invalid");
            }
            try
            {
                var lcard = _appDbContext.Cards.Find(card.Id);
                if (card == null) return NotFound($"Language Card not found with id {card.Id}");
                card.Front = card.Front;
                card.Back = card.Back;
                card.TagId = card.TagId;
                card.Known = card.Known;
                card.Example = card.Example;
                _appDbContext.SaveChanges();
                return Ok("Language card updated.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("addMoreExample/{id}")]
        public IActionResult AddMoreCardExample(CardAddExampleDto cardAddExample)
        {
            try
            {
                var card = _appDbContext.Cards.Find(cardAddExample.id);
                if (card == null) return NotFound($"Language Card not found with id {cardAddExample.id}");
                card.Example = card.Example + " " + cardAddExample.sentence;
                _appDbContext.SaveChanges();
                return Ok("Language card updated.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [EnableCors]
        [HttpPut("MarkCardAsKnown/{id}")]
        public IActionResult MarkCardAsKnown(int id)
        {
            if (id == null)
            {
                return BadRequest("Data is invalid");
            }
            try
            {
                var card = _appDbContext.Cards.Find(id);
                if (card == null) return NotFound($"Language Card not found with id {id}");
                card.Known = true;
                _appDbContext.SaveChanges();
                return Ok("Language card updated.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult DeleteCard(int id)
        {
            try
            {
                var card = _appDbContext.Cards.Find(id);
                if (card == null) return BadRequest($"Card not found with id {id}");
                _appDbContext.Cards.Remove(card);
                _appDbContext.SaveChanges();
                return Ok("Language Card Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}