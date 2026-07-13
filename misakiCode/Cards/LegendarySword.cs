
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using misaki.misakiCode.Character;

namespace misaki.misakiCode.Cards;
public class LegendarySword : misakiCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(20m, ValueProp.Move)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> { HoverTipFactory.FromPower<VigorPower>() };
    public LegendarySword()
        : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        int randomVigor = new System.Random().Next(1, 21);
        await PowerCmd.Apply<VigorPower>(
            choiceContext,
            base.Owner.Creature,
            randomVigor,
            base.Owner.Creature,
            null
            );
    }
    public static async Task<CardModel> CreateInHand(Player owner, ICombatState combatState, bool isPoweredUp)
    {
        var sword = combatState.CreateCard<LegendarySword>(owner);
        bool isMisaki = owner.Character is MisakiCharacter; 
        if (isMisaki)
        {
            if (isPoweredUp) 
            {
                sword.EnergyCost.SetThisCombat(Math.Max(0, sword.EnergyCost.Canonical - 1));
                sword.AddKeyword(CardKeyword.Exhaust);
            }
        }
        await CardPileCmd.AddGeneratedCardsToCombat(new[] { sword }, PileType.Hand, owner);
        return sword;
    }
    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(10m);
    }
}