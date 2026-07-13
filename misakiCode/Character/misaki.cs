using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using misaki.misakiCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using misaki.misakiCode.Cards;
using misaki.misakiCode.Relics;

namespace misaki.misakiCode.Character;

public class MisakiCharacter : PlaceholderCharacterModel
{
    public const string CharacterId = "misaki";
    public static readonly Color Color = new("ffffff");
    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 70;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<MisakiStrike>(),
        ModelDb.Card<MisakiStrike>(),
        ModelDb.Card<MisakiStrike>(),
        ModelDb.Card<MisakiStrike>(),
        ModelDb.Card<MisakiStrike>(),
        ModelDb.Card<MisakiDefend>(),
        ModelDb.Card<MisakiDefend>(),
        ModelDb.Card<MisakiDefend>(),
        ModelDb.Card<MisakiDefend>(),
        ModelDb.Card<SakuraSetsudan>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<SwordGust>()
    ];

    public override string CustomVisualPath => "res://misaki/scenes/model/misaki/character_view.tscn";
    public override CardPoolModel CardPool => ModelDb.CardPool<misakiCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<misakiRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<misakiPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }
    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomEnergyCounterPath => "res://misaki/scenes/ui/energy_counter.tscn";
    public override Color EnergyLabelOutlineColor => new(0.8f, 0f, 0.1f);
    
    public override string CustomCharacterSelectBg =>
        "res://misaki/scenes/model/misaki/character_select.tscn";
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
    public override string CustomMerchantAnimPath => "res://misaki/scenes/merchant/merchanttest.tscn";
    public override string CustomRestSiteAnimPath => "res://misaki/scenes/restsite/misakirestsite.tscn";


}
