﻿using Wirehome.Extensions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Wirehome.Contracts.Core;

namespace Wirehome.Extensions.Messaging.SamsungMessages
{
    public class SamsungControlMessage : IBinaryMessage
    {
        public string Address { get; set; }
        public string Code { get; set; }
        public int Port { get; set; } = 55000;

        private readonly string AppKey = "Wirehome";
        private readonly string NullValue = char.ToString((char)0x00);
        private readonly string AppString = "samsung.remote";

        private byte[] CreateIdentifier()
        {
            var myIpBase64 = Base64Encode("192.168.1.35");
            var myMacBase64 = Base64Encode("0C-89-10-CD-43-28");
            var nameBase64 = Base64Encode(AppKey);

            var message =
                char.ToString((char)0x64) +
                NullValue +
                Format(myIpBase64.Length) +
                NullValue +
                myIpBase64 +
                Format(myMacBase64.Length) +
                NullValue +
                myMacBase64 +
                Format(nameBase64.Length) +
                NullValue +
                nameBase64;
            var wrappedMessage = NullValue + Format(AppString.Length) + NullValue + AppString + Format(message.Length) + NullValue + message;

            return ConvertToBytes(wrappedMessage);
        }

        private byte[] CreateSecondParameter()
        {
            var message = ((char)0xc8) + ((char)0x00) + string.Empty;

            var wrappedMessage = NullValue + Format(AppString.Length) + NullValue + AppString + Format(message.Length) + NullValue + message;
            return ConvertToBytes(wrappedMessage);
        }

        private byte[] CreateCommand(string command)
        {
            var encodedCommand = Base64Encode(command);

            var message = NullValue + NullValue + NullValue + char.ToString((char)encodedCommand.Length) + NullValue + encodedCommand;
            var wrappedMessage = NullValue + Format(AppString.Length) + NullValue + AppString + Format(message.Length) + NullValue + message;

            return ConvertToBytes(wrappedMessage);
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private static byte[] ConvertToBytes(string value)
        {
            return Encoding.ASCII.GetBytes(value);
        }

        private static string Format(int value)
        {
            return char.ToString((char)value);
        }

        public bool CanDeserialize(byte messageType, byte messageSize)
        {
            return false;
        }

        public bool CanSerialize(string messageType)
        {
            return messageType == GetType().Name; 
        }

        public object Deserialize(IBinaryReader reader, byte? messageSize = null)
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize()
        {
            var identifier = CreateIdentifier();
            var secondParameter = CreateSecondParameter();
            var command = CreateCommand(Code);

            var binaryMessage = new List<byte>();
            binaryMessage.AddRange(identifier);
            binaryMessage.AddRange(secondParameter);
            binaryMessage.AddRange(command);
            
            return binaryMessage.ToArray();
        }

        public MessageType Type()
        {
            return MessageType.Samsung;
        }

        public string MessageAddress()
        {
            return $"{Address}:{Port}";
        }
    }
}