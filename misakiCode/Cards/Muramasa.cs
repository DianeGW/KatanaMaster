using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
//using misaki.misaki.scenes.vfx;

namespace misaki.misakiCode.Cards;

public class Muramasa : misakiCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => new CardKeyword[] 
    { 
        CardKeyword.Exhaust 
    };
    private decimal _currentDamage = 0m; 
    public Muramasa() : base(1, CardType.Attack, CardRarity.Token, TargetType.AllEnemies)
    {
    }
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(0m, ValueProp.Move), 
    };
    public void SetForgedDamage(decimal amount)
    {
        base.DynamicVars.Damage.BaseValue = amount;
        _currentDamage = amount; 
    }
    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        base.DynamicVars.Damage.BaseValue = _currentDamage;
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "attack", 0.0f);
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(base.CombatState)
            .Execute(choiceContext);
    }
}