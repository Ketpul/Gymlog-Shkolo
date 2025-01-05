﻿using Gymlog.Core.Contracts;
using Gymlog.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Gymlog.Constants.MessageConstants;
using static Gymlog.Constants.RoleConstants;

namespace Gymlog.Controllers
{

    public class CardController : BaseController
    {
        private readonly ICardService cardService;
        public CardController(ICardService _cardService)
        {
            cardService = _cardService;
        }

        [Authorize(Roles = AdminRole)]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Authorize(Roles = AdminRole)]
        [HttpGet]
        public IActionResult ViewCard(int? cardNumber, string? cardId)
        {

            if (cardId != null)
            {
                return RedirectToAction("CheckCard", new { cardId = cardId, check = true });

            }
            else if (cardNumber.HasValue)
            {

                return RedirectToAction("CheckCard", new { cardNumber = cardNumber.Value, cardId = cardId, check = true });
            }


            return View();
        }


        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> CheckCard(int cardNumber, string? cardId, bool check)
        {
            if (cardNumber != null && cardNumber != 0 || cardId != null)
            {
                var model = await cardService.CheckCardAsync(cardNumber, cardId, GetUserId(), check);
                if (model == null)
                {
                    TempData["UserMessageError"] = "Няма карта с този номер!";
                    return RedirectToAction("ViewCard");
                }
                return View(model);
            }
            else
            {
                TempData["UserMessageError"] = "Трябва едно от двете полета да е попълнено!";
                return RedirectToAction("ViewCard");
            }
        }


        //[HttpPost]
        //[HttpGet]
        //public async Task<IActionResult> CheckCard(int cardNumber, string phoneNumber)
        //{
        //    if (cardNumber != 0)
        //    {
        //        var model = await cardService.CheckCardAsync(cardNumber, phoneNumber, GetUserId());
        //        if (model == null)
        //        {
        //            TempData["UserMessageError"] = "Няма карта с този номер";
        //            return RedirectToAction("ViewCard");
        //        }
        //        return RedirectToAction("ViewCard", new { cardNumber = model.Id });
        //    }
        //    else if (!string.IsNullOrEmpty(phoneNumber))
        //    {
        //        var model = await cardService.CheckCardAsync(cardNumber, phoneNumber, GetUserId());
        //        if (model == null)
        //        {
        //            TempData["UserMessageError"] = "Няма потребител с този телефонен номер";
        //            return RedirectToAction("ViewCard");
        //        }
        //        return RedirectToAction("ViewCard", new { cardNumber = model.Id });
        //    }
        //    else
        //    {
        //        TempData["UserMessageError"] = "Трябва едно от двете полета да е попълнено!";
        //        return RedirectToAction("ViewCard");
        //    }
        //}

        [Authorize(Roles = AdminRole)]
        [HttpPost]
        public async Task<IActionResult> Create(string firtsName, string lastName, DateTime startData, DateTime endData, string cardId)
        {
            var card = await cardService.CreateAsync(firtsName, lastName, startData, endData, cardId);

            TempData[UserMessageSuccess] = "Картата е добавена";

            return RedirectToAction(nameof(CheckCard), new { cardNumber = card });
        }

        //[HttpGet]
        //public async Task<IActionResult> MyCard()
        //{
        //    var model = await cardService.MyCard(GetUserId());

        //    return View(model);
        //}

        [Authorize(Roles = AdminRole)]
        [HttpGet]
        public async Task<IActionResult> EditCard(int cardId)
        {
            var model = await cardService.GetViewForEdit(cardId);
            if (model == null)
            {
                TempData[UserMessageError] = "Несъществуване такава карта";
                return RedirectToAction(nameof(CheckCard), new { cardNumber = cardId });
            }

            return View(model);
        }

        [Authorize(Roles = AdminRole)]
        [HttpPost]
        public async Task<IActionResult> EditCard(EditCard card)
        {
            if (!ModelState.IsValid)
            {
                return View(card);
            }
            await cardService.EditAsync(card);

            TempData[UserMessageSuccess] = "Картата е редактирана";

            return RedirectToAction("CheckCard", new { cardNumber = card.Id, check = false });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCard(int cardId)
        {
            await cardService.DeleteCardAsync(cardId);

            TempData[UserMessageError] = "Картата е изтрита успешно!";

            return RedirectToAction(nameof(ViewCard));
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllCards(string? searchQuery, string cardStatus = "all")
        {
            ViewData["SearchQuery"] = searchQuery;
            ViewData["CardStatus"] = cardStatus;

            var cards = await cardService.ViewAllCardsAsync(searchQuery, cardStatus);
            return View(cards);
        }

        [HttpGet]
        public async Task<IActionResult> ViewCardHistory(DateTime? data)
        {
            var selectedDate = data ?? DateTime.Now;

            var cards = await cardService.ViewCardHistoryAsync(selectedDate);

            return View(cards);
        }



        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }
    }
}

