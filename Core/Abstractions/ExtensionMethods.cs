﻿using Discord;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;

namespace Dexter.Core.Abstractions {
    public static class ExtensionMethods {
        public static async Task SendEmbed(this EmbedBuilder Embed, IMessageChannel Channel) =>
            await Channel.SendMessageAsync(string.Empty, false, Embed.Build());

        public static EmbedBuilder AddField(this EmbedBuilder Embed, bool Condition, string Name, object Value) {
            if (Condition)
                Embed.AddField(Name, Value);

            return Embed;
        }

        public static EmbedBuilder GetParametersForCommand(this EmbedBuilder Embed, CommandService CommandService, string Command) {
            SearchResult Result = CommandService.Search(Command);

            foreach (CommandMatch Match in Result.Commands) {
                CommandInfo CommandInfo = Match.Command;

                string CommandDescription = $"Parameters: {string.Join(", ", CommandInfo.Parameters.Select(p => p.Name))}";

                if (CommandInfo.Parameters.Count > 0)
                    CommandDescription = $"Parameters: {string.Join(", ", CommandInfo.Parameters.Select(p => p.Name))}";
                else
                    CommandDescription = "No parameters";

                if (!string.IsNullOrEmpty(CommandInfo.Summary))
                    CommandDescription += $"\nSummary: {CommandInfo.Summary}";

                Embed.AddField(string.Join(", ", CommandInfo.Aliases), CommandDescription);
            }

            return Embed;
        }
    }
}
