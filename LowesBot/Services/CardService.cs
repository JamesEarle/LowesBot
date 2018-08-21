using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LowesBot.Models;
using LowesBot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LowesBot.Services
{
    public static class CardService
    {
        public static async Task ShowPurchaseOrderStatusAsync(IDialogContext context, PurchaseOrderData data, ResumeAfter<IMessageActivity> callback)
        {
            var card = CardFactory.GetPurchaseOrderStatusCard(data);
            await context.PostAsync(card);
            if (callback != null)
            {
                context.Wait(callback);
            }
        }

        public static async Task ShowInvoiceStatusAsync(IDialogContext context, InvoiceData data, ResumeAfter<IMessageActivity> callback)
        {
            var card = CardFactory.GetInvoiceStatusCard(data);
            await context.PostAsync(card);
            if (callback != null)
            {
                context.Wait(callback);
            }
        }

        public static async Task ShowOrderStatusCardAsync(IDialogContext context, OrderData order, ResumeAfter<IMessageActivity> callback)
        {
            var card = CardFactory.GetOrderStatusCard(order);
            await context.PostAsync(card);
            if (callback != null)
            {
                context.Wait(callback);
            }
        }

        public static async Task ShowStoreContactCardAsync(IDialogContext context, StoreData storeInfo, ResumeAfter<IMessageActivity> callback)
        {
            var card = CardFactory.GetStoreCards(storeInfo).First();
            await context.PostAsync(card);
            if (callback != null)
            {
                context.Wait(callback);
            }
        }

        public enum Arrangement { Normal, Carousel }

        public static async Task ShowStoreContactCardsAsync(IDialogContext context, IEnumerable<StoreData> stores, Arrangement arrangement, ResumeAfter<IMessageActivity> callback)
        {
            if (arrangement == Arrangement.Normal)
            {
                foreach (var store in stores)
                {
                    await ShowStoreContactCardAsync(context, store, null);
                }
            }
            else if (arrangement == Arrangement.Carousel)
            {
                var cards = CardFactory.GetStoreCards(stores.ToArray());
                var carousel = cards.ToCarousel(context);
                await context.PostAsync(carousel);
            }
            if (callback != null)
            {
                context.Wait(callback);
            }
        }
    }
}