using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.HoverTips;
using misaki.misakiCode.Character;
using misaki.misakiCode.Powers;

namespace misaki.misakiCode.Cards;

public class SwordTrial : misakiCard
{
    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            if (base.CombatState == null)
            {
                return false;
            }
            int currentGust = Owner?.Creature?.GetPowerAmount<SwordGustPower>() ?? 0;
            return currentGust >= 6;
        }
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords => new CardKeyword[] 
    { 
        MisakiCardKeyWords.Aura 
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new EnergyVar(1) 
    };
    public SwordTrial()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        var gustPower = Owner.Creature.GetPowerAmount<SwordGustPower>();
        bool hasEnoughGust = gustPower >= 6;
        List<MisakiStrike> strikesToMove = base.Owner.PlayerCombatState.AllCards
            .OfType<MisakiStrike>()
            .Where(c => c.Pile == null || c.Pile.Type != PileType.Hand)
            .Take(3)
            .ToList();
        if (strikesToMove.Count > 0)
        {
                if (hasEnoughGust)
                {
                    foreach (var strike in strikesToMove)
                    {
                        strike.EnergyCost.AddThisTurnOrUntilPlayed(-1, true);
                    }
                    await PowerCmd.Apply<SwordGustPower>(choiceContext, base.Owner.Creature, -6, base.Owner.Creature,
                        this);
                }
                await CardPileCmd.Add(strikesToMove, PileType.Hand);
        }
    }
    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}