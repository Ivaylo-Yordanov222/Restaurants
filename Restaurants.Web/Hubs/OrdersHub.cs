using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Restaurants.Common.Constants;
using Restaurants.Common.Table.BindingModels;
using Restaurants.Models;
using Restaurants.Services.Cooker.Interfaces;

namespace Restaurants.Web.Hubs
{
    public class OrdersHub : Hub
    {
        private readonly ICookerOrdersService cookerOrdersService;
        private readonly UserManager<User> userManager;
        private readonly string groupName = "Cookers";

        public OrdersHub(ICookerOrdersService cookerOrdersService, UserManager<User> userManager)
        {
            this.cookerOrdersService = cookerOrdersService;
            this.userManager = userManager;
        }

        public override Task OnConnectedAsync()
        {
            string userId = this.Context.UserIdentifier;
            User user = this.userManager.FindByIdAsync(userId).Result;
            if(this.userManager.IsInRoleAsync(user, BussinessLogicConstants.AdminRole).Result || 
                this.userManager.IsInRoleAsync(user, BussinessLogicConstants.CookerRole).Result)
            {
                Groups.AddToGroupAsync(Context.ConnectionId, groupName).Wait();
            }
            return base.OnConnectedAsync();
        }
        public  async Task UpdateOrder(string orderId)
        {
            int[] changedOrder =  await this.cookerOrdersService.Agree(int.Parse(orderId));
            int id = changedOrder[0];
            int status = changedOrder[1];

            await UpdateUserStatus(orderId,status);
            await Clients.Group(groupName).SendAsync("UpdateOrderStatus", id, status);
        }

        public async Task RemoveOrder(string orderId)
        {
            int[] changedOrder = await this.cookerOrdersService.Agree(int.Parse(orderId));
            int id = changedOrder[0];
            await Clients.Group(groupName).SendAsync("RemoveOrderFromList", id);
        }

        private Task UpdateUserStatus(string orderId, int status)
        {
            string userId = this.cookerOrdersService.GetUserId(int.Parse(orderId));

            return Clients.User(userId).SendAsync("ChangeUserOrderStatus", orderId, status);
        }
    }
}
