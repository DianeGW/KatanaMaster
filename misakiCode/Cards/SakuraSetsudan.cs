
 using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using misaki.misakiCode.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;
using misaki.misaki.scenes.vfx;
namespace misaki.misakiCode.Cards;

public class SakuraSetsudan : misakiCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(3m, ValueProp.Move),
        new DynamicVar("Power", 1m)
    };
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
    {
        HoverTipFactory.FromPower<SwordGustPower>(), 
    };
    public SakuraSetsudan() : base(0, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
    }
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithAttackerAnim("talent", 0.4f)
                .BeforeDamage(async delegate 
                {
                    await Cmd.Wait(0.9f); 
                    Slashes.Slash(cardPlay.Target);
                })
                .Execute(choiceContext);
            await PowerCmd.Apply<SwordGustPower>(
                choiceContext, 
                base.Owner.Creature, (int)DynamicVars["Power"].BaseValue, base.Owner.Creature,this);
        }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
        DynamicVars["Power"].UpgradeValueBy(1m);
    }
}