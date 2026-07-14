using ChronicleKeeper.Core.DTOs.Guild;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Guilds.Commands
{
    public class CreateGuildCommand : IRequest<GuildDto>
    {
        public GuildCreateDto GuildCreateDto { get; set; } = new();
    }

    public class UpdateGuildCommand : IRequest<GuildDto>
    {
        public int Id { get; set; }
        public GuildUpdateDto GuildUpdateDto { get; set; } = new();
    }

    public class DeleteGuildCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class AddGuildFactionCommand : IRequest<bool>
    {
        public int GuildId { get; set; }
        public int FactionId { get; set; }
    }

    public class RemoveGuildFactionCommand : IRequest<bool>
    {
        public int GuildId { get; set; }
        public int FactionId { get; set; }
    }

    public class AddGuildProfessionCommand : IRequest<bool>
    {
        public int GuildId { get; set; }
        public int ProfessionId { get; set; }
    }

    public class RemoveGuildProfessionCommand : IRequest<bool>
    {
        public int GuildId { get; set; }
        public int ProfessionId { get; set; }
    }

    public class AddGuildSocialClassCommand : IRequest<bool>
    {
        public int GuildId { get; set; }
        public int SocialClassId { get; set; }
    }

    public class RemoveGuildSocialClassCommand : IRequest<bool>
    {
        public int GuildId { get; set; }
        public int SocialClassId { get; set; }
    }
}
