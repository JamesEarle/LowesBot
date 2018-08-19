using System;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace LowesBot.Models
{
    public static class UserData
    {
        public static UserDataProperty<Place> Location { get; set; } = new UserDataProperty<Place>(nameof(Location));
    }

    public class UserDataProperty<T>
    {
        private string _key;

        public UserDataProperty(string key)
        {
            _key = key;
        }

        public bool HasValue(IDialogContext context)
        {
            return context.UserData.ContainsKey(_key);
        }

        public bool TryRead(IDialogContext context, out T value)
        {
            if (context.UserData.TryGetValue<string>(_key, out var json))
            {
                value = JsonConvert.DeserializeObject<T>(json);
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }
        public void Write(IDialogContext context, T value)
        {
            var json = JsonConvert.SerializeObject(value);
            context.UserData.SetValue(_key, json);
        }

        public void Clear(IDialogContext context)
        {
            context.UserData.RemoveValue(_key);
        }
    }
}
