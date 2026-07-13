using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace misaki.misaki.scenes.vfx;

public static class Slashes
{
    private const string Root = "res://misaki/scenes/vfx/";

    public static Node2D? Slash(Creature target)
    {
        // 1. Create the node
        var node = Create(target, "slashes");
        
        // 2. Register/Parent to the combat container
        if (node != null)
        {
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(node);
            
            // 3. Play your animation after it's added to the tree
            var anim = node.GetNodeOrNull<AnimationPlayer>("Node2D/Sprite2D/AnimationPlayer");
            anim?.Play("slash");
        }
        
        return node;
    }

    private static Node2D? Create(Creature target, string name)
    {
        if (TestMode.IsOn) return null;

        string path = $"{Root}{name}.tscn";
        var scene = GD.Load<PackedScene>(path);
        if (scene == null)
        {
            GD.PushError($"[Slashes] Failed to load VFX: {path}");
            return null;
        }

        var node = scene.Instantiate<Node2D>();

        var creatureNode = target.GetCreatureNode();
        if (creatureNode != null)
        {
            node.GlobalPosition = creatureNode.VfxSpawnPosition;
        }

        return node;
    }
}