using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using misaki.misaki.scenes.vfx;
using misaki.misakiCode.Character;
using misaki.misakiCode.Powers;

namespace misaki.misakiCode.Cards;

public class OniSlash : misakiCard
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
            return currentGust >= 5;
        }
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords => new CardKeyword[] 
    { 
        MisakiCardKeyWords.Aura 
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(9m, ValueProp.Move),
        new CardsVar(1),
    };

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
    {
        HoverTipFactory.FromPower<SwordGustPower>(),
        
    };
    public OniSlash()
        : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
        var gustPower = Owner.Creature.GetPowerAmount<SwordGustPower>();
        if (gustPower >= 5)
        {
            await PowerCmd.Apply<SwordGustPower>(choiceContext, base.Owner.Creature, -3, base.Owner.Creature, this);
            await CardPileCmd.Draw(choiceContext, 2, base.Owner);
        }
    }
    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
    }
}