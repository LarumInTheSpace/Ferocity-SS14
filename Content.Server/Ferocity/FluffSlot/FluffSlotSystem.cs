using Content.Server.GameTicking;
using Content.Server.Hands.Systems;
using Content.Shared.Inventory;
using Content.Shared.Clothing.Components;
using Content.Server.Storage.EntitySystems;
using Robust.Shared.ContentPack;
using Robust.Shared.Prototypes;
using Content.Server.Popups;

namespace Content.Server.Ferocity.FluffSlot;

public sealed class FluffSlot : EntitySystem
{
    [Dependency] private readonly InventorySystem _inventorySystem = default!;
    [Dependency] private readonly HandsSystem _handsSystem = default!;
    [Dependency] private readonly StorageSystem _storageSystem = default!;
    [Dependency] private readonly IResourceManager _res = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly PopupSystem _popup = default!;


    public override void Initialize()
    {
        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerSpawned);
    }

    private void OnPlayerSpawned(PlayerSpawnCompleteEvent args)
    {
        if (_prototypeManager.TryIndex<FluffSlotPrototype>(args.Player.Data.UserName, out var loadout))
        {
            var entity = Spawn(loadout.Item, Transform(args.Mob).Coordinates);
            
            if (!TryComp<ClothingComponent>(entity, out var clothing))
            {
                _handsSystem.TryPickup(args.Mob, entity);
                return;
            }

            string? firstSlotName = null;
            bool isEquiped = false;

            foreach (var slot in _inventorySystem.GetSlots(args.Mob))
            {

                if (!clothing.Slots.HasFlag(slot.SlotFlags))
                    continue;

                if (firstSlotName == null)
                    firstSlotName = slot.Name;
                
                if (_inventorySystem.TryGetSlotEntity(args.Mob, slot.Name, out var _))
                    continue;

                if (_inventorySystem.TryEquip(args.Mob, entity, slot.Name, true))
                {
                    isEquiped = true;
                    break;
                }
            }
            
            if (isEquiped || firstSlotName == null)
                return;

            if (_inventorySystem.TryGetSlotEntity(args.Mob, firstSlotName, out var slotEntity) &&
                _inventorySystem.TryGetSlotEntity(args.Mob, "back", out var backEntity) &&
                _storageSystem.CanInsert(backEntity.Value, slotEntity.Value, out _))
            {
                _storageSystem.Insert(backEntity.Value, slotEntity.Value);
            }

            _inventorySystem.TryEquip(args.Mob, entity, firstSlotName, true);
        }
    }
}