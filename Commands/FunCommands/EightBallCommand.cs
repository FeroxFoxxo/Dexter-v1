﻿using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Dexter.Commands.FunCommands {
    public partial class FunCommands {

        [Command("8ball")]
        [Summary("Ask the Magic 8-Ball a question and it'll reach into the future to find the answers-")]
        [Alias("8-ball")]

        public async Task EightBallCommand([Remainder] string Message) {
            string Result = new Random().Next(4) == 3 ? "uncertain" : new Random(Message.GetHashCode()).Next(2) == 0 ? "yes" : "no";

            string[] Responces = FunConfiguration.EightBall[Result];

            Emote emoji = Emote.Parse(FunConfiguration.EmojiIDs[FunConfiguration.EightBallEmoji[Result]]);

            await Context.Channel.SendMessageAsync($"{Responces[new Random().Next(Responces.Length)]}, **{Context.Message.Author}** {emoji}");
        }

    }
}
