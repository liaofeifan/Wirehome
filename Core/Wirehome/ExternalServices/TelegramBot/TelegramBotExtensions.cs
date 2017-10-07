﻿using System;
using Wirehome.Contracts.ExternalServices.TelegramBot;

namespace Wirehome.ExternalServices.TelegramBot
{
    public static class TelegramBotExtensions
    {
        public static void EnqueueResponse(this TelegramBotMessageReceivedEventArgs messageReceivedEventArgs, string text, TelegramMessageFormat format)
        {
            if (messageReceivedEventArgs == null) throw new ArgumentNullException(nameof(messageReceivedEventArgs));
            if (text == null) throw new ArgumentNullException(nameof(text));

            messageReceivedEventArgs.TelegramBotService.EnqueueMessage(
                messageReceivedEventArgs.Message.CreateResponse(text, format));
        }
    }
}