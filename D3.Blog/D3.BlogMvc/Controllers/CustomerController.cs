using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using D3.Blog.Application.Interface;
using D3.Blog.Application.ViewModels;
using D3.Blog.Domain.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace D3.BlogMvc.Controllers
{
    public class CustomerController : Controller
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly ICustomerService _customerAppService;

        public CustomerController (
            ICustomerService customerAppService,
            INotificationHandler<DomainNotification> notifications)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _customerAppService = customerAppService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
            return View(_customerAppService.GetAll());
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CustomerViewModel customerViewModel)
        {
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
            if (!ModelState.IsValid) return View(customerViewModel);
            _customerAppService.Add(customerViewModel);

            if (IsValidOperation())
                ViewBag.Sucesso = "Customer Registered!";

            return View(customerViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
            if (id == null)
            {
                return NotFound();
            }
            var customerViewModel = await _customerAppService.GetById(id.Value);
            if (customerViewModel == null)
            {
                return NotFound();
            }
            return View(customerViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CustomerViewModel customerViewModel)
        {
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
            if (!ModelState.IsValid) return View(customerViewModel);

            _customerAppService.Update(customerViewModel);

            if (IsValidOperation())
                ViewBag.Sucesso = "Customer Updated!";

            return View(customerViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
            if (id == null)
            {
                return NotFound();
            }
            var customerViewModel = await _customerAppService.GetById(id.Value);

            if (customerViewModel == null)
            {
                return NotFound();
            }

            return View(customerViewModel);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
            _customerAppService.Remove(id);
            var customerViewModel = await _customerAppService.GetById(id);
            if (!IsValidOperation())
            {
                return View(customerViewModel);
            }

            ViewBag.Sucesso = "Customer Removed!";
            return RedirectToAction("Index");
        }



        public bool IsValidOperation()
        {
            return (!_notifications.HasNotifications());
        }
    }
}