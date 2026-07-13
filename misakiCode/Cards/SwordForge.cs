using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace misaki.misakiCode.Cards;

public class SwordForge : misakiCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => new CardKeyword[] 
    { 
        CardKeyword.Exhaust 
    };
    public SwordForge() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", 0.0f);
        decimal totalForgedDamage = 0m;
        List<CardModel> strikesToSmelt = base.Owner.PlayerCombatState.AllCards
            .Where(c =>  c.Tags.Contains(CardTag.Strike))
            .Where(c => c.Pile != null && c.Pile.Type != PileType.Exhaust)
            .ToList();
        foreach (CardModel strike in strikesToSmelt)
        {
                totalForgedDamage += strike.DynamicVars.Damage.BaseValue;
            await CardCmd.Exhaust(choiceContext, strike);
        }
        Muramasa muramasa = base.CombatState.CreateCard<Muramasa>(base.Owner);
        muramasa.SetForgedDamage(totalForgedDamage);
        await CardPileCmd.AddGeneratedCardToCombat(muramasa, PileType.Hand, base.Owner);
    }
}