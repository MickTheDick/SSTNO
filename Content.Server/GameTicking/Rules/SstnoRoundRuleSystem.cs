using Content.Server.GameTicking.Rules.Components;
using Content.Shared.GameTicking;
using Content.Shared.GameTicking.Rules;

namespace Content.Server.GameTicking.Rules;

/// <summary>
/// SSTNO round rule. It does not spawn antagonists, shuttles, or station events.
/// </summary>
public sealed class SstnoRoundRuleSystem : GameRuleSystem<SstnoRoundRuleComponent>
{
    protected override void AppendRoundEndText(Entity<SstnoRoundRuleComponent> rule, ref RoundEndTextAppendEvent args)
    {
        args.AddLine(Loc.GetString("sstno-round-end-text"));
    }
}
